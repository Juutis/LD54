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

    [SerializeField]
    private UIInventoryItem uiInventoryItemGhost;

    public Sprite PLACEHOLDER_SPRITE;

    [SerializeField]
    private int width = 15;
    [SerializeField]
    private int height = 8;

    private void Initialize()
    {
        uiInventoryGrid.Initialize(width, height);
    }


    void Start()
    {
        Initialize();
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
                uiInventoryItemGhost.Initialize(uiInventoryItem.InventoryItem, true);
            }
            ItemPlacement placement = InventoryManager.main.GetItemPlacement(uiInventoryItem.InventoryItem, closestNode.Y, closestNode.X);
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

    public void HideGhost()
    {
        uiInventoryItemGhost.gameObject.SetActive(false);
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
