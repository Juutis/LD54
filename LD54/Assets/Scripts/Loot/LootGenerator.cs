using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LootGenerator
{
    Queue<LootItemData> lootItems;
    List<BaseLootConfig> baseLootConfigs;
    LevelConfig levelConfig;

    public LootGenerator(LevelConfig levelConfig)
    {
        this.levelConfig = levelConfig;
        this.baseLootConfigs = Resources.LoadAll("BaseLootConfigs", typeof(BaseLootConfig)).Select(it => (BaseLootConfig)it).ToList();
    }

    public void InitializeLoot()
    {
        List<LootItemData> items = new();

        for(var i = 0; i < levelConfig.ItemAmount; i++) {
            var itemTier = randomItemTier(levelConfig);
            var itemRarity = randomLootRarity(levelConfig);

            List<BaseLootConfig> dropTierConfigs = baseLootConfigs.Where(x => x.Tier == itemTier).ToList();
            if (!dropTierConfigs.Any())
            {
                Debug.LogError($"No BaseLootConfigs for tier ${itemTier}");
                continue;
            }
            
            RarityConfig rconf = LootManager.main.GetRarityConfig(itemRarity);
            BaseLootConfig lconf = dropTierConfigs[Random.Range(0, dropTierConfigs.Count())];

            items.Add(new LootItemData(rconf, lconf));
        }

        Helpers.Shuffle(items);
        lootItems = new(items);
    }

    private LootRarity randomLootRarity(LevelConfig levelConfig) {
        var weightedList = new List<LootRarity>();
        foreach(var rarityRate in levelConfig.RarityDropRates) {
            for(var i = 0; i < rarityRate.Weight; i++) {
                weightedList.Add(rarityRate.Rarity);
            }
        }
        return weightedList[Random.Range(0, weightedList.Count())];
    }

    private ItemTier randomItemTier(LevelConfig levelConfig) {
        var weightedList = new List<ItemTier>();
        foreach(var rate in levelConfig.DropRates) {
            for(var i = 0; i < rate.Weight; i++) {
                weightedList.Add(rate.Tier);
            }
        }
        return weightedList[Random.Range(0, weightedList.Count())];
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
