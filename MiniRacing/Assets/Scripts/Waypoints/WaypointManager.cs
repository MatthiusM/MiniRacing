using System.Collections.Generic;
using UnityEngine;

public class WaypointManager : MonoBehaviour
{
    private List<Waypoint> waypoints = new();

    public static WaypointManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            InitWaypoints();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AddWaypoint(Vector3 position)
    {
        waypoints.Add(new Waypoint(position));
    }

    private void InitWaypoints()
    {
        foreach (Transform child in transform)
        {
            MeshRenderer renderer = child.GetComponent<MeshRenderer>();
            renderer.enabled = false;
            AddWaypoint(child.position);
        }
    }

    public List<Waypoint> GetWaypoints()
    {
        return waypoints;
    }

    private void OnDrawGizmos()
    {
        //draw the waypoints
        foreach (Waypoint waypoint in waypoints)
        {
            Gizmos.color = Color.gray;
            float size = 1f;
            Gizmos.DrawCube(waypoint.Position, new Vector3(size, size, size));
        }
    }
}
