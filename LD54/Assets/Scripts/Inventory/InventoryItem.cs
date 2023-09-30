public class InventoryItem
{
    private readonly InventoryShape shape;
    public InventoryShape Shape { get { return shape; } }
    private readonly ItemIdentity identity;
    public ItemIdentity Identity { get { return identity; } }

    private InventoryNode node;
    public InventoryNode Node { get { return node; } }

    public InventoryItem(InventoryShape shape, ItemIdentity identity)
    {
        this.identity = identity;
        this.shape = shape;
    }

    public void SetNode(InventoryNode newNode)
    {
        node = newNode;
    }

    public override string ToString()
    {
        return $"Item[{identity}] with shape {shape}";
    }
}