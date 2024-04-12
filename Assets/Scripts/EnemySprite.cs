using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySprite : MonoBehaviour
{

    public void StopMoving()
    {
        transform.parent.GetComponent<Enemy>().StopMoving();
    }

    public void ResumeMoving()
    {
        transform.parent.GetComponent<Enemy>().ResumeMoving();
    }
}
