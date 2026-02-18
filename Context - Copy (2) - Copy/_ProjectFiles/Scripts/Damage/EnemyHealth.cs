using UnityEngine;
using System;

/// <summary>
/// Manages enemy health, damage, and death behavior.
/// When enemy dies, it falls over (-90° X rotation) and stays for a few seconds before being destroyed.
/// 
/// SETUP:
/// 1. Attach to enemy prefab root
/// 2. Assign EnemyData ScriptableObject in Inspector
/// 3. Configure death settings (rotation speed, corpse lifetime, etc.)
/// 4. Requires: EnemyAI component, NavMeshAgent component
/// 
/// EVENTS:
/// - OnEnemyDeath (static): Broadcasts when ANY enemy dies - other systems can listen
/// - OnHealthChanged (instance): Broadcasts when THIS enemy's health changes - for health bar
/// 
/// DEATH BEHAVIOR:
/// - Disables AI and NavMesh (stops moving/attacking)
/// - Rotates -90° on X axis (falls flat on ground)
/// - Optionally disables collider (player can walk through corpse)
/// - Optionally sinks into ground before destruction
/// - Destroys GameObject after specified lifetime
/// </summary>
public class EnemyHealth : MonoBehaviour, IDamageable
{
    [Header("Data")]
    [SerializeField] private EnemyData enemyData;

    [Header("Death Animation")]
    [Tooltip("How fast enemy falls over (degrees per second)")]
    [SerializeField] private float deathRotationSpeed = 360f;

    [Tooltip("How long corpse stays before destruction (seconds)")]
    [SerializeField] private float corpseLifetime = 2f;

    [Header("Corpse Behavior")]
    [Tooltip("Disable collider on death so player can walk through corpse")]
    [SerializeField] private bool disableColliderOnDeath = true;

    [Tooltip("Make corpse sink into ground before destruction (visual polish)")]
    [SerializeField] private bool sinkIntoGround = false;

    [Tooltip("How fast to sink (units per second)")]
    [SerializeField] private float sinkSpeed = 0.5f;

    [Tooltip("Wait this long before starting to sink")]
    [SerializeField] private float sinkDelay = 1f;

    // Current state
    private float currentHealth;
    public bool IsDead { get; private set; }

    // Events
    public static event Action<EnemyHealth> OnEnemyDeath; // Static - any system can listen to ALL enemy deaths
    public event Action<float, float> OnHealthChanged; // Instance - for THIS enemy's health bar

    // Death animation tracking
    private bool isFallingOver = false;
    private bool isSinking = false;
    private Quaternion targetRotation;
    private float sinkTimer = 0f;

    private void Start()
    {
        // Initialize health from EnemyData
        if (enemyData != null)
        {
            currentHealth = enemyData.maxHealth;
            OnHealthChanged?.Invoke(currentHealth, enemyData.maxHealth);
        }
        else
        {
            Debug.LogError($"EnemyHealth on {gameObject.name}: No EnemyData assigned!");
            currentHealth = 50f; // Fallback value
        }

        IsDead = false;
    }

    private void Update()
    {
        // Animate falling over
        if (isFallingOver)
        {
            transform.rotation = Quaternion.RotateTowards(
                transform.rotation,
                targetRotation,
                deathRotationSpeed * Time.deltaTime
            );
        }

        // Animate sinking into ground (if enabled)
        if (isSinking)
        {
            sinkTimer += Time.deltaTime;

            if (sinkTimer >= sinkDelay)
            {
                Vector3 pos = transform.position;
                pos.y -= sinkSpeed * Time.deltaTime;
                transform.position = pos;
            }
        }
    }

    /// <summary>
    /// Called when enemy takes damage from any source (bullets, explosions, etc.)
    /// Implements IDamageable interface.
    /// </summary>
    public void TakeDamage(float damage)
    {
        if (IsDead) return;

        // Reduce health
        currentHealth -= damage;
        currentHealth = Mathf.Max(currentHealth, 0);

        // Notify health bar (if enemy has one)
        OnHealthChanged?.Invoke(currentHealth, enemyData.maxHealth);

        Debug.Log($"{gameObject.name} took {damage} damage. Health: {currentHealth}/{enemyData.maxHealth}");

        // Check for death
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    /// <summary>
    /// Handles enemy death: disables AI, plays effects, triggers fall-over animation
    /// </summary>
    private void Die()
    {
        IsDead = true;

        Debug.Log($"{gameObject.name} died!");

        // Broadcast death event (WaveManager, ScoreSystem, DropSystem can listen)
        OnEnemyDeath?.Invoke(this);

        // Disable AI and movement
        EnemyAI ai = GetComponent<EnemyAI>();
        if (ai != null)
        {
            ai.enabled = false;
        }

        UnityEngine.AI.NavMeshAgent agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        if (agent != null)
        {
            agent.enabled = false;
        }

        // Disable collider so player can walk through corpse
        if (disableColliderOnDeath)
        {
            Collider col = GetComponent<Collider>();
            if (col != null)
            {
                col.enabled = false;
            }
        }

        // Set up fall-over rotation (-90 degrees on X axis)
        Vector3 currentEuler = transform.eulerAngles;
        targetRotation = Quaternion.Euler(currentEuler.x - 90f, currentEuler.y, currentEuler.z);
        isFallingOver = true;

        // Enable sinking effect (if configured)
        if (sinkIntoGround)
        {
            isSinking = true;
        }

        // Play death effects
        if (enemyData != null)
        {
            // Death sound
            if (enemyData.deathSound != null)
            {
                AudioSource.PlayClipAtPoint(enemyData.deathSound, transform.position);
            }

            // Death particle effect
            if (enemyData.deathEffect != null)
            {
                GameObject effect = Instantiate(enemyData.deathEffect, transform.position, Quaternion.identity);
                Destroy(effect, 3f);
            }
        }

        // Destroy corpse after specified lifetime
        Destroy(gameObject, corpseLifetime);
    }

    /// <summary>
    /// Gets enemy data (used by drop system, score system, etc.)
    /// </summary>
    public EnemyData GetEnemyData()
    {
        return enemyData;
    }
}