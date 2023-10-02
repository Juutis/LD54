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

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Show(InventoryItem item, string lore, string price)
    { 
        itemName.text = item.Name;
        itemDescription.text = lore;
        itemPrice.text = price;
        itemIcon.sprite = item.Sprite;
    }
}
