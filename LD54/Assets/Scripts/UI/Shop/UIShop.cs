using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public delegate void Callback();
public class UIShop : MonoBehaviour
{

    public static UIShop main;
    void Awake()
    {
        main = this;
    }

    [SerializeField]
    private Animator animator;
    Callback shopFinished;

    [SerializeField]
    private Transform container;

    [SerializeField]
    private UIGoldDisplay uiGoldDisplay;
    [SerializeField]
    private UpgradeConfig testUpgrade;

    [SerializeField]
    private List<UIShopUpgrade> uiShopUpgrades = new();

    private bool isShown = false;
    private bool itemsSold = false;

    public void Show(Callback afterFinish)
    {
        itemsSold = false;
        Time.timeScale = 0f;
        container.gameObject.SetActive(true);
        shopFinished = afterFinish;
        animator.SetTrigger("Show");
        MusicManager.main.SwitchMusic(true);
    }

    public void Hide()
    {
        animator.SetTrigger("Hide");
    }

    public void AnimationCallShowFinish()
    {
        isShown = true;
    }

    public void AnimationCallHideFinish()
    {
        Time.timeScale = 1f;
        isShown = false;
        container.gameObject.SetActive(false);
        shopFinished();
        MusicManager.main.SwitchMusic(false);
    }
    public void AnimationCallHideSellOverlayFinish()
    {
        itemsSold = true;
    }


    public void SellInventory()
    {
        if (!isShown || itemsSold)
        {
            return;
        }
        float inventoryValue = InventoryManager.main.GetInventoryPrice();
        InventoryManager.main.EmptyInventory();
        int gainedGold = Mathf.FloorToInt(inventoryValue);
        UpdateGold(gainedGold);
        animator.SetTrigger("HideSellOverlay");
    }

    public void BuyUpgrade(UpgradeConfig upgrade)
    {
        if (!isShown || !itemsSold)
        {
            return;
        }
        if (GameManager.Main.PlayerProgress.CanBuy(upgrade))
        {
            Debug.Log($"Bought upgrade {upgrade.name}.");
            UpdateGold(-upgrade.Price);
            AddUpgrade(upgrade);
        }
        else
        {
            Debug.Log("Not enough money / missing required upgrade.");
        }
    }

    public void AddUpgrade(UpgradeConfig upgrade)
    {
        GameManager.Main.PlayerProgress.AddUpgrade(upgrade);
        InventoryManager.main.InventoryUpgrade(upgrade);
    }

    public void UpdateGold(int change)
    {
        GameManager.Main.PlayerProgress.UpdateGold(change);
        uiGoldDisplay.UpdateValue(change);
        foreach (UIShopUpgrade upgrade in uiShopUpgrades)
        {
            upgrade.UpdateStatus();
        }
    }


    void Update()
    {

        if (Input.GetKeyUp(KeyCode.P))
        {
            UpdateGold(50);
        }

        if (Input.GetKeyUp(KeyCode.O))
        {
            UpdateGold(-10);
        }

        if (Input.GetKeyUp(KeyCode.I))
        {
            Debug.Log("buy...");
            BuyUpgrade(testUpgrade);
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            if (!isShown)
            {
                Show(delegate
                {
                    Debug.Log("Shop closed");
                });
            }
            else
            {
                Hide();
            }
        }

    }

}
