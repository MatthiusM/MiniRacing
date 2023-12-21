using System.Collections.Generic;
using UnityEngine;

public class WaypointManager : MonoBehaviour
{
    private List<Waypoint> waypoints = new List<Waypoint>();
    private Dictionary<GameObject, int> waypointMap = new Dictionary<GameObject, int>();

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

    public void AddWaypoint(Vector3 position, GameObject gameObject)
    {
        waypoints.Add(new Waypoint(position, gameObject));       
    }

    private void InitWaypoints()
    {
        int childNum = 0;
        foreach (Transform child in transform)
        {
            AddWaypoint(child.position, child.gameObject);
            waypointMap[child.gameObject] = childNum;
            childNum++;
        }
    }

    public List<Waypoint> GetWaypoints()
    {
        return waypoints;
    }

    public int GetWaypointIndex(GameObject gameObject)
    {
        if (waypointMap.TryGetValue(gameObject, out int waypoint))
        {
            return waypoint;
        }
        return -1;
    }

    public Waypoint GetWaypointByIndex(int index)
    {
        if (index >= 0 && index < waypoints.Count)
        {
            return waypoints[index];
        }
        else
        {
            Debug.LogError("Invalid waypoint index: " + index);
            return null;
        }
    }
}
