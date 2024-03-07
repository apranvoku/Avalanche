using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunPosition : MonoBehaviour
{
    public AgentRotate ar;
    // Start is called before the first frame update
    void Start()
    {
        ar = GameObject.Find("Character").GetComponent<AgentRotate>();
    }

    // Update is called once per frame
    void Update()
    {
        //transform.position = ar.PlayerToMouse.normalized * 5f;
        transform.rotation = Quaternion.LookRotation(Vector3.forward, ar.PlayerToMouse); // This code rotates sprite to mouse.
    }
}
