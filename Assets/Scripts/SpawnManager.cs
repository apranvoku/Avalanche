using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.PlayerSettings;

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
    public GameObject exit;
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

    public void EnemyDestroyed(Vector3 pos)
    {
        enemiesCount--;
        if (enemiesCount == 0)
        {
            //Do whatever needs to be done to end level.
            StartCoroutine(SpawnExit(pos));
        }
    }

    public IEnumerator SpawnExit(Vector3 pos)
    {
        yield return new WaitForSecondsRealtime(3f);
        Instantiate(exit, new Vector3(pos.x, pos.y, 0), Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
