using System;
using System.Collections.Generic;
using UnityEngine;

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

    private LootItemData itemData;
    private bool stackable = false;

    public InventoryItem(InventoryShape shape, ItemIdentity identity, Sprite sprite, string name, ItemTier tier, LootRarity rarity)
    {
        this.sprite = sprite;
        this.identity = identity;
        this.shape = shape;
        color = new Color(0.2f, 0.2f, 0.2f, 0.5f);

        if (this.shape.ShapeType == InventoryShapeType.Single)
        {
            stackable = true;
        }

        this.name = name;
        this.tier = tier;
        this.rarity = rarity;
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

    public override string ToString()
    {
        return $"Item[{identity}] with shape {shape}";
    }
}