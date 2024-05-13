using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using UnityEngine.UIElements;


public abstract class Enemy : MonoBehaviour
{
    public bool dead { get; set; }

    public abstract void TakeDamage(float damage);

    public abstract void SelfDestroy();

    public abstract void StopMoving();

    public abstract void ResumeMoving();
}
