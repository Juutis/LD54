using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;


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

    [SerializeField]
    private TextMeshProUGUI textValue;

    [SerializeField]
    private TextMeshProUGUI titleText;

    [SerializeField]
    private TextMeshProUGUI descriptionText;

    [SerializeField]
    private TextMeshProUGUI flavorText;

    private Animator anim;

    void Start()
    {
        Initialize();
    }

    public void Initialize()
    {
        textValue.text = $"{upgrade.Price}";
        UpdateStatus();
        //        Debug.Log("updateStatus");
        titleText.text = upgrade.Title;
        descriptionText.text = upgrade.Description;
        flavorText.text = upgrade.FlavorText;
        anim = GetComponent<Animator>();
    }

    public void Update()
    {
        UpdateStatus();
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
            status = UIShopUpgradeStatus.Unavailable;
            purchasedIndicator.SetActive(false);
            unavailableIndicator.SetActive(true);
        }
    }

    public void HandleClick()
    {
        if (status == UIShopUpgradeStatus.Available)
        {
            UIShop.main.BuyUpgrade(upgrade);
            anim.SetBool("Done", true);
            UpdateStatus();
        }
    }


}