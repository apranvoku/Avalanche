using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class CoinAttract : MonoBehaviour
{
    public GameObject player;
    NavMeshAgent agent;
    public float pickupDistance;

    // Start is called before the first frame update
    void Awake()
    {
        player = GameObject.Find("Agent");
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
    }


    // Update is called once per frame
    void Update()
    {
        //Replace 5f with pickup radius future.
        if(Vector3.Distance(player.transform.position, agent.transform.position) < pickupDistance)
        {
            agent.destination = player.transform.position;
            agent.velocity = agent.desiredVelocity;
        }
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {

    }
}
