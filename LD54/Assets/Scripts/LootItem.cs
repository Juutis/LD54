using System;
using UnityEngine;
using UnityEngine.Events;

public class LootItem : MonoBehaviour
{
    private LootItemData lootData;
    private SpriteRenderer spriteRenderer;
    private bool thrown = false;
    private float lerp_t = 0f;
    private Transform start;
    private Transform end;

    public LootItemData LootData { get { return lootData; } }

    private UnityAction afterCallback;

    public void Initialize(LootItemData data)
    {
        lootData = data;
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (thrown)
        {
            lerp_t += Time.deltaTime * 1f;
            lerp_t = Mathf.Min(lerp_t, 1f);
            float lerp_t2 = lerp_t * lerp_t;

            transform.position = new Vector2(
                Mathf.Lerp(start.position.x, end.position.x, lerp_t),
                Mathf.LerpUnclamped(start.position.y, end.position.y, 1f - 8f * (0.625f * lerp_t2 - 0.5f * lerp_t))
            );

            transform.localScale = new Vector2(
                Mathf.Lerp(0.5f, 0.75f, lerp_t),
                Mathf.Lerp(0.5f, 0.75f, lerp_t)
            );

            if (Mathf.Approximately(lerp_t, 1) || lerp_t > 1)
            {
                thrown = false;
                if (afterCallback != null)
                {
                    afterCallback();
                }
                afterCallback = null;
                Hide();
            }
        }
    }

    public void Hide()
    {
        spriteRenderer.enabled = false;
    }

    public void Throw(Transform start, Transform end, UnityAction after)
    {
        lerp_t = 0f;
        afterCallback = after;
        spriteRenderer.enabled = true;
        spriteRenderer.sprite = lootData.Sprite;
        this.start = start;
        this.end = end;
        this.thrown = true;
    }
}

[Serializable]
public class LootItemData
{
    public LootRarity Rarity { get; set; }
    public string Prefix { get; set; }
    public float PriceScale { get; set; }
    public ItemTier Tier { get; set; }
    public string LootName { get; set; }
    public float BasePrice { get; set; }
    public Sprite Sprite { get; set; }
    public InventoryShapeType Shape { get; set; }
    public string Lore { get; set; }

    public LootItemData(RarityConfig rarityConfig, BaseLootConfig lootConfig)
    {
        Rarity = rarityConfig.Rarity;
        Prefix = rarityConfig.Prefixes[UnityEngine.Random.Range(0, rarityConfig.Prefixes.Count)];
        PriceScale = rarityConfig.PriceScale;

        Tier = lootConfig.Tier;
        Shape = lootConfig.Shape;
        BasePrice = lootConfig.BasePrice;
        Sprite = lootConfig.Sprites[UnityEngine.Random.Range(0, lootConfig.Sprites.Count)];
        Shape = lootConfig.Shape;
        Lore = lootConfig.Lore;
        LootName = lootConfig.LootName;
    }
}