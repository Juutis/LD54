using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIInventoryItemShapePart : MonoBehaviour
{
    [SerializeField]
    private Image imgBg;

    public void Initialize(Color color)
    {
        SetColor(color);
    }

    public void SetColor(Color color)
    {
        imgBg.color = color;
    }
}
