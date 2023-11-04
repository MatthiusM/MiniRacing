using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public enum Axle
    {
        Front,
        Rear
    }

    [System.Serializable]
    public struct Wheel
    {
        public GameObject gameobject;
        public WheelCollider WheelCollider;
        public Axle axle;
    }

    [SerializeField]
    private Wheel[] wheels = new Wheel[4];

    [SerializeField]
    private float maxTorque = 10000f;

    [SerializeField]
    private float stopTolerance = 1f; 

    private float torque;
    private float steer;

    private readonly float steeringAngle = 30.0f;

    private enum DriveState
    {
        Forward,
        Braking,
        Reversing,
        Stopped
    }

    private DriveState currentDriveState = DriveState.Stopped;
    private DriveState previousDriveState = DriveState.Stopped;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.centerOfMass = transform.position; //adjuct the center of mass so the car doesn't tip over when turning
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

    private void Drive()
    {
        foreach (Wheel wheel in wheels)
        {
            wheel.WheelCollider.brakeTorque = 0f;
            wheel.WheelCollider.motorTorque = torque * Time.deltaTime;
        }
    }

    private void Brake()
    {
        foreach (Wheel wheel in wheels)
        {
            wheel.WheelCollider.motorTorque = 0f;
            wheel.WheelCollider.brakeTorque = Mathf.Pow(Mathf.Abs(torque), 3) * Time.deltaTime;
        }
    }

    private void ChangeDriveState()
    {
        float input = Input.GetAxis("Vertical");
        bool isNearlyStopped = rb.velocity.magnitude < stopTolerance;

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
    
    private void Stop()
    {
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        foreach (Wheel wheel in wheels)
        {
            wheel.WheelCollider.motorTorque = 0f;
            wheel.WheelCollider.brakeTorque = 1000f;
        }
    }

    private void Steer()
    {
        foreach (Wheel wheel in wheels)
        {
            if(wheel.axle == Axle.Front)
            {
                wheel.WheelCollider.steerAngle = steer * steeringAngle;
                wheel.gameobject.transform.localRotation = Quaternion.Euler(0, steer * steeringAngle, 0);
            }           
        }
    }
}
