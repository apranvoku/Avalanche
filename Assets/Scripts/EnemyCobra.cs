using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class EnemyCobra : MonoBehaviour
{
    public GameObject player;
    public Player playerScript;
    Slider slider;
    NavMeshAgent agent;
    public float hp;
    private float total_hp;
    private float fireDelay;
    private float attackDelay;
    public bool canAttack;
    public bool readyToFire;
    public bool firing;
    private bool attacking;
    public GameObject coinDrop;
    public GameObject coinDropParent;
    public GameObject enemyProjectilleParent;
    private Animator animator;
    public GameObject bulletFrag;
    public float attackRange;
    private Vector3 distToPlayer;
    //needs to be set in editor
    public GameObject poisonBullet;
    public GameObject enemyProjectileOrigin;

    // Start is called before the first frame update
    void Awake()
    {
        hp = 30;
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
        fireDelay = 0.1f;
        readyToFire = false;
        firing = false;
        attacking = false;
        canAttack = true;
        attackRange = 100f;
        attackDelay = 5f;
    }


    // Update is called once per frame
    void Update()
    {
        if (!attacking)
        {
            if (canAttack && (Vector3.Distance(transform.position, player.transform.position) <= attackRange))
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
        GameObject bullet = collision.gameObject;
        if (bullet.GetComponentInChildren<SpriteRenderer>().sortingLayerName == "Projectiles")
        {
            Instantiate(bulletFrag, new Vector3(collision.GetContact(0).point.x, collision.GetContact(0).point.y, 0f), Quaternion.AngleAxis(180f, Vector3.up));
            Destroy(bullet);
            TakeDamage(AgentMovement.Instance.GetComponentInChildren<Shoot>().selectedGun.damage); //Pass bullet damage here.
            Debug.Log(AgentMovement.Instance.GetComponentInChildren<Shoot>().selectedGun.damage);
        }
        else if(bullet.GetComponentInChildren<SpriteRenderer>().sortingLayerName == "Player")
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
            GetComponent<CircleCollider2D>().enabled = false;
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

    Quaternion GetRotationToTarget(GameObject targetObject)
    {
        // Calculate the direction vector from this GameObject to the targetObject
        Vector3 direction = targetObject.transform.position - transform.position;

        // Calculate the angle in degrees around the z-axis
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // Create a quaternion that rotates around the z-axis by the calculated angle
        Quaternion rotation = Quaternion.Euler(0, 0, angle);

        return rotation;
    }

    public void Shoot()
    {
        Instantiate(poisonBullet, enemyProjectileOrigin.transform.position, transform.rotation * GetRotationToTarget(player), enemyProjectilleParent.transform);
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
        yield return new WaitForSecondsRealtime(fireDelay);
        readyToFire = true;
    }

    public IEnumerator AttackDelay()
    {
        yield return new WaitForSecondsRealtime(attackDelay);
        if(!animator.GetCurrentAnimatorStateInfo(0).IsName("Death"))
        {
            canAttack = true;
        }
    }

}
