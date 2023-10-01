using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootItem : MonoBehaviour
{
    private LootItemData lootData;

    public LootItemData LootData { get { return lootData; } }

    public void Initialize(LootItemData data)
    {
        lootData = data;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}

public class LootItemData
{
    public LootRarity Rarity { get; set; }
    public string Prefix { get; set; }
    public float PriceScale { get; set; }
    public ItemTier Tier { get; set; }
    public string LootName { get; set; }
    public float BasePrice { get; set; }
    public Sprite Sprite { get; set; }
    public InventoryShapeType Shape { get; set; }

    public LootItemData(RarityConfig rarityConfig, BaseLootConfig lootConfig)
    {
        Rarity = rarityConfig.Rarity;
        Prefix = rarityConfig.Prefixes[Random.Range(0, rarityConfig.Prefixes.Count)];
        PriceScale = rarityConfig.PriceScale;

        Tier = lootConfig.Tier;
        Shape = lootConfig.Shape;
        BasePrice = lootConfig.BasePrice;
        Sprite = lootConfig.Sprites[Random.Range(0, lootConfig.Sprites.Count)];
        Shape = lootConfig.Shape;
    }
}