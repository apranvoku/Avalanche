using OpenCover.Framework.Model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    public float bulletSpeed;
    public GameObject player;
    public Vector3 velocityVector;
    public Vector3 JitterOffset;
    public float Jitter;
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, 2f);
        player = GameObject.Find("Agent");
        Jitter = 0.1f;
        JitterOffset = new Vector3(Random.Range(-Jitter, Jitter), Random.Range(-Jitter, Jitter), 0f);
        velocityVector = transform.right.normalized + JitterOffset;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += velocityVector.normalized * Time.deltaTime * bulletSpeed;
    }
}
