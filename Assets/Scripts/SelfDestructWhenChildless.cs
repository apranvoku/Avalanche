using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDestructWhenChildless : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        // Check if the object has no children
        if (transform.childCount == 0)
        {
            // Destroy the object if it has no children
            Destroy(gameObject);
        }
    }
}
