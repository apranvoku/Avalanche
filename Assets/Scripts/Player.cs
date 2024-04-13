using System.Collections;
using System.Collections.Generic;
using System.Data.SqlTypes;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public float hp;
    private float max_hp;
    private Animator animator;

    // Start is called before the first frame update
    void Awake()
    {
        hp = 5;
        max_hp = hp;
        animator = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject entity = collision.gameObject;
        if (entity.GetComponentInChildren<SpriteRenderer>().sortingLayerName == "Items")
        {
            Shop.Instance.GetMoney(10);

            Destroy(entity);
        }
        else if (LayerMask.LayerToName(entity.gameObject.layer) == "Exit") {
            //Do action to exit level
            //SceneManager.LoadScene("Intro");
            GetComponent<CircleCollider2D>().enabled = false;
            transform.parent.GetComponent<AgentMovement>().ExitAnimation(entity.transform.position);
        }


    }

    public void TakeDamage(int damage)
    {
        transform.parent.GetComponent<AgentMovement>().enabled = false;
        animator.Play("Base Layer.Hitstun", 0);
        hp -= damage;
        if(hp <= 0)
        {
            GetComponent<CircleCollider2D>().enabled = false;
            animator.Play("Base Layer.Death", 0);
        }
    }

    public void Die()
    {
        
    }
}
