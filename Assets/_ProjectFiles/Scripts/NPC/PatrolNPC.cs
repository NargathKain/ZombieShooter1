using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// NPC that patrols between waypoints or wanders randomly within an area.
/// Uses NavMeshAgent for movement.
///
/// SETUP:
/// 1. Attach to NPC with NavMeshAgent component
/// 2. Choose patrol mode: Waypoints or Random Wander
/// 3. For Waypoints: Create empty GameObjects as waypoints, assign to array
/// 4. For Random: Set wander radius and center point
/// </summary>
public class PatrolNPC : MonoBehaviour
{
    public enum PatrolMode { Waypoints, RandomWander }

    [Header("Patrol Settings")]
    [SerializeField] private PatrolMode mode = PatrolMode.RandomWander;

    [Header("Waypoint Mode")]
    [Tooltip("Array of waypoint transforms to patrol between")]
    [SerializeField] private Transform[] waypoints;
    [Tooltip("Wait time at each waypoint")]
    [SerializeField] private float waypointWaitTime = 2f;

    [Header("Random Wander Mode")]
    [Tooltip("Center point for wandering (uses start position if null)")]
    [SerializeField] private Transform wanderCenter;
    [Tooltip("Radius to wander within")]
    [SerializeField] private float wanderRadius = 10f;
    [Tooltip("Time to wait before picking new destination")]
    [SerializeField] private float wanderWaitTime = 3f;

    [Header("Movement")]
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float stoppingDistance = 0.5f;

    [Header("Animation (Optional)")]
    [SerializeField] private Animator animator;
    [SerializeField] private string speedParameter = "Speed";

    private NavMeshAgent agent;
    private int currentWaypointIndex;
    private float waitTimer;
    private Vector3 startPosition;
    private bool isWaiting;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        if (agent == null)
        {
            Debug.LogError("[PatrolNPC] NavMeshAgent required! Adding one.");
            agent = gameObject.AddComponent<NavMeshAgent>();
        }

        agent.speed = moveSpeed;
        agent.stoppingDistance = stoppingDistance;
        startPosition = transform.position;

        if (wanderCenter == null)
        {
            // Use start position as wander center
        }
    }

    private void Start()
    {
        SetNextDestination();
    }

    private void Update()
    {
        // Handle waiting at destination
        if (isWaiting)
        {
            waitTimer -= Time.deltaTime;
            if (waitTimer <= 0f)
            {
                isWaiting = false;
                SetNextDestination();
            }
        }
        else if (!agent.pathPending && agent.remainingDistance <= stoppingDistance)
        {
            // Arrived at destination
            StartWaiting();
        }

        // Update animator
        if (animator != null)
        {
            float speed = agent.velocity.magnitude / moveSpeed;
            animator.SetFloat(speedParameter, speed);
        }
    }

    private void StartWaiting()
    {
        isWaiting = true;
        waitTimer = (mode == PatrolMode.Waypoints) ? waypointWaitTime : wanderWaitTime;
    }

    private void SetNextDestination()
    {
        Vector3 destination;

        if (mode == PatrolMode.Waypoints)
        {
            if (waypoints == null || waypoints.Length == 0)
            {
                Debug.LogWarning("[PatrolNPC] No waypoints assigned!");
                return;
            }

            destination = waypoints[currentWaypointIndex].position;
            currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
        }
        else // RandomWander
        {
            destination = GetRandomPointInRadius();
        }

        agent.SetDestination(destination);
    }

    private Vector3 GetRandomPointInRadius()
    {
        Vector3 center = (wanderCenter != null) ? wanderCenter.position : startPosition;

        for (int i = 0; i < 10; i++) // Try 10 times to find valid point
        {
            Vector3 randomDirection = Random.insideUnitSphere * wanderRadius;
            randomDirection.y = 0;
            Vector3 randomPoint = center + randomDirection;

            NavMeshHit hit;
            if (NavMesh.SamplePosition(randomPoint, out hit, 2f, NavMesh.AllAreas))
            {
                return hit.position;
            }
        }

        // Fallback to center if no valid point found
        return center;
    }

    private void OnDrawGizmosSelected()
    {
        // Draw wander radius
        if (mode == PatrolMode.RandomWander)
        {
            Gizmos.color = Color.yellow;
            Vector3 center = (wanderCenter != null) ? wanderCenter.position :
                             (Application.isPlaying ? startPosition : transform.position);
            Gizmos.DrawWireSphere(center, wanderRadius);
        }

        // Draw waypoint path
        if (mode == PatrolMode.Waypoints && waypoints != null && waypoints.Length > 1)
        {
            Gizmos.color = Color.cyan;
            for (int i = 0; i < waypoints.Length; i++)
            {
                if (waypoints[i] == null) continue;

                int nextIndex = (i + 1) % waypoints.Length;
                if (waypoints[nextIndex] != null)
                {
                    Gizmos.DrawLine(waypoints[i].position, waypoints[nextIndex].position);
                }
                Gizmos.DrawWireSphere(waypoints[i].position, 0.3f);
            }
        }
    }
}
