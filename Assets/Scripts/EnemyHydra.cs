using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class EnemyHydra : Enemy
{
    public GameObject Head1;
    public GameObject Head2;
    public GameObject Head3;
    public GameObject Head4;
    public GameObject Head5;


    public GameObject HydraBullet1;
    public GameObject HydraBullet2;
    public GameObject HydraBullet3;

    private bool phase1;
    private bool phase2;
    private bool phase3;
    private bool phase4;

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

    private int enemyIndex;

    // Start is called before the first frame update
    void Awake()
    {
        phase1 = true;
        phase2 = false; 
        phase3 = false;
        phase4 = false;

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

        enemyIndex = SpawnManager.enemyIndex % 20;
        //Debug.Log(enemyIndex);
        SpawnManager.enemyIndex++;
        agent.avoidancePriority = SpawnManager.enemyseed;
        StartCoroutine(PhaseOneShoot());
    }


    // Update is called once per frame
    void Update()
    {
        if(phase1)
        {

        }
        else if(phase2)
        {

        }
        else if(phase3)
        {

        }
        else if (phase4)
        {

        }
    }

    public IEnumerator PhaseOneShoot()
    {
        while(phase1)
        {
            for(int i = 0; i  < 4; i++)
            {
                GameObject bullet = Instantiate(HydraBullet1, Head1.transform.GetChild(0).position, Head1.transform.GetChild(0).rotation, Head1.transform.GetChild(0));
                bullet.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
                bullet.GetComponent<HydraProjectile>().Jitter = 0;
                bullet.GetComponent<HydraProjectile>().bulletSpeed = 30;
                bullet.transform.parent = bullet.transform.parent.parent; //Move parent so bullets keeps its trajectory.
                Head1.transform.GetChild(0).transform.Rotate(0f, 0f, 120f);
            }
            Head1.transform.GetChild(0).transform.Rotate(0f, 0f, 5f);
            yield return new WaitForSecondsRealtime(0.15f);
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
        //animator.Play("Base Layer.Hitstun", 0);
        hp -= damage;
        slider.value = (hp / total_hp) * 0.8f + 0.2f; //Bound slider from 0.3f to 1f, slider looks ugly when going below 0.3f;
        if (hp <= 0)
        {
            GetComponent<CircleCollider2D>().enabled = false;
            //animator.Play("Base Layer.Death", 0);
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
