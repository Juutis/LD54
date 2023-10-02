using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIInventoryItem : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField]
    private Image imgBg;
    [SerializeField]
    private Image imgIcon;

    [SerializeField]
    private Color highlightColor;
    [SerializeField]
    private Color ghostBgColor;
    [SerializeField]
    private Color blockedColor;
    [SerializeField]
    private Color placeableColor;
    [SerializeField]
    private Color ghostIconColor;
    private Color originalColor;

    [SerializeField]
    private Transform container;
    [SerializeField]
    private Text stackCounterText;

    private InventoryItem inventoryItem;

    public InventoryItem InventoryItem { get { return inventoryItem; } }

    private bool isHovered = false;
    public bool IsHovered { get { return isHovered; } }
    private bool isDragging = false;
    public bool IsDragging { get { return isDragging; } }

    private Vector2 positionAtDragStart;
    private Vector2 dragOffset;
    bool isGhost = false;

    [SerializeField]
    private UIInventoryItemShapePart shapePartPrefab;

    [SerializeField]
    private Transform shapePartContainer;

    private List<UIInventoryItemShapePart> shapeParts = new();

    private ItemPlacement lastPlacement;

    private Vector2 nodeSize;

    private bool isBufferItem = false;
    public bool IsBufferItem { get { return isBufferItem; } }
    int shapeWidth = 1;
    int shapeHeight = 1;

    [SerializeField]
    float alphaWhenDragging = 0.5f;
    private Color originalIconColor;
    public void InitializeAsBufferItem(InventoryItem item, Vector2 nodeSize, bool isGhost = false)
    {
        isBufferItem = true;
        Initialize(item, nodeSize, isGhost);
        RectTransform rt = GetComponent<RectTransform>();
        rt.anchoredPosition = new Vector2(nodeSize.x, nodeSize.y);
        RectTransform containerRt = shapePartContainer.GetComponent<RectTransform>();
        if (!isGhost)
        {
            containerRt.localScale = new Vector2(1.0f / shapeWidth, 1.0f / shapeHeight);
        }
    }
    public void Initialize(InventoryItem item, Vector2 nodeSize, bool isGhost = false)
    {
        this.nodeSize = nodeSize;
        inventoryItem = item;
        originalColor = imgBg.color;
        originalIconColor = imgIcon.color;
        imgIcon.sprite = item.Sprite;
        RectTransform rt = GetComponent<RectTransform>();
        if (!isBufferItem)
        {
            rt.anchoredPosition = new Vector2(item.Node.X * nodeSize.x, -item.Node.Y * nodeSize.y);
        }
        rt.sizeDelta = nodeSize;
        this.isGhost = isGhost;
        string ghost = isGhost ? "-Ghost" : "";
        name = $"UI-Item{ghost} {item}";
        Draw();
        if (isGhost)
        {
            imgBg.color = ghostBgColor;
            imgIcon.color = ghostIconColor;
            stackCounterText.enabled = false;
        }

        if (item.Shape.ShapeType != InventoryShapeType.Single)
        {
            stackCounterText.enabled = false;
        }
        if (!isBufferItem)
        {
            var maxDimension = Mathf.Max(shapeWidth, shapeHeight);
            imgIcon.transform.localScale = new Vector2(maxDimension, maxDimension);
        }
    }

    public void Kill()
    {
        Destroy(gameObject);
    }
    public void Hide()
    {
        Destroy(gameObject);
    }

    private void Draw()
    {
        RectTransform containerRt = shapePartContainer.GetComponent<RectTransform>();
        RectTransform iconRt = imgIcon.GetComponent<RectTransform>();
        containerRt.sizeDelta = nodeSize;
        iconRt.sizeDelta = nodeSize;
        foreach (Transform child in shapePartContainer)
        {
            Destroy(child.gameObject);
        }
        shapeParts = new();
        int[,] positions = inventoryItem.Shape.Positions;
        shapeHeight = inventoryItem.Shape.Positions.GetLength(0);
        shapeWidth = inventoryItem.Shape.Positions.GetLength(1);
        for (int row = 0; row < shapeHeight; row += 1)
        {
            for (int col = 0; col < shapeWidth; col += 1)
            {
                int position = positions[row, col];
                if (position == 1)
                {
                    UIInventoryItemShapePart shapePart = Instantiate(shapePartPrefab, shapePartContainer);
                    if (isGhost)
                    {
                        shapePart.Initialize(placeableColor, nodeSize);
                    }
                    else
                    {
                        shapePart.Initialize(inventoryItem.Color, nodeSize);

                    }
                    shapePart.transform.localPosition = new Vector2(col * nodeSize.x, -row * nodeSize.y);
                    shapeParts.Add(shapePart);
                }
            }
        }
    }

    public void Highlight()
    {
        foreach (UIInventoryItemShapePart shapePart in shapeParts)
        {
            shapePart.SetColor(highlightColor);
        }

        string price = InventoryManager.main.GetItemPrice(inventoryItem);
        string lore = InventoryManager.main.GetItemLore(inventoryItem);
        Debug.Log($"highlighted {inventoryItem.Name} price: {price}");
        UIInventoryManager.main.ShowTooltip(inventoryItem, lore, price);
        isHovered = true;
    }

    public void Unhighlight()
    {
        foreach (UIInventoryItemShapePart shapePart in shapeParts)
        {
            shapePart.SetColor(inventoryItem.Color);
        }
        isHovered = false;
    }


    public void HighlightAsPlaceable()
    {
        foreach (UIInventoryItemShapePart shapePart in shapeParts)
        {
            shapePart.SetColor(placeableColor);
        }
    }

    public void HighlightAsBlocked()
    {
        foreach (UIInventoryItemShapePart shapePart in shapeParts)
        {
            shapePart.SetColor(blockedColor);
        }

    }

    public void StartDrag()
    {
        isDragging = true;
        positionAtDragStart = transform.position;
        dragOffset = positionAtDragStart - new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        imgIcon.color = new Color(imgIcon.color.r, imgIcon.color.g, imgIcon.color.b, alphaWhenDragging);
        imgBg.color = new Color(imgBg.color.r, imgBg.color.g, imgBg.color.b, alphaWhenDragging);
        foreach (UIInventoryItemShapePart shapePart in shapeParts)
        {
            shapePart.DisableRaycastTarget();
        }
    }

    public void EndDrag()
    {
        isDragging = false;
        foreach (UIInventoryItemShapePart shapePart in shapeParts)
        {
            shapePart.EnableRaycastTarget();
        }
        if (UIInventoryManager.main.DisposalIsHovered())
        {
            if (isBufferItem)
            {
                UIInventoryManager.main.RemoveBufferItem(this);
            }
            else
            {

                UIInventoryManager.main.RemoveItem(this);
            }
            InventoryManager.main.RemoveItem(inventoryItem);
            UIInventoryManager.main.UnhighlightDisposal();
            UIInventoryManager.main.HideGhost();
            return;
        }
        UIInventoryManager.main.HideGhost();
        //Debug.Log($"Lastplacement: {lastPlacement.Success}");
        if (lastPlacement.Success)
        {
            //Debug.Log($"Lastplacement: {lastPlacement.Success} ({lastPlacement.Nodes.First()})");
            UIInventoryManager.main.RemoveItem(this);
            InventoryManager.main.MoveItem(inventoryItem, lastPlacement);
        }
        transform.position = positionAtDragStart;
        imgIcon.color = originalColor;
        imgBg.color = originalIconColor;
    }


    public void OnPointerEnter(PointerEventData pointerEventData)
    {
        if (isGhost)
        {
            return;
        }
        Highlight();
    }
    public void OnPointerExit(PointerEventData pointerEventData)
    {
        if (isGhost)
        {
            return;
        }
        Unhighlight();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (isGhost)
        {
            return;
        }
        if (isDragging)
        {
            EndDrag();
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (isGhost)
        {
            return;
        }
        if (isHovered)
        {
            StartDrag();
        }
    }

    void Update()
    {
        if (isGhost)
        {
            return;
        }
        if (isDragging)
        {
            HandleDragging();
        }

        stackCounterText.text = $"x{inventoryItem.StackCount}";
    }

    void HandleDragging()
    {
        if (isBufferItem)
        {
            transform.position = Input.mousePosition + new Vector3(-1f, 1f, 0f);
        }
        else
        {
            transform.position = Input.mousePosition + new Vector3(dragOffset.x, dragOffset.y, 0f);
        }
        Vector2 moveDelta = (Vector2)transform.position - positionAtDragStart;
        lastPlacement = UIInventoryManager.main.ShowGhost(this);
        if (UIInventoryManager.main.DisposalIsHovered())
        {
            UIInventoryManager.main.HighlightDisposal();
        }

        //Debug.Log(moveDelta);
    }
}
