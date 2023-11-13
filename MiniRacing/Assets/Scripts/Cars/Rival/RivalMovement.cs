using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RivalMovement : MonoBehaviour
{
    private List<Waypoint> waypoints = new();

    // Start is called before the first frame update
    void Start()
    {
        waypoints = WaypointManager.Instance.GetWaypoints();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
