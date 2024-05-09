using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentSprite : MonoBehaviour
{
    public void ResumeMovement()
    {
        transform.parent.parent.GetComponent<AgentMovement>().enabled = true;
    }

    public void StopMovement()
    {
        transform.parent.parent.GetComponent<AgentMovement>().enabled = false;
    }

    public void Die()
    {
        transform.parent.GetComponent<Player>().Die();
    }
    public void ZoomCamera()
    {
        GameObject.Find("Main Camera").GetComponent<CameraFollow>().ZoomToPlayer();
    }
}
