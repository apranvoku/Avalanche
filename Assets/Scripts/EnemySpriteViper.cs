using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpriteViper : MonoBehaviour
{

    public void StopMoving()
    {
        transform.parent.GetComponent<EnemyViper>().StopMoving();
    }

    public void ResumeMoving()
    {
        transform.parent.GetComponent<EnemyViper>().ResumeMoving();
    }

    public void SelfDestroy()
    {
        transform.parent.GetComponent<EnemyViper>().SelfDestroy();
    }

    public void StartAttacking()
    {
        transform.parent.GetComponent<EnemyViper>().StartAttacking();
    }

    public void StopAttacking()
    {
        transform.parent.GetComponent<EnemyViper>().StopAttacking();
    }
}
