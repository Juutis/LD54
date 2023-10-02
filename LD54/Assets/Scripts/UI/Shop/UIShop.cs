using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

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
    Callback retry;

    [SerializeField]
    private Transform container;

    [SerializeField]
    private UIGoldDisplay uiGoldDisplay;
    [SerializeField]
    private UpgradeConfig testUpgrade;
    [SerializeField]
    private UILevelNameDisplay uiLevelNameDisplay;

    [SerializeField]
    private List<UIShopUpgrade> uiShopUpgrades = new();

    private bool isShown = false;
    private bool itemsSold = false;

    [SerializeField]
    private Button nextQuestButton;
    [SerializeField]
    private Button retryQuestButton;
    [SerializeField]
    private GameObject nextQuestRequirementNotification;
    [SerializeField]
    private TMP_Text nextQuestRequirementText;

    public void Show(Callback afterFinish, Callback retry)
    {
        itemsSold = false;
        Time.timeScale = 0f;
        container.gameObject.SetActive(true);
        shopFinished = afterFinish;
        this.retry = retry;
        animator.SetTrigger("Show");
        MusicManager.main.SwitchMusic(true);
    }

    public void Hide()
    {
        nextQuestRequirementNotification.SetActive(false);
        animator.SetTrigger("Hide");
        retryPrevious = false;
    }

    private bool retryPrevious = false;

    public void RetryPrevious()
    {
        nextQuestRequirementNotification.SetActive(false);
        animator.SetTrigger("Hide");
        retryPrevious = true;
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
        if (retryPrevious) {
            retry();
        } else {
            shopFinished();
        }
        MusicManager.main.SwitchMusic(false);
    }
    public void AnimationCallHideSellOverlayFinish()
    {
        itemsSold = true;
    }

    public void SellInventory()
    {
        if (itemsSold) {
            Debug.Log("Items already sold!");
        }
        if (!isShown) {
            Debug.Log("Trying to sell without showing UI!");
        }
        if (!isShown || itemsSold)
        {
            return;
        }
        float inventoryValue = InventoryManager.main.GetInventoryPrice();
        InventoryManager.main.EmptyInventory();
        InventoryManager.main.EmptyBuffer();
        int gainedGold = Mathf.FloorToInt(inventoryValue);
        UpdateGold(gainedGold);
        animator.SetTrigger("HideSellOverlay");

        var goldRequirement = GameManager.Main.currentLevel.TargetGold;
        if (gainedGold >= goldRequirement) {
            nextQuestButton.interactable = true;
            nextQuestRequirementNotification.SetActive(false);
        } else {
            nextQuestButton.interactable = false;
            nextQuestRequirementNotification.SetActive(true);
            nextQuestRequirementText.text = "You gained " + gainedGold + " gold.\nGain at least " + goldRequirement + " gold in a single run to unlock the next quest.";
        }
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

    public void SetLevelName(string levelName)
    {
        uiLevelNameDisplay.SetLevelName(levelName);
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
                },delegate
                {
                    Debug.Log("Retry previous");
                });
            }
            else
            {
                Hide();
            }
        }

    }

}
