using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class LootManager : MonoBehaviour
{
    public static LootManager instance;

    [SerializeField]
    public List<RarityConfig> RarityConfigs;

    private Dictionary<LootRarity, RarityConfig> rarityConfigsDict;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        var rarityTypeGroups = RarityConfigs.GroupBy(x => x.Rarity);

        if (rarityTypeGroups.Any(x => x.Count() > 1))
        {
            var multipleRarityConfigNames = rarityTypeGroups
                .Where(x => x.Count() > 1)
                .Select(x => x.Key.ToString());

            string nameList = string.Join(", ", multipleRarityConfigNames);

            Debug.LogError($"LootManager: More than one rarity config for a rarity type(s): {nameList}");
        }

        rarityConfigsDict = RarityConfigs.ToDictionary(x => x.Rarity, x => x);
    }

    public RarityConfig GetRarityConfig(LootRarity rarity)
    {
        if (!rarityConfigsDict.ContainsKey(rarity))
        {
            Debug.LogError($"LootManager: No RarityConfig for {rarity}");
            return null;
        }

        return rarityConfigsDict[rarity];
    }

}
