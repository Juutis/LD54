using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LootGenerator
{
    Queue<LootItemData> lootItems;
    List<BaseLootConfig> baseLootConfigs;
    LevelConfig levelConfig;

    public LootGenerator(LevelConfig levelConfig, List<BaseLootConfig> baseLootConfigs)
    {
        this.levelConfig = levelConfig;
        this.baseLootConfigs = baseLootConfigs;
    }

    public void InitializeLoot()
    {
        List<LootItemData> items = new();

        foreach (DropRate dropRate in levelConfig.DropRates)
        {
            List<BaseLootConfig> dropTierConfigs = baseLootConfigs.Where(x => x.Tier == dropRate.Tier).ToList();
            if (!dropTierConfigs.Any())
            {
                Debug.LogError($"No BaseLootConfigs for tier ${dropRate.Tier}");
                continue;
            }

            int itemDrops = Mathf.CeilToInt(levelConfig.ItemAmount * dropRate.Percentage / 100);

            for (int i = 0; i < itemDrops; i++)
            {
                RarityConfig rconf = LootManager.instance.GetRarityConfig(dropRate.Rarity);
                BaseLootConfig lconf = dropTierConfigs[Random.Range(0, dropTierConfigs.Count() - 1)];

                items.Add(new LootItemData(rconf, lconf));
            }
        }

        Helpers.Shuffle(items);
        lootItems = new(items);
    }

    public int LootCount() { return lootItems.Count; }

    public LootItemData NextLoot()
    {
        if (lootItems.Count == 0)
        {
            return null;
        }

        return lootItems.Dequeue();
    }
}
