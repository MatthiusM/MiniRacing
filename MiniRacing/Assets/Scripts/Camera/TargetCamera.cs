using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class TargetCamera : MonoBehaviour
{
    private CinemachineVirtualCamera cinemachineCamera;

    [SerializeField] 
    private GameObject cameraTarget;

    private void Start()
    {
        cinemachineCamera = GetComponent<CinemachineVirtualCamera>();

        cinemachineCamera.Follow = cameraTarget.transform;
        cinemachineCamera.LookAt = cameraTarget.transform;
    }
}
