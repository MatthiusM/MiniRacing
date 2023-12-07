using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RivalPathfinding : MonoBehaviour
{
    public UnityEvent<Waypoint> updateWaypoint = new();

    [SerializeField]
    private List<Waypoint> waypoints;
    private int currentWaypointIndex = 0;
    private readonly float distanceToWaypointThreshold = 10f;

    void Start()
    {
        waypoints = WaypointManager.Instance.GetWaypoints();
        
        //set the first waypoint
        updateWaypoint.Invoke(GetCurrentWaypoint());
    }

    private void Update()
    {
        UpdateWaypoint(transform.position);
    }

    private Waypoint GetCurrentWaypoint()
    {
        return waypoints[currentWaypointIndex];
    }

    private void UpdateWaypoint(Vector3 currentPosition)
    {
        Vector3 targetWaypoint = waypoints[currentWaypointIndex].Position;
        if (Vector3.Distance(currentPosition, targetWaypoint) < distanceToWaypointThreshold)
        {
            currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Count;
            updateWaypoint.Invoke(GetCurrentWaypoint());
        }
    }
}
