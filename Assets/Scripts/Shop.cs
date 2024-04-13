using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    public GameObject moneyDisplay;
    private CanvasGroup myGroup;
    private static int m_referenceCount = 0;
    private static Shop instance;
    public Shoot shootScript;
    public int money;

    private bool machinegunBought;
    private bool shotgunBought;
    private bool rocketLauncherBought;


    public static Shop Instance
    {
        get { return instance; }
    }

    void Awake()
    {
        m_referenceCount++;
        if (m_referenceCount > 1)
        {
            DestroyImmediate(this.gameObject);
            return;
        }

        instance = this;
        // Use this line if you need the object to persist across scenes
        DontDestroyOnLoad(this.gameObject);
    }

    public void Start()
    {
        myGroup = GameObject.Find("ShopCanvas").GetComponent<CanvasGroup>();
        moneyDisplay = GameObject.Find("MoneyText");

        machinegunBought = false;
        shotgunBought = false;
        rocketLauncherBought = false;   

        money = 0;
    }

    public bool SpendMoney(int moneySpent) { 
        if(money - moneySpent < 0)
        {
            return false;
        }
        else
        {
            money -= moneySpent;
            UpdateMoney();
            return true;
        }
    }

    public void GetMoney(int moneyGot)
    {
        money += moneyGot;
        UpdateMoney();
    }

    public void UpdateMoney()
    {
        if (money <= 9)
        {
            moneyDisplay.GetComponent<TextMeshProUGUI>().text = "00" + money.ToString();
        }
        else if (money < 100 && money > 9)
        {
            moneyDisplay.GetComponent<TextMeshProUGUI>().text = "0" + money.ToString();
        }
        else
        {
            moneyDisplay.GetComponent<TextMeshProUGUI>().text = money.ToString();
        }
    }

    public void OpenShop()
    {
        StartCoroutine(FadeToBlack(1f));
    }

    public void CloseShop()
    {
        StartCoroutine(FadeToWhite(1f));
    }

    public IEnumerator FadeToBlack(float duration)
    {
        for (float i = 0; i < duration; i += Time.deltaTime) 
        {
            myGroup.alpha = i;
            yield return null;
        }
    }

    public IEnumerator FadeToWhite(float duration)
    {
        for (float i = 0; i < duration; i += Time.deltaTime)
        {
            myGroup.alpha = 1f-i;
            yield return null;
        }
    }
    public void UpgradeDamage(int cost)
    {
        shootScript.currentGun.upgradeDamage();
        SpendMoney(cost);
    }
    public void UpgradePenetration(int cost)
    {
        shootScript.currentGun.upgradePenetration();
        SpendMoney(cost);
    }

    public void upgradeReload(int cost)
    {
        shootScript.currentGun.upgradeReload();
        SpendMoney(cost);
    }

    public void BuyMachineGun(int cost)
    {
        if(machinegunBought == false)
        {
            machinegunBought = true;
            SpendMoney(cost);
        }
        shootScript.SwitchGun(Guns.Machinegun);
    }

    public void BuyShotGun(int cost)
    {
        if (shotgunBought == false)
        {
            shotgunBought = true;
            SpendMoney(cost);
        }
        shootScript.SwitchGun(Guns.Shotgun);
    }

    public void BuyRocketLauncher(int cost)
    {
        if (rocketLauncherBought == false)
        {
            rocketLauncherBought = true;
            SpendMoney(cost);
        }
        shootScript.SwitchGun(Guns.Rocketlauncher);
    }

    public void BuyPistol() //this just swaps.
    {
        shootScript.SwitchGun(Guns.Pistol);
    }
}
