using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class EnemyMommaSnake : Enemy
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
    private Vector3 distToPlayer;
    public GameObject baby;
    public  GameObject babySpawns;

    // Start is called before the first frame update
    void Awake()
    {
        hp = 90;
        total_hp = hp;
        player = GameObject.Find("Agent");
        playerScript = player.GetComponentInChildren<Player>();
        coinDropParent = GameObject.Find("coinDropParent");
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        slider = GetComponentInChildren<Slider>();
        animator = GetComponentInChildren<Animator>();
    }


    // Update is called once per frame
    void Update()
    {
        agent.destination = player.transform.position;
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
            GetComponent<BoxCollider2D>().enabled = false;
            animator.Play("Base Layer.Death", 0);
            GameObject coindrop = GameObject.Instantiate(coinDrop, transform.position, Quaternion.identity, coinDropParent.transform);
            //We can use the coindrop GO to set coin values.
        }
    }

    public override void SelfDestroy()
    {
        foreach (Transform child in babySpawns.transform)
        {
            // Instantiate the prefab at the child's position and rotation
            Instantiate(baby, child.position, child.rotation, transform.parent);
            GetComponentInParent<SpawnManager>().EnemySpawned();
        }
        GetComponentInParent<SpawnManager>().EnemyDestroyed(transform.position);
        Destroy(transform.gameObject);
    }

    public override void StopMoving()
    {
        agent.isStopped = true;
    }

    public override void ResumeMoving()
    {
        agent.isStopped = false;
    }

    public void SpawnEnemies()
    {
    }
}
