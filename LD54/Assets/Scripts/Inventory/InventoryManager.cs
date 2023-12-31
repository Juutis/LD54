using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
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
    private List<UpgradeConfig> upgradeConfigs;

    private bool autoStacker = false;
    private bool autoJunkRemoval = false;

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
        foreach (var upgrade in upgradeConfigs)
        {
            InventoryUpgrade(upgrade);
            GameManager.Main.PlayerProgress.AddUpgrade(upgrade);
        }
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
        else if (upgrade.Type == UpgradeType.JunkRemoveButton)
        {
            UIInventoryManager.main.EnableJunkRemoveButton();
        }
        else if (upgrade.Type == UpgradeType.AutoJunkRemover)
        {
            autoJunkRemoval = true;
        }
        else if (upgrade.Type == UpgradeType.StackAllButton)
        {
            UIInventoryManager.main.EnableStackAllButton();
        }
        else if (upgrade.Type == UpgradeType.AutoStacker)
        {
            autoStacker = true;
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

    public void MoveItem(InventoryItem item, ItemPlacement placement, bool isBufferItem = false)
    {
        inventory.MoveItem(item, placement, isBufferItem);
    }

    public ItemPlacement GetItemPlacement(InventoryItem item, int startY, int startX, bool forceNoStack = true)
    {
        return inventory.GetItemPlacement(item, startY, startX, forceNoStack);
    }

    public ItemInsertResult AddItem(LootItemData lootData)
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
            return Mathf.FloorToInt(item.ItemPrice * item.RarityScale) + "";
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

    public void EmptyBuffer()
    {
        Debug.Log("EmptyBuffer");
        inventory.EmptyBuffer();
    }

    public static Color GetRarityColor(LootRarity rarity) => rarity switch
    {
        LootRarity.Common => Color.gray - new Color(0.0f, 0.0f, 0.0f, 0.5f),
        LootRarity.Uncommon => Color.green - new Color(0.0f, 0.0f, 0.0f, 0.5f),
        LootRarity.Rare => Color.blue - new Color(0.0f, 0.0f, 0.0f, 0.5f),
        LootRarity.Legendary => new Color(1.0f, 0.5f, 0.0f, 0.5f),
        _ => Color.magenta
    };

    private bool upgradeTriggered = false;

    void Update()
    {
        if (upgradeTriggered) return;
        foreach (var upgrade in upgradeConfigs)
        {
            InventoryUpgrade(upgrade);
            GameManager.Main.PlayerProgress.AddUpgrade(upgrade);
        }
        upgradeTriggered = true;
    }


    public void StackSingles()
    {
        //inventory.StackSingles();
        StackStackables();
    }

    public void StackStackables()
    {
        inventory.StackStackables();
    }

    public void DeleteJunk()
    {
        inventory.DeleteJunk();
    }
}
