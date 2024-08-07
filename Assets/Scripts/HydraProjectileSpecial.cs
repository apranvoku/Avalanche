//using OpenCover.Framework.Model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HydraProjectileSpecial : MonoBehaviour
{
    public float bulletSpeed;
    public GameObject player;
    public Vector3 velocityVector;
    public Vector3 JitterOffset;
    public float Jitter;
    public Player playerScript;
    private EnemyHydra hydra;
    private bool updateTarget;

    // Start is called before the first frame update
    void Start()
    {
        Jitter = 0f;
        bulletSpeed = 70f;
        hydra = GetComponentInParent<EnemyHydra>();
        Destroy(gameObject, 10f);
        player = GameObject.Find("Agent");
        playerScript = player.GetComponentInChildren<Player>();
        JitterOffset = new Vector3(Random.Range(-Jitter, Jitter), Random.Range(-Jitter, Jitter), 0f);
        velocityVector = transform.right.normalized + JitterOffset;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += velocityVector.normalized * Time.deltaTime * bulletSpeed;
        if(Vector3.Distance(hydra.transform.position, transform.position) > 150f && !updateTarget)
        {
            bulletSpeed = 30f;
            velocityVector = player.transform.position - transform.position;
            updateTarget = true;
        }
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject bullet = collision.gameObject;
        if (bullet.GetComponentInChildren<SpriteRenderer>().sortingLayerName == "Player")
        {
            playerScript.TakeDamage(1);
        }
    }
}
