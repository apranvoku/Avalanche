using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class GameOverScreen : MonoBehaviour
{

    private Shop shopScript;
    private CanvasGroup myGroup;
    private CanvasGroup buttonGroup;
    public Transform textParent;

    private void Start()
    {
        shopScript = transform.GetComponent<Shop>();
        myGroup = GameObject.Find("GameOverCanvas").GetComponent<CanvasGroup>();
        buttonGroup = GameObject.Find("GameOverTextAndRetry").GetComponent<CanvasGroup>();
    }

    public void BackToTitleScreen()
    {
        PauseScreen.canPause = false;
        GameObject.Find("Agent").GetComponentInChildren<Player>().ResetAllStats();
        GameObject.Find("Agent").GetComponent<AgentMovement>().enabled = true;
        StartCoroutine(FadeOut(.5f));
        shopScript.QuitLevel();
    }

    public void OpenGameOverScreen()
    {
        GameObject.Find("Agent").GetComponent<AgentMovement>().enabled = false;
        myGroup.alpha = 1f;
        StartCoroutine(GameOverAnimationText(.01f));
        StartCoroutine(FadeIn(1f));
    }

    public void RetryLevel()
    {
        GameObject.Find("Agent").GetComponentInChildren<Player>().ResetAllStats();
        GameObject.Find("Agent").GetComponent<AgentMovement>().enabled = true;
        StartCoroutine(FadeOut(.5f));
        PauseScreen.canPause = true;
        shopScript.RetryLevel();
    }

    public IEnumerator FadeIn(float duration)
    {

        yield return new WaitForSeconds(0.5f); ;
        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            buttonGroup.alpha = Mathf.Lerp(0f, 1f, elapsedTime / duration);
            yield return null;
        }
        buttonGroup.alpha = 1f;
        myGroup.interactable = true;
        myGroup.blocksRaycasts = true;
    }

    public IEnumerator FadeOut(float duration)
    {
        GameObject.Find("Main Camera").GetComponent<CameraFollow>().ZoomToDefault();
        myGroup.interactable = false;
        myGroup.blocksRaycasts = false;
        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            buttonGroup.alpha = Mathf.Lerp(1f, 0f, elapsedTime / duration);
            yield return null;
        }
        myGroup.alpha = 0f;
        buttonGroup.alpha = 0f;
        foreach (Transform child in textParent)
        {
            TextMeshProUGUI textObject = child.GetComponent<TextMeshProUGUI>();
            if (textObject != null)
            {
                textObject.text = "";
            }
        }
    }

    public IEnumerator GameOverAnimationText(float interval)
    {
        foreach (Transform child in textParent)
        {
            TextMeshProUGUI textObject = child.GetComponent<TextMeshProUGUI>();
            if (textObject != null)
            {
                for (int i = 0; i < 16; i++)
                {
                    textObject.text += "S";
                    yield return new WaitForSeconds(interval);
                }
            }
        }
    }

}
