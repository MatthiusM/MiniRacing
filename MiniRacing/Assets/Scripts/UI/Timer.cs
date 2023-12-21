using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI timerText;
    
    private float startTime;
    private bool timerActive = true;

    private List<CarData> carsData = new();

    private string currentTime;

    void Start()
    {
        timerText.text = string.Format("{0:00}:{1:00}", 0, 0);
        GameManager.Instance.AddListenerOnInitialise(StartTimer);

        GameObject[] carObjects = GameObject.FindGameObjectsWithTag("Car");
        foreach (GameObject carObject in carObjects)
        {
            if (carObject.transform.parent == null)
            {
                carsData.Add(carObject.GetComponent<CarData>());
            }
        }
    }

    void Update()
    {
        if (GameManager.Instance.CurrentState != GameState.Playing) { return; }

        if (timerActive)
        {
            float t = Time.time - startTime;

            int minutes = (int)(t / 60F);
            int seconds = (int)(t % 60F);
            
            
            currentTime = string.Format("{0:00}:{1:00}", minutes, seconds);
            
            timerText.text = currentTime;
            
            if (minutes >= 99 && seconds >= 59)
            {
                currentTime = "99:59";
                timerText.text = currentTime;
                timerActive = false;
            }
            
            SetTimes();
        }
    }

    void StartTimer()
    {
        startTime = Time.time;
    }

    private void SetTimes()
    {
        for (int i = 0; i < carsData.Count; i++)
        {
            if (carsData[i].finished) { continue; }
            carsData[i].time = currentTime;
        }
    }
}
