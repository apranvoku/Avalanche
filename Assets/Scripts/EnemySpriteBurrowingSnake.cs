using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpriteBurrowingSnake : MonoBehaviour
{

    public void StopMoving()
    {
        transform.parent.GetComponent<Enemy>().StopMoving();
    }

    public void ResumeMoving()
    {
        transform.parent.GetComponent<Enemy>().ResumeMoving();
    }

    public void SelfDestroy()
    {
        transform.parent.GetComponent<Enemy>().SelfDestroy();
    }

    public void SpeedUp()
    {
        transform.parent.GetComponent<EnemyBurrowingSnake>().SpeedUp();
    }

    public void SlowDown()
    {
        transform.parent.GetComponent<EnemyBurrowingSnake>().SlowDown();

    }

    public void StartInvulnerable()
    {
        transform.parent.GetComponent<EnemyBurrowingSnake>().StartInvulnerable();
    }

    public void StopInvulnerable()
    {
        transform.parent.GetComponent<EnemyBurrowingSnake>().StopInvulnerable();
    }

    public void StartParticles()
    {
        transform.parent.GetComponent<EnemyBurrowingSnake>().StartParticles();
    }

    public void StopParticles()
    {
        transform.parent.GetComponent<EnemyBurrowingSnake>().StopParticles();
    }
}
