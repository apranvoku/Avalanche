using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndScreen : MonoBehaviour
{
    public GameObject screen;

    private void Awake()
    {
        if (GameManager.winState == true)
        {
            OpenScreen();
            GameManager.winState = false;
        }
    }

    public void OpenScreen()
    {
        screen.SetActive(true);
    }

    public void CloseScreen()
    {
        screen.SetActive(false);
    }
}
