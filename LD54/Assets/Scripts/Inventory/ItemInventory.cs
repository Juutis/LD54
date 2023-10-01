using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ItemInventory
{
    private int itemIndex = 0;
    private InventoryGrid grid;
    private readonly int width;
    private readonly int height;
    private readonly char emptyChar;
    private readonly char lockedChar;
    private List<InventoryItem> inventoryItems = new();
    private List<InventoryItem> bufferedItems = new();
    private readonly char[] itemCharacterSet;
    private List<Vector2Int> openSlots;

    public ItemInventory(int width, int height, char emptyChar, char lockedChar, string itemCharacters, List<Vector2Int> openSlots)
    {
        itemCharacterSet = itemCharacters.ToCharArray();
        this.emptyChar = emptyChar;
        this.lockedChar = lockedChar;
        this.width = width;
        this.height = height;
        this.openSlots = openSlots;
        Initialize();
    }

    private void Initialize()
    {

        inventoryItems = new();
        bufferedItems = new();
        grid = new InventoryGrid(width, height, emptyChar, lockedChar, openSlots);
        Sprite sprite = UIInventoryManager.main.PLACEHOLDER_SPRITE;

        //InventoryItem ring = CreateItem();
        //InventoryItem ring2 = CreateItem(new LootItemData(new() { Prefixes = new() { "moi" } }, new() { Sprites = new() { sprite } }) { LootName = "Ring", Shape = InventoryShapeType.Single });
        /*InventoryItem lStick = CreateItem("LStick", InventoryShapeType.L, sprite);
        InventoryItem box = CreateItem("Box", InventoryShapeType.Square2x2, sprite);
        InventoryItem bigBox = CreateItem("BigBox", InventoryShapeType.Square4x4, sprite);
        InventoryItem donut = CreateItem("Donut", InventoryShapeType.Donut3x3, sprite);*/

        LootItemData ring = new LootItemData(new() { Prefixes = new() { "moi" } }, new() { Sprites = new() { sprite } }) { LootName = "Ring", Shape = InventoryShapeType.Single };
        LootItemData ring2 = new LootItemData(new() { Prefixes = new() { "moi" } }, new() { Sprites = new() { sprite } }) { LootName = "Ring", Shape = InventoryShapeType.Single };
        LootItemData ring3 = new LootItemData(new() { Prefixes = new() { "moi" } }, new() { Sprites = new() { sprite } }) { LootName = "Ring", Shape = InventoryShapeType.Single };
        LootItemData ring4 = new LootItemData(new() { Prefixes = new() { "moi" } }, new() { Sprites = new() { sprite } }) { LootName = "Ring", Shape = InventoryShapeType.Single };
        LootItemData ring5 = new LootItemData(new() { Prefixes = new() { "moi" } }, new() { Sprites = new() { sprite } }) { LootName = "Ring", Shape = InventoryShapeType.Single };
        LootItemData ring6 = new LootItemData(new() { Prefixes = new() { "moi" } }, new() { Sprites = new() { sprite } }) { LootName = "Ring", Shape = InventoryShapeType.Single };
        LootItemData ring7 = new LootItemData(new() { Prefixes = new() { "moi" } }, new() { Sprites = new() { sprite } }) { LootName = "pantsu", Shape = InventoryShapeType.Pants };
        AddItem(ring);
        AddItem(ring2);
        AddItem(ring3);
        AddItem(ring4);
        AddItem(ring5);
        AddItem(ring6);
        AddItem(ring7);
        //grid.InsertItemRandomly(ring);
        //grid.InsertItemRandomly(ring2);
        //UIInventoryManager.main.AddItem(ring);
        //UIInventoryManager.main.AddItem(ring2);
        /*grid.InsertItemRandomly(lStick);
        UIInventoryManager.main.AddItem(lStick);
        grid.InsertItemRandomly(box);
        UIInventoryManager.main.AddItem(box);
        grid.InsertItemRandomly(bigBox);
        UIInventoryManager.main.AddItem(bigBox);
        grid.InsertItemRandomly(donut);
        UIInventoryManager.main.AddItem(donut);*/

    }

    public void RemoveItem(InventoryItem item)
    {
        inventoryItems.Remove(item);
        grid.RemoveItem(item);
    }

    public bool AddItem(LootItemData lootData)
    {
        InventoryItem item = CreateItem(lootData);
        bool wasInserted = grid.InsertItemRandomly(item);
        if (wasInserted)
        {
            inventoryItems.Add(item);
            UIInventoryManager.main.AddItem(item);
        }
        else
        {
            Debug.Log("No space, put it somewhere else!");
            bufferedItems.Add(item);
            UIInventoryManager.main.AddItemToBuffer(item);
        }
        return wasInserted;
    }

    public ItemPlacement GetItemPlacement(InventoryItem item, int startY, int startX, bool forceNoStack = true)
    {
        return grid.GetItemPlacement(item, startY, startX, forceNoStack);
    }

    public InventoryItem CreateItem(LootItemData itemData)
    {
        char itemChar;
        if (itemIndex >= itemCharacterSet.Length)
        {
            itemChar = itemCharacterSet[itemIndex % itemCharacterSet.Length];
        }
        else
        {
            itemChar = itemCharacterSet[itemIndex];
        }

        float itemPrice = itemData.BasePrice * itemData.PriceScale;

        ItemIdentity identity = new(itemData.LootName, itemChar, itemIndex);
        InventoryItem inventoryItem = new(InventoryShapes.Shapes[itemData.Shape], identity, itemData.Sprite, itemData.LootName, itemData.Tier, itemData.Rarity, itemPrice);
        itemIndex += 1;

        return inventoryItem;
    }

    public void MoveItem(InventoryItem item, ItemPlacement placement)
    {
        grid.MoveItem(item, placement);
        if (!placement.isStacked)
        {
            UIInventoryManager.main.AddItem(item);
        }
    }

    public override string ToString()
    {
        return grid.ToString();
    }

    public void SetOpenSlots(List<Vector2Int> slots)
    {
        openSlots = slots;
        grid.SetOpenSlots(openSlots);
    }

    public float GetInventoryPrice()
    {
        return inventoryItems.Sum(x => x.ItemPrice);
    }
}