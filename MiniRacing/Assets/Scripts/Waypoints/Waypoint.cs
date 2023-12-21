using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoint
{
    public Vector3 Position { get; private set; }
    public GameObject gameObject { get; private set; }

    public Waypoint(Vector3 position, GameObject gameObject)
    {
        this.Position = position;
        this.gameObject = gameObject;
    }
}
