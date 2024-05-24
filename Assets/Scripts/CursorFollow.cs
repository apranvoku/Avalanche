using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CursorFollow : MonoBehaviour
{
    private RectTransform m_CursorTransform;
    // Start is called before the first frame update
    void Start()
    {
        m_CursorTransform = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        m_CursorTransform.position = Mouse.current.position.ReadValue();
    }
}
