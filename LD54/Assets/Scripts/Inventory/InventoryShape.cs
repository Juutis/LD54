using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;

public enum InventoryShapeType
{
    Single,
    Square2x2,
    AlmostSquare3x3,
    Square3x3,
    Square4x4,
    L,
    Donut3x3,
    Row2x1,
    InverseT,
    Pants,
    WideTopNarrowBottom3x3,
    Row3x2WithHole,
    Cross3x3,
    InverseL,
    Triangle
}

public static class InventoryShapes
{
    public static Dictionary<InventoryShapeType, int[,]> ShapeArrays = new()
    {
        {InventoryShapeType.Single, new int [,] {{1}}},
        {InventoryShapeType.L, new int [,] {
            {0,1},
            {0,1},
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
        {InventoryShapeType.AlmostSquare3x3, new int [,] {
            {0,1,1},
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
            {0,0,1,0,0},
            {0,0,1,0,0},
            {0,0,1,0,0},
            {1,0,1,0,1},
            {1,1,1,1,1}
        }},
        {InventoryShapeType.Pants, new int [,] {
            {1,1,1,1,1},
            {1,1,1,1,1},
            {1,1,0,1,1},
            {1,1,0,1,1},
            {1,1,0,1,1}
        }},
        {InventoryShapeType.WideTopNarrowBottom3x3, new int [,] {
            {1,1,1},
            {1,1,1},
            {0,1,0},
        }},
        {InventoryShapeType.Row3x2WithHole, new int [,] {
            {1,0,1},
            {1,1,1}
        }},
        {InventoryShapeType.Cross3x3, new int [,] {
            {0,1,0},
            {1,1,1},
            {0,1,0},
        }},
        {InventoryShapeType.InverseL, new int [,] {
            {1,1},
            {1,0},
        }},
        {InventoryShapeType.Triangle, new int [,] {
            {1,1,1},
            {1,1,0},
            {1,0,0},
        }},
    };

    public static Dictionary<InventoryShapeType, InventoryShape> Shapes =
        Enum.GetValues(typeof(InventoryShapeType))
        .OfType<InventoryShapeType>()
        .ToDictionary(it => it, it => new InventoryShape(it));
}

public class InventoryShape
{
    private int[,] positions;
    public int[,] Positions { get { return positions; } }

    private readonly InventoryShapeType shapeType;
    public InventoryShapeType ShapeType { get { return shapeType; } }

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