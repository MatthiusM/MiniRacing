using System.Collections.Generic;
using UnityEngine;

public class CarWaypointManager : MonoBehaviour
{
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
        if (currentWaypointIndex == 0)
        {
            lapsCompleted++;
        }
        SetWaypoints();
    }

    void SetWaypoints()
    {
        currentWaypoint = WaypointManager.Instance.GetWaypointByIndex(currentWaypointIndex);
    }
}
