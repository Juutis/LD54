using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BaseLootConfig", menuName = "ScriptableObjects/BaseLootConfig", order = 1)]
public class BaseLootConfig : ScriptableObject
{
    public ItemTier Tier;
    public string LootName;
    [TextArea]
    public string Lore;
    public float BasePrice;
    public List<Sprite> Sprites;
    public InventoryShapeType Shape;
}


public enum ItemTier
{
    Junk,
    Potion,
    Gem,
    Other
}
