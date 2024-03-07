using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public GameObject player;
    NavMeshAgent agent;

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
        agent.destination = player.transform.position;
    }
}
