using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PauseScreen : MonoBehaviour
{
    private Shop shopScript;
    private CanvasGroup myGroup;
    public static bool isPaused;
    public static bool canPause;

    private void Awake()
    {
        shopScript = transform.GetComponent<Shop>();
        myGroup = GameObject.Find("PauseCanvas").GetComponent<CanvasGroup>();
        isPaused = false;
        canPause = true;
    }

    private void Update()
    {
        if (Keyboard.current.escapeKey.wasPressedThisFrame && canPause)
        {
            TogglePause();
        }
    }

    public void OpenPauseScreen()
    {
        myGroup.alpha = 1f;
        myGroup.interactable = true;
        myGroup.blocksRaycasts = true;
    }

    public void ClosePauseScreen()
    {
        myGroup.interactable = false;
        myGroup.blocksRaycasts = false;
        myGroup.alpha = 0f;
    }

    public void TogglePause()
    {
        isPaused = !isPaused;

        if (isPaused)
        {
            // Show pause menu
            OpenPauseScreen();
            Time.timeScale = 0; // Pause the game
        }
        else
        {
            // Hide pause menu
            ClosePauseScreen();
            Time.timeScale = 1; // Resume the game
            
        }
    }

    public void QuitGame()
    {
        TogglePause();
        GameObject.Find("NewCanvas").GetComponent<GameOverScreen>().BackToTitleScreen();
    }

    public void RetryLevel()
    {
        TogglePause();
        GameObject.Find("Agent").GetComponent<AgentMovement>().enabled = false;
        GameObject.Find("Agent").GetComponentInChildren<Player>().ResetAllStats();
        shopScript.RetryLevel();
    }
}
