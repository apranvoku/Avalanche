using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Tilemaps;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public enum Guns
{
    None,
    Pistol,
    Machinegun,
    Shotgun,
    Rocketlauncher
}

public class Shoot : MonoBehaviour
{
    public bool GODMODE;

    public GameObject PistolBullets;
    public GameObject MachineGunBullets;
    public GameObject ShotgunBullets;
    public GameObject RocketLauncherBullets;

    private GameObject activeBulletUI;

    public GameObject machinegunProjectile;
    public GameObject shotgunProjectile;
    public GameObject rocketProjectile;
    public GameObject defaultProjectile;

    private GameObject projectile;
    public GameObject projectileParent;
    public GameObject muzzleFlash;
    public GameObject muzzleFlashLocation;
    public Gun selectedGun;
    public Guns selectedGuns;

    public GameObject gunOrigin;

    public Pistol pistol;
    public Machinegun machinegun;
    public Shotgun shotgun;
    public Rocketlauncher rocketlauncher;

    public Slider GunCursor;

    private float fireTimeStamp;
    public bool reloading;
    private bool resetingGunUI;
    // Start is called before the first frame update
    void Start()
    {
        GunCursor = GameObject.Find("GunCursor").GetComponent<Slider>();
        PistolBullets = GameObject.Find("PistolBullets");
        MachineGunBullets = GameObject.Find("MachineGunBullets");
        MachineGunBullets.SetActive(false);
        ShotgunBullets = GameObject.Find("ShotgunBullets");
        ShotgunBullets.SetActive(false);
        RocketLauncherBullets = GameObject.Find("RocketLauncherBullets");
        RocketLauncherBullets.SetActive(false);

        reloading = false;
        resetingGunUI = false;
        projectile = defaultProjectile;

        pistol = new Pistol();
        pistol.ammoRemaining = pistol.maxAmmo;
        machinegun = new Machinegun();
        machinegun.ammoRemaining = machinegun.maxAmmo;
        shotgun = new Shotgun();
        shotgun.ammoRemaining = shotgun.maxAmmo;
        rocketlauncher = new Rocketlauncher();
        rocketlauncher.ammoRemaining = rocketlauncher.maxAmmo;

        if(GODMODE)
        {
            for(int i = 0; i < 10; i++)
            {
                pistol.upgradeDamage();
                pistol.upgradeDamage();
                pistol.upgradePenetration();
                pistol.upgradeReload();
            }
        }


        selectedGun = pistol;
        selectedGuns = Guns.Pistol;
        activeBulletUI = PistolBullets;
        //SwitchGun(Guns.Rocketlauncher); //WE NEED THE GIGA LAUNCHER!!

        muzzleFlashLocation = GameObject.Find("MuzzleFlashLocation");
        fireTimeStamp = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (!PauseScreen.isPaused && !reloading)
        {
            float timePassed = Time.time - fireTimeStamp;

            if (Mouse.current.leftButton.isPressed && timePassed >= (1f / selectedGun.fireRate) && selectedGun.ammoRemaining > 0)
            {
                selectedGun.ammoRemaining -= 1;
                activeBulletUI.transform.GetChild(selectedGun.ammoRemaining).gameObject.SetActive(false);
                if (selectedGun != shotgun)
                {
                    Instantiate(muzzleFlash, muzzleFlashLocation.transform.position, Quaternion.identity);
                    Instantiate(projectile, transform.position, Quaternion.identity, null);
                    fireTimeStamp = Time.time;
                }
                else
                {
                    float rotation = -50;
                    for (int i = 0; i < 6; i++)
                    {
                        Instantiate(muzzleFlash, muzzleFlashLocation.transform.position, Quaternion.identity);
                        GameObject pellet = Instantiate(projectile, transform.position, Quaternion.identity, null);

                        pellet.transform.Rotate(new Vector3(0f, 0f, rotation));
                        rotation += 20f;
                    }
                    fireTimeStamp = Time.time;
                }
            }
            else if (Keyboard.current.rKey.wasPressedThisFrame)
            {
                reloading = true;
                StartCoroutine(Reload());
            }
            else if(selectedGun.ammoRemaining == 0)
            {
                reloading = true;
                StartCoroutine(Reload());
            }
        }
    }

    public IEnumerator Reload()
    {
        //set new objects of the gun to be reloaded, saves reference even if selected gun changes
        GameObject reloadingBulletUI = activeBulletUI.gameObject;
        Gun reloadingGun = selectedGun;
        int ramainingAmmoToReload = reloadingGun.ammoRemaining;

        reloadingBulletUI.GetComponent<CanvasGroup>().alpha = 0.3f;

        for (float reload = 0; (reload < reloadingGun.reloadTime) ; reload += Time.deltaTime)
        {
            if (!reloading)
            {
                if (!resetingGunUI)
                {
                    StopReloading(reloadingBulletUI, reloadingGun, ramainingAmmoToReload);
                }
                else
                {
                    resetingGunUI = false;
                }
                yield break;
            }
            //Radially fill cursor.
            float reloadPercentage = reload / reloadingGun.reloadTime;
            GunCursor.value = reloadPercentage;
            //Reactive bullet UI gameobjects as gun reloads.
            if (reloadPercentage > (float)(reloadingGun.ammoRemaining)/ reloadingGun.maxAmmo)
            {
                reloadingBulletUI.transform.GetChild(reloadingGun.ammoRemaining).gameObject.SetActive(true);
                reloadingGun.ammoRemaining += 1;
            }
            yield return null;
        }
        reloadingBulletUI.GetComponent<CanvasGroup>().alpha = 1f;
        reloading = false;
    }

    public void StopReloading(GameObject GunToReloadBulletUI, Gun GunToReload, int NumBulletsToReset)
    {
        GunCursor.value = 1;
        GunToReload.ammoRemaining = NumBulletsToReset;
        GunToReloadBulletUI.GetComponent<CanvasGroup>().alpha = 1f;
        for (int bulletNumber = GunToReload.maxAmmo; bulletNumber > NumBulletsToReset; bulletNumber--)
        {
            GunToReloadBulletUI.transform.GetChild(bulletNumber-1).gameObject.SetActive(false);
        }
    }

    public void ResetAmmoUI()
    {
        Guns equipedGunWhilereseting = selectedGuns;
        reloading = false;
        resetingGunUI = true;
        foreach (Guns gunValue in System.Enum.GetValues(typeof(Guns)))
        {
            if (gunValue != Guns.None)
            {
                SwitchGun(gunValue);
                GunCursor.value = 1;
                activeBulletUI.GetComponent<CanvasGroup>().alpha = 1f;
                foreach (Transform bullet in activeBulletUI.transform)
                {
                    bullet.gameObject.SetActive(true);
                }
                selectedGun.ammoRemaining = selectedGun.maxAmmo;
            }
        }
        SwitchGun(equipedGunWhilereseting);
    }

    public void SwitchGun(Guns gun)
    {
        reloading = false;
        selectedGuns = gun;
        switch (gun)
            {
                case Guns.Pistol:
                    selectedGun = pistol;
                    projectile = defaultProjectile;
                    activeBulletUI.SetActive(false);
                    PistolBullets.SetActive(true);
                    activeBulletUI = PistolBullets;
                    ActivateGunGO(Guns.Pistol);
                    break;

                case Guns.Rocketlauncher:
                    selectedGun = rocketlauncher;
                    projectile = rocketProjectile;
                    activeBulletUI.SetActive(false);
                    RocketLauncherBullets.SetActive(true);
                    activeBulletUI = RocketLauncherBullets;
                    ActivateGunGO(Guns.Rocketlauncher);
                    break;

                case Guns.Machinegun:
                    selectedGun = machinegun;
                    projectile = machinegunProjectile;
                    activeBulletUI.SetActive(false);
                    MachineGunBullets.SetActive(true);
                    activeBulletUI = MachineGunBullets;
                    ActivateGunGO(Guns.Machinegun);
                    break;

                case Guns.Shotgun:
                    selectedGun = shotgun;
                    projectile = shotgunProjectile;
                    activeBulletUI.SetActive(false);
                    ShotgunBullets.SetActive(true);
                    activeBulletUI = ShotgunBullets;
                    ActivateGunGO(Guns.Shotgun);
                    break;
            }
    }

    public void ActivateGunGO(Guns gun)
    {
        foreach(Transform child in gunOrigin.transform)
        {
            if(child.gameObject.name != gun.ToString())
            {
                child.gameObject.SetActive(false);
            }
            else
            {
                child.gameObject.SetActive(true);
                muzzleFlashLocation = child.GetChild(0).gameObject;
            }
        }
    }
}
