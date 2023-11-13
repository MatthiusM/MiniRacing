using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RivalMovement : CarMovement
{
    private List<Waypoint> waypoints = new();
    private int currentWaypointIndex = 0;

    private readonly float distanceToWaypointThreshold = 10f;
    private float distanceToWaypoint;

    [SerializeField]
    private float targetSpeed = 10f;

    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
        waypoints = WaypointManager.Instance.GetWaypoints();
        currentDriveState = DriveState.Forward;
    }

    // Update is called once per frame
    void Update()
    {
        if (waypoints.Count == 0) return;

        Waypoint targetWaypoint = waypoints[currentWaypointIndex];
        Vector3 relativeVector = transform.InverseTransformPoint(targetWaypoint.Position);
        steer = (relativeVector.x / relativeVector.magnitude) * steeringAngle;

        distanceToWaypoint = Vector3.Distance(transform.position, targetWaypoint.Position);
        ChangeDriveState();
        if (distanceToWaypoint < distanceToWaypointThreshold) // Threshold for reaching the waypoint
        {
            currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Count;
            Debug.Log($"currentWaypointIndex: {currentWaypointIndex} / {waypoints.Count}");
        }        
    }

    private void FixedUpdate()
    {
        Steer();
        switch (currentDriveState)
        {
            case DriveState.Forward:
                Drive();
                StartCoroutine(IncreaseTorqueOverTime(0.5f));
                break;
            case DriveState.Braking:
                Brake();
                break;
        }
    }

    protected override void Drive()
    {
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


    private void ChangeDriveState()
    {
        if (distanceToWaypoint < distanceToWaypointThreshold && currentDriveState != DriveState.Braking && rb.velocity.magnitude > targetSpeed)
        {
            torque = 0;
            currentDriveState = DriveState.Braking;
        }
        else if (rb.velocity.magnitude <= targetSpeed && currentDriveState == DriveState.Braking)
        {
            torque = 0;
            currentDriveState = DriveState.Forward;
        }

        Debug.Log($"currentDriveState: {currentDriveState}");
    }

    private IEnumerator IncreaseTorqueOverTime(float duration)
    {
        float startTime = Time.time;
        while (Time.time - startTime < duration)
        {
            float progress = (Time.time - startTime) / duration;
            torque = Mathf.Lerp(0, maxTorque, progress);
            yield return null;
        }
        torque = maxTorque;
    }
}
