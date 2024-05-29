using OpenCover.Framework.Model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public GameObject bulletFrag;
    public float bulletSpeed;
    private int PenetrationsLeft;//Based on penetration upgrade level
    private AgentRotate ar;
    private Vector3 velocityVector;
    private Vector3 JitterOffset;
    public float Jitter;
    public bool rocketBullet;
    private Gun firedGun;
    public float range;

    public float bulletTimeStamp;
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, range);

        ar = GameObject.Find("Character").GetComponent<AgentRotate>();
        transform.position += ar.PlayerToMouse.normalized * 6f; //Set the projectile to spawn a bit in front of the player.
        transform.rotation = Quaternion.LookRotation(Vector3.forward, ar.PlayerToMouse); // Set Initial rotation;
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
        PenetrationsLeft = firedGun.penetration;
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
                //int CircleCast(Vector2 origin, float radius, Vector2 direction, ContactFilter2D contactFilter, List<RaycastHit2D> results, float distance = Mathf.Infinity); 
                List<RaycastHit2D> hitResults = new List<RaycastHit2D>();
                ContactFilter2D filter = new ContactFilter2D();
                Physics2D.CircleCast(transform.position, (float)firedGun.penetration, Vector2.right, filter.NoFilter(), hitResults);

                foreach (RaycastHit2D hitResult in hitResults)
                {
                    if (hitResult.transform.GetComponentInChildren<Enemy>() != null)
                    {
                        hitResult.transform.GetComponentInChildren<Enemy>().TakeDamage(firedGun.damage);
                    }
                }
                Destroy(transform.gameObject);
            }
            else
            {
                if (enemy.transform.GetComponentInChildren<Enemy>() != null)
                {
                    enemy.transform.GetComponentInChildren<Enemy>().TakeDamage(firedGun.damage);
                }
                PenetrationsLeft -= 1;
                if (PenetrationsLeft <= 0)
                {
                    Destroy(transform.gameObject);
                }
            }
        }
    }
}
