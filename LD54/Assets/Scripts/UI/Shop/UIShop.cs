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
    Callback showFinished;
    Callback hideFinished;

    [SerializeField]
    private Transform container;

    [SerializeField]
    private UIGoldDisplay uiGoldDisplay;
    [SerializeField]
    private UpgradeConfig testUpgrade;

    [SerializeField]
    private List<UIShopUpgrade> uiShopUpgrades = new();

    private bool isShown = false;
    public void Show(Callback afterFinish)
    {
        Time.timeScale = 0f;
        container.gameObject.SetActive(true);
        showFinished = afterFinish;
        animator.SetTrigger("Show");
    }

    public void Hide(Callback afterFinish)
    {
        hideFinished = afterFinish;
        animator.SetTrigger("Hide");
    }

    public void AnimationCallShowFinish()
    {
        isShown = true;
        showFinished();
    }

    public void AnimationCallHideFinish()
    {
        Time.timeScale = 1f;
        isShown = false;
        container.gameObject.SetActive(false);
        hideFinished();
    }


    public void SellInventory()
    {
        if (!isShown)
        {
            return;
        }
        float inventoryValue = InventoryManager.main.GetInventoryPrice();
        InventoryManager.main.EmptyInventory();
        int gainedGold = Mathf.FloorToInt(inventoryValue);
        uiGoldDisplay.UpdateValue(gainedGold);
        GameManager.Main.PlayerProgress.UpdateGold(gainedGold);
    }

    public void BuyUpgrade(UpgradeConfig upgrade)
    {
        if (!isShown)
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
                    Debug.Log("Show finished");
                });
            }
            else
            {
                Hide(delegate
                {
                    Debug.Log("Hide finished");
                });
            }
        }

    }

}
