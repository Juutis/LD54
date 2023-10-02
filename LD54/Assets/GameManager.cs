using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private PlayerProgress progress = new PlayerProgress();
    public PlayerProgress PlayerProgress { get { return progress; } }

    [SerializeField]
    private LootItem lootItemPrefab;
    [SerializeField]
    private Enemy enemyPrefab;
    [SerializeField]
    private Transform enemyAnchor;
    [SerializeField]
    private List<LevelConfig> levelConfigs;

    private int currentLevelNum = 0;
    private LevelConfig currentLevel;

    private bool isLoaded = false;

    private float enemyXSpace = 10f;

    private List<Enemy> activeEnemies = new();

    public static GameManager Main;

    void Awake()
    {
        Main = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        foreach (LevelConfig levelConfig in levelConfigs)
        {
            if (levelConfig.Encounters.Count * levelConfig.EnemyMinItems > levelConfig.ItemAmount)
            {
                Debug.LogError($"{levelConfig.LevelName} level has less total items than [ enemies ({levelConfig.Encounters.Count}) * min items ({levelConfig.EnemyMinItems}) ]!");
            }
            else if (levelConfig.Encounters.Count * levelConfig.EnemyMaxItems < levelConfig.ItemAmount)
            {
                Debug.LogError($"{levelConfig.LevelName} level has too many total items for the enemy count ({levelConfig.Encounters.Count}) / max items per enemy ({levelConfig.EnemyMaxItems})!");
            }
        }
        MusicManager.main.StartMusic(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (!isLoaded)
        {
            isLoaded = true;
            Debug.Log("Loading level");
            LoadLevel();
        }
    }

    private void LoadLevel()
    {
        if (currentLevelNum >= levelConfigs.Count)
        {
            Debug.LogError("No victory/end state implemented!");
            return;
        }

        currentLevel = levelConfigs[currentLevelNum];

        LootGenerator generator = new LootGenerator(currentLevel);
        generator.InitializeLoot();

        InstantiateEnemiesWithLoot(generator);

        UIShop.main.SetLevelName(currentLevel.LevelName);
    }

    private void InstantiateEnemiesWithLoot(LootGenerator generator)
    {
        List<Enemy> enemies = new List<Enemy>();

        foreach (Enemy enemyPrefab in currentLevel.Encounters)
        {
            Enemy enemy = Instantiate(enemyPrefab);
            enemies.Add(enemy);

            List<LootItem> items = new();

            for (int j = 0; j < currentLevel.EnemyMinItems; j++)
            {
                LootItem item = Instantiate(lootItemPrefab);
                item.Initialize(generator.NextLoot());

                items.Add(item);
            }

            enemy.AddItems(items);
        }

        List<Enemy> tmpEnemies = new(enemies);

        LootItemData data;
        while ((data = generator.NextLoot()) != null)
        {
            LootItem item = Instantiate(lootItemPrefab);
            item.Initialize(data);

            while (tmpEnemies.Count > 0)
            {
                Enemy e = tmpEnemies[Random.Range(0, tmpEnemies.Count - 1)];
                if (e.ItemCount() >= currentLevel.EnemyMaxItems)
                {
                    tmpEnemies.Remove(e);
                }
                else
                {
                    e.AddItem(item);
                    break;
                }
            }
        }

        Helpers.Shuffle(enemies);

        // spread enemies around
        int i = 0;
        foreach (Enemy enemy in enemies)
        {
            enemy.transform.position = new Vector2((i + 1f) * enemyXSpace, enemyAnchor.position.y);
            i++;
            activeEnemies.Add(enemy);
        }
    }

    public void EnemyHandled(Enemy enemy)
    {
        activeEnemies.Remove(enemy);
        if (activeEnemies.Count == 0)
        {
            Invoke("LevelOutro", 2.0f);
            Invoke("LevelEnd", 10.0f);
        }
    }

    public void LevelOutro()
    {
        ScrollingWorld.Instance.Outro();
    }

    public void LevelEnd()
    {
        Debug.Log("LEVEL FINISHED!");
        UIShop.main.Show(delegate
        {
            Debug.Log("Shop closed.");
            ScrollingWorld.Instance.Reset();
            currentLevelNum += 1;
            LoadLevel();
        });
    }

}

[System.Serializable]
public class PlayerProgress
{
    int gold = 0;
    public int Gold { get { return gold; } }

    private List<UpgradeConfig> boughtUpgrades = new();

    public bool HasEnoughGold(int price)
    {
        return gold >= price;
    }

    public bool HasUpgrade(UpgradeConfig config)
    {
        return boughtUpgrades.Contains(config);
    }

    public void AddUpgrade(UpgradeConfig config)
    {
        boughtUpgrades.Add(config);
    }

    public bool CanBuy(UpgradeConfig upgrade)
    {
        if (boughtUpgrades.Contains(upgrade))
        {
            return false;
        }
        foreach (UpgradeConfig requirement in upgrade.Requirements)
        {
            if (!HasUpgrade(requirement))
            {
                return false;
            }
        }
        return HasEnoughGold(upgrade.Price);
    }

    public void UpdateGold(int change)
    {
        gold += change;
    }

}