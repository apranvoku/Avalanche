using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelpScreen : MonoBehaviour
{

    public GameObject helpUI;

    public void OpenHelpScreen()
    {
        helpUI.SetActive(true);
    }

    public void CloseHelpScreen()
    {
        helpUI.SetActive(false);
    }
}
