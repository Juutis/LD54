using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIItemTooltip : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI itemName;
    [SerializeField]
    private TextMeshProUGUI itemDescription;
    [SerializeField]
    private TextMeshProUGUI itemPrice;
    [SerializeField]
    private Image itemIcon;
    [SerializeField]
    private TextMeshProUGUI itemRarity;

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
        itemRarity.color = GetRarityColor(item.Rarity);
    }

    public static Color GetRarityColor(LootRarity rarity) => rarity switch
    {
        LootRarity.Common => new Color(0.6f, 0.6f, 0.6f),
        LootRarity.Uncommon => Color.green,
        LootRarity.Rare => new Color(0.5f, 0.5f, 1.0f),
        LootRarity.Legendary => new Color(1.0f, 0.5f, 0.0f, 1.0f),
        _ => Color.clear
    };

    public void Hide()
    {
        foreach (Transform t in transform)
        {
            t.gameObject.SetActive(false);
        }
    }
}
