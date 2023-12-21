using UnityEngine;

public class WaypointRayCast : MonoBehaviour
{
    [SerializeField]
    private float rayLengthLeft = 10f;

    [SerializeField]
    private float rayLengthRight = 10f;

    [SerializeField]
    private LayerMask carLayer;

    void Update()
    {
        RaycastCheck(transform.position, -transform.right, rayLengthLeft);
        RaycastCheck(transform.position, transform.right, rayLengthRight);
    }

    void RaycastCheck(Vector3 startPosition, Vector3 direction, float rayCastLength)
    {
        RaycastHit hit;

        if (Physics.Raycast(startPosition, direction, out hit, rayCastLength, carLayer))
        {
            CarWaypointManager carWaypointManager = hit.collider.GetComponent<CarWaypointManager>();
            if (carWaypointManager.CurrentWaypointIndex == WaypointManager.Instance.GetWaypointIndex(this.gameObject))
            {
                carWaypointManager.IncrementWaypoint();
            }
            
        }
    }

    void OnDrawGizmos()
    {
        // left ray
        Gizmos.color = Color.blue; 
        Vector3 leftEnd = transform.position - transform.right * rayLengthLeft;
        Gizmos.DrawLine(transform.position, leftEnd);

        //right ray
        Gizmos.color = Color.red; 
        Vector3 rightEnd = transform.position + transform.right * rayLengthRight;
        Gizmos.DrawLine(transform.position, rightEnd);

        // Draw a cube to represent the GameObject
        Gizmos.color = Color.grey;
        Gizmos.DrawCube(transform.position, new Vector3(1, 1, 1));
    }
}
