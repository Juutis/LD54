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
    public UIInventoryGrid UIInventoryGrid { get { return uiInventoryGrid; } }

    public Sprite PLACEHOLDER_SPRITE;

    [SerializeField]
    private int columns = 15;
    [SerializeField]
    private int rows = 8;

    private void Initialize()
    {
        uiInventoryGrid.Initialize(columns, rows);
    }


    void Start()
    {
        Initialize();
    }

    public ItemPlacement ShowGhost(UIInventoryItem uiInventoryItem)
    {
        return uiInventoryGrid.ShowGhost(uiInventoryItem);
    }

    public void OpenSlot(Vector2Int slot)
    {
        uiInventoryGrid.OpenSlot(slot);
    }

    public void HideGhost()
    {
        uiInventoryGrid.HideGhost();
    }

    public void AddItem(InventoryItem item)
    {
        uiInventoryGrid.AddItem(item);
    }

    public void RemoveItem(UIInventoryItem uiInventoryItem)
    {
        uiInventoryGrid.RemoveItem(uiInventoryItem);
    }

    public UIInventoryNode ClosestNode(Vector2 position)
    {
        return uiInventoryGrid.ClosestNode(position);
    }
}
