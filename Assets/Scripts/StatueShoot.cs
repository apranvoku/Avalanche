using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatueShoot : MonoBehaviour
{
    public GameObject Projectile;
    private EnemyHydra hydra;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(ShootPattern());
        hydra = GetComponentInParent<EnemyHydra>();
    }

    public IEnumerator ShootPattern()
    {
        yield return new WaitForSeconds(2f);
        while(!hydra.phase4started)
        {
            GameObject BulletA = GameObject.Instantiate(Projectile, transform.position, Quaternion.identity, transform);
            GameObject BulletB = GameObject.Instantiate(Projectile, transform.position, Quaternion.identity, transform);
            BulletB.transform.Rotate(0f, 0f, 180f); ;
            BulletA.transform.parent = null; //Move parent so bullets keeps its trajectory.
            BulletB.transform.parent = null; //Move parent so bullets keeps its trajectory.
            yield return new WaitForSeconds(0.15f);
        }
        while(!hydra.hydraDead)
        {
            GameObject BulletA = GameObject.Instantiate(Projectile, transform.position, transform.rotation, transform);
            GameObject BulletB = GameObject.Instantiate(Projectile, transform.position, transform.rotation, transform);
            BulletB.GetComponent<HydraProjectile>().velocityVector = BulletB.transform.right * -1f;
            BulletB.transform.Rotate(0f, 0f, 180f);
            transform.Rotate(0f, 0f, 10f);
            BulletA.transform.parent = null; //Move parent so bullets keeps its trajectory.
            BulletB.transform.parent = null; //Move parent so bullets keeps its trajectory.
            yield return new WaitForSeconds(0.3f);
        }
        Destroy(transform.parent.parent.gameObject);
        yield return null;
    }
}
