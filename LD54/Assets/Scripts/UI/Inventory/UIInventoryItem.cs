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

    public void Initialize(InventoryItem item, Vector2 nodeSize, bool isGhost = false)
    {
        this.nodeSize = nodeSize;
        inventoryItem = item;
        originalColor = imgBg.color;
        imgIcon.sprite = item.Sprite;
        RectTransform rt = GetComponent<RectTransform>();
        rt.anchoredPosition = new Vector2(item.Node.X * nodeSize.x, -item.Node.Y * nodeSize.y);
        rt.sizeDelta = nodeSize;
        this.isGhost = isGhost;
        string ghost = isGhost ? "-Ghost" : "";
        name = $"UI-Item{ghost} {item}";
        Draw();
        if (isGhost)
        {
            imgBg.color = ghostBgColor;
            imgIcon.color = ghostIconColor;
        }
    }

    private void Draw()
    {
        foreach (Transform child in shapePartContainer)
        {
            Destroy(child.gameObject);
        }
        shapeParts = new();
        int[,] positions = inventoryItem.Shape.Positions;
        for (int row = 0; row < positions.GetLength(0); row += 1)
        {
            for (int col = 0; col < positions.GetLength(1); col += 1)
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
    }

    public void EndDrag()
    {
        isDragging = false;
        UIInventoryManager.main.HideGhost();
        Debug.Log($"Lastplacement: {lastPlacement.Success}");
        if (lastPlacement.Success)
        {
            Debug.Log($"Lastplacement: {lastPlacement.Success} ({lastPlacement.Nodes.First()})");
            UIInventoryManager.main.RemoveItem(this);
            InventoryManager.main.MoveItem(inventoryItem, lastPlacement);
        }
        transform.position = positionAtDragStart;
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
    }

    void HandleDragging()
    {
        transform.position = Input.mousePosition + new Vector3(dragOffset.x, dragOffset.y, 0f);
        Vector2 moveDelta = (Vector2)transform.position - positionAtDragStart;
        lastPlacement = UIInventoryManager.main.ShowGhost(this);

        //Debug.Log(moveDelta);
    }
}
