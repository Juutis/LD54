using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BaseLootConfig", menuName = "ScriptableObjects/BaseLootConfig", order = 1)]
public class BaseLootConfig : ScriptableObject
{
    // public ItemShape Shape;
    public ItemTier Tier;
    public string LootName;
    public float BasePrice;
    public List<Sprite> Sprites;
}

// public class ItemShape : ScriptableObject
// {
// 
// }


public enum ItemTier
{
    Junk,
    Potion,
    Gem,
    Other
}
