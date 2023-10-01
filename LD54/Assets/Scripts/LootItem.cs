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
    private RarityConfig rarityConfig;
    private BaseLootConfig lootConfig;

    public BaseLootConfig LootConfig { get { return lootConfig; } }

    public LootItemData(RarityConfig rarityConfig, BaseLootConfig lootConfig)
    {
        this.rarityConfig = rarityConfig;
        this.lootConfig = lootConfig;
    }
}