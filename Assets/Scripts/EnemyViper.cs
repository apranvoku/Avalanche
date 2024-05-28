using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class EnemyViper : Enemy
{

    public GameObject player;
    public Player playerScript;
    Slider slider;
    NavMeshAgent agent;
    public float hp;
    private float total_hp;
    public GameObject coinDropParent;
    private Animator animator;
    private Vector3 distToPlayer;
    public float attackRange;
    private float attackDelay;
    private float attackLungelength;
    private bool attacking;
    public bool canAttack;
    public CapsuleCollider2D idleHitbox;
    public CapsuleCollider2D lungeHitbox;

    private float speed;
    private float dashSpeed;

    public List<Item> ItemDropList;

    // Start is called before the first frame update
    void Awake()
    {
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
        attackDelay = 3f;
        attackLungelength = 3f;

        speed = 5f;
        dashSpeed = 100f;

        base.dead = false;
    }

    // Update is called once per frame
    void Update()
    {
        distToPlayer = player.transform.position - transform.position;
        if (distToPlayer.x > 0)
        {
            if (transform.localScale.x < 0)
            {
                transform.localScale = new Vector3(transform.localScale.x * -1f, transform.localScale.y, transform.localScale.z);
            }
        }
        else if (distToPlayer.x < 0)
        {
            if (transform.localScale.x > 0)
            {
                transform.localScale = new Vector3(transform.localScale.x * -1f, transform.localScale.y, transform.localScale.z);
            }
        }

        if (!attacking)
        {
            if ((Vector3.Distance(transform.position, player.transform.position) <= attackRange) && canAttack)
            {
                animator.SetTrigger("Awake");
                canAttack = false;
                StartCoroutine(AttackDelay());
            }
        }

        if (!agent.isStopped)
        {
            agent.destination = player.transform.position;
        }
        
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponentInChildren<SpriteRenderer>().sortingLayerName == "Player")
        {
            playerScript.TakeDamage(1);
        }
    }

    public override void TakeDamage(float damage)
    {
        animator.Play("Base Layer.Hitstun", 0);
        hp -= damage;
        slider.value = (hp / total_hp) * 0.8f + 0.2f; //Bound slider from 0.3f to 1f, slider looks ugly when going below 0.3f;
        if (hp <= 0)
        {
            idleHitbox.enabled = false;
            lungeHitbox.enabled = false;
            animator.Play("Base Layer.Death", 0);
            if (!base.dead)
            {
                DropItem();
            }
            base.dead = true;
            //We can use the coindrop GO to set coin values.
        }
    }
    public void DropItem()
    {
        foreach (Item loot in ItemDropList)
        {
            if (loot.dropRate > Random.Range(0, 100))
            {
                GameObject coindrop = GameObject.Instantiate(loot.drop, transform.position, Quaternion.identity, coinDropParent.transform);
            }


        }
    }

    public override void SelfDestroy()
    {
        GetComponentInParent<SpawnManager>().EnemyDestroyed(transform.position);
        Destroy(transform.gameObject);
    }

    public override void StopMoving()
    {
        lungeHitbox.enabled = false;
        agent.destination = transform.position;
        agent.isStopped = true;
    }

    public override void ResumeMoving()
    {
        agent.isStopped = false;
    }

    public void StartAttacking()
    {
        attacking = true;
        lungeHitbox.enabled = true;
        StartCoroutine(AttackStopTimer());
    }
    public void StopAttacking()
    {
        lungeHitbox.enabled = false;
        attacking = false;
        canAttack = true;
    }

    public void GoToSleep()
    {
        animator.ResetTrigger("Awake");
    }
    public void SpeedUp()
    {
        agent.speed = dashSpeed;
    }

    public void SlowDown()
    {
        agent.speed = speed;
    }

    public IEnumerator AttackDelay()
    {
        yield return new WaitForSeconds(attackDelay);
        animator.SetTrigger("Looping");
        if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Death"))
        {
            animator.Play("Base Layer.AttackAnticipation", 0);
        }
    }

    public IEnumerator AttackStopTimer()
    {
        yield return new WaitForSeconds(attackLungelength);
        animator.ResetTrigger("Looping");
    }
}
