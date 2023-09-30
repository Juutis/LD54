using System.Collections;
using System.Collections.Generic;
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
}
