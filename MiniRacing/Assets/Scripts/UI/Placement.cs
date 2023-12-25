using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEditor.Animations;

public class Placement : MonoBehaviour
{
    [SerializeField]
    private GameObject placementUI;

    [SerializeField]
    private GameObject HUD;

    private List<GameObject> placements = new();

    private int currentPlacement = 0;

    private void Start()
    {
        for (int i = 0; i < placementUI.transform.childCount; i++)
        {
            GameObject child = placementUI.transform.GetChild(i).gameObject;

            if (child.name.Contains("Panel"))
            {
                placements.Add(child);
                child.SetActive(false);
            }
        }
        placementUI.SetActive(false);        
    }
    public void AddPlacement(CarData carData)
    {
        if(carData.name == "Player") { placementUI.SetActive(true); HUD.SetActive(false); }

        placements[currentPlacement].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = carData.name;
        placements[currentPlacement].transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = carData.time;
        placements[currentPlacement].transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = carData.placement;

        placements[currentPlacement].SetActive(true);
        
        currentPlacement++;
    }

}
