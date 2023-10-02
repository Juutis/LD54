using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class UIInventoryGrid : MonoBehaviour
{
    [SerializeField]
    private UIInventoryNode nodePrefab;
    [SerializeField]
    private UIInventoryItem itemPrefab;
    private List<UIInventoryNode> nodes;
    private List<UIInventoryItem> items;
    [SerializeField]
    private Transform nodeContainer;
    [SerializeField]
    private Transform itemContainer;
    private Vector2 nodeSize;

    [SerializeField]
    private GridLayoutGroup grid;


    [SerializeField]
    private UIInventoryItem uiInventoryItemGhost;

    public Vector2 Initialize(int columns, int rows)
    {
        nodes = new();
        items = new();
        RectTransform rt = GetComponent<RectTransform>();
        float width = rt.rect.width;
        float height = rt.rect.height;
        nodeSize = new(width / columns, height / rows);
        grid.cellSize = nodeSize;
        for (int row = 0; row < rows; row += 1)
        {
            for (int col = 0; col < columns; col += 1)
            {

                UIInventoryNode node = Instantiate(nodePrefab, nodeContainer);
                node.Initialize(row, col, nodeSize);
                nodes.Add(node);
            }
        }
        return nodeSize;
    }

    public void HideGhost()
    {
        uiInventoryItemGhost.gameObject.SetActive(false);
    }

    public ItemPlacement ShowGhost(UIInventoryItem uiInventoryItem)
    {
        UIInventoryNode closestNode = ClosestNode((Vector2)uiInventoryItem.transform.position);
        if (closestNode != null)
        {
            //closestNode.Highlight(Color.magenta);
            if (!uiInventoryItemGhost.gameObject.activeSelf)
            {
                uiInventoryItemGhost.gameObject.SetActive(true);
                if (uiInventoryItem.IsBufferItem)
                {
                    uiInventoryItemGhost.InitializeAsBufferItem(uiInventoryItem.InventoryItem, nodeSize, true);
                }
                else
                {
                    uiInventoryItemGhost.Initialize(uiInventoryItem.InventoryItem, nodeSize, true);
                }
            }
            ItemPlacement placement = InventoryManager.main.GetItemPlacement(uiInventoryItem.InventoryItem, closestNode.Y, closestNode.X, false);
            uiInventoryItemGhost.transform.position = closestNode.transform.position;
            if (!placement.Success)
            {
                uiInventoryItemGhost.HighlightAsBlocked();
            }
            else
            {
                uiInventoryItemGhost.HighlightAsPlaceable();
                return placement;
            }
        }
        return new ItemPlacement
        {
            Success = false
        };
    }

    public void AddItem(InventoryItem inventoryItem)
    {
        UIInventoryNode node = nodes.Where(node => node != null && node.X == inventoryItem.Node.X && node.Y == inventoryItem.Node.Y).FirstOrDefault();
        if (node == null)
        {
            Debug.Log($"Couldn't find node {inventoryItem.Node}!");
            return;
        }
        //Debug.Log($"Node: {node} ({node.transform.position}) ");
        UIInventoryItem uiItem = Instantiate(itemPrefab, itemContainer);
        uiItem.Initialize(inventoryItem, nodeSize);
        //Debug.Log($"{uiItem.transform.position}");
        items.Add(uiItem);
    }

    public void OpenSlot(Vector2Int slot)
    {
        UIInventoryNode foundNode = nodes.Find(node => node.Y == slot.y && node.X == slot.x);
        if (foundNode == null)
        {
            Debug.Log($"Couldn't find node at {slot}!");
            return;
        }
        foundNode.Open();
    }

    public void CloseSlot(Vector2Int slot)
    {
        UIInventoryNode foundNode = nodes.Find(node => node.Y == slot.y && node.X == slot.x);
        if (foundNode == null)
        {
            Debug.Log($"Couldn't find node at {slot}!");
            return;
        }
        foundNode.Close();
    }

    public void RemoveItem(UIInventoryItem uiInventoryItem)
    {
        items.Remove(uiInventoryItem);
        uiInventoryItem.Kill();
    }

    private float NodeDistance(UIInventoryNode node, Vector2 position)
    {
        return Mathf.Abs(((Vector2)node.transform.position - position).magnitude);
    }

    public UIInventoryNode ClosestNode(Vector2 position)
    {
        return nodes.Aggregate((node1, node2) => NodeDistance(node1, position) > NodeDistance(node2, position) ? node2 : node1);
    }

    public void EmptyInventory()
    {
        for (int index = items.Count - 1; index >= 0; index -= 1)
        {
            UIInventoryItem item = items[index];
            RemoveItem(item);
        }
        HideGhost();
    }
}
