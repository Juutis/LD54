using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIInventoryNode : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField]
    private Transform itemContainer;
    [SerializeField]
    private Image imgBg;
    [SerializeField]
    private Color highlightColor;
    [SerializeField]
    private Color openColor;
    [SerializeField]
    private Color lockedColor;
    private Color originalColor;
    private bool isHighlighted = false;
    public bool IsHighlighted { get { return isHighlighted; } }

    private int x = 0;
    private int y = 0;
    public int X { get { return x; } }
    public int Y { get { return y; } }
    private bool isBufferNode = false;

    public void Initialize(int row, int col, Vector2 size, bool isBufferNode = false)
    {
        this.isBufferNode = isBufferNode;
        name = $"UINode [Y: {row}][X: {col}]";
        y = row;
        x = col;
        imgBg.color = lockedColor;
        originalColor = imgBg.color;
        RectTransform rt = GetComponent<RectTransform>();
        rt.sizeDelta = size;
    }

    public void Highlight(Color color)
    {
        if (isBufferNode)
        {
            return;
        }
        imgBg.color = color;
        isHighlighted = true;
    }
    public void Highlight()
    {
        if (isBufferNode)
        {
            return;
        }
        imgBg.color = highlightColor;
        isHighlighted = true;
    }

    public void Unhighlight()
    {
        if (isBufferNode)
        {
            return;
        }
        imgBg.color = originalColor;
        isHighlighted = false;
    }

    public void Open()
    {
        if (isBufferNode)
        {
            return;
        }
        imgBg.color = openColor;
        originalColor = imgBg.color;
    }

    public void Close()
    {
        imgBg.color = lockedColor;
        originalColor = imgBg.color;
    }

    public void OnPointerEnter(PointerEventData pointerEventData)
    {
        if (isBufferNode)
        {
            return;
        }
        Highlight();
    }
    public void OnPointerExit(PointerEventData pointerEventData)
    {
        if (isBufferNode)
        {
            return;
        }
        Unhighlight();
    }
}
