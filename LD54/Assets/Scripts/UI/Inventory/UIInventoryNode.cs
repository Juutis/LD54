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
    private Color originalColor;
    private bool isHighlighted = false;
    public bool IsHighlighted { get { return isHighlighted; } }

    private int x = 0;
    private int y = 0;
    public int X { get { return x; } }
    public int Y { get { return y; } }

    public void Initialize(int row, int col)
    {
        name = $"UINode [Y: {row}][X: {col}]";
        y = row;
        x = col;
        originalColor = imgBg.color;
    }

    public void Highlight(Color color)
    {
        imgBg.color = color;
        isHighlighted = true;
    }
    public void Highlight()
    {
        imgBg.color = highlightColor;
        isHighlighted = true;
    }

    public void Unhighlight()
    {
        imgBg.color = originalColor;
        isHighlighted = false;
    }

    public void OnPointerEnter(PointerEventData pointerEventData)
    {
        Highlight();
    }
    public void OnPointerExit(PointerEventData pointerEventData)
    {
        Unhighlight();
    }
}