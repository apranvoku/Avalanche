using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System.Net;

public class GameOverScreen : MonoBehaviour
{

    private Shop shopScript;
    private CanvasGroup myGroup;
    private CanvasGroup buttonGroup;
    private Loading loadingManager;
    public Transform textParent;

    private TextMeshProUGUI level;
    private TextMeshProUGUI time;

    private void Start()
    {
        shopScript = transform.GetComponent<Shop>();
        myGroup = GameObject.Find("GameOverCanvas").GetComponent<CanvasGroup>();
        buttonGroup = GameObject.Find("GameOverTextAndRetry").GetComponent<CanvasGroup>();
        loadingManager = GameObject.Find("NewCanvas").GetComponent<Loading>();
        level = GameObject.Find("GameOverLevel").GetComponent<TextMeshProUGUI>();
        time = GameObject.Find("GameOverTime").GetComponent<TextMeshProUGUI>();

    }

    public void BackToTitleScreen()
    {
        TimerController.instance.EndTimer();
        loadingManager.LoadScene("Intro");
        PauseScreen.canPause = false;
        GameObject.Find("Agent").GetComponentInChildren<Player>().ResetAllStats();
        GameObject.Find("Agent").GetComponent<AgentMovement>().enabled = true;
        StartCoroutine(FadeOut());
        GameManager.loop = 0;
        shopScript.resetCurrentLevel();
    }

    public void OpenGameOverScreen()
    {
        SetLevel();
        SetTimer();
        GameObject.Find("Agent").GetComponent<AgentMovement>().OnDisable();
        GameObject.Find("Agent").GetComponent<AgentMovement>().enabled = false;
        myGroup.alpha = 1f;
        StartCoroutine(GameOverAnimationText(.01f));
        StartCoroutine(FadeIn(1f));
    }

    public void RetryLevel()
    {
        loadingManager.LoadScene(SceneManager.GetActiveScene().name);
        GameObject.Find("Agent").GetComponentInChildren<Player>().ResetAllStats();
        GameObject.Find("Agent").GetComponent<AgentMovement>().enabled = true;
        StartCoroutine(FadeOut());
        Shop.Instance.ResetMoneyToSnapshot();
        PauseScreen.canPause = true;
    }

    public void SetLevel()
    {
        level.text = "Level: " + GameManager.instance.GetLoop() + "-" + shopScript.GetLevel();
    }
    public void SetTimer()
    {
        time.text = TimerController.instance.GetTimeString();
    }

    public IEnumerator FadeIn(float duration)
    {
        yield return new WaitForSeconds(0.5f);
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

    public IEnumerator FadeOut()
    {
        GameObject.Find("Main Camera").GetComponent<CameraFollow>().ZoomToDefault();
        myGroup.interactable = false;
        myGroup.blocksRaycasts = false;
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
        yield return null;
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
