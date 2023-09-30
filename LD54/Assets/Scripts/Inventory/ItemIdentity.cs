public class ItemIdentity
{
    private readonly string name;

    private readonly char character;
    public char Character { get { return character; } }
    private readonly int index;
    public int Index { get { return index; } }


    public string Name { get { return name; } }

    public ItemIdentity(string name, char character, int index)
    {
        this.character = character;
        this.index = index;
        this.name = name;
    }
    public override string ToString()
    {
        return $"'{Name}' #{Index} char: '{character}'";
    }
}