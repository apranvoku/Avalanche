using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float bulletSpeed;
    public AgentRotate ar;
    public Vector3 velocityVector;
    public Vector3 JitterOffset;
    public float Jitter;
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, 2f);
        ar = GameObject.Find("Character").GetComponent<AgentRotate>();
        transform.position += ar.PlayerToMouse.normalized * 3f; //Set the projectile to spawn a bit in front of the player.
        transform.rotation = Quaternion.LookRotation(Vector3.forward, ar.PlayerToMouse); // Set Initial rotation;
        Jitter = 50f;
        JitterOffset = new Vector3(Random.Range(-Jitter, Jitter), Random.Range(-Jitter, Jitter), 0f);
        velocityVector = ar.PlayerToMouse + JitterOffset;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += velocityVector.normalized * Time.deltaTime * bulletSpeed;
    }
}
