using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpriteCobra : MonoBehaviour
{

    public void StopMoving()
    {
        transform.parent.GetComponent<EnemyCobra>().StopMoving();
    }

    public void ResumeMoving()
    {
        transform.parent.GetComponent<EnemyCobra>().ResumeMoving();
    }

    public void SelfDestroy()
    {
        transform.parent.GetComponent<EnemyCobra>().SelfDestroy();
    }

    public void BeginShoot()
    {
        transform.parent.GetComponent<EnemyCobra>().BeginShoot();
    }
    public void StopShoot()
    {
        transform.parent.GetComponent<EnemyCobra>().StopShoot();
    }

    public void StopAttacking()
    {
        transform.parent.GetComponent<EnemyCobra>().StopAttacking();
    }
}
