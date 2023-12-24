using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Coins : MonoBehaviour
{

    private TextMeshProUGUI coinText;
    void Start()
    {
        coinText = GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        coinText.text = SavedData.instance.Coins.ToString();
    }
}
