using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Tilemaps;
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

    private float fireTimeStamp;


    public bool readyToFire;
    // Start is called before the first frame update
    void Start()
    {
        projectile = defaultProjectile;

        pistol = new Pistol();
        machinegun = new Machinegun();
        shotgun = new Shotgun();
        rocketlauncher = new Rocketlauncher();

        selectedGun = pistol;
        //SwitchGun(Guns.Rocketlauncher); //WE NEED THE GIGA LAUNCHER!!

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
            if(selectedGun != shotgun)
            {
                Instantiate(muzzleFlash, muzzleFlashLocation.transform.position, Quaternion.identity);
                Instantiate(projectile, transform.position, Quaternion.identity, null);
                fireTimeStamp = Time.time;
            }
            else
            {
                float rotation = -50;
                for(int i = 0; i < 6; i++)
                {
                    Instantiate(muzzleFlash, muzzleFlashLocation.transform.position, Quaternion.identity);
                    GameObject pellet = Instantiate(projectile, transform.position, Quaternion.identity, null);

                    pellet.transform.Rotate(new Vector3(0f, 0f, rotation));
                    rotation += 20f;
                }
                fireTimeStamp = Time.time;
            }
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
                projectile = defaultProjectile;
                ActivateGunGO(Guns.Pistol);
            break;

            case Guns.Rocketlauncher: 
                selectedGun=rocketlauncher;
                projectile = rocketProjectile;
                ActivateGunGO(Guns.Rocketlauncher);
                break;

            case Guns.Machinegun:
                selectedGun = machinegun;
                projectile = defaultProjectile;
                ActivateGunGO(Guns.Machinegun);
                break;

            case Guns.Shotgun:
                selectedGun = shotgun;
                projectile = defaultProjectile;
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
