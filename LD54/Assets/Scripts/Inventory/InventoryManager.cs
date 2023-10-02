using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{

    public static InventoryManager main;

    void Awake()
    {
        main = this;
    }

    [SerializeField]
    private char emptyInventorySlotCharacter = '▓';
    [SerializeField]
    private char lockedInventorySlotCharacter = '░';
    [SerializeField]
    private string itemCharacters = "abcdefghijklmnopqrstuvwxyz1234567890";
    [TextArea(20, 20)]
    public string inventoryDebug = "";
    ItemInventory inventory;

    [SerializeField]
    private int width = 15;
    [SerializeField]
    private int height = 8;

    [SerializeField]
    private UpgradeConfig upgradeConfig;

    // Dictionary for known item prices (those that have been sold)
    Dictionary<string, float> itemPriceDict = new();

    void Start()
    {
        InitInventory();
    }

    private void InitInventory()
    {
        List<Vector2Int> openSlots = new();
        inventory = new ItemInventory(width, height, emptyInventorySlotCharacter, lockedInventorySlotCharacter, itemCharacters, openSlots);
        inventoryDebug = inventory.ToString();
        UpgradeInventorySize(upgradeConfig);
    }

    public void InventoryUpgrade(UpgradeConfig upgrade)
    {
        if (upgrade.Type == UpgradeType.Inventory)
        {
            UpgradeInventorySize(upgrade);
        }
        else if (upgrade.Type == UpgradeType.Buffer)
        {
            UIInventoryManager.main.BufferSizeUpgrade(upgrade);
        }
    }

    private void UpgradeInventorySize(UpgradeConfig upgrade)
    {
        List<RectInt> openAreas = upgrade.InventoryAreas;
        List<Vector2Int> openSlots = new();
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                foreach (RectInt area in openAreas)
                {
                    if (
                        x >= area.x &&
                        x < area.x + area.width &&
                        y >= area.y &&
                        y < area.y + area.height
                    )
                    {
                        openSlots.Add(new Vector2Int(x, y));
                    }
                }
            }
        }

        inventory.SetOpenSlots(openSlots);
    }

    public void MoveItem(InventoryItem item, ItemPlacement placement)
    {
        inventory.MoveItem(item, placement);
    }

    public ItemPlacement GetItemPlacement(InventoryItem item, int startY, int startX, bool forceNoStack = true)
    {
        return inventory.GetItemPlacement(item, startY, startX, forceNoStack);
    }

    public bool AddItem(LootItemData lootData)
    {
        return inventory.AddItem(lootData);
    }

    public void RemoveItem(InventoryItem item)
    {
        inventory.RemoveItem(item);
    }

    public void SetItemPrice(InventoryItem item, float price)
    {
        if (!itemPriceDict.ContainsKey(item.ItemKey()))
        {
            itemPriceDict.Add(item.ItemKey(), price);
        }
    }

    public string GetItemPrice(InventoryItem item)
    {
        if (itemPriceDict.ContainsKey(item.ItemKey()))
        {
            float itemPrice = itemPriceDict[item.ItemKey()];
            float stackRarityPrice = item.StackCount * itemPrice * item.RarityScale;
            return stackRarityPrice.ToString();
        }

        return "???";
    }

    public string GetItemLore(InventoryItem item)
    {
        if (itemPriceDict.ContainsKey(item.ItemKey()))
        {
            return item.Lore;
        }

        return "Sell this item to learn it's description and price";
    }

    public void UpdateDebug()
    {
        if (inventory == null)
        {
            return;
        }
        inventoryDebug = inventory.ToString();
    }

    public float GetInventoryPrice()
    {
        return inventory.GetInventoryPrice();
    }

    public void EmptyInventory()
    {
        Debug.Log("EmptyInv");
        foreach (InventoryItem item in inventory.GetItems())
        {
            if (!itemPriceDict.ContainsKey(item.ItemKey()))
            {
                itemPriceDict.Add(item.ItemKey(), item.ItemPrice);
            }
        }
        inventory.EmptyInventory();
    }

    public static Color GetRarityColor(LootRarity rarity) => rarity switch
    {
        LootRarity.Common => Color.gray,
        LootRarity.Uncommon => Color.white,
        LootRarity.Rare => Color.blue,
        LootRarity.Legendary => Color.red + Color.yellow,
        _ => Color.magenta
    };


    void Update()
    {
        //Debug.Log($"You have {inventory.GetInventoryPrice()} monies");
    }
}
