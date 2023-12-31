using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class InventoryGrid
{
    private readonly InventoryNode[,] nodes;
    private int width;
    private int height;
    private char emptyChar;
    private char lockedChar;
    private void UpdateSize()
    {
        height = nodes.GetLength(0);
        width = nodes.GetLength(1);
    }

    private void FillGrid(List<Vector2Int> openSlots)
    {
        for (int row = 0; row < height; row++)
        {
            for (int col = 0; col < width; col++)
            {
                nodes[row, col] = new InventoryNode(row, col);
            }
        }
        foreach (Vector2Int slot in openSlots)
        {
            nodes[slot.y, slot.x].Open();
            UIInventoryManager.main.OpenSlot(slot);
        }
    }

    public void SetOpenSlots(List<Vector2Int> openSlots)
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (openSlots.Any(slot => slot.x == x && slot.y == y))
                {
                    nodes[y, x].Open();
                    UIInventoryManager.main.OpenSlot(new Vector2Int(x, y));
                }
                else
                {
                    nodes[y, x].Close();
                    UIInventoryManager.main.CloseSlot(new Vector2Int(x, y));
                }
            }
        }
    }

    private List<InventoryNode> FindJunkNodes()
    {
        List<InventoryNode> junkNodes = new List<InventoryNode>();

        for (int row = 0; row < height; row++)
        {
            for (int col = 0; col < width; col++)
            {
                InventoryNode node = nodes[row, col];
                if (node.IsEmpty || node.IsLocked)
                {
                    continue;
                }

                bool junky = node.InventoryItem.Tier == ItemTier.Junk || node.InventoryItem.Tier == ItemTier.LargeJunk;

                if (junky)
                {
                    junkNodes.Add(node);
                }
            }
        }

        return junkNodes;
    }

    public List<InventoryItem> GetJunkItems()
    {
        List<InventoryNode> junkNodes = FindJunkNodes();
        HashSet<InventoryItem> items = new();

        foreach (InventoryNode node in junkNodes)
        {
            if (!items.Contains(node.InventoryItem))
            {
                items.Add(node.InventoryItem);
            }
        }

        return items.ToList();
    }

    private List<InventoryNode> FindSingleNodes()
    {
        List<InventoryNode> singleItemNodes = new List<InventoryNode>();
        for (int row = 0; row < height; row++)
        {
            for (int col = 0; col < width; col++)
            {
                InventoryNode node = nodes[row, col];
                if (!node.IsEmpty && !node.IsLocked && node.InventoryItem.Shape.ShapeType == InventoryShapeType.Single)
                {
                    singleItemNodes.Add(node);
                }
            }
        }

        return singleItemNodes;
    }

    public Dictionary<string, List<InventoryNode>> GetUniqueStackables()
    {
        Dictionary<string, List<InventoryNode>> stackableNodes = new();
        for (int row = 0; row < height; row++)
        {
            for (int col = 0; col < width; col++)
            {
                InventoryNode node = nodes[row, col];
                if (!node.IsEmpty && !node.IsLocked && node.InventoryItem != null && node.InventoryItem.Stackable)
                {
                    string stackKey = node.InventoryItem.GetStackKey();
                    if (!stackableNodes.ContainsKey(stackKey))
                    {
                        stackableNodes[stackKey] = new() { node };
                    }
                    else
                    {
                        stackableNodes[stackKey].Add(node);
                    }

                }
            }
        }
        return stackableNodes;
    }

    public List<InventoryItem> StackSingles()
    {
        List<InventoryItem> deletedItems = new();
        List<InventoryNode> singleNodes = FindSingleNodes();
        Dictionary<string, List<InventoryNode>> stackables = new();

        foreach (InventoryNode node in singleNodes)
        {
            string nodeStackKey = node.InventoryItem.GetStackKey();
            if (!stackables.ContainsKey(nodeStackKey))
            {
                stackables.Add(nodeStackKey, new() { node });
            }
            else
            {
                stackables[nodeStackKey].Add(node);
            }
        }

        Dictionary<string, List<InventoryNode>> modifiedStackables = new(stackables);
        foreach (KeyValuePair<string, List<InventoryNode>> kvp in stackables)
        {
            modifiedStackables[kvp.Key] = kvp.Value.OrderBy(x => x.InventoryItem.StackCount).ToList();
            modifiedStackables[kvp.Key][0].InventoryItem.AddStack(kvp.Value.Count - 1);

            for (int i = 1; i < modifiedStackables[kvp.Key].Count; i++)
            {
                deletedItems.Add(modifiedStackables[kvp.Key][i].InventoryItem);
                modifiedStackables[kvp.Key][i].InventoryItem.ClearNodes();
                modifiedStackables[kvp.Key][i].Clear();
            }
        }

        return deletedItems;
    }

    private List<InventoryNode> GetEmptyNodes()
    {
        List<InventoryNode> emptyNodes = new List<InventoryNode>();
        for (int row = 0; row < height; row++)
        {
            for (int col = 0; col < width; col++)
            {
                InventoryNode node = nodes[row, col];
                if (node.IsEmpty && !node.IsLocked)
                {
                    emptyNodes.Add(node);
                }
            }
        }
        return emptyNodes;
    }

    public InventoryGrid(int gridWidth, int gridHeight, char emptyChar, char lockedChar, List<Vector2Int> openSlots)
    {
        this.emptyChar = emptyChar;
        this.lockedChar = lockedChar;
        nodes = new InventoryNode[gridHeight, gridWidth];
        UpdateSize();
        FillGrid(openSlots);
    }

    public InventoryNode GetNode(int y, int x)
    {
        if (x < 0 || x >= width || y < 0 || y >= height)
        {
            return null;
        }

        //UnityEngine.Debug.Log($"Attempt to get node: Y: {y}, X: {x} (height: {height}, width: {width})");
        return nodes[y, x];
    }

    public void MoveItem(InventoryItem item, ItemPlacement placement)
    {
        RemoveItem(item);
        //UnityEngine.Debug.Log($"move node to {placement.Nodes.First().Y}, {placement.Nodes.First().X}");

        if (placement.isStacked)
        {
            InsertStack(item, placement);
        }
        else
        {
            InsertItem(item, placement.Origin, false);
        }

    }

    public bool InsertItemRandomly(InventoryItem item)
    {
        List<InventoryNode> emptyNodes = GetEmptyNodes();
        System.Random random = new();
        List<InventoryNode> randomizedNodes = emptyNodes.OrderBy((item) => random.Next()).ToList();

        foreach (InventoryNode node in randomizedNodes)
        {
            bool success = InsertItem(item, node);
            if (success)
            {
                return true;
            }
        }
        return false;
    }

    public ItemPlacement GetItemPlacement(InventoryItem item, int startY, int startX, bool forceNoStack = true)
    {
        InventoryShape shape = item.Shape;
        bool success = true;
        int failedNodes = 0;
        List<InventoryNode> placementNodes = new();
        for (int row = 0; row < shape.Positions.GetLength(0); row += 1)
        {
            for (int col = 0; col < shape.Positions.GetLength(1); col += 1)
            {
                int position = shape.Positions[row, col];
                if (position == 0)
                {
                    continue;
                }
                InventoryNode node = GetNode(startY + row, startX + col);
                bool stackPossible = node?.InventoryItem?.IsStackable(item) ?? false;

                if (stackPossible && !forceNoStack && !node.IsSame(item))
                {
                    return new ItemPlacement
                    {
                        Nodes = new() { node }, // Only 1x1 supported!
                        Success = true,
                        isStacked = true
                    };
                }
                else if (node == null || !node.IsEmptyOrSame(item) || node.IsLocked)
                {
                    success = false;
                    failedNodes += 1;
                }

                placementNodes.Add(node);
            }
        }
        return new ItemPlacement
        {
            Origin = GetNode(startY, startX),
            Nodes = placementNodes,
            Success = success,
            FailedNodes = failedNodes,
            isStacked = false
        };
    }

    public bool InsertItem(InventoryItem item, InventoryNode startingNode, bool forceNoStack = true)
    {
        ItemPlacement itemPlacement = GetItemPlacement(item, startingNode.Y, startingNode.X, forceNoStack);
        if (!itemPlacement.Success)
        {
            UnityEngine.Debug.Log($"Failed to place {item}. Blocked by {itemPlacement.FailedNodes} nodes.");
            return false;
        }
        foreach (InventoryNode placementNode in itemPlacement.Nodes)
        {
            item.AddPlacementNode(placementNode);
            placementNode.SetItem(item);
        }
        item.SetStartNode(startingNode);
        InventoryManager.main.UpdateDebug();
        return true;
    }

    public bool InsertStack(InventoryItem item, ItemPlacement placement)
    {
        placement.Nodes[0].InventoryItem.AddStack(item.StackCount);
        InventoryManager.main.UpdateDebug();
        return true;
    }

    public void RemoveItem(InventoryItem item)
    {
        //        Debug.Log($"Cleaning up item {item}");
        foreach (InventoryNode node in item.Nodes)
        {
            node.Clear();
        }
        item.Nodes.Clear();
    }

    public override string ToString()
    {
        string representation = "";
        for (int row = 0; row < height; row++)
        {
            for (int col = 0; col < width; col++)
            {
                InventoryNode node = nodes[row, col];
                if (node.IsLocked)
                {
                    representation += lockedChar;
                }
                else if (node.IsEmpty)
                {
                    representation += emptyChar;
                }
                else
                {
                    representation += node.InventoryItem.Identity.Character;
                }
            }
            representation += "\n";
        }
        return representation;
    }
}

public struct ItemPlacement
{
    public InventoryNode Origin;
    public List<InventoryNode> Nodes;
    public int FailedNodes;
    public bool Success;
    public bool isStacked;
}