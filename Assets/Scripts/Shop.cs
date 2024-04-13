using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    public Image blackScreen;
    private CanvasGroup myGroup;
    private static int m_referenceCount = 0;
    private static Shop instance;

    public static Shop Instance
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

    public void Start()
    {
        myGroup = GetComponent<CanvasGroup>();
    }

    public void OpenShop()
    {
        StartCoroutine(FadeToBlack(1f));
    }

    public IEnumerator FadeToBlack(float duration)
    {
        Color color = blackScreen.GetComponent<Image>().color;
        for (float i = 0; i < duration; i += Time.deltaTime) 
        {
            blackScreen.GetComponent<Image>().color = new Color(color.r, color.g, color.b, i);
            myGroup.alpha = i;
            yield return null;
        }
    }

    public IEnumerator FadeToWhite(float duration)
    {
        Color color = blackScreen.GetComponent<Image>().color;
        for (float i = 0; i < duration; i += Time.deltaTime)
        {
            blackScreen.GetComponent<Image>().color = new Color(color.r, color.g, color.b, 1f-i);
            myGroup.alpha = 1f-i;
            yield return null;
        }
    }
}
