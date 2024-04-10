using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    private static int m_referenceCount = 0;
    private static SpawnManager instance;

    public static SpawnManager Instance
    {
        get { return instance; }
    }

    void Awake()
    {
        m_referenceCount++;
        if (m_referenceCount > 1)
        {
            DestroyImmediate(this.gameObject);
            return;
        }

        instance = this;
        // Use this line if you need the object to persist across scenes
        DontDestroyOnLoad(this.gameObject);
    }

    public List<GameObject> enemies;
    public List<int> toSpawn;
    public int enemiesCount;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnSnakeBabies());
    }

    public IEnumerator SpawnSnakeBabies()
    {
        foreach (int spawnNumber in toSpawn)
        {
            enemiesCount += spawnNumber;
        }

        for (int i = 0; i < enemies.Count; i++)
        {
            for (int j = toSpawn[i]; j > 0; j += 0)
            {
                foreach (Transform portal in transform)
                {
                    Instantiate(enemies[i], portal.position, Quaternion.identity, portal);
                    yield return new WaitForSecondsRealtime(1f);
                    j -= 1;
                    if (j <= 0)
                    {
                        break;
                    }
                }
            }
        }
    }

    public void EnemyDestroyed()
    {
        enemiesCount--;
        if (enemiesCount == 0)
        {
            //Do whatever needs to be done to end level.
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
