using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Placement : MonoBehaviour
{
    [SerializeField]
    private GameObject player;

    private TextMeshProUGUI placementText;
    private List<CarWaypointManager> cars = new();

    private void Start()
    {
        placementText = GetComponent<TextMeshProUGUI>();

        GameObject[] carObjects = GameObject.FindGameObjectsWithTag("Car");
        foreach (GameObject carObject in carObjects)
        {
            if (carObject.transform.parent == null)
            {
                cars.Add(carObject.GetComponent<CarWaypointManager>());
            }
        }
        placementText.text = $"{GetCarIndexByInstanceID(player.GetInstanceID()) + 1} / {cars.Count}";
    }

    private void Update()
    {
        if(GameManager.Instance.CurrentState != GameState.Playing) { return; }

        SortCarsByPlacement();
        placementText.text = $"{GetCarIndexByInstanceID(player.GetInstanceID()) + 1} / {cars.Count}";
    }

    private int GetCarIndexByInstanceID(int instanceID)
    {
        for (int i = 0; i < cars.Count; i++)
        {
            if (cars[i].gameObject.GetInstanceID() == instanceID)
            {
                return i;
            }
        }

        return -1; 
    }


    private void SortCarsByPlacement()
    {
        cars = cars.OrderByDescending(car => car.LapsCompleted)
                    .ThenByDescending(car => car.CurrentWaypointIndex)
                    .ThenBy(car => Vector3.Distance(car.transform.position, WaypointManager.Instance.GetWaypointByIndex(car.CurrentWaypointIndex).Position))
                    .ToList();
    }
}
