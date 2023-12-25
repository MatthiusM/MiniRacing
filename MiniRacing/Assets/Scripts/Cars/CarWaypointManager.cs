using System.Collections.Generic;
using UnityEngine;

public class CarWaypointManager : MonoBehaviour
{
    [SerializeField]
    private Placement placement;

    private static int waypointsCount;
    private int currentWaypointIndex = 0;
    private int lapsCompleted = 0; 
    private Waypoint currentWaypoint = null;

    public int CurrentWaypointIndex
    {
        get { return currentWaypointIndex; }
    }

    public Waypoint CurrentWaypoint
    {
        get { return currentWaypoint; }
    }

    public int LapsCompleted
    {
        get { return lapsCompleted; }
    }

    private void Start()
    {
        waypointsCount = WaypointManager.Instance.GetWaypoints().Count;
        SetWaypoints();
    }

    public void IncrementWaypoint()
    {
        currentWaypointIndex = (currentWaypointIndex + 1) % waypointsCount;
        if (currentWaypointIndex == 0 && !this.GetComponent<CarData>().finished)
        {
            lapsCompleted++;
            this.GetComponent<CarData>().finished = true;
            placement.AddPlacement(this.GetComponent<CarData>());
            if(this.gameObject.name == "Player")
            {
                Debug.Log("add coins");
                SavedData.instance.AddCoins(50);
            }
        }
        SetWaypoints();
    }

    void SetWaypoints()
    {
        currentWaypoint = WaypointManager.Instance.GetWaypointByIndex(currentWaypointIndex);
    }
}
