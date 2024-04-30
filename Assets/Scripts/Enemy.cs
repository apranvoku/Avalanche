using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public abstract class Enemy : MonoBehaviour
{

    public abstract void TakeDamage(float damage);

    public abstract void SelfDestroy();

    public abstract void StopMoving();

    public abstract void ResumeMoving();
}
