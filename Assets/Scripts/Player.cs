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
    public int money;

    public GameObject moneyDisplay;

    // Start is called before the first frame update
    void Awake()
    {
        hp = 5;
        max_hp = hp;
        money = 0;
        moneyDisplay = GameObject.Find("MoneyText");
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
            money++;
            if (money < 10) {
                moneyDisplay.GetComponent<TextMeshProUGUI>().text = "0" + money.ToString();
            }
            else {
                moneyDisplay.GetComponent<TextMeshProUGUI>().text = money.ToString();
            }

            Destroy(entity);
        }
        else if (entity.GetComponent<SpriteRenderer>().sortingLayerName == "Exit") {
            //Do action to exit level
            SceneManager.LoadScene("Intro");
        }


    }

    public void TakeDamage(int damage)
    {
        hp -= damage;
        if(hp <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        Debug.Log("DEATH");
        transform.parent.gameObject.SetActive(false);
    }
}
