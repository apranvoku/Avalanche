using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class EnemyCobra : Enemy
{
    private GameObject player;
    private Player playerScript;
    Slider slider;
    NavMeshAgent agent;
    private float total_hp;

    private bool canAttack;
    private bool readyToFire;
    private bool firing;
    private bool attacking;
    private GameObject coinDropParent;
    private GameObject enemyProjectilleParent;
    private Animator animator;
    private Vector3 distToPlayer;

    //needs to be set in editor
    public GameObject poisonBullet;
    public GameObject enemyProjectileOrigin;
    public float hp;
    public float fireDelay;
    public float attackDelay;
    public float attackRange;


    public List<Item> ItemDropList;

    // Start is called before the first frame update
    void Awake()
    {
        //hp = 40;
        total_hp = hp;
        player = GameObject.Find("Agent");
        playerScript = player.GetComponentInChildren<Player>();
        coinDropParent = GameObject.Find("coinDropParent");
        enemyProjectilleParent = GameObject.Find("EnemyProjectilleParent");
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        slider = GetComponentInChildren<Slider>();
        animator = GetComponentInChildren<Animator>();
        readyToFire = false;
        firing = false;
        attacking = false;
        canAttack = true;
        //fireDelay = 0.1f;
        //attackRange = 100f;
        //attackDelay = 5f;

        base.dead = false;
    }


    // Update is called once per frame
    void Update()
    {
        if (!attacking)
        {
            if (canAttack && (Vector3.Distance(transform.position, player.transform.position) <= attackRange) && (hp>0))
            {
                canAttack = false;
                StartCoroutine(AttackDelay());
                attacking = true;
                StopMoving();
                animator.Play("Base Layer.AttackAnticipation", 0);
            }
            else
            {
                agent.destination = player.transform.position;
            }
        }
        else if (firing && readyToFire)
        {
            readyToFire = false;
            Shoot();
        }
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
            GetComponent<CircleCollider2D>().enabled = false;
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
        agent.velocity = Vector3.zero;
        agent.isStopped = true;
    }

    public override void ResumeMoving()
    {
        agent.isStopped = false;
    }

    Quaternion GetRotationToTarget(GameObject targetObject)
    {
        // Calculate the direction vector from this GameObject to the targetObject
        Vector3 direction = targetObject.transform.position - enemyProjectileOrigin.transform.position;

        // Calculate the angle in degrees around the z-axis
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // Create a quaternion that rotates around the z-axis by the calculated angle
        Quaternion rotation = Quaternion.Euler(0, 0, angle);

        return rotation;
    }

    public void Shoot()
    {
        Instantiate(poisonBullet, enemyProjectileOrigin.transform.position, enemyProjectileOrigin.transform.rotation * GetRotationToTarget(player), enemyProjectilleParent.transform);
        StartCoroutine(FireDelay());
    }

    public void BeginShoot()
    {
        readyToFire = true;
        firing = true;
    }
    public void StopShoot()
    {
        readyToFire = false;
        firing = false;
    }

    public void StopAttacking()
    {
        readyToFire = false;
        firing = false;
        attacking = false;
    }

    public IEnumerator FireDelay()
    {
        yield return new WaitForSeconds(fireDelay);
        if (hp > 0)
        {
            readyToFire = true;
        }
    }

    public IEnumerator AttackDelay()
    {
        yield return new WaitForSeconds(attackDelay);
        if(hp > 0)
        {
            canAttack = true;
        }
    }

}
