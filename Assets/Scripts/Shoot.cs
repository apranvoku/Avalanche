using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public enum Guns
{
    Pistol,
    Machinegun,
    Shotgun,
    Rocketlauncher
}

public class Shoot : MonoBehaviour
{
    public GameObject projectile;
    public GameObject projectileParent;
    public Gun currentGun;

    public SpriteRenderer currentGunSprite;
    public Sprite pistolSprite;
    public Sprite machineGunSprite;
    public Sprite shotgunSprite;
    public Sprite rocketLauncherSprite;

    public Pistol pistol;
    public Machinegun machinegun;
    public Shotgun shotgun;
    public Rocketlauncher rocketlauncher;

    public bool readyToFire;
    // Start is called before the first frame update
    void Start()
    {
        pistol = new Pistol();
        machinegun = new Machinegun();
        shotgun = new Shotgun();
        rocketlauncher = new Rocketlauncher();

        currentGun = pistol;
        readyToFire = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame && readyToFire)
        {
            readyToFire = false;
            Instantiate(projectile, transform.position, Quaternion.identity, projectileParent.transform);
            StartCoroutine(FireDelay());
        }
    }

    public IEnumerator FireDelay()
    {
        yield return new WaitForSecondsRealtime(0.1f);
        readyToFire = true;
    }

    public void SwitchGun(Guns gun)
    {
        switch(gun)
        {
            case Guns.Pistol: 
                currentGun = pistol;
            break;

            case Guns.Rocketlauncher: 
                currentGun=rocketlauncher;
            break;

            case Guns.Machinegun:
                currentGun = machinegun;
            break;

            case Guns.Shotgun:
                currentGun = shotgun;
            break;
        }
    }
}
