using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpOrb : MonoBehaviour
{
    SpriteRenderer sr;
    public Color color1 = Color.blue;
    public Color color2 = Color.red;
    private bool collected = false;
    // Start is called before the first frame update
    void Start()
    {
        sr = GetComponentInChildren<SpriteRenderer>();
        StartCoroutine(Phosphoresce());
    }

    // Update is called once per frame
    void Update()
    {

    }

    public IEnumerator Phosphoresce()
    {
        while (!collected)
        {
            for(float i = 0; i < 1f; i+= Time.deltaTime)
            {
                sr.color = Color.Lerp(color1, color2, i);
                yield return null;
            }
            for (float i = 0; i < 1f; i += Time.deltaTime)
            {
                sr.color = Color.Lerp(color2, color1, i);
                yield return null;
            }
        }
    }
}