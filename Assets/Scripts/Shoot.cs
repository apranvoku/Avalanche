using System;
using System.Collections;
using System.Collections.Generic;
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
    public GameObject PistolBullets;
    public GameObject MachineGunBullets;
    public GameObject ShotgunBullets;
    public GameObject RocketLauncherBullets;

    private GameObject activeBulletUI;

    public GameObject rocketProjectile;
    public GameObject defaultProjectile;

    private GameObject projectile;
    public GameObject projectileParent;
    public GameObject muzzleFlash;
    public GameObject muzzleFlashLocation;
    public Gun selectedGun;

    public GameObject gunOrigin;

    public Pistol pistol;
    public Machinegun machinegun;
    public Shotgun shotgun;
    public Rocketlauncher rocketlauncher;

    public Slider GunCursor;

    private float fireTimeStamp;
    public bool reloading;
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
        projectile = defaultProjectile;

        pistol = new Pistol();
        pistol.ammoRemaining = pistol.maxAmmo;
        machinegun = new Machinegun();
        machinegun.ammoRemaining = machinegun.maxAmmo;
        shotgun = new Shotgun();
        shotgun.ammoRemaining = shotgun.maxAmmo;
        rocketlauncher = new Rocketlauncher();
        rocketlauncher.ammoRemaining = rocketlauncher.maxAmmo;

        pistol.upgradeDamage();
        pistol.upgradeDamage();
        pistol.upgradeDamage();
        pistol.upgradeDamage();
        pistol.upgradeDamage();
        pistol.upgradeDamage();
        pistol.upgradeDamage();

        selectedGun = pistol;
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
            else if(selectedGun.ammoRemaining == 0)
            {
                reloading = true;
                StartCoroutine(Reload());
            }
        }
    }

    public IEnumerator Reload()
    {
        //selectedGun.ammoRemaining = 0;
        for (float reload = 0; reload < selectedGun.reloadTime; reload += Time.deltaTime)
        {
            //Radially fill cursor.
            float reloadPercentage = reload / selectedGun.reloadTime;
            GunCursor.value = reloadPercentage;
            //Reactive bullet UI gameobjects as gun reloads.
            if (reloadPercentage > (float)(selectedGun.ammoRemaining)/selectedGun.maxAmmo)
            {
                activeBulletUI.transform.GetChild(selectedGun.ammoRemaining).gameObject.SetActive(true);
                selectedGun.ammoRemaining += 1;
            }
            yield return null;
        }
        selectedGun.ammoRemaining = selectedGun.maxAmmo;
        reloading = false;
    }

    public void SwitchGun(Guns gun)
    {
        if(!reloading)
        {
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
                    projectile = defaultProjectile;
                    activeBulletUI.SetActive(false);
                    MachineGunBullets.SetActive(true);
                    activeBulletUI = MachineGunBullets;
                    ActivateGunGO(Guns.Machinegun);
                    break;

                case Guns.Shotgun:
                    selectedGun = shotgun;
                    projectile = defaultProjectile;
                    activeBulletUI.SetActive(false);
                    ShotgunBullets.SetActive(true);
                    activeBulletUI = ShotgunBullets;
                    ActivateGunGO(Guns.Shotgun);
                    break;
            }
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
