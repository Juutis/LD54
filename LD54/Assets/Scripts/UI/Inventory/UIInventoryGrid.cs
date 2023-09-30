using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

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

    public void Initialize(int width, int height)
    {
        nodes = new();
        items = new();
        for (int row = 0; row < height; row += 1)
        {
            for (int col = 0; col < width; col += 1)
            {

                UIInventoryNode node = Instantiate(nodePrefab, nodeContainer);
                node.Initialize(row, col);
                nodes.Add(node);
            }
        }
    }
    public void AddItem(InventoryItem inventoryItem)
    {
        UIInventoryNode node = nodes.Find(node => node != null && node.X == inventoryItem.Node.X && node.Y == inventoryItem.Node.Y);
        if (node == null)
        {
            Debug.Log($"Couldn't find node {inventoryItem.Node}!");
            return;
        }
        //Debug.Log($"Node: {node} ({node.transform.position}) ");
        UIInventoryItem uiItem = Instantiate(itemPrefab, itemContainer);
        uiItem.Initialize(inventoryItem);
        //Debug.Log($"{uiItem.transform.position}");
        items.Add(uiItem);
    }

    public void RemoveItem(UIInventoryItem uiInventoryItem)
    {
        items.Remove(uiInventoryItem);
        Destroy(uiInventoryItem.gameObject);
    }

    private float NodeDistance(UIInventoryNode node, Vector2 position)
    {
        return Mathf.Abs(((Vector2)node.transform.position - position).magnitude);
    }

    public UIInventoryNode ClosestNode(Vector2 position)
    {
        return nodes.Aggregate((node1, node2) => NodeDistance(node1, position) > NodeDistance(node2, position) ? node2 : node1);
    }
}
