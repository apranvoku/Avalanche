using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySprite : MonoBehaviour
{

    public void StopMoving()
    {
        transform.parent.GetComponent<EnemyGardenSnake>().StopMoving();
    }

    public void ResumeMoving()
    {
        transform.parent.GetComponent<EnemyGardenSnake>().ResumeMoving();
    }

    public void SelfDestroy()
    {
        transform.parent.GetComponent<EnemyGardenSnake>().SelfDestroy();
    }
}
