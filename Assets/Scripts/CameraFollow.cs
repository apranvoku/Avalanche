using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private Vector3 offset;
    private GameObject player;
    private Vector3 defaultOffset;
    private Vector3 zoomOffset;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Agent");
        transform.position = new Vector3(player.transform.position.x, player.transform.position.y, transform.position.z);
        offset = transform.position - player.transform.position;
        defaultOffset = new Vector3(offset.x, offset.y, offset.z);
        zoomOffset = new Vector3(offset.x, offset.y, -40);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = player.transform.position + offset;
    }

    public void ZoomToPlayer()
    {
        StartCoroutine(ZoomCamera(1f));
    }

    public void ZoomToDefault()
    {
        offset = defaultOffset;
    }

    public IEnumerator ZoomCamera(float duration)
    {
        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            offset = Vector3.Lerp(offset, zoomOffset, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure final position
        transform.position = zoomOffset;
    }
}
