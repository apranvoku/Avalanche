using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class AgentMovement : MonoBehaviour
{
    private Vector3 target;
    private NavMeshAgent agent;
    public float agentTargetSpeed;
    private Animator animator;
    private static AgentMovement instance;

    private bool enableInputs;
    private bool isDestroying;

    private static int m_referenceCount = 0;

    public bool devAgentDisplay;
    public GameObject targObj;
    public GameObject ag;

    private NavMeshPath path;
    private NavMeshHit hit;

    public static AgentMovement Instance
    {
        get { return instance; }
    }

    // Start is called before the first frame update
    void Awake()
    {
        isDestroying = false;
        m_referenceCount++;
        if (m_referenceCount > 1)
        {
            isDestroying = true;
            DestroyImmediate(this.gameObject);
            return;
        }

        instance = this;
        // Use this line if you need the object to persist across scenes
        DontDestroyOnLoad(this.gameObject);

        animator = GetComponentInChildren<Animator>();
        agent = GetComponent<NavMeshAgent>();
        target = transform.position;
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        enableInputs = true;


        path = new NavMeshPath();
        hit = new NavMeshHit();

        if (devAgentDisplay)
        {
            targObj.SetActive(true);
            ag.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (enableInputs && !PauseScreen.isPaused)
        {
            SetTargetPosition();
            SetAgentPosition();
        }
    }

    private void SetTargetPosition()
    {
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
            target = transform.position +  (targetDelta * Time.deltaTime * agentTargetSpeed).normalized;
            animator.Play("Base Layer.Walk", 0);
        }
        else
        {
            //target = transform.position;
            animator.Play("Base Layer.Idle", 0);
        }

        agent.velocity = agent.desiredVelocity;
        if (devAgentDisplay)
        {
            targObj.transform.position = target;
        }
    }

    private void SetAgentPosition()
    {
        if (!UnityEngine.AI.NavMesh.CalculatePath(transform.position, new Vector3(target.x, target.y, target.z), NavMesh.AllAreas, path))
        {
            NavMesh.SamplePosition(target, out hit, 1.5f, NavMesh.AllAreas);
            if (!UnityEngine.AI.NavMesh.CalculatePath(transform.position, hit.position, NavMesh.AllAreas, path))
            {
                agent.SetDestination(new Vector3(target.x, target.y, target.z));
            }
            else
            {
                agent.SetPath(path);
            }
        }
        else 
        {
            agent.SetPath(path);
        }
        //if (agent.SetPath(path))
        //{
        //    agent.SetDestination(new Vector3(target.x, target.y, target.z));
        //}
        if (devAgentDisplay)
        {
            ag.transform.position = agent.transform.position;
        }
    }

    public void OnDisable()
    {
        if (!isDestroying)
        {
            enableInputs = false;
            path.ClearCorners();
            agent.enabled = false;
            GetComponentInChildren<AgentRotate>().enabled = false;
            GetComponentInChildren<Shoot>().enabled = false;
        }

    }

    public void DisableNavAgent()
    {
        agent.enabled = false;
    }

    public void OnEnable()
    {
        transform.rotation = Quaternion.identity;
        transform.localScale = Vector3.one;
        enableInputs = true;
        agent.enabled = true;
        target = transform.position;
        SetTargetPosition();
        SetAgentPosition();
        GetComponentInChildren<AgentRotate>().enabled = true;
        GetComponentInChildren<Shoot>().enabled = true;
        //PauseScreen.canPause = true;
    }

    //Stuff for exit level animation
    public void ExitAnimation(Vector3 doorPos)
    {
        //Stop movements and player inputs;
        OnDisable();
        animator.Play("Base Layer.Walk", 0);

        Vector3 difference = transform.Find("Character").transform.position - doorPos;

        //Start the exit camera animation (lerp to Door)
        GameObject.Find("Main Camera").GetComponent<CameraFollow>().ZoomToExit(doorPos);

        // Set the parent's position to the target position
        transform.position = doorPos;

        // Move the child back to its original global position
        transform.Find("Character").transform.position = transform.position + difference;

        StartCoroutine(RotateObject(500.0f, 2f));
        StartCoroutine(ScaleToZero(0.5f, 1.5f));

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

        //load next scene after spin animation ends
        Shop.Instance.OpenShop();
        //SceneManager.LoadScene("Level_2_Rooms");
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
