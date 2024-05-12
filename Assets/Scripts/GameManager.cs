using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static int m_referenceCount = 0;
    public static int loop = 0;

    // Start is called before the first frame update
    void Awake()
    {
        m_referenceCount++;
        if (m_referenceCount > 1)
        {
            DestroyImmediate(this.gameObject);
            return;
        }

        // Use this line if you need the object to persist across scenes
        DontDestroyOnLoad(this.gameObject);
    }
    
}
