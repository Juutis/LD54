using UnityEngine;

public class UIInventoryManager : MonoBehaviour
{
    public static UIInventoryManager main;
    void Awake()
    {
        main = this;
    }

    [SerializeField]
    private UIInventoryGrid uiInventoryGrid;


    [SerializeField]
    private UIItemBuffer uiItemBuffer;
    [SerializeField]
    private UIItemDisposal uiItemDisposal;
    [SerializeField]
    private UIItemTooltip uiItemTooltip;

    public Sprite PLACEHOLDER_SPRITE;

    [SerializeField]
    private int columns = 15;
    [SerializeField]
    private int rows = 8;
    [SerializeField]
    private int bufferSize = 5;

    private bool showingGhost = false;

    private void Initialize()
    {
        Vector2 nodeSize = uiInventoryGrid.Initialize(columns, rows);
        uiItemBuffer.Initialize(bufferSize);
        uiItemDisposal.Initialize();
    }


    void Start()
    {
        Initialize();
    }

    public ItemPlacement ShowGhost(UIInventoryItem uiInventoryItem)
    {
        showingGhost = true;
        return uiInventoryGrid.ShowGhost(uiInventoryItem);
    }

    public void OpenSlot(Vector2Int slot)
    {
        uiInventoryGrid.OpenSlot(slot);
    }

    public void CloseSlot(Vector2Int slot)
    {
        uiInventoryGrid.CloseSlot(slot);
    }

    public void HideGhost()
    {
        showingGhost = false;
        uiInventoryGrid.HideGhost();
    }

    public void AddItem(InventoryItem item)
    {
        uiInventoryGrid.AddItem(item);
    }
    public void AddItemToBuffer(InventoryItem item)
    {
        uiItemBuffer.AddItem(item);
    }

    public void RemoveItem(UIInventoryItem uiInventoryItem)
    {
        uiInventoryGrid.RemoveItem(uiInventoryItem);
    }
    public void RemoveBufferItem(UIInventoryItem uiInventoryItem)
    {
        uiItemBuffer.RemoveItem(uiInventoryItem);
    }

    public UIInventoryNode ClosestNode(Vector2 position)
    {
        return uiInventoryGrid.ClosestNode(position);
    }

    public bool DisposalIsHovered()
    {
        return uiItemDisposal.IsHovered;
    }

    public void HighlightDisposal()
    {
        uiItemDisposal.Highlight();
    }
    public void UnhighlightDisposal()
    {
        uiItemDisposal.Unhighlight();
    }

    public void BufferSizeUpgrade(UpgradeConfig upgrade)
    {
        if (upgrade.Type == UpgradeType.Buffer)
        {
            uiItemBuffer.UpdateBufferSize(upgrade.BufferLength);
        }
    }

    public void ShowTooltip(InventoryItem item, string lore, string price)
    {
        uiItemTooltip.Show(item, lore, price);
    }

    public void HideTooltip()
    {
        uiItemTooltip.Hide();
    }

    public bool IsShowingGhost()
    {
        return showingGhost;
    }

    public void EmptyInventory()
    {
        uiInventoryGrid.EmptyInventory();
    }
}
