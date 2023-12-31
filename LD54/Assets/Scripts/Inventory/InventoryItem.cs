using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class InventoryItem
{
    private readonly InventoryShape shape;
    public InventoryShape Shape { get { return shape; } }
    private readonly ItemIdentity identity;
    public ItemIdentity Identity { get { return identity; } }

    private InventoryNode node;
    public InventoryNode Node { get { return node; } }
    private List<InventoryNode> nodes = new();
    public List<InventoryNode> Nodes { get { return nodes; } }

    private Sprite sprite;
    public Sprite Sprite { get { return sprite; } }

    private Color color;
    public Color Color { get { return color; } }

    private string name;
    public string Name { get { return name; } }

    private ItemTier tier;
    public ItemTier Tier { get { return tier; } }

    private LootRarity rarity;
    public LootRarity Rarity { get { return rarity; } }

    private int stackCount = 1;
    public int StackCount { get { return stackCount; } }

    private float itemPrice = 0f;
    public float ItemPrice { get { return itemPrice; } }

    private float rarityScale = 0f;
    public float RarityScale { get { return rarityScale; } }

    private string lore;
    public string Lore { get { return lore; } }

    public string LootName { get { return name; } }
    private bool stackable = false;
    public bool Stackable { get { return stackable; } }


    public InventoryItem(InventoryShape shape, ItemIdentity identity, Sprite sprite, string name, ItemTier tier, LootRarity rarity, float itemPrice, string lore, float rarityScale, bool stackable)
    {
        this.sprite = sprite;
        this.identity = identity;
        this.shape = shape;

        this.stackable = stackable;
        color = InventoryManager.GetRarityColor(rarity);
        this.name = name;
        this.tier = tier;
        this.rarity = rarity;
        this.itemPrice = itemPrice;
        this.lore = lore;
        this.rarityScale = rarityScale;
    }

    public void ClearNodes()
    {
        nodes = new();
    }

    public void SetStartNode(InventoryNode newNode)
    {
        node = newNode;
    }

    public void AddPlacementNode(InventoryNode newNode)
    {
        nodes.Add(newNode);
    }

    public bool AddStack(int count)
    {
        if (stackable)
        {
            stackCount += count;
            return true;
        }

        return false;
    }

    public string ItemKey()
    {
        return $"{this.name};{this.tier.ToString()}";
    }

    public bool IsStackable(string name, ItemTier tier, LootRarity rarity)
    {
        if (!stackable) { return false; }
        else if (this.name != name) { return false; }
        else if (this.tier != tier) { return false; }
        else if (this.rarity != rarity) { return false; }

        return true;
    }

    public bool IsStackable(InventoryItem item)
    {
        return IsStackable(item.name, item.tier, item.rarity);
    }

    public string GetStackKey()
    {
        return $"{this.name};{this.tier};{this.rarity}";
    }

    public override string ToString()
    {
        return $"Item[{identity}] with shape {shape}";
    }
}