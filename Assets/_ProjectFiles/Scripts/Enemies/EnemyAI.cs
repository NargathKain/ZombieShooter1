using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// Basic zombie AI that chases and attacks the player.
/// Uses NavMesh for pathfinding.
/// Damages player when in range (like Minecraft zombies).
/// 
/// SETUP:
/// 1. Add NavMeshAgent component to enemy
/// 2. Add this script to enemy
/// 3. Assign EnemyData ScriptableObject
/// 4. Make sure scene has NavMesh baked
/// 5. Player must be tagged "Player"
/// 
/// REQUIRES:
/// - EnemyHealth component on same GameObject
/// - NavMeshAgent component
/// - EnemyData asset assigned
/// - Player tagged as "Player"
/// </summary>
[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(EnemyHealth))]
public class EnemyAI : MonoBehaviour
{
    [Header("Data")]
    [SerializeField] private EnemyData enemyData;

    [Header("Debug")]
    [SerializeField] private bool showDebugGizmos = true;

    // References
    private NavMeshAgent agent;
    private EnemyHealth health;
    private Transform player;
    private PlayerHealth playerHealth;
    private Animator animator;

    // State
    private float attackTimer;
    private bool isChasing;
    private float idleSoundTimer;

    // Constants
    private const string PLAYER_TAG = "Player";

    private void Start()
    {
        // Get required components
        agent = GetComponent<NavMeshAgent>();
        health = GetComponent<EnemyHealth>();
        animator = GetComponentInChildren<Animator>();

        // Validate
        if (enemyData == null)
        {
            Debug.LogError($"EnemyAI on {gameObject.name}: No EnemyData assigned!");
            enabled = false;
            return;
        }

        // Configure NavMeshAgent from EnemyData
        agent.speed = enemyData.moveSpeed;
        agent.stoppingDistance = enemyData.stoppingDistance;

        // Find player
        GameObject playerObj = GameObject.FindGameObjectWithTag(PLAYER_TAG);
        if (playerObj != null)
        {
            player = playerObj.transform;
            playerHealth = playerObj.GetComponent<PlayerHealth>();

            if (playerHealth == null)
            {
                Debug.LogError($"EnemyAI: Player doesn't have PlayerHealth component!");
            }
        }
        else
        {
            Debug.LogError($"EnemyAI: No GameObject tagged '{PLAYER_TAG}' found!");
            enabled = false;
        }

        // Initialize timers
        attackTimer = 0f;
        idleSoundTimer = enemyData.idleSoundInterval;
    }

    private void Update()
    {
        // Don't do anything if dead
        if (health.IsDead)
        {
            agent.isStopped = true;
            return;
        }

        // Don't chase if player is dead
        if (playerHealth != null && playerHealth.IsDead)
        {
            agent.isStopped = true;
            isChasing = false;
            return;
        }

        // Check if player is in detection range
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= enemyData.detectionRange)
        {
            // Chase player
            ChasePlayer();

            // Attack if in range
            if (distanceToPlayer <= enemyData.attackRange)
            {
                AttackPlayer();
            }
        }
        else
        {
            // Stop chasing if player too far
            if (isChasing)
            {
                agent.isStopped = true;
                isChasing = false;
            }
        }

        // Update timers
        attackTimer -= Time.deltaTime;

        // Play idle sounds occasionally
        PlayIdleSounds();

        // Update animator
        UpdateAnimator();
    }

    /// <summary>
    /// Update animator parameters based on chase state
    /// </summary>
    private void UpdateAnimator()
    {
        if (animator == null) return;

        // Use chase state for stable animation, not fluctuating velocity
        float targetSpeed = isChasing ? agent.speed : 0f;
        animator.SetFloat("Speed", targetSpeed, 0.15f, Time.deltaTime);
    }

    /// <summary>
    /// Move toward player using NavMesh
    /// </summary>
    private void ChasePlayer()
    {
        if (!isChasing)
        {
            agent.isStopped = false;
            isChasing = true;
        }

        agent.SetDestination(player.position);
    }

    /// <summary>
    /// Damage player when in attack range (Minecraft-style)
    /// </summary>
    private void AttackPlayer()
    {
        // Cooldown between attacks
        if (attackTimer > 0f) return;

        // Deal damage to player
        if (playerHealth != null && !playerHealth.IsDead)
        {
            playerHealth.TakeDamage(enemyData.attackDamage);

            Debug.Log($"{enemyData.enemyName} attacked player for {enemyData.attackDamage} damage!");

            // Play attack sound
            if (enemyData.attackSound != null)
            {
                AudioSource.PlayClipAtPoint(enemyData.attackSound, transform.position);
            }

            // Reset cooldown
            attackTimer = enemyData.attackCooldown;
        }
    }

    /// <summary>
    /// Play random idle sounds periodically
    /// </summary>
    private void PlayIdleSounds()
    {
        if (enemyData.idleSounds == null || enemyData.idleSounds.Length == 0) return;

        idleSoundTimer -= Time.deltaTime;

        if (idleSoundTimer <= 0f)
        {
            // Pick random idle sound
            AudioClip randomSound = enemyData.idleSounds[Random.Range(0, enemyData.idleSounds.Length)];

            if (randomSound != null)
            {
                AudioSource.PlayClipAtPoint(randomSound, transform.position, 0.5f);
            }

            // Reset timer with some randomness
            idleSoundTimer = enemyData.idleSoundInterval + Random.Range(-1f, 1f);
        }
    }

    /// <summary>
    /// Draw debug visualizations in Scene view
    /// </summary>
    private void OnDrawGizmos()
    {
        if (!showDebugGizmos || enemyData == null) return;

        // Detection range (yellow sphere)
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, enemyData.detectionRange);

        // Attack range (red sphere)
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, enemyData.attackRange);

        // Line to player (if chasing)
        if (isChasing && player != null)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawLine(transform.position, player.position);
        }
    }
}