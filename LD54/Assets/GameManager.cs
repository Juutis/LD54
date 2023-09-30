using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private LootItem lootItemPrefab;
    [SerializeField]
    private Enemy enemyPrefab;
    [SerializeField]
    private List<LevelConfig> levelConfigs;

    private int currentLevelNum = 0;
    private LevelConfig currentLevel;

    private List<Enemy> enemies;

    private bool isLoaded = false;

    private float enemyXSpace = 10f;

    // Start is called before the first frame update
    void Start()
    {
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

        List<Enemy> enemies = new List<Enemy>();

        for (int i = 0; i < currentLevel.EnemyAmount; i++)
        {
            Enemy enemy = Instantiate(enemyPrefab);
            enemies.Add(enemy);
            enemy.transform.position = new Vector2((i + 1f) * enemyXSpace, 0);

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

            int x = 0;
            while (tmpEnemies.Count > 0)
            {
                Debug.Log($"x {x} {generator.LootCount()}");
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
                x++;
            }
        }
    }
}
