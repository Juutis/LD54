using System.Collections.Generic;
using UnityEngine;

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

    public InventoryItem(InventoryShape shape, ItemIdentity identity, Sprite sprite)
    {
        this.sprite = sprite;
        this.identity = identity;
        this.shape = shape;
        color = new Color(0.2f, 0.2f, 0.2f, 0.5f);
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


    public override string ToString()
    {
        return $"Item[{identity}] with shape {shape}";
    }
}