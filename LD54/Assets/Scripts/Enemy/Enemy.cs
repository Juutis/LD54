using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

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
            GameManager.Main.EnemyHandled(this);
            return false;
        }

        LootItem item = lootItems.Last();
        bool foundRare = lootItems.Any(lItem => lItem.LootData.Rarity == LootRarity.Rare);
        bool foundLegendary = lootItems.Any(lItem => lItem.LootData.Rarity == LootRarity.Legendary);
        bool foundUncommon = lootItems.Any(lItem => lItem.LootData.Rarity == LootRarity.Uncommon);
        if (foundLegendary)
        {

            Debug.Log("foundLegendary");
            SoundManager.main.PlaySound(GameSoundType.HeroHappy);
        }
        else if (foundRare)
        {
            Debug.Log("foundRare");
            SoundManager.main.PlaySound(GameSoundType.HeroHappy);
        }
        else if (foundUncommon)
        {
            Debug.Log("foundUncommon");
            SoundManager.main.PlaySound(GameSoundType.HeroMild);
        }
        else
        {
            SoundManager.main.PlaySound(GameSoundType.HeroGrumble);
        }
        item.transform.position = transform.position;
        item.Throw(transform, ScrollingWorld.Instance.GetSquire());
        item.transform.parent = null;
        InventoryManager.main.AddItem(item.LootData);
        lootItems.Remove(item);
        return true;
    }
}
