using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Shoot : MonoBehaviour
{
    public GameObject projectile;
    public GameObject projectileParent;

    public bool readyToFire;
    // Start is called before the first frame update
    void Start()
    {
        readyToFire = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (Mouse.current.leftButton.isPressed && readyToFire)
        {
            readyToFire = false;
            Instantiate(projectile, transform.position, Quaternion.identity, projectileParent.transform);
            StartCoroutine(FireDelay());
        }
    }

    public IEnumerator FireDelay()
    {
        yield return new WaitForSecondsRealtime(0.03f);
        readyToFire = true;
    }
}
