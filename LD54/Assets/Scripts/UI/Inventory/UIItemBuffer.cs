using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class UIItemBuffer : MonoBehaviour
{
    private List<UIInventoryNode> nodes = new();
    private Queue<UIInventoryItem> items = new();

    [SerializeField]
    private UIInventoryItem itemPrefab;
    [SerializeField]
    private UIInventoryNode nodePrefab;
    [SerializeField]
    private Transform nodeContainer;
    [SerializeField]
    private Transform itemContainer;
    Vector2 nodeSize;
    int size = 5;
    public void Initialize(Vector2 nodeSize, int size)
    {
        this.size = size;
        this.nodeSize = nodeSize;
        FillGrid();
    }

    private void FillGrid()
    {
        for (int row = 0; row < size; row++)
        {
            UIInventoryNode node = Instantiate(nodePrefab, nodeContainer);
            node.Initialize(row, 0, nodeSize);
            nodes.Add(node);
        }
    }
    public void AddItem(InventoryItem inventoryItem)
    {
        UIInventoryItem uiItem = Instantiate(itemPrefab, itemContainer);
        uiItem.InitializeAsBufferItem(inventoryItem, nodeSize);
        items.Enqueue(uiItem);
        if (items.Count >= size)
        {
            UIInventoryItem lastItem = items.Dequeue();
            if (lastItem != null)
            {
                Debug.Log($"Hide {lastItem}");
                lastItem.Hide();
            }
        }
        Draw();
    }

    public void Draw()
    {
        foreach (UIInventoryItem item in items)
        {
            RectTransform rt = item.GetComponent<RectTransform>();
            rt.SetAsFirstSibling();
        }
    }
}
