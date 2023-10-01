using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIInventoryItemShapePart : MonoBehaviour
{
    [SerializeField]
    private Image imgBg;

    public void Initialize(Color color, Vector2 nodeSize)
    {
        RectTransform rt = GetComponent<RectTransform>();
        rt.sizeDelta = nodeSize;
        SetColor(color);
    }

    public void SetColor(Color color)
    {
        imgBg.color = color;
    }
}
