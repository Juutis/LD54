using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;


public enum UIShopUpgradeStatus
{
    Available,
    Purchased,
    Unavailable
}
public class UIShopUpgrade : MonoBehaviour
{
    private UIShopUpgradeStatus status = UIShopUpgradeStatus.Unavailable;

    [SerializeField]
    private UpgradeConfig upgrade;

    [SerializeField]
    private GameObject unavailableIndicator;
    [SerializeField]
    private GameObject purchasedIndicator;

    void Start()
    {
        Initialize();
    }

    public void Initialize()
    {
        UpdateStatus();
        Debug.Log("updateStatus");
    }

    public void UpdateStatus()
    {
        if (GameManager.Main.PlayerProgress.HasUpgrade(upgrade))
        {
            status = UIShopUpgradeStatus.Purchased;
            purchasedIndicator.SetActive(true);
            unavailableIndicator.SetActive(true);
        }
        else if (GameManager.Main.PlayerProgress.CanBuy(upgrade))
        {
            purchasedIndicator.SetActive(false);
            status = UIShopUpgradeStatus.Available;
            unavailableIndicator.SetActive(false);
        }
        else
        {
            purchasedIndicator.SetActive(false);
            unavailableIndicator.SetActive(true);
        }
    }

    public void HandleClick()
    {
        if (status == UIShopUpgradeStatus.Available)
        {
            UIShop.main.BuyUpgrade(upgrade);
            UpdateStatus();
        }
    }


}