using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RarityConfig", menuName = "ScriptableObjects/RarityConfig", order = 1)]
public class RarityConfig : ScriptableObject
{
    public LootRarity Rarity;
    public List<string> Prefixes;
    public float PriceScale;
    // public Color color;
    // public Sprite border;
    // public Sprite background;
}

public enum LootRarity
{
    Common,
    Uncommon,
    Rare,
    Legendary
}
