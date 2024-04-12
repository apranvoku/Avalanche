using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoading : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void LoadLevel(int level)
    {
        if (level == 1)
        {
            SceneManager.LoadScene("Level_PlainPlane");
        }
    }

}
