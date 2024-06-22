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

    public GameObject OuroborousStatue;
    public Transform HydraCenter;

    public GameObject HydraBullet1;
    public GameObject HydraBullet2;

    public bool phase1;
    public bool phase2;
    public bool phase3;
    public bool phase4;

    public bool phase2started;
    public bool phase3started;
    public bool phase4started;
    public bool hydraDead;

    public GameObject player;
    public Player playerScript;
    Transform bossPips;
    NavMeshAgent agent;
    public float hp;
    private float total_hp;
    public GameObject coinDropParent;
    private Animator animator;

    public List<Item> ItemDropList;

    private int enemyIndex;

    // Start is called before the first frame update
    void Awake()
    {
        phase1 = true;
        phase2 = false;
        phase3 = false;
        phase4 = false;

        phase2started = false;
        phase3started = false;
        phase4started = false;
        hydraDead = false;

        total_hp = hp;
        //hp = 1f * hp; //Phase test
        player = GameObject.Find("Agent");
        playerScript = player.GetComponentInChildren<Player>();
        coinDropParent = GameObject.Find("coinDropParent");
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        bossPips = GameObject.Find("BossHP").transform.GetChild(1);
        animator = GetComponentInChildren<Animator>();
        base.dead = false;

        enemyIndex = SpawnManager.enemyIndex % 20;
        //Debug.Log(enemyIndex);
        SpawnManager.enemyIndex++;
        agent.avoidancePriority = SpawnManager.enemyseed;
    }


    // Update is called once per frame
    void Update()
    {
        if (phase1)
        {
            StartCoroutine(RoutineA());
            phase1 = false;
        }
        else if (phase2)
        {
            StartCoroutine(RoutineB());
            phase2 = false;
        }
        else if (phase3)
        {
            StartCoroutine(BeginPhaseC());
            StartCoroutine(RoutineC());
            phase3 = false;
        }
        else if (phase4)
        {
            StartCoroutine(RoutineD());
            phase4 = false;
        }
    }
    public IEnumerator RoutineA()
    {
        while (!phase2started)
        {
            for (int i = 0; i < 3; i++)
            {
                GameObject bullet = Instantiate(HydraBullet1, transform.GetChild(i + 1).GetChild(0).position, transform.GetChild(i + 1).GetChild(0).rotation, transform.GetChild(i + 1).GetChild(0));
                bullet.GetComponent<HydraProjectile>().Jitter = 0;
                bullet.GetComponent<HydraProjectile>().bulletSpeed = 30;
                bullet.transform.parent = bullet.transform.parent.parent; //Move parent so bullets keeps its trajectory.
                transform.GetChild(i + 1).transform.GetChild(0).transform.Rotate(0f, 0f, 120f);
            }
            Head1.transform.GetChild(0).transform.Rotate(0f, 0f, 5f);
            Head2.transform.GetChild(0).transform.Rotate(0f, 0f, 5f);
            Head3.transform.GetChild(0).transform.Rotate(0f, 0f, 5f);
            yield return new WaitForSecondsRealtime(0.15f);
        }
        yield return null;
    }

    public IEnumerator RoutineB()
    {
        int i = 0;
        while (!phase3started)
        {
            //transform GetChild(1) is head, transfrom.GetChild(1).GetChild(0) is empty head rotation
            GameObject bullet = Instantiate(HydraBullet2, transform.GetChild(i % 5 + 1).position, transform.GetChild(1).GetChild(0).rotation, transform.GetChild(1).GetChild(0));
            bullet.transform.parent = bullet.transform.parent.parent; //Move parent so bullets keeps its trajectory.
            transform.GetChild(1).transform.GetChild(0).transform.Rotate(0f, 0f, 10f); //Rotate parent object for next bullets
            yield return new WaitForSecondsRealtime(0.15f);
            i++;
        }
        yield return null;
    }

    public IEnumerator RoutineC()
    {
        while (!phase4started)
        {
            HydraCenter.Rotate(0f, 0f, Time.deltaTime * 60f);
            yield return null;
        }
        yield return null;
    }

    public IEnumerator RoutineD()
    {
        while (!hydraDead)
        {
            for (int i = 0; i < 16; i++)
            {
                //transform GetChild(1) is head, transfrom.GetChild(1).GetChild(0) is empty head rotation
                GameObject bullet = Instantiate(HydraBullet1, transform.GetChild(1).GetChild(0).position, transform.GetChild(1).GetChild(0).rotation, transform.GetChild(1).GetChild(0));
                bullet.GetComponent<HydraProjectile>().Jitter = 0;
                bullet.GetComponent<HydraProjectile>().bulletSpeed = 30;
                bullet.transform.parent = bullet.transform.parent.parent; //Move parent so bullets keeps its trajectory.
                transform.GetChild(1).transform.GetChild(0).transform.Rotate(0f, 0f, 10f); //Rotate parent object for next bullets
            }
            transform.GetChild(1).transform.GetChild(0).transform.Rotate(0f, 0f, 5f); //Rotate parent object for next bullets
            yield return new WaitForSecondsRealtime(1f);
        }
        yield return null;
    }

    public IEnumerator BeginPhaseC()
    {
        GameObject Statue1 = GameObject.Instantiate(OuroborousStatue, HydraCenter.position, Quaternion.identity, HydraCenter);
        GameObject Statue2 = GameObject.Instantiate(OuroborousStatue, HydraCenter.position, Quaternion.identity, HydraCenter);
        GameObject Statue3 = GameObject.Instantiate(OuroborousStatue, HydraCenter.position, Quaternion.identity, HydraCenter);
        GameObject Statue4 = GameObject.Instantiate(OuroborousStatue, HydraCenter.position, Quaternion.identity, HydraCenter);
        for (float distance = 0; distance < 150f; distance += Time.deltaTime * 150f)
        {
            Statue1.transform.position += new Vector3(Time.deltaTime * 150f, 0f, 0f);
            Statue2.transform.position += new Vector3(-Time.deltaTime * 150f, 0f, 0f);
            Statue3.transform.position += new Vector3(0, Time.deltaTime * 150f, 0f);
            Statue4.transform.position += new Vector3(0, -Time.deltaTime * 150f, 0f);
            yield return null;
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
        float hp_fraction = hp / total_hp;
        for (float i = bossPips.childCount-1; i >= 0; i--)
        {

            if(i/(bossPips.childCount-1) > hp_fraction)
            {
                if(bossPips.GetChild((int)i).gameObject.activeSelf)
                {
                    bossPips.GetChild((int)i).gameObject.SetActive(false);
                }
            }
        }
        //slider.value = (hp / total_hp) * 0.8f + 0.2f; //Bound slider from 0.3f to 1f, slider looks ugly when going below 0.3f;
        if (hp <= 0)
        {
            GetComponent<PolygonCollider2D>().enabled = false;
            //animator.Play("Base Layer.Death", 0);
            if (!base.dead)
            {
                DropItem();
            }
            base.dead = true;
            hydraDead = true;
            Destroy(gameObject);
            //Need hydra death anim.
        }
        else if (hp / total_hp <= 0.25f && !phase4started)
        {
            phase4started = true;
            phase4 = true;
        }
        else if (hp / total_hp <= 0.5f && !phase3started)
        {
            phase3started = true;
            phase3 = true;
        }
        else if (hp / total_hp <= 0.75f && !phase2started)
        {
            phase2started = true;
            phase2 = true;
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