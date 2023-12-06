using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RivalCar : Car
{
    private RivalPathfinding pathfinding;

    [SerializeField]
    float topSpeed = 10f;

    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
        pathfinding = new RivalPathfinding(WaypointManager.Instance.GetWaypoints());
        torque = maxTorque;
        currentDriveState = DriveState.Forward;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateWaypoint();
    }

    private void FixedUpdate()
    {
        Steer();
        Drive();
    }

    protected override void Drive()
    {
        if (rb.velocity.magnitude > topSpeed) { Brake(); return; }

        foreach (Wheel wheel in wheels)
        {
            wheel.WheelCollider.brakeTorque = 0f;
            wheel.WheelCollider.motorTorque = torque * Time.deltaTime;
        }
    }

    protected override void Brake()
    {
        foreach (Wheel wheel in wheels)
        {
            wheel.WheelCollider.motorTorque = 0f;
            wheel.WheelCollider.brakeTorque = Mathf.Pow(Mathf.Abs(torque), 3) * Time.deltaTime;
        }
    }

    protected override void Stop()
    {
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        foreach (Wheel wheel in wheels)
        {
            wheel.WheelCollider.motorTorque = 0f;
            wheel.WheelCollider.brakeTorque = 1000f;
        }
    }

    protected override void Steer()
    {
        foreach (Wheel wheel in wheels)
        {
            if (wheel.axle == Axle.Front)
            {
                wheel.WheelCollider.steerAngle = steer;
                wheel.gameobject.transform.localRotation = Quaternion.Euler(0, steer, 0);
            }
        }
        AdjustCenterOfMassBased();
    }

    private void UpdateWaypoint()
    {
        pathfinding.UpdateWaypoint(transform.position);

        Waypoint targetWaypoint = pathfinding.GetCurrentWaypoint();
        Vector3 directionToWaypoint = transform.InverseTransformPoint(targetWaypoint.Position);
        steer = (directionToWaypoint.x / directionToWaypoint.magnitude) * steeringAngle;

        Debug.Log($"Next waypoint: {pathfinding.CurrentWaypointIndex}");
    }
}