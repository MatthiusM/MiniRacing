using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RivalDetection : MonoBehaviour
{
    public UnityEvent<Collider> closestObstacle = new();

    [SerializeField]
    private float detectionRange = 3.5f;

    [SerializeField]
    private LayerMask obstacleLayerMask;

    Color randomColor;

    private Collider previousObstacle = null;

    private void Start()
    {
        randomColor = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
    }

    private void Update()
    {
        Collider currentClosestObstacle = FindClosestObstacle();

        if (currentClosestObstacle != previousObstacle)
        {
            closestObstacle.Invoke(currentClosestObstacle);

            previousObstacle = currentClosestObstacle;
        }
    }

    private Collider FindClosestObstacle()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, detectionRange, obstacleLayerMask);

        Collider closestObstacle = null;
        float closestDistance = float.MaxValue;

        foreach (Collider collider in colliders)
        {
            if (collider != this.GetComponent<Collider>())
            {
                float distance = Vector3.Distance(transform.position, collider.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestObstacle = collider;
                }
            }
        }

        return closestObstacle;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = randomColor;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }
}
