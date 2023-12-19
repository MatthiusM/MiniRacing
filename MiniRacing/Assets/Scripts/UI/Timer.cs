using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    private TextMeshProUGUI timerText;
    private float startTime;
    private bool timerActive = true;

    void Start()
    {
        timerText = GetComponent<TextMeshProUGUI>();
        startTime = Time.time;
    }

    void Update()
    {
        if (timerActive)
        {
            float t = Time.time - startTime;

            int minutes = (int)(t / 60F);
            int seconds = (int)(t % 60F);

            timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);

            if (minutes >= 99 && seconds >= 59)
            {
                timerText.text = "99:59";
                timerActive = false;
            }
        }
    }
}
