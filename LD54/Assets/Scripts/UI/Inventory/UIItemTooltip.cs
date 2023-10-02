using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIItemTooltip : MonoBehaviour
{
    [SerializeField]
    private Text itemName;
    [SerializeField]
    private Text itemDescription;
    [SerializeField]
    private Text itemPrice;
    [SerializeField]
    private Image itemIcon;
    [SerializeField]
    private Text itemRarity;

    // Start is called before the first frame update
    void Start()
    {
        foreach (Transform t in transform)
        {
            t.gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Show(InventoryItem item, string lore, string price)
    {
        foreach (Transform t in transform)
        {
            t.gameObject.SetActive(true);
        }
        itemName.text = item.Name;
        itemDescription.text = lore;
        itemPrice.text = price;
        itemIcon.sprite = item.Sprite;
        itemRarity.text = item.Rarity.ToString();
        itemRarity.color = item.Color;
    }

    public void Hide()
    {
        foreach (Transform t in transform)
        {
            t.gameObject.SetActive(false);
        }
    }
}
