using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.PlasticSCM.Editor.WebApi;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Position : MonoBehaviour
{
    [SerializeField]
    private GameObject player;

    private TextMeshProUGUI positionText;
    private List<CarWaypointManager> cars = new();
    private List<CarData> carsData = new();

    private void Start()
    {
        positionText = GetComponent<TextMeshProUGUI>();

        GameObject[] carObjects = GameObject.FindGameObjectsWithTag("Car");
        foreach (GameObject carObject in carObjects)
        {
            if (carObject.transform.parent == null)
            {
                cars.Add(carObject.GetComponent<CarWaypointManager>());
                carsData.Add(carObject.GetComponent<CarData>());
            }
        }
        SetPlacements();
    }

    private void Update()
    {
        if (GameManager.Instance.CurrentState != GameState.Playing) { return; }

        SortCarsByPlacement();
        SetPlacements();
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
        // First, sort only the cars that haven't finished the race
        var racingCars = cars.Where(car => !car.GetComponent<CarData>().finished)
                             .OrderByDescending(car => car.LapsCompleted)
                             .ThenByDescending(car => car.CurrentWaypointIndex)
                             .ThenBy(car => Vector3.Distance(car.transform.position, WaypointManager.Instance.GetWaypointByIndex(car.CurrentWaypointIndex).Position))
                             .ToList();

        // Now add the cars that have finished in their current order
        var finishedCars = cars.Where(car => car.GetComponent<CarData>().finished).ToList();
        cars = racingCars.Concat(finishedCars).ToList();
    }

    private void SetPlacements()
    {
        for (int i = 0; i < carsData.Count; i++)
        {
            if (carsData[i].finished) { continue; }
            carsData[i].placement = $"{GetCarIndexByInstanceID(carsData[i].gameObject.GetInstanceID()) + 1} / {cars.Count}";
        }
        positionText.text = $"{GetCarIndexByInstanceID(player.GetInstanceID()) + 1} / {cars.Count}";
    }
}
