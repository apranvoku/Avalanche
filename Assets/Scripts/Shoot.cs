using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Shoot : MonoBehaviour
{
    public GameObject projectile;
    public GameObject projectileParent;
    public Gun currentGun;

    public Sprite pistolSprite;
    public Sprite machineGunSprite;
    public Sprite shotgunSprite;
    public Sprite rocketLauncherSprite;

    public bool readyToFire;
    // Start is called before the first frame update
    void Start()
    {
        Pistol pistol = new Pistol();
        Machinegun machinegun = new Machinegun();
        Shotgun shotgun = new Shotgun();
        Rocketlauncher rocketlauncher = new Rocketlauncher();

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
}
