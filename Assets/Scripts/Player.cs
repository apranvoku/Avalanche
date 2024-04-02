using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float hp;
    private float max_hp;

    // Start is called before the first frame update
    void Awake()
    {
        hp = 5;
        max_hp = hp;
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
            Destroy(entity);
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
