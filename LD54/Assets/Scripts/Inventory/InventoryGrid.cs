using System.Collections.Generic;
using System.Linq;
using System;

public class InventoryGrid
{
    private readonly InventoryNode[,] nodes;
    private int width;
    private int height;
    private char emptyChar;
    private void UpdateSize()
    {
        width = nodes.GetLength(0);
        height = nodes.GetLength(1);
    }

    private void FillGrid()
    {
        for (int row = 0; row < width; row++)
        {
            for (int col = 0; col < height; col++)
            {
                nodes[row, col] = new InventoryNode(row, col);
            }
        }
    }

    private List<InventoryNode> GetEmptyNodes()
    {
        List<InventoryNode> emptyNodes = new List<InventoryNode>();
        for (int row = 0; row < width; row++)
        {
            for (int col = 0; col < height; col++)
            {
                InventoryNode node = nodes[row, col];
                if (node.IsEmpty)
                {
                    emptyNodes.Add(node);
                }
            }
        }
        return emptyNodes;
    }

    public InventoryGrid(int gridWidth, int gridHeight, char emptyChar)
    {
        this.emptyChar = emptyChar;
        nodes = new InventoryNode[gridHeight, gridWidth];
        UpdateSize();
        FillGrid();
    }

    public InventoryNode GetNode(int y, int x)
    {
        if (x < 0 || x >= width || y < 0 || y >= height)
        {
            return null;
        }
        return nodes[y, x];
    }

    public bool InsertItemRandomly(InventoryItem item)
    {
        List<InventoryNode> emptyNodes = GetEmptyNodes();
        Random random = new();
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

    public ItemPlacement GetItemPlacement(InventoryItem item, InventoryNode startingNode)
    {
        InventoryShape shape = item.Shape;
        bool success = true;
        int failedNodes = 0;
        int currentX = startingNode.X;
        int currentY = startingNode.Y;
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
                InventoryNode node = GetNode(currentY + row, currentX + col);
                if (node == null || !node.IsEmpty)
                {
                    success = false;
                    failedNodes += 1;
                }
                placementNodes.Add(node);
            }
        }
        return new ItemPlacement
        {
            Nodes = placementNodes,
            Success = success,
            FailedNodes = failedNodes
        };
    }

    public bool InsertItem(InventoryItem item, InventoryNode startingNode)
    {
        ItemPlacement itemPlacement = GetItemPlacement(item, startingNode);
        if (!itemPlacement.Success)
        {
            //UnityEngine.Debug.Log($"Failed to place {item}. Blocked by {itemPlacement.FailedNodes} nodes.");
            return false;
        }
        foreach (InventoryNode placementNode in itemPlacement.Nodes)
        {
            placementNode.SetItem(item);
        }
        item.SetNode(startingNode);

        return true;
    }

    public override string ToString()
    {
        string representation = "";
        for (int row = 0; row < height; row++)
        {
            for (int col = 0; col < width; col++)
            {
                InventoryNode node = nodes[row, col];
                if (node.IsEmpty)
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
    public List<InventoryNode> Nodes;
    public int FailedNodes;
    public bool Success;
}