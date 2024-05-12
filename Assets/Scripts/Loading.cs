using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Loading : MonoBehaviour
{

    public GameObject loadingUI;

    public void LoadScene(string scene)
    {
        StartCoroutine(LoadSceneAsync(scene));
    }

    public IEnumerator LoadSceneAsync(string scene)
    {

        loadingUI.SetActive(true);

        AsyncOperation operation = SceneManager.LoadSceneAsync(scene);


        while (!operation.isDone)
        {
            yield return null;
        }


        loadingUI.SetActive(false);
    }
}
