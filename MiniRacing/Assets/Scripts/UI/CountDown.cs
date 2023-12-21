using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class CountDown : MonoBehaviour
{
    private TextMeshProUGUI countdownText;
    private float countdownTime = 3.0f;

    private UnityEvent countDownFinished = new();

    void Start()
    {
        countdownText = GetComponentInChildren<TextMeshProUGUI>();
        StartCoroutine(StartCountdown());
    }

    private IEnumerator StartCountdown()
    {
        while (countdownTime > 0)
        {
            countdownText.text = Mathf.CeilToInt(countdownTime).ToString();
            yield return new WaitForSeconds(1.0f);
            countdownTime -= 1.0f;
        }

        countdownText.text = "Go!";

        yield return new WaitForSeconds(1.0f);

        countDownFinished.Invoke();

        this.gameObject.SetActive(false);
    }
    public void AddListenerCountDownFinished(UnityAction listener)
    {
        countDownFinished.AddListener(listener);
    }
}
