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

    void acceleromiterDirection()
    {
        Vector3 tilt = Quaternion.Euler(90, 0, 0) * Input.acceleration;

        float xRotation = tilt.x;

        float yRotation = tilt.y;

        verticalInput = 0;
        horizontalInput = 0;

        if (xRotation > 0.1) // right
        {
            horizontalInput = 1;
        }
        else if (xRotation < -0.1) // left
        {
            horizontalInput = -1;
        }

        if (yRotation > 0.1) // forwards
        {
            Debug.Log("forwards");
            verticalInput = 1;
        }
        else if (yRotation < -0.1) // backwards
        {
            Debug.Log("backwards");
            verticalInput = -1;
        }
    }

    private void FixedUpdate()
    {
        //UpdateInputs();
        acceleromiterDirection();
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
            if (GetMPH() >= SavedData.instance.MaxSpeed)
            {
                wheel.WheelCollider.brakeTorque = 0f;
                wheel.WheelCollider.motorTorque = 0f;
            }
            else {
                wheel.WheelCollider.brakeTorque = 0f;
                wheel.WheelCollider.motorTorque = !IsInput() ? 0 : maxTorque * verticalInput;
            }
            
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
                    currentDriveState = DriveState.Braking;
                }
                else if (Mathf.Approximately(verticalInput, 0f) && isNearlyStopped)
                {
                    currentDriveState = DriveState.Stopped;
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
                    currentDriveState = DriveState.Braking;
                }
                else if (Mathf.Approximately(verticalInput, 0f) && isNearlyStopped)
                {
                    currentDriveState = DriveState.Stopped;
                }
                break;
        }

        //Debug.Log($"Current Drive State: {currentDriveState}");
    }
}
