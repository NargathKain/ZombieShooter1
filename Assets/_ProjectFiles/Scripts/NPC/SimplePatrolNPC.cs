using UnityEngine;

/// <summary>
/// Simple patrol NPC that moves between waypoints without NavMesh.
/// Uses direct transform movement.
///
/// SETUP:
/// 1. Attach to NPC GameObject
/// 2. Create empty GameObjects as waypoints
/// 3. Assign waypoints to the array
/// </summary>
public class SimplePatrolNPC : MonoBehaviour
{
    public enum PatrolMode { Loop, PingPong, Random }

    [Header("Waypoints")]
    [Tooltip("Waypoint GameObjects to patrol between")]
    [SerializeField] private GameObject[] waypoints;

    [Header("Movement")]
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float rotationSpeed = 5f;
    [SerializeField] private float arrivalDistance = 0.3f;
    [SerializeField] private PatrolMode patrolMode = PatrolMode.Loop;

    [Header("Waiting")]
    [SerializeField] private float waitTimeAtWaypoint = 2f;

    [Header("Animation (Optional)")]
    [SerializeField] private Animator animator;
    [SerializeField] private string walkingBool = "IsWalking";

    [Header("Grounding")]
    [Tooltip("Keep NPC grounded using raycast")]
    [SerializeField] private bool stickToGround = true;
    [SerializeField] private float groundCheckDistance = 2f;
    [SerializeField] private LayerMask groundLayer = ~0;
    [SerializeField] private float groundOffset = 0f;

    private int currentWaypointIndex;
    private int direction = 1; // For PingPong mode
    private float waitTimer;
    private bool isWaiting;
    private bool isMoving;

    private void Start()
    {
        if (waypoints == null || waypoints.Length == 0)
        {
            Debug.LogWarning("[SimplePatrolNPC] No waypoints assigned!");
            enabled = false;
            return;
        }

        currentWaypointIndex = 0;
        isMoving = true;
        UpdateAnimation();
    }

    private void Update()
    {
        if (waypoints == null || waypoints.Length == 0) return;

        if (isWaiting)
        {
            waitTimer -= Time.deltaTime;

            // Turn towards next waypoint while waiting
            TurnTowardsNextWaypoint();

            if (waitTimer <= 0f)
            {
                isWaiting = false;
                isMoving = true;
                SelectNextWaypoint();
                UpdateAnimation();
            }
            return;
        }

        MoveTowardsWaypoint();
        StickToGround();
    }

    private void StickToGround()
    {
        if (!stickToGround) return;

        Vector3 rayOrigin = transform.position + Vector3.up * 0.5f;
        if (Physics.Raycast(rayOrigin, Vector3.down, out RaycastHit hit, groundCheckDistance, groundLayer))
        {
            Vector3 pos = transform.position;
            pos.y = hit.point.y + groundOffset;
            transform.position = pos;
        }
    }

    private void TurnTowardsNextWaypoint()
    {
        // Preview next waypoint index
        int nextIndex = currentWaypointIndex;
        switch (patrolMode)
        {
            case PatrolMode.Loop:
                nextIndex = (currentWaypointIndex + 1) % waypoints.Length;
                break;
            case PatrolMode.PingPong:
                nextIndex = currentWaypointIndex + direction;
                if (nextIndex >= waypoints.Length) nextIndex = waypoints.Length - 2;
                if (nextIndex < 0) nextIndex = 1;
                break;
            case PatrolMode.Random:
                return; // Can't predict random, skip turning
        }

        if (waypoints[nextIndex] == null) return;

        Vector3 targetPos = waypoints[nextIndex].transform.position;
        Vector3 directionToNext = (new Vector3(targetPos.x, transform.position.y, targetPos.z) - transform.position).normalized;

        if (directionToNext != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(directionToNext);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }

    private void MoveTowardsWaypoint()
    {
        GameObject target = waypoints[currentWaypointIndex];
        if (target == null) return;

        Vector3 targetPosition = new Vector3(target.transform.position.x, transform.position.y, target.transform.position.z);
        Vector3 direction = (targetPosition - transform.position).normalized;
        float distance = Vector3.Distance(transform.position, targetPosition);

        // Rotate towards target
        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }

        // Move towards target
        if (distance > arrivalDistance)
        {
            // Don't overshoot - limit movement to remaining distance
            float moveDistance = Mathf.Min(moveSpeed * Time.deltaTime, distance);
            transform.position += direction * moveDistance;
        }
        else
        {
            // Snap to waypoint position (prevents overshoot)
            transform.position = targetPosition;

            // Arrived at waypoint
            isMoving = false;
            isWaiting = true;
            waitTimer = waitTimeAtWaypoint;
            UpdateAnimation();
        }
    }

    private void SelectNextWaypoint()
    {
        switch (patrolMode)
        {
            case PatrolMode.Loop:
                currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
                break;

            case PatrolMode.PingPong:
                currentWaypointIndex += direction;
                if (currentWaypointIndex >= waypoints.Length - 1 || currentWaypointIndex <= 0)
                {
                    direction *= -1;
                }
                break;

            case PatrolMode.Random:
                int newIndex;
                do
                {
                    newIndex = Random.Range(0, waypoints.Length);
                } while (newIndex == currentWaypointIndex && waypoints.Length > 1);
                currentWaypointIndex = newIndex;
                break;
        }
    }

    private void UpdateAnimation()
    {
        if (animator != null)
        {
            animator.SetBool(walkingBool, isMoving);
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (waypoints == null || waypoints.Length == 0) return;

        Gizmos.color = Color.green;
        for (int i = 0; i < waypoints.Length; i++)
        {
            if (waypoints[i] == null) continue;

            // Draw waypoint sphere
            Gizmos.DrawWireSphere(waypoints[i].transform.position, 0.3f);

            // Draw line to next waypoint
            int nextIndex = (i + 1) % waypoints.Length;
            if (waypoints[nextIndex] != null)
            {
                Gizmos.DrawLine(waypoints[i].transform.position, waypoints[nextIndex].transform.position);
            }
        }
    }
}
