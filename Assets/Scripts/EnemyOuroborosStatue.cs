using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyOuroborosStatue : Enemy
{

    private Slider slider;
    public float hp;
    private float total_hp;
    private Animator animator;
    private GameObject coinDropParent;

    public List<Item> ItemDropList;

    // Start is called before the first frame update
    void Awake()
    {
        hp = 300;
        total_hp = hp;
        slider = GetComponentInChildren<Slider>();
        base.dead = false;
        animator = GetComponentInChildren<Animator>();
        coinDropParent = GameObject.Find("coinDropParent");
        PositionOverride(); //Hacky method to make the statue not spawn on the boss

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

    /*
     * Hacky Method to make the statue not spawn on te final boss
     */
    private void PositionOverride()
    {
        if (transform.position.x == 0)
        {
            transform.position = new Vector3(100,260, transform.position.z);
        }
    }


    public override void SelfDestroy()
    {
        GetComponentInParent<SpawnManager>().EnemyDestroyed(transform.position);
        Destroy(transform.gameObject);
    }

    public override void StopMoving() { }

    public override void ResumeMoving() { }
}
