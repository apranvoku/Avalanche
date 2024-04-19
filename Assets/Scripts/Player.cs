using System.Collections;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Diagnostics.CodeAnalysis;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public float hp;
    private float max_hp;
    public Transform heartContainer;
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
        UpdateHearts((int)hp);
        if(hp <= 0)
        {
            GetComponent<CircleCollider2D>().enabled = false;
            animator.Play("Base Layer.Death", 0);
            //Die stuff
        }
        else
        {
            StartCoroutine(HitStunDelay(0.2f));
        }
    }

    public IEnumerator HitStunDelay(float delay)
    {
        yield return new WaitForSecondsRealtime(delay);
        transform.parent.GetComponent<AgentMovement>().enabled = true;
    }

    public void Die()
    {
        
    }

    public void UpdateHearts(int heartsRemaining)
    {
        foreach(Transform heart in heartContainer) 
        { 
            if(heartsRemaining > 0)
            {
                heart.gameObject.SetActive(true);
            }
            else
            {
                heart.gameObject.SetActive(false);
            }
            heartsRemaining--;
        }
    }
}
