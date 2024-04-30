using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExitUI : MonoBehaviour
{

    public Image img;
    private Transform target;
    private bool startUI;

    // Update is called once per frame
    void Update()
    {
        if(startUI && (target != null))
        { 
            float minX = img.GetPixelAdjustedRect().width / 1.5f;
            float maxX = Screen.width - minX;

            float minY = img.GetPixelAdjustedRect().height / 1.5f;
            float maxY = Screen.height - minY;

            Vector2 pos = Camera.main.WorldToScreenPoint(target.position);

            if (Vector3.Dot((target.position - transform.position), transform.forward) < 0)
            {
                if (pos.x < Screen.width / 2)
                {
                    pos.x = maxX;
                }
                else
                {
                    pos.x = minX;
                }
            }
            pos.x = Mathf.Clamp(pos.x, minX, maxX);
            pos.y = Mathf.Clamp(pos.y, minY, maxY);

            img.transform.position = pos;
        }
        else
        {
            img.enabled = false;
        }
    }

    public void SetTarget(Transform targetToSet)
    {
        target = targetToSet;
        img.enabled = true;
        startUI = true;
    }

    public void ResetTarget()
    {
        img.enabled = false;
        startUI = false;
        target = null;
    }
}
