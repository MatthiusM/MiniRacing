using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Speed : MonoBehaviour
{
    [SerializeField]
    private GameObject player;

    private TextMeshProUGUI speedText;

    private Car playerCar;

    void Start()
    {
        speedText = GetComponent<TextMeshProUGUI>();
        playerCar = player.GetComponent<Car>();
    }

    // Update is called once per frame
    void Update()
    {
        speedText.text = $"{playerCar.GetMPH()}MPH";
    }
}
