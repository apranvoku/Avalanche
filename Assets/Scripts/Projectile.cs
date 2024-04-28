using OpenCover.Framework.Model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public GameObject bulletFrag;
    public float bulletSpeed;
    public AgentRotate ar;
    public Vector3 velocityVector;
    public Vector3 JitterOffset;
    public float Jitter;
    public bool rocketBullet;
    private Gun firedGun;

    public float bulletTimeStamp;
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, 2f);
        ar = GameObject.Find("Character").GetComponent<AgentRotate>();
        transform.position += ar.PlayerToMouse.normalized * 6f; //Set the projectile to spawn a bit in front of the player.
        transform.rotation = Quaternion.LookRotation(Vector3.forward, ar.PlayerToMouse); // Set Initial rotation;
        Jitter = 0.1f;
        JitterOffset = new Vector3(Random.Range(-Jitter, Jitter), Random.Range(-Jitter, Jitter), 0f);
        velocityVector = ar.PlayerToMouse.normalized + JitterOffset;
        firedGun = AgentMovement.Instance.GetComponentInChildren<Shoot>().selectedGun;
        if (firedGun.GetType() == typeof(Rocketlauncher))
        {
            bulletTimeStamp = Time.time;
            bulletSpeed /= 10;
            rocketBullet = true;
        }
        else { rocketBullet = false; }

    }

    // Update is called once per frame
    void Update()
    {
        if (rocketBullet)
        {
            transform.position += velocityVector.normalized * Time.deltaTime * bulletSpeed * Mathf.Pow((1f + (Time.time - bulletTimeStamp)), 3);
        }
        else
        {
            transform.position += velocityVector.normalized * Time.deltaTime * bulletSpeed;
        }
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject enemy = collision.gameObject;
        if (enemy.GetComponentInChildren<SpriteRenderer>().sortingLayerName == "Enemy")
        {
            Instantiate(bulletFrag, new Vector3(collision.GetContact(0).point.x, collision.GetContact(0).point.y, 0f), Quaternion.AngleAxis(180f, Vector3.up));
            if (rocketBullet)
            {
                Debug.Log("RocketBullet!");
                //int CircleCast(Vector2 origin, float radius, Vector2 direction, ContactFilter2D contactFilter, List<RaycastHit2D> results, float distance = Mathf.Infinity); 
                List<RaycastHit2D> hitResults = new List<RaycastHit2D>();
                ContactFilter2D filter = new ContactFilter2D();
                Physics2D.CircleCast(transform.position, 30f, Vector2.right, filter.NoFilter(), hitResults);

                foreach (RaycastHit2D hitResult in hitResults)
                {
                    Debug.Log(hitResult.transform.position);
                    Debug.Log(hitResult.transform.transform.name);
                    if (hitResult.transform.GetComponentInChildren<Enemy>() != null)
                    {
                        hitResult.transform.GetComponentInChildren<Enemy>().TakeDamage(firedGun.damage);
                    }
                    if (hitResult.transform.GetComponentInChildren<EnemyCobra>() != null)
                    {
                        hitResult.transform.GetComponentInChildren<EnemyCobra>().TakeDamage(firedGun.damage);
                    }
                    if (hitResult.transform.GetComponentInChildren<EnemyViper>() != null)
                    {
                        hitResult.transform.GetComponentInChildren<EnemyViper>().TakeDamage(firedGun.damage);
                    }
                }
            }
            else
            {
                if (enemy.transform.GetComponentInChildren<Enemy>() != null)
                {
                    enemy.transform.GetComponentInChildren<Enemy>().TakeDamage(firedGun.damage);
                }
                if (enemy.transform.GetComponentInChildren<EnemyCobra>() != null)
                {
                    enemy.transform.GetComponentInChildren<EnemyCobra>().TakeDamage(firedGun.damage);
                }
                if (enemy.transform.GetComponentInChildren<EnemyViper>() != null)
                {
                    enemy.transform.GetComponentInChildren<EnemyViper>().TakeDamage(firedGun.damage);
                }
            }
            Debug.Log(firedGun.damage);
        }
        Destroy(transform.gameObject);
    }
}
