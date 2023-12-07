using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoint
{
    public Vector3 Position { get; private set; }

    public Waypoint(Vector3 position)
    {
        this.Position = position;
    }
}
