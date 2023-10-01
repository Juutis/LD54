using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "InventoryUpgrade", menuName = "ScriptableObjects/InventoryUpgrade", order = 1)]
public class UpgradeConfig : ScriptableObject
{
    public UpgradeType Type;
    public int Price;

    public List<UpgradeConfig> Requirements;

    [Header("Inventory upgrade")]
    public List<RectInt> InventoryAreas;
    [Header("Buffer upgrade")]
    public int BufferLength;
}

public enum UpgradeType {
    Inventory,
    Buffer,
    StackAllButton,
    AutoStacker,
    JunkRemoveButton, // TODO
    AutoJunkRemover, // TODO
    FasterDeleteItem // TODO
}
