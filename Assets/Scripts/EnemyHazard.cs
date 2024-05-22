using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class EnemyHazard : MonoBehaviour
{
    public float bulletSpeed;
    private GameObject player;
    private Vector3 playerSnapshot;
    private Vector3 originalPosition;
    private GameObject projectileSprite;
    public GameObject AoeSprite;
    public float arcHeight;


    // Start is called before the first frame update
    void Start()
    {
        gameObject.transform.rotation = Quaternion.identity;
        player = GameObject.Find("Agent");
        playerSnapshot = player.transform.position;
        projectileSprite = transform.GetChild(0).gameObject;
        originalPosition = projectileSprite.transform.position;
        StartCoroutine(MoveParabolic(projectileSprite.transform, originalPosition, playerSnapshot, arcHeight, bulletSpeed));
    }

    // Update is called once per frame
    void Update()
    {
    }

    IEnumerator MoveParabolic(Transform obj, Vector3 start, Vector3 end, float arcHeight, float speed)
    {
        // Calculate the total distance and duration
        float distance = Vector3.Distance(start, end);
        float duration = distance / speed;

        // Calculate the midpoint and the peak height
        Vector3 midPoint = (start + end) / 2;
        midPoint.y += arcHeight;

        // Initialize elapsed time
        float elapsedTime = 0;

        while (elapsedTime < duration)
        {
            // Calculate the t value
            float t = elapsedTime / duration;

            // Calculate the parabolic position
            Vector3 currentPos = Parabola(start, midPoint, end, t);

            // Update the object's position
            obj.position = currentPos;

            // Increment elapsed time
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure the final position is set
        obj.position = end;

        Instantiate(AoeSprite, playerSnapshot, Quaternion.identity, transform.parent);
        Destroy(gameObject);
    }

    public Vector3 Parabola(Vector3 start, Vector3 peak, Vector3 end, float t)
    {
        // Interpolate between the start and the peak
        Vector3 startToPeak = Vector3.Lerp(start, peak, t);
        // Interpolate between the peak and the end
        Vector3 peakToEnd = Vector3.Lerp(peak, end, t);
        // Interpolate between the two parabolic positions to get the final position
        return Vector3.Lerp(startToPeak, peakToEnd, t);
    
    }
}
