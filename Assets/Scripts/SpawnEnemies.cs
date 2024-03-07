using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemies : MonoBehaviour
{
    public GameObject enemy1;
    // Start is called before the first frame update
    void Start()
    {
        SpawnEnemiesFun(10);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SpawnEnemiesFun(int numSpawn)
    {
        StartCoroutine(SpawnEnemiesRoutine(numSpawn));
    }

    public IEnumerator SpawnEnemiesRoutine(int numSpawn)
    {
        while(numSpawn > 0)
        {
            Instantiate(enemy1, transform.position, Quaternion.identity, transform);
            yield return new WaitForSeconds(0.5f);
            numSpawn--;
        }
    }
}
