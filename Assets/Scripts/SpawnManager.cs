using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class SpawnManager : MonoBehaviour
{
    public List<GameObject> enemies;
    public List<int> toSpawn;
    private int maxEnemiesCount;
    public GameObject exit;
    public int enemiesOnScreenCap;
    private int currentEnemyCount;

    // Start is called before the first frame update
    void Start()
    {
        currentEnemyCount = 0;
        
        StartCoroutine(SpawnSnakes());
    }

    public IEnumerator SpawnSnakes()
    {
        foreach (int spawnNumber in toSpawn)
        {
            maxEnemiesCount += spawnNumber;
        }
        maxEnemiesCount = maxEnemiesCount * (GameManager.loop + 1);

        for (int i = 0; i < enemies.Count; i++)
        {
            for (int j = toSpawn[i] * (GameManager.loop + 1); j > 0; j += 0)
            {
                foreach (Transform portal in transform)
                {
                    if ((!portal.GetComponent<Renderer>().isVisible) &&(currentEnemyCount < enemiesOnScreenCap))
                    {
                        Instantiate(enemies[i], portal.position, Quaternion.identity, portal);
                        currentEnemyCount++;
                        j -= 1;
                        if (j <= 0)
                        {
                            break;
                        }
                    }
                }

                yield return new WaitForSeconds(0.4f);
            }
        }
    }

    public void EnemyDestroyed(Vector3 pos)
    {
        maxEnemiesCount--;
        currentEnemyCount--;
        if (maxEnemiesCount == 0)
        {
            //Do whatever needs to be done to end level.
            StartCoroutine(SpawnExit(pos));
        }
    }

    public void EnemySpawned()
    {
        maxEnemiesCount++;
        currentEnemyCount++;
    }

    public IEnumerator SpawnExit(Vector3 pos)
    {
        yield return null;
        Instantiate(exit, new Vector3(pos.x, pos.y, 0), Quaternion.identity);
    }

}
