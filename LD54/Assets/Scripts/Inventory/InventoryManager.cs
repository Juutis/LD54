using System.Collections.Generic;
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

    void Start()
    {
        InitInventory();
    }

    private void InitInventory()
    {
        List<Vector2Int> openSlots = new();
        for (var i = 0; i < 30; i++) {
            for (var j = 0; j < 15; j++) {
                openSlots.Add(new Vector2Int(i, j));
            }
        }
        inventory = new ItemInventory(width, height, emptyInventorySlotCharacter, lockedInventorySlotCharacter, itemCharacters, openSlots);
        inventoryDebug = inventory.ToString();
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

    public void UpdateDebug()
    {
        if (inventory == null)
        {
            return;
        }
        inventoryDebug = inventory.ToString();
    }

    void Update()
    {

    }
}
