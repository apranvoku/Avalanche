using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
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
    }


    // Update is called once per frame
    void Update()
    {
        agent.destination = player.transform.position;
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject bullet = collision.gameObject;
        if (bullet.GetComponentInChildren<SpriteRenderer>().sortingLayerName == "Projectiles")
        {
            Destroy(bullet);
            TakeDamage(10); //Pass bullet damage here.
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
            GameObject coindrop = GameObject.Instantiate(coinDrop, transform.position, Quaternion.identity, coinDropParent.transform);
            //We can use the coindrop GO to set coin values.
            SpawnManager.Instance.EnemyDestroyed(transform.position);
            Destroy(transform.gameObject);
        }
    }

    public void StopMoving()
    {
        agent.isStopped = true;
    }

    public void ResumeMoving()
    {
        agent.isStopped = false;
    }

}
