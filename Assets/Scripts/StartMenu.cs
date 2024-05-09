using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenu : MonoBehaviour
{
    public void LoadFirstLevel(string firstLevelName)
    {
        SceneManager.LoadScene(firstLevelName);
        PauseScreen.canPause = true;
    }
}
