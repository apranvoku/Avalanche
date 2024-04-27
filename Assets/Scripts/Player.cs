using System.Collections;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Diagnostics.CodeAnalysis;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public float hp;
    private float max_hp;
    public Transform heartContainer;
    public Transform redVignette;
    private Animator animator;
    private float flashDuration;
    private float invincibilityLength;

    // Start is called before the first frame update
    void Awake()
    {
        hp = 5;
        max_hp = hp;
        animator = GetComponentInChildren<Animator>();
        flashDuration = 0.2f;
        invincibilityLength = .75f;
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
            transform.parent.GetComponent<AgentMovement>().ExitAnimation(entity.transform.position);
        }


    }

    public void TakeDamage(int damage)
    {
        transform.parent.GetComponent<AgentMovement>().enabled = false;
        StartCoroutine(FlashRed());
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
            StartCoroutine(InvincibilityTimer());
        }
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

    public IEnumerator FlashRed()
    {
        float elapsedTime = 0f;
        Color red = redVignette.GetComponent<Image>().color;
        Color targetRed = new Color(red.r, red.g, red.b, 130f / 255f);

        while (elapsedTime < flashDuration)
        {
            float t = elapsedTime / flashDuration;
            redVignette.GetComponent<Image>().color = Color.Lerp(red, targetRed, t);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        //yield return new WaitForSeconds(0.1f);

        elapsedTime = 0f;

        while (elapsedTime < flashDuration)
        {
            float t = elapsedTime / flashDuration;
            redVignette.GetComponent<Image>().color = Color.Lerp(targetRed, red, t);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure final alpha value
        redVignette.GetComponent<Image>().color = red;


    }

    public IEnumerator InvincibilityTimer()
    {
        GetComponent<CircleCollider2D>().enabled = false;
        yield return new WaitForSecondsRealtime(invincibilityLength);
        GetComponent<CircleCollider2D>().enabled = true;
    }
}
