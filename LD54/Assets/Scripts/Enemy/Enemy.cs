using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEditor.Progress;

public class Enemy : MonoBehaviour
{
    List<LootItem> lootItems = new();

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void AddItems(List<LootItem> items)
    {
        lootItems.AddRange(items);

        items.ForEach(x => x.transform.parent = transform);
    }
    public void AddItem(LootItem item)
    {
        lootItems.Add(item);
        item.transform.parent = transform;
    }

    public int ItemCount()
    {
        return lootItems.Count;
    }

    public bool Loot()
    {
        if (lootItems.Count == 0)
        {
            return false;
        }

        LootItem item = lootItems.Last();
        item.transform.parent = null;
        lootItems.Remove(item);
        return true;
    }
}
