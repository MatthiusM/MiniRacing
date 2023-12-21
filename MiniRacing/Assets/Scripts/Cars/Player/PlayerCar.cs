using Unity.Mathematics;
using UnityEngine;

public class PlayerCar : Car
{
    private DriveState previousDriveState;
    private float verticalInput;
    private float horizontalInput;

    new void Start()
    {
        base.Start();
        previousDriveState = currentDriveState;
    }

    void Update()
    {
        if (GameManager.Instance.CurrentState != GameState.Playing) { return; }

        ChangeDriveState();
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
        Steer();
    }

    private void FixedUpdate()
    {
        UpdateInputs();    
    }
    private void UpdateInputs()
    {
        verticalInput = Input.GetAxis("Vertical");
        horizontalInput = Input.GetAxis("Horizontal");
    }

    bool IsInput()
    {
        return (math.abs(verticalInput) > 0.1f);
    }

    protected override void Drive()
    {
        foreach (Wheel wheel in wheels)
        {
            wheel.WheelCollider.brakeTorque = 0f;
            wheel.WheelCollider.motorTorque = !IsInput() ? 0 : maxTorque * verticalInput;
        }
    }

    protected override void Brake()
    {
        if (!IsInput()) { return; }

        foreach (Wheel wheel in wheels)
        {
            wheel.WheelCollider.motorTorque = 0f;
            wheel.WheelCollider.brakeTorque = !IsInput() ? 0 : Mathf.Pow(Mathf.Abs(maxTorque * verticalInput), 3);
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
                wheel.WheelCollider.steerAngle = horizontalInput * steeringAngle;
                wheel.gameobject.transform.localRotation = Quaternion.Euler(0, horizontalInput * steeringAngle, 0);
            }
        }

        AdjustCenterOfMassBased();
    }

    private void ChangeDriveState()
    {
        bool isNearlyStopped = GetMPH() <= 0;

        previousDriveState = currentDriveState;

        switch (currentDriveState)
        {
            case DriveState.Stopped:
                if (verticalInput > 0)
                {
                    currentDriveState = DriveState.Forward;
                }
                else if (verticalInput < 0)
                {
                    currentDriveState = DriveState.Reversing;
                }
                break;
            case DriveState.Forward:
                if (verticalInput < 0)
                {
                    currentDriveState = isNearlyStopped ? DriveState.Stopped : DriveState.Braking;
                }
                break;
            case DriveState.Braking:
                if (isNearlyStopped)
                {
                    currentDriveState = DriveState.Stopped;
                }
                else if (verticalInput > 0)
                {
                    currentDriveState = DriveState.Forward;
                }
                else if (verticalInput < 0)
                {
                    currentDriveState = DriveState.Reversing;
                }
                break;
            case DriveState.Reversing:
                if (verticalInput > 0)
                {
                    currentDriveState = isNearlyStopped ? DriveState.Stopped : DriveState.Braking;
                }
                break;
        }

        Debug.Log($"Current Drive State: {currentDriveState}");
    }
}
