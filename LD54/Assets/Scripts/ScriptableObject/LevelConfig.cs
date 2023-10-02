using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelConfig", menuName = "ScriptableObjects/LevelConfig", order = 1)]
public class LevelConfig : ScriptableObject
{
    public string LevelName;
    public List<DropRate> DropRates;
    public List<RarityDropRate> RarityDropRates;
    public int ItemAmount;
    public List<Enemy> Encounters { get { return getEnemies(); } }
    public int EnemyMinItems;
    public int EnemyMaxItems;
    public List<EncounterCount> EncounterCounts;
    public int TargetGold;

    private List<Enemy> getEnemies() {
        var enemies = new List<Enemy>();
        foreach(var enemyCount in EncounterCounts) {
            for(var i = 0; i < enemyCount.Count; i++) {
                enemies.Add(enemyCount.Enemy);
            }
        }
        return enemies;
    }
}

[Serializable]
public class DropRate
{
    public ItemTier Tier;
    public int Weight;
}

[Serializable]
public class RarityDropRate
{
    public LootRarity Rarity;
    public int Weight;
}

[Serializable]
public class EncounterCount
{
    public Enemy Enemy;
    public int Count;
}
