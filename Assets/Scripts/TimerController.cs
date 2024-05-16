using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TimerController : MonoBehaviour
{
    public static TimerController instance;

    private bool timerGoing;
    private TimeSpan timePlaying;
    private float elapsedTime;

    private string timePlayingString;


    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
        timePlayingString = "Time: 00:00.00";
    }

    private void Start()
    {
        timerGoing = false;
    }

    public void BeginTimer()
    {
        timerGoing = true;
        elapsedTime = 0f;

        StartCoroutine(UpdateTimer());
    }

    public void EndTimer()
    {
        timerGoing = false;
    }

    public string GetTimeString()
    {
        timePlayingString = "Time: " + timePlaying.ToString("mm':'ss'.'ff");
        return timePlayingString;
    }

    public IEnumerator UpdateTimer()
    {
        while (timerGoing)
        {
            elapsedTime += Time.deltaTime;
            timePlaying = TimeSpan.FromSeconds(elapsedTime);
            yield return null;
        }
    }
}
