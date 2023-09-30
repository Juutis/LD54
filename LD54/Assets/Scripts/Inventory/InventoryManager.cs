using UnityEngine;

public class InventoryManager : MonoBehaviour
{


    [SerializeField]
    private char emptyInventorySlotCharacter = '■';
    [SerializeField]
    private string itemCharacters = "abcdefghijklmnopqrstuvwxyz1234567890";
    [TextArea(20, 20)]
    public string inventoryDebug = "";
    ItemInventory inventory;

    [SerializeField]
    private int size = 5;
    void Start()
    {
        InitInventory();
    }

    private void InitInventory()
    {
        inventory = new ItemInventory(size, size, emptyInventorySlotCharacter, itemCharacters);
        inventoryDebug = inventory.ToString();
    }

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Space))
        {
            InitInventory();
        }
    }
}
