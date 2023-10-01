using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
public class UIItemBuffer : MonoBehaviour
{
    private List<UIInventoryNode> nodes = new();
    private List<UIInventoryItem> items = new();

    [SerializeField]
    private UIInventoryItem itemPrefab;
    [SerializeField]
    private UIInventoryNode nodePrefab;
    [SerializeField]
    private Transform nodeContainer;
    [SerializeField]
    private Transform itemContainer;

    [SerializeField]
    private GridLayoutGroup grid;
    Vector2 nodeSize;
    int size = 5;
    public void Initialize(int size)
    {
        this.size = size;
        nodeSize = grid.cellSize;
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

    public void RemoveItem(UIInventoryItem uiItem)
    {
        items.Remove(uiItem);
        uiItem.Hide();
    }
    public void AddItem(InventoryItem inventoryItem)
    {
        UIInventoryItem uiItem = Instantiate(itemPrefab, itemContainer);
        uiItem.InitializeAsBufferItem(inventoryItem, nodeSize);
        items.Add(uiItem);
        if (items.Count > size)
        {
            UIInventoryItem lastItem = items.FirstOrDefault();
            if (lastItem != null)
            {
                Debug.Log($"Hide {lastItem}");
                RemoveItem(lastItem);
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

    public void UpdateBufferSize(int size)
    {
        if (size == 0)
        {
            return;
        }
        else if (size <= this.size)
        {
            return;
        }

        int added = size - this.size;
        this.size = size;

        for (int i = 0; i < added; i++)
        {
            UIInventoryNode node = Instantiate(nodePrefab, nodeContainer);
            node.Initialize(i, 0, nodeSize);
            nodes.Add(node);
        }
    }
}
