public class InventoryNode
{
    private InventoryGrid grid;
    private readonly int x;
    private readonly int y;

    private InventoryItem inventoryItem;
    //
    public bool IsEmpty { get { return inventoryItem == null; } }
    public int X { get { return x; } }
    public int Y { get { return y; } }
    public InventoryItem InventoryItem { get { return inventoryItem; } }
    public InventoryNode GetNeighbor(InventoryDirection direction)
    {
        int offsetX = 0;
        int offsetY = 0;
        switch (direction)
        {
            case InventoryDirection.Up:
                offsetY = 1;
                break;
            case InventoryDirection.Right:
                offsetX = 1;
                break;
            case InventoryDirection.Down:
                offsetY = -1;
                break;
            case InventoryDirection.Left:
                offsetX = -1;
                break;
        }

        return grid.GetNode(Y + offsetY, X + offsetX);
    }

    public bool IsEmptyOrSame(InventoryItem item)
    {
        return IsEmpty || inventoryItem.Identity.Index == item.Identity.Index;
    }

    public InventoryNode(int y, int x)
    {
        this.y = y;
        this.x = x;
    }

    public void SetItem(InventoryItem inventoryItem)
    {
        this.inventoryItem = inventoryItem;
    }

    public void Clear()
    {
        inventoryItem = null;
    }
}


public enum InventoryDirection
{
    Up,
    Right,
    Down,
    Left
}