using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class EnemyGardenSnake : Enemy
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
    
    public List<Item> ItemDropList;

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
        base.dead = false;
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
                GameObject.Instantiate(loot.drop, transform.position, Quaternion.identity, coinDropParent.transform);
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
        agent.isStopped = true;
    }

    public override void ResumeMoving()
    {
        agent.isStopped = false;
    }

}
