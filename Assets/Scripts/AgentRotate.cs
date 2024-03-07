using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class AgentRotate : MonoBehaviour
{
    private Vector3 MouseScreenToCameraSpace;
    private Vector3 PlayerScreenToCameraSpace;

    public Vector3 PlayerToMouse;
    public Vector3 WorldToScreenPointVector;
    // Start is called before the first frame update
    void Start()
    {
        MouseScreenToCameraSpace = new Vector3();
        PlayerScreenToCameraSpace = new Vector3();

        PlayerToMouse = new Vector3();
        WorldToScreenPointVector = new Vector3();
    }

    // Update is called once per frame
    void Update()
    {
        MouseScreenToCameraSpace = new Vector3(Mouse.current.position.ReadValue().x, Mouse.current.position.ReadValue().y, 0f);
        WorldToScreenPointVector = Camera.main.WorldToScreenPoint(transform.position);
        PlayerScreenToCameraSpace = new Vector3(WorldToScreenPointVector.x, WorldToScreenPointVector.y, 0f);
        PlayerToMouse = MouseScreenToCameraSpace - PlayerScreenToCameraSpace;

        if(PlayerToMouse.x > 0)
        {
            if(transform.localScale.x < 0) 
            {
                transform.localScale = new Vector3(transform.localScale.x * -1f, transform.localScale.y, transform.localScale.z);
            }
        }
        if (PlayerToMouse.x < 0)
        {
            if (transform.localScale.x > 0)
            {
                transform.localScale = new Vector3(transform.localScale.x * -1f, transform.localScale.y, transform.localScale.z);
            }
        }

        //transform.rotation = Quaternion.LookRotation(Vector3.forward, PlayerToMouse); This code rotates sprite to mouse.
    }
}
