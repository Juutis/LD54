using System.Collections.Generic;

public class ItemInventory
{
    private int itemIndex = 0;
    private InventoryGrid grid;
    private readonly int width;
    private readonly int height;
    private readonly char emptyChar;
    private List<InventoryItem> inventoryItems = new();
    private readonly char[] itemCharacterSet;

    public ItemInventory(int width, int height, char emptyChar, string itemCharacters)
    {
        itemCharacterSet = itemCharacters.ToCharArray();
        this.emptyChar = emptyChar;
        this.width = width;
        this.height = height;
        Initialize();
    }

    private void Initialize()
    {
        inventoryItems = new();
        grid = new InventoryGrid(width, height, emptyChar);

        InventoryItem ring = CreateItem("Ring", InventoryShapeType.Single);
        InventoryItem lStick = CreateItem("LStick", InventoryShapeType.L);
        InventoryItem box = CreateItem("Box", InventoryShapeType.Square2x2);
        InventoryItem bigBox = CreateItem("BigBox", InventoryShapeType.Square4x4);
        InventoryItem donut = CreateItem("Donut", InventoryShapeType.Donut3x3);

        grid.InsertItemRandomly(ring);
        grid.InsertItemRandomly(lStick);
        grid.InsertItemRandomly(box);
        grid.InsertItemRandomly(bigBox);
        grid.InsertItemRandomly(donut);
    }

    public InventoryItem CreateItem(string name, InventoryShapeType shapeType)
    {
        ItemIdentity identity = new(name, itemCharacterSet[itemIndex], itemIndex);
        InventoryItem inventoryItem = new(InventoryShapes.Shapes[shapeType], identity);
        itemIndex += 1;
        inventoryItems.Add(inventoryItem);
        return inventoryItem;
    }

    public override string ToString()
    {
        return grid.ToString();
    }
}