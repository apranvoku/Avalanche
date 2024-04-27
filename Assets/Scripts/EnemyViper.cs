using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class EnemyViper : MonoBehaviour
{

    public GameObject player;
    public Player playerScript;
    Slider slider;
    NavMeshAgent agent;
    public float hp;
    private float total_hp;
    public GameObject coinDrop;
    public GameObject coinDropParent;
    private Animator animator;
    public GameObject bulletFrag;
    private Vector3 distToPlayer;
    public float attackRange;
    private float attackDelay;
    private bool attacking;
    public bool canAttack;

    // Start is called before the first frame update
    void Awake()
    {
        hp = 30;
        total_hp = hp;
        player = GameObject.Find("Agent");
        playerScript = player.GetComponentInChildren<Player>();
        coinDropParent = GameObject.Find("coinDropParent");
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        slider = GetComponentInChildren<Slider>();
        animator = GetComponentInChildren<Animator>();
        attacking = false;
        canAttack = true;
        attackRange = 50f;
        attackDelay = 5f;
    }

    // Update is called once per frame
    void Update()
    {
        if (!attacking && canAttack)
        {
            if (Vector3.Distance(transform.position, player.transform.position) <= attackRange)
            {
                animator.SetTrigger("Awake");
                canAttack = false;
                StartCoroutine(AttackDelay());
            }
        }
        else if (attacking && !agent.isStopped)
        {
            agent.destination = player.transform.position;
        }
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject bullet = collision.gameObject;
        if (bullet.GetComponentInChildren<SpriteRenderer>().sortingLayerName == "Projectiles")
        {
            Instantiate(bulletFrag, new Vector3(collision.GetContact(0).point.x, collision.GetContact(0).point.y, 0f), Quaternion.AngleAxis(180f, Vector3.up));
            Destroy(bullet);
            TakeDamage(AgentMovement.Instance.GetComponentInChildren<Shoot>().selectedGun.damage); //Pass bullet damage here.
            Debug.Log(AgentMovement.Instance.GetComponentInChildren<Shoot>().selectedGun.damage);
        }
        else if (bullet.GetComponentInChildren<SpriteRenderer>().sortingLayerName == "Player")
        {
            playerScript.TakeDamage(1);
        }
    }

    public void TakeDamage(float damage)
    {
        animator.Play("Base Layer.Hitstun", 0);
        hp -= damage;
        slider.value = (hp / total_hp) * 0.8f + 0.2f; //Bound slider from 0.3f to 1f, slider looks ugly when going below 0.3f;
        if (hp <= 0)
        {
            GetComponentInChildren<CapsuleCollider2D>().enabled = false;
            animator.Play("Base Layer.Death", 0);
            GameObject coindrop = GameObject.Instantiate(coinDrop, transform.position, Quaternion.identity, coinDropParent.transform);
            //We can use the coindrop GO to set coin values.
            GetComponentInParent<SpawnManager>().EnemyDestroyed(transform.position);
        }
    }

    public void SelfDestroy()
    {
        Destroy(transform.gameObject);
    }

    public void StopMoving()
    {
        agent.isStopped = true;
    }

    public void ResumeMoving()
    {
        agent.isStopped = false;
    }

    public void StartAttacking()
    {
        attacking = true;
        StartCoroutine(AttackStopTimer());
    }
    public void StopAttacking()
    {
        attacking = false;
        canAttack = true;
    }

    public IEnumerator AttackDelay()
    {
        yield return new WaitForSecondsRealtime(attackDelay);
        animator.SetTrigger("Looping");
        animator.Play("Base Layer.AttackAnticipation", 0);
    }

    public IEnumerator AttackStopTimer()
    {
        yield return new WaitForSecondsRealtime(attackDelay);
        animator.ResetTrigger("Looping");
    }
}
