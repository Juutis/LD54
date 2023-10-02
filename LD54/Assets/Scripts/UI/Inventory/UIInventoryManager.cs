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
    private PoppingText poppingTextPrefab;

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

    private bool stackAllButtonEnabled = false;
    private bool junkRemoveButtonEnabled = false;
    public Transform GarbageThrowTarget;
    public Transform BufferThrowTarget;
    public Transform InventoryThrowTarget;
    public Transform ThrownTargetOrigin;
    public Transform PoppingTextLocation;

    [SerializeField]
    private UIThrownItem uiThrownItemPrefab;
    private void Initialize()
    {
        Vector2 nodeSize = uiInventoryGrid.Initialize(columns, rows);
        uiItemBuffer.Initialize(bufferSize);
        uiItemDisposal.Initialize();
        //ShowPoppingText("Test");
    }


    void Start()
    {
        Initialize();
    }

    public void ShowPoppingText(string message)
    {
        PoppingText poppingText = Instantiate(poppingTextPrefab);
        Debug.Log("Creating poppingText");
        poppingText.Show(PoppingTextLocation.transform.position, message);
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
    public bool AddItemToBuffer(InventoryItem item)
    {
        return uiItemBuffer.AddItem(item);
    }
    public bool CanAddToBuffer()
    {
        return uiItemBuffer.CanAdd();
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

    public void EmptyBuffer()
    {
        uiItemBuffer.EmptyBuffer();
    }

    public void DeleteItem(InventoryItem item)
    {
        uiInventoryGrid.RemoveItem(item);
    }

    public void EnableJunkRemoveButton()
    {
        junkRemoveButtonEnabled = true;
        DeleteJunkButton.SetActive(true);
    }

    public bool IsJunkRemoveButtonVisible()
    {
        return junkRemoveButtonEnabled;
    }

    public GameObject StackAllButton, DeleteJunkButton;

    public void EnableStackAllButton()
    {
        stackAllButtonEnabled = true;
        StackAllButton.SetActive(true);
    }

    public bool IsStackAllButtonVisible()
    {
        return stackAllButtonEnabled;
    }

    public void StackSingles()
    {
        InventoryManager.main.StackSingles();
    }

    public void DeleteJunk()
    {
        InventoryManager.main.DeleteJunk();
    }

    public void AnimateThrownItem(Sprite sprite, Vector2 target)
    {
        UIThrownItem thrownItem = Instantiate(uiThrownItemPrefab, transform);
        thrownItem.Initialize(sprite, ThrownTargetOrigin.transform.position, target);
    }
    public void AnimateThrownItem(Sprite sprite, Vector2 target, Vector2 origin)
    {
        UIThrownItem thrownItem = Instantiate(uiThrownItemPrefab, transform);
        thrownItem.Initialize(sprite, origin, target);
    }
}
