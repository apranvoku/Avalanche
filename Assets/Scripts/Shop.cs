using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    public Image blackScreen;

    public Sprite pistolSprite;
    public Sprite machineGunSprite;
    public Sprite shotGunSprite;
    public Sprite rocketLauncherSprite;

    public Image primaryGunBG;
    public Image secondaryGunBG;

    public GameObject PenPips;
    public GameObject ReloadPips;
    public GameObject DamagePips;
    public TextMeshProUGUI PenUpgradePrice;
    public TextMeshProUGUI ReloadUpgradePrice;
    public TextMeshProUGUI DamageUpgradePrice;


    public GameObject moneyDisplay;
    private CanvasGroup myGroup;
    private static int m_referenceCount = 0;
    private static Shop instance;
    public Shoot shootScript;
    public int money;
    private int moneySnapshot;

    public List<string> levels;
    public int currentLevel;

    private bool machinegunBought;
    private bool shotgunBought;
    private bool rocketLauncherBought;

    public Guns primaryGun;
    public Guns secondaryGun;


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
        primaryGun = Guns.Pistol;
        primaryGunBG.GetComponent<Image>().color = Color.green;

        secondaryGun = Guns.None;

        currentLevel = 1; //Start menu handles first level.

        myGroup = GameObject.Find("ShopCanvas").GetComponent<CanvasGroup>();
        moneyDisplay = GameObject.Find("MoneyText");


        shootScript = GameObject.Find("Character").GetComponent<Shoot>();

        machinegunBought = false;
        shotgunBought = false;
        rocketLauncherBought = false;
        myGroup.interactable = false;

        moneySnapshot = 0;
        money = 0;
        //temp for testing.
        //money = 90000;
        //UpdateMoney();
        //temp for testing.
        //OpenShop();
        //CloseShop();
    }

    public void Update()
    {
        if(secondaryGun != Guns.None) //Don't allow a swap when no gun is selected.
        {
            if (Mouse.current.scroll.ReadValue() != Vector2.zero)
            {
                if (primaryGunBG.GetComponent<Image>().color == Color.green) //We need to swap to secondary. (green means selected)
                {
                    shootScript.SwitchGun(secondaryGun);
                    primaryGunBG.GetComponent<Image>().color = Color.white;
                    secondaryGunBG.GetComponent<Image>().color = Color.green;
                }
                else //We need to swap back to primary.
                {
                    shootScript.SwitchGun(primaryGun);
                    primaryGunBG.GetComponent<Image>().color = Color.green;
                    secondaryGunBG.GetComponent<Image>().color = Color.white;
                }
                InvalidateShop();

            }
            if (Keyboard.current.digit1Key.wasPressedThisFrame && shootScript.selectedGuns != primaryGun)
            {
                //Debug.Log("Swapping to primary!");
                shootScript.SwitchGun(primaryGun);
                primaryGunBG.GetComponent<Image>().color = Color.green;
                secondaryGunBG.GetComponent<Image>().color = Color.white;
                InvalidateShop();
            }
            if (Keyboard.current.digit2Key.wasPressedThisFrame && shootScript.selectedGuns != secondaryGun)
            {
                //Debug.Log("Swapping to secondary!");
                shootScript.SwitchGun(secondaryGun);
                primaryGunBG.GetComponent<Image>().color = Color.white;
                secondaryGunBG.GetComponent<Image>().color = Color.green;
                InvalidateShop();
            }
        }
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

    public void InvalidateShop()
    {
        int penPipCounter = shootScript.selectedGun.penetrationUpgradeLevel;
        int reloadPipCounter = shootScript.selectedGun.reloadUpgradeLevel;
        int damagePipCounter = shootScript.selectedGun.damageUpgradeLevel;

        //Maybe pistol gets 50% off prices?

        if (shootScript.selectedGun.penetrationUpgradeLevel < shootScript.selectedGun.maxPenetrationUpgradeLevel)
        {
            PenUpgradePrice.text = (400 + shootScript.selectedGun.penetrationUpgradeLevel * 700).ToString(); //Price scaling, open to change.
        }
        else
        {
            PenUpgradePrice.text = "MAX";
        }
        if (shootScript.selectedGun.reloadUpgradeLevel < shootScript.selectedGun.maxReloadUpgradeLevel)
        {
            ReloadUpgradePrice.text = (100 + shootScript.selectedGun.reloadUpgradeLevel * 200).ToString(); //Price scaling, open to change.
        }
        else
        {
            ReloadUpgradePrice.text = "MAX";
        }
        if (shootScript.selectedGun.damageUpgradeLevel < shootScript.selectedGun.maxDamageUpgradeLevel)
        {
            DamageUpgradePrice.text = (100 + shootScript.selectedGun.damageUpgradeLevel * 200).ToString(); //Price scaling, open to change.
        }
        else
        {
            DamageUpgradePrice.text = "MAX";
        }

        foreach (Transform pip in PenPips.transform) //
        {
            if(penPipCounter > 0)
            {
                pip.gameObject.SetActive(true);
            }
            else
            {
                pip.gameObject.SetActive(false);
            }
            penPipCounter--;
        }

        foreach (Transform pip in ReloadPips.transform) //
        {
            if (reloadPipCounter > 0)
            {
                pip.gameObject.SetActive(true);
            }
            else
            {
                pip.gameObject.SetActive(false);
            }
            reloadPipCounter--;
        }

        foreach (Transform pip in DamagePips.transform) //
        {
            if (damagePipCounter > 0)
            {
                pip.gameObject.SetActive(true);
            }
            else
            {
                pip.gameObject.SetActive(false);
            }
            damagePipCounter--;
        }
    }

    public void CloseShop()
    {
        StartCoroutine(FadeToWhite(1f));
        //I do some weird stuff to make the Looping work
        currentLevel++;
        moneySnapshot = money;
        if (currentLevel >= levels.Count)
        {
            currentLevel = 1;
            GameManager.loop++;
            GameObject.Find("Agent").GetComponent<AgentMovement>().enabled = true;
        }
        SceneManager.LoadScene(levels[currentLevel-1]);
        
        AgentMovement.Instance.transform.position = Vector3.zero;
        AgentMovement.Instance.OnEnable();
        PauseScreen.canPause = true;
    }

    public void ResetMoneyToSnapshot()
    {
        money = moneySnapshot;
        UpdateMoney();
    }

    public string GetLevel()
    {
        return currentLevel.ToString();
    }

    public void resetCurrentLevel()
    {
        currentLevel = 1;
        ResetPurchases();
        money = 0;
        moneySnapshot = 0;
        UpdateMoney();
    }

    public IEnumerator FadeToBlack(float duration)
    {
        Color screencolor = blackScreen.color; 
        for (float i = 0; i < duration; i += Time.deltaTime) 
        {
            blackScreen.color = new Color(screencolor.r, screencolor.g, screencolor.b, i);
            myGroup.alpha = i;
            yield return null;
        }
        blackScreen.color = new Color(screencolor.r, screencolor.g, screencolor.b, 1f);
        myGroup.alpha = 1f;
        myGroup.interactable = true;
    }

    public IEnumerator FadeToWhite(float duration)
    {
        myGroup.interactable = false;
        Color screencolor = blackScreen.color;
        for (float i = 0; i < duration; i += Time.deltaTime)
        {
            blackScreen.color = new Color(screencolor.r, screencolor.g, screencolor.b, 1f - i);
            myGroup.alpha = 1f-i;
            yield return null;
        }
        blackScreen.color = new Color(screencolor.r, screencolor.g, screencolor.b, 0f);
        myGroup.alpha = 0f;
    }

    public void UpgradePenetration()
    {
        if (shootScript.selectedGun.penetrationUpgradeLevel < shootScript.selectedGun.maxPenetrationUpgradeLevel) {
            int cost = 400 + shootScript.selectedGun.penetrationUpgradeLevel * 700;
            if (SpendMoney(cost))
            {
                shootScript.selectedGun.upgradePenetration();
                InvalidateShop();
            }
        }
    }

    public void UpgradeDamage()
    {
        if (shootScript.selectedGun.damageUpgradeLevel < shootScript.selectedGun.maxDamageUpgradeLevel)
        {
            int cost = 100 + shootScript.selectedGun.damageUpgradeLevel * 200;
            if (SpendMoney(cost))
            {
                shootScript.selectedGun.upgradeDamage();
                InvalidateShop();
            }
        }
    }

    public void UpgradeReload()
    {
        if (shootScript.selectedGun.reloadUpgradeLevel < shootScript.selectedGun.maxReloadUpgradeLevel)
        {
            int cost = 100 + shootScript.selectedGun.reloadUpgradeLevel * 200;
            if (SpendMoney(cost))
            {
                shootScript.selectedGun.upgradeReload();
                InvalidateShop();
            }
        }
    }

    public void BuyMachineGun(int cost)
    {
        if(!machinegunBought && SpendMoney(cost))
        {
            machinegunBought = true;
        }
        if((primaryGun != Guns.Machinegun) && machinegunBought) 
        {
            secondaryGunBG.transform.GetChild(0).GetComponentInChildren<Image>().sprite = primaryGunBG.transform.GetChild(0).GetComponentInChildren<Image>().sprite;
            primaryGunBG.transform.GetChild(0).GetComponentInChildren<Image>().sprite = machineGunSprite;
            secondaryGun = primaryGun;
            primaryGun = Guns.Machinegun;
            shootScript.SwitchGun(primaryGun);
            SelectPrimaryGunInUI();
        }
    }

    public void BuyShotGun(int cost)
    {
        if (!shotgunBought && SpendMoney(cost))
        {
            shotgunBought = true;
        }
        if ((primaryGun != Guns.Shotgun) && shotgunBought)
        {
            secondaryGunBG.transform.GetChild(0).GetComponentInChildren<Image>().sprite = primaryGunBG.transform.GetChild(0).GetComponentInChildren<Image>().sprite;
            primaryGunBG.transform.GetChild(0).GetComponentInChildren<Image>().sprite = shotGunSprite;
            secondaryGun = primaryGun;
            primaryGun = Guns.Shotgun;
            shootScript.SwitchGun(primaryGun);
            SelectPrimaryGunInUI();
        }
    }

    public void BuyRocketLauncher(int cost)
    {
        if (!rocketLauncherBought && SpendMoney(cost))
        {
            rocketLauncherBought = true;
        }
        if ((primaryGun != Guns.Rocketlauncher) && rocketLauncherBought)
        {
            secondaryGunBG.transform.GetChild(0).GetComponentInChildren<Image>().sprite = primaryGunBG.transform.GetChild(0).GetComponentInChildren<Image>().sprite;
            primaryGunBG.transform.GetChild(0).GetComponentInChildren<Image>().sprite = rocketLauncherSprite;
            secondaryGun = primaryGun;
            primaryGun = Guns.Rocketlauncher;
            shootScript.SwitchGun(primaryGun);
            SelectPrimaryGunInUI();
        }
    }

    public void BuyPistol() //this just swaps.
    {
        if (primaryGun != Guns.Pistol)
        {
            secondaryGunBG.transform.GetChild(0).GetComponentInChildren<Image>().sprite = primaryGunBG.transform.GetChild(0).GetComponentInChildren<Image>().sprite;
            primaryGunBG.transform.GetChild(0).GetComponentInChildren<Image>().sprite = pistolSprite;
            secondaryGun = primaryGun;
        }
        primaryGun = Guns.Pistol;
        shootScript.SwitchGun(primaryGun);
        SelectPrimaryGunInUI();
    }

    public void SelectPrimaryGunInUI()
    {
        primaryGunBG.GetComponent<Image>().color = Color.green;
        secondaryGunBG.GetComponent<Image>().color = Color.white;
    }

    public void ResetPurchases()
    {
        BuyRocketLauncher(0);
        shootScript.selectedGun.ResetStats();
        InvalidateShop();
        BuyShotGun(0);
        shootScript.selectedGun.ResetStats();
        InvalidateShop();
        BuyMachineGun(0);
        shootScript.selectedGun.ResetStats();
        InvalidateShop();
        machinegunBought = false;
        shotgunBought = false;
        rocketLauncherBought = false;
        BuyPistol();
        shootScript.selectedGun.ResetStats();
        InvalidateShop();
        secondaryGun = Guns.None;
        SelectPrimaryGunInUI();
        secondaryGunBG.transform.GetChild(0).GetComponentInChildren<Image>().sprite = null;

    }
}
