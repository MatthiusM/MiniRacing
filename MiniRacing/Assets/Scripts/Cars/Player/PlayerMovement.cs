using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : CarMovement
{    
    [SerializeField]
    private float stoppingSpeedThreshold = 1f; 
    
    private DriveState previousDriveState;

    new void Start()
    {
        base.Start();
        previousDriveState = currentDriveState;
    }

    void Update()
    {
        GetInput();
        ChangeDriveState();
    }

    private void FixedUpdate()
    {
        Steer();
        switch (currentDriveState)
        {
            case DriveState.Forward:
            case DriveState.Reversing:
                Drive();
                break;
            case DriveState.Braking:
                Brake();
                break;
            case DriveState.Stopped:
                Stop();
                break;
        }
    }

    private void GetInput()
    {
        torque = maxTorque * Input.GetAxis("Vertical");
        steer = Input.GetAxis("Horizontal");
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
            if(wheel.axle == Axle.Front)
            {
                wheel.WheelCollider.steerAngle = steer * steeringAngle;
                wheel.gameobject.transform.localRotation = Quaternion.Euler(0, steer * steeringAngle, 0);
            }           
        }

        AdjustCenterOfMassBased();
    }
    
    private void ChangeDriveState()
    {
        float input = Input.GetAxis("Vertical");
        bool isNearlyStopped = rb.velocity.magnitude < stoppingSpeedThreshold;

        previousDriveState = currentDriveState;

        switch (currentDriveState)
        {
            case DriveState.Stopped:
                if (input > 0)
                {
                    currentDriveState = DriveState.Forward;
                }
                else if (input < 0)
                {
                    currentDriveState = DriveState.Reversing;
                }
                break;
            case DriveState.Forward:
                if (input <= 0 && isNearlyStopped)
                {
                    currentDriveState = DriveState.Stopped;
                }
                else if (input <= 0)
                {
                    currentDriveState = DriveState.Braking;
                }
                break;
            case DriveState.Braking:
                if (isNearlyStopped)
                {
                    currentDriveState = DriveState.Stopped;
                }
                else if (input > 0 && previousDriveState == DriveState.Forward)
                {
                    currentDriveState = DriveState.Forward;
                }
                else if (input < 0 && previousDriveState == DriveState.Reversing)
                {
                    currentDriveState = DriveState.Reversing;
                }
                break;
            case DriveState.Reversing:
                if (input >= 0 && isNearlyStopped)
                {
                    currentDriveState = DriveState.Stopped;
                }
                else if (input >= 0)
                {
                    currentDriveState = DriveState.Braking;
                }
                break;
        }

        Debug.Log($"Current Drive State: {currentDriveState}");
    }
}
