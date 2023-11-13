using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CarMovement : MonoBehaviour
{
    [SerializeField]
    protected Wheel[] wheels = new Wheel[4];

    [SerializeField]
    protected float maxTorque = 10000f;

    protected readonly float steeringAngle = 30.0f;

    protected DriveState currentDriveState = DriveState.Stopped;

    protected float torque;
    protected float steer;

    protected Rigidbody rb;
    protected Vector3 centerOfMass;
    protected Vector3 centerOfMassOffset = new(0.3f, 0, 0);

    protected void Start()
    {
        rb = GetComponent<Rigidbody>();
        centerOfMass = rb.centerOfMass;
    }

    protected abstract void Drive();
    protected abstract void Brake();
    protected abstract void Steer();
    protected abstract void Stop();

    protected void AdjustCenterOfMassBased()
    {
        if (steer < 0) // Turning left
        {
            rb.centerOfMass = centerOfMass - centerOfMassOffset;
        }
        else if (steer > 0) // Turning right
        {
            rb.centerOfMass = centerOfMass + centerOfMassOffset;
        }
        else // No steering
        {
            rb.centerOfMass = centerOfMass;
        }
    }

}
