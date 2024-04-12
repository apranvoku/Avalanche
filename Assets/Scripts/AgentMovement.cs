using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class AgentMovement : MonoBehaviour
{
    private Vector3 target;
    private NavMeshAgent agent;
    public float agentTargetSpeed;
    private Animator animator;
    private bool enableInputs;

    // Start is called before the first frame update
    void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        agent = GetComponent<NavMeshAgent>();
        target = transform.position;
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        enableInputs = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (enableInputs)
        {
            SetTargetPosition();
            SetAgentPosition();
            if ((target - transform.position).magnitude > 0.5f)
            {
                target = transform.position + (target - transform.position).normalized * 0.5f;
            }
        }
    }

    private void SetTargetPosition()
    {
        /*if(Keyboard.current.wKey.isPressed)
        { 
             target = target + new Vector3(0, Time.deltaTime * agentTargetSpeed, 0);
        }
        if (Keyboard.current.sKey.isPressed)
        {
            target = target + new Vector3(0f, -Time.deltaTime * agentTargetSpeed, 0);
        }
        if (Keyboard.current.aKey.isPressed)
        {
            target = target + new Vector3(-Time.deltaTime * agentTargetSpeed, 0, 0);
        }
        if (Keyboard.current.dKey.isPressed)
        {
            target = target + new Vector3(Time.deltaTime * agentTargetSpeed, 0, 0);
        }*/
        Vector3 targetDelta = new Vector3();
        bool moving = false;
        if (Keyboard.current.wKey.isPressed)
        {
            moving = true;
            targetDelta = targetDelta + new Vector3(0, 1, 0);
        }
        if (Keyboard.current.sKey.isPressed)
        {
            moving = true;
            targetDelta = targetDelta + new Vector3(0f, -1, 0);
        }
        if (Keyboard.current.aKey.isPressed)
        {
            moving = true;
            targetDelta = targetDelta + new Vector3(-1, 0, 0);
        }
        if (Keyboard.current.dKey.isPressed)
        {
            moving = true;
            targetDelta = targetDelta + new Vector3(1, 0, 0);
        }
        if (moving)
        {
            target = target + (targetDelta.normalized * Time.deltaTime * agentTargetSpeed);
            animator.Play("Base Layer.Walk", 0);
        }
        else
        {
            animator.Play("Base Layer.Idle", 0);
        }

        
    }

    private void SetAgentPosition()
    {
        //target.z = 0f;
        agent.SetDestination(new Vector3(target.x, target.y, target.z));
        //Debug.Log(target);
    }

    //Stuff for exit level animation
    public void ExitAnimation(Vector3 doorPos)
    {
        //Stop movements and player inputs;
        enableInputs = false;
        agent.enabled = false;
        GetComponentInChildren<AgentRotate>().enabled = false;
        animator.Play("Base Layer.Walk", 0);

        Vector3 difference = transform.Find("Character").transform.position - doorPos;

        // Set the parent's position to the target position
        transform.position = doorPos;

        // Move the child back to its original global position
        transform.Find("Character").transform.position = transform.position + difference;

        StartCoroutine(RotateObject(200.0f, 10f));
        StartCoroutine(ScaleToZero(2f, 3f));

    }

    private IEnumerator ScaleToZero(float delay, float duration)
    {
        yield return new WaitForSeconds(delay);

        float timer = 0.0f;
        Vector3 initialScale = transform.localScale;
        Vector3 targetScale = Vector3.zero;

        while (timer < duration)
        {
            transform.localScale = Vector3.Lerp(initialScale, targetScale, timer / duration);
            timer += Time.deltaTime;
            yield return null;
        }

        // Ensure the object reaches exactly the target scale
        transform.localScale = targetScale;
    }

    private IEnumerator RotateObject(float rotationSpeed, float duration)
    {
        float timer = 0.0f;
        while (timer < duration)
        {
            transform.Rotate(Vector3.forward, rotationSpeed * Time.deltaTime);
            timer += Time.deltaTime;
            yield return null;
        }
    }

}
