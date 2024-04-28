using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

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
    public GameObject projectile;
    public GameObject projectileParent;
    public GameObject muzzleFlash;
    public GameObject muzzleFlashLocation;
    public Gun selectedGun;

    public GameObject gunOrigin;

    public Pistol pistol;
    public Machinegun machinegun;
    public Shotgun shotgun;
    public Rocketlauncher rocketlauncher;

    private float fireTimeStamp;


    public bool readyToFire;
    // Start is called before the first frame update
    void Start()
    {
        

        pistol = new Pistol();
        machinegun = new Machinegun();
        shotgun = new Shotgun();
        rocketlauncher = new Rocketlauncher();

        selectedGun = pistol;
        readyToFire = true;

        muzzleFlashLocation = GameObject.Find("MuzzleFlashLocation");

        fireTimeStamp = 0;
    }

    // Update is called once per frame
    void Update()
    {

        float timePassed = Time.time - fireTimeStamp;

        if (Mouse.current.leftButton.isPressed && timePassed >= (1f / selectedGun.fireRate))
        {

            //readyToFire = false;
            Instantiate(muzzleFlash, muzzleFlashLocation.transform.position, Quaternion.identity);
            Instantiate(projectile, transform.position, Quaternion.identity, projectileParent.transform);
            fireTimeStamp = Time.time;
            //StartCoroutine(FireDelay());
        }
    }

    public void DisableFire()
    {
        readyToFire = false;
    }

    public void EnableFire()
    {
        readyToFire = true;
    }

    public IEnumerator FireDelay()
    {
        yield return new WaitForSecondsRealtime(1f/selectedGun.fireRate);
        readyToFire = true;
    }

    public void SwitchGun(Guns gun)
    {
        switch(gun)
        {
            case Guns.Pistol: 
                selectedGun = pistol;
                ActivateGunGO(Guns.Pistol);
            break;

            case Guns.Rocketlauncher: 
                selectedGun=rocketlauncher;
                ActivateGunGO(Guns.Rocketlauncher);
                break;

            case Guns.Machinegun:
                selectedGun = machinegun;
                ActivateGunGO(Guns.Machinegun);
                break;

            case Guns.Shotgun:
                selectedGun = shotgun;
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
