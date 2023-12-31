using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RivalCar : Car
{
    [SerializeField]
    private float topSpeed = 20f;

    private Waypoint currentWaypoint = null;

    private Collider obstacle = null;

    private RivalDetection rivalDetection;
    private CarWaypointManager carWaypointManager = null;

    private void Awake()
    {
        rivalDetection = GetComponent<RivalDetection>();
        carWaypointManager = GetComponent<CarWaypointManager>();    

        rivalDetection.closestObstacle.AddListener(SetObstacle);
    }

    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
        torque = maxTorque;
        currentDriveState = DriveState.Forward;       
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.CurrentState != GameState.Playing) { return; }

        SetSteer();
        Steer();
        Drive();
    }

    protected override void Drive()
    {
        if (currentWaypoint == null)
        {
            Stop();
            return;
        }

        if (GetMPH() > topSpeed) { Brake(); return; }

        foreach (Wheel wheel in wheels)
        {
            wheel.WheelCollider.brakeTorque = 0f;
            wheel.WheelCollider.motorTorque = torque;
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
        if(float.IsNaN(steer)) { return; }

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

    private void SetSteer()
    {
        float steerValue = 0f;
        currentWaypoint = carWaypointManager?.CurrentWaypoint;
        
        // Steering towards waypoint
        if (currentWaypoint != null)
        {
            Vector3 directionToWaypoint = transform.InverseTransformPoint(currentWaypoint.Position);
            steerValue += (directionToWaypoint.x / directionToWaypoint.magnitude) * steeringAngle;
        }

        // Obstacle avoidance
        if (obstacle != null)
        {
            Vector3 directionToObstacle = transform.InverseTransformPoint(obstacle.transform.position);

            // Steer in the opposite direction of the obstacle
            steerValue -= (directionToObstacle.x / directionToObstacle.magnitude) * steeringAngle;
        }

        steer = Mathf.Clamp(steerValue, -steeringAngle, steeringAngle);
    }
    
    private void SetObstacle(Collider obstacle)
    {
        this.obstacle = obstacle;
    }
}