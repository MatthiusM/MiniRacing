using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarData : MonoBehaviour
{
    public bool finished = false;
    private new string name;
    public string placement;
    public string time;

    public string Name
    {
        get { return name; }
    }

    private void Start()
    {
        name = gameObject.name;
    }

}
