using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHydraHead : Enemy
{
    public override void ResumeMoving()
    {
        throw new System.NotImplementedException();
    }

    public override void SelfDestroy()
    {
        throw new System.NotImplementedException();
    }

    public override void StopMoving()
    {
        throw new System.NotImplementedException();
    }

    public override void TakeDamage(float damage)
    {
        transform.parent.GetComponent<EnemyHydra>().TakeDamage(damage*5f);
    }
}
