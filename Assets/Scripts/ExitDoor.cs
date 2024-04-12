using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitDoor : MonoBehaviour
{

    public void EnableExitTrigger()
    {
        transform.GetComponent<CircleCollider2D>().enabled = true;
    }
}
