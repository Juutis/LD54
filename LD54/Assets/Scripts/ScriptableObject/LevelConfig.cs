using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelConfig", menuName = "ScriptableObjects/LevelConfig", order = 1)]
public class LevelConfig : ScriptableObject
{
    public string LevelName;
    public List<DropRate> DropRates;
    public int ItemAmount;
    public List<Enemy> Encounters;
    public int EnemyMinItems;
    public int EnemyMaxItems;
}

[Serializable]
public class DropRate
{
    public LootRarity Rarity;
    public ItemTier Tier;
    public float Percentage;
}
