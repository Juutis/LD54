using System.Collections;
using System.Collections.Generic;

public enum InventoryShapeType
{
    Single,
    Square2x2,
    Square3x3,
    Square4x4,
    L,
    Donut3x3,
    Row2x1,
    InverseT,
    Pants
}

public static class InventoryShapes
{
    public static Dictionary<InventoryShapeType, int[,]> ShapeArrays = new()
    {
        {InventoryShapeType.Single, new int [,] {{1}}},
        {InventoryShapeType.L, new int [,] {
            {1,0},
            {1,0},
            {1,1}
        }},
        {InventoryShapeType.Square2x2, new int [,] {
            {1,1},
            {1,1}
        }},
        {InventoryShapeType.Square3x3, new int [,] {
            {1,1,1},
            {1,1,1},
            {1,1,1}
        }},
        {InventoryShapeType.Square4x4, new int [,] {
            {1,1,1,1},
            {1,1,1,1},
            {1,1,1,1},
            {1,1,1,1}
        }},
        {InventoryShapeType.Donut3x3, new int [,] {
            {1,1,1,},
            {1,0,1,},
            {1,1,1,},
        }},
        {InventoryShapeType.Row2x1, new int [,] {
            {1,1}
        }},
        {InventoryShapeType.InverseT, new int [,] {
            {0,1,0,},
            {0,1,0,},
            {1,1,1,}
        }},
        {InventoryShapeType.Pants, new int [,] {
            {1,1,1,},
            {1,0,1,},
            {1,0,1,}
        }}
    };

    public static Dictionary<InventoryShapeType, InventoryShape> Shapes = new()
    {
        {InventoryShapeType.Single, new InventoryShape(InventoryShapeType.Single)},
        {InventoryShapeType.L, new InventoryShape(InventoryShapeType.L)},
        {InventoryShapeType.Square2x2, new InventoryShape(InventoryShapeType.Square2x2)},
        {InventoryShapeType.Square3x3, new InventoryShape(InventoryShapeType.Square3x3)},
        {InventoryShapeType.Square4x4, new InventoryShape(InventoryShapeType.Square4x4)},
        {InventoryShapeType.Donut3x3, new InventoryShape(InventoryShapeType.Donut3x3)},
        {InventoryShapeType.Row2x1, new InventoryShape(InventoryShapeType.Row2x1)},
        {InventoryShapeType.InverseT, new InventoryShape(InventoryShapeType.InverseT)},
        {InventoryShapeType.Pants, new InventoryShape(InventoryShapeType.Pants)},
    };
}

public class InventoryShape
{
    private int[,] positions;
    public int[,] Positions { get { return positions; } }

    private readonly InventoryShapeType shapeType;

    public InventoryShape(InventoryShapeType shapeType)
    {
        positions = InventoryShapes.ShapeArrays[shapeType];
        this.shapeType = shapeType;
    }

    public override string ToString()
    {
        return $"{shapeType}";
    }

}