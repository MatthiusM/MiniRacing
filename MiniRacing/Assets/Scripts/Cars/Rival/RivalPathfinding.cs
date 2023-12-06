using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RivalPathfinding
{
    private readonly List<Waypoint> waypoints;
    private int currentWaypointIndex = 0;
    private readonly float distanceToWaypointThreshold = 10f;

    public RivalPathfinding(List<Waypoint> waypoints)
    {
        this.waypoints = waypoints;
    }

    public int CurrentWaypointIndex
    {
        get { return currentWaypointIndex; }
    }

    public float DistanceToWaypointThreshold
    {
        get { return distanceToWaypointThreshold; }
    }
    
    public Waypoint GetCurrentWaypoint()
    {
        return waypoints[currentWaypointIndex];
    }

    public void UpdateWaypoint(Vector3 currentPosition)
    {
        Waypoint targetWaypoint = waypoints[currentWaypointIndex];
        if (Vector3.Distance(currentPosition, targetWaypoint.Position) < distanceToWaypointThreshold)
        {
            currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Count;
        }
    }
}
