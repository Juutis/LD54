using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using System.Runtime.CompilerServices;

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

    [SerializeField]
    private GameObject graphics;

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
        items = items.Where(x => x != null).ToList();
        if (items.Count > size)
        {
            UIInventoryItem lastItem = items.FirstOrDefault(x => x != null);
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
            if (item == null)
            {
                continue;
            }
            RectTransform rt = item.GetComponent<RectTransform>();
            rt.SetAsFirstSibling();
        }
    }

    public void UpdateBufferSize(int size)
    {
        if (size == 0)
        {
            graphics.SetActive(false);
            return;
        }
        else if (size <= this.size)
        {
            graphics.SetActive(true);
            return;
        }
        graphics.SetActive(true);

        int added = size - this.size;
        this.size = size;

        for (int i = 0; i < added; i++)
        {
            UIInventoryNode node = Instantiate(nodePrefab, nodeContainer);
            node.Initialize(i, 0, nodeSize);
            nodes.Add(node);
        }
    }

    public void EmptyBuffer() {
        for (int index = items.Count - 1; index >= 0; index -= 1)
        {
            UIInventoryItem item = items[index];
            RemoveItem(item);
        }
    }
}
