using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIItemDisposal : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private bool isHovered = false;
    public bool IsHovered { get { return isHovered; } }

    [SerializeField]
    private Image imgIcon;

    private Sprite originalSprite;
    [SerializeField]
    private Sprite hoverSprite;

    public void Initialize()
    {
        originalSprite = imgIcon.sprite;
    }

    public void Highlight()
    {
        imgIcon.sprite = hoverSprite;
    }

    public void Unhighlight()
    {
        imgIcon.sprite = originalSprite;
    }



    public void OnPointerEnter(PointerEventData eventData)
    {
        isHovered = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isHovered = false;
        Unhighlight();
    }

}
