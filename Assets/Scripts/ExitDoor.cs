using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitDoor : MonoBehaviour
{

    public void Awake()
    {
        if (!transform.GetComponent<Renderer>().isVisible) 
        {
            GameObject.Find("ExitIcon").GetComponent<ExitUI>().SetTarget(transform);
        }
    }

    public void EnableExitTrigger()
    {
        transform.GetComponent<CircleCollider2D>().enabled = true;
    }

    private void OnBecameInvisible()
    {
        if (GameObject.Find("ExitIcon") != null)
        {
            GameObject.Find("ExitIcon").GetComponent<ExitUI>().SetTarget(transform);
        }
    }

    private void OnBecameVisible()
    {
        GameObject.Find("ExitIcon").GetComponent<ExitUI>().ResetTarget();
    }
}
