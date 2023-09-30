using UnityEngine;

public class InventoryManager : MonoBehaviour
{

    public static InventoryManager main;

    void Awake()
    {
        main = this;
    }

    [SerializeField]
    private char emptyInventorySlotCharacter = '■';
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
        inventory = new ItemInventory(width, height, emptyInventorySlotCharacter, itemCharacters);
        inventoryDebug = inventory.ToString();
    }

    public void MoveItem(InventoryItem item, ItemPlacement placement)
    {
        inventory.MoveItem(item, placement);
    }

    public ItemPlacement GetItemPlacement(InventoryItem item, int startY, int startX)
    {
        return inventory.GetItemPlacement(item, startY, startX);
    }

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Space))
        {
            InitInventory();
        }
    }
}
