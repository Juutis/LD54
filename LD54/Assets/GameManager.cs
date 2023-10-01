using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{
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

        LootGenerator generator = new LootGenerator(currentLevel, LootManager.instance.GetBaseLootConfigs());
        generator.InitializeLoot();

        InstantiateEnemiesWithLoot(generator);
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
        foreach(Enemy enemy in enemies)
        {
            enemy.transform.position = new Vector2((i + 1f) * enemyXSpace, enemyAnchor.position.y);
            i++;
        }
    }
}
