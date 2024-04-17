using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentSprite : MonoBehaviour
{
    public void ResumeMovement()
    {
        transform.parent.parent.GetComponent<AgentMovement>().enabled = true;
    }
}
