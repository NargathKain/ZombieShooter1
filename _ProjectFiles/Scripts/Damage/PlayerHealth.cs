using UnityEngine;
using System;
using Synty.AnimationBaseLocomotion.Samples.InputSystem;

/// <summary>
/// Manages player health system.
/// Broadcasts events when health changes or player dies.
/// Attach this component to the Player GameObject.
///
/// EVENTS:
/// - OnHealthChanged: Fired when health value changes (for UI updates)
/// - OnPlayerDeath: Fired when health reaches 0 (for game over)
/// - OnPlayerRespawn: Fired when player respawns (for future use)
/// </summary>
public class PlayerHealth : MonoBehaviour, IDamageable
{
    [Header("Health Settings")]
    [SerializeField] private float maxHealth = 100f;
    private float currentHealth;

    [Header("Debug")]
    [SerializeField] private float debugDamageAmount = 10f;

    // Input reference
    private InputReader inputReader;

    // Events for other systems to listen to
    public static event Action<float, float> OnHealthChanged; // current, max
    public static event Action OnPlayerDeath;
    public static event Action OnPlayerRespawn;

    // Public property so other scripts can check if player is dead
    public bool IsDead { get; private set; }

    private void Start()
    {
        currentHealth = maxHealth;
        IsDead = false;

        // Find InputReader and subscribe to debug damage event
        inputReader = FindFirstObjectByType<InputReader>();
        //if (inputReader != null)
        //{
        //    inputReader.onDebugDamage += OnDebugDamage;
        //}

        // Notify any listeners of initial health
        OnHealthChanged?.Invoke(currentHealth, maxHealth);
    }

    private void OnDestroy()
    {
        //// Unsubscribe from events
        //if (inputReader != null)
        //{
        //    inputReader.onDebugDamage -= OnDebugDamage;
        //}
    }

    /// <summary>
    /// Called when debug damage key (K) is pressed via InputReader.
    /// </summary>
    private void OnDebugDamage()
    {
        TakeDamage(debugDamageAmount);
    }

    /// <summary>
    /// Called when player takes damage from any source (enemies, hazards, etc.)
    /// </summary>
    public void TakeDamage(float damage)
    {
        if (IsDead) return;

        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        // Broadcast health change event
        OnHealthChanged?.Invoke(currentHealth, maxHealth);

        Debug.Log($"Player took {damage} damage. Health: {currentHealth}/{maxHealth}");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    /// <summary>
    /// Heals the player by specified amount
    /// </summary>
    public void Heal(float amount)
    {
        if (IsDead) return;

        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        OnHealthChanged?.Invoke(currentHealth, maxHealth);

        Debug.Log($"Player healed {amount}. Health: {currentHealth}/{maxHealth}");
    }

    /// <summary>
    /// Handles player death
    /// </summary>
    private void Die()
    {
        IsDead = true;
        Debug.Log("Player died!");

        // Broadcast death event (GameManager will handle game over)
        OnPlayerDeath?.Invoke();

        // Future: Play death animation, disable controls, etc.
    }

    /// <summary>
    /// Call this to respawn the player (from GameManager in future)
    /// </summary>
    public void Respawn()
    {
        currentHealth = maxHealth;
        IsDead = false;

        OnHealthChanged?.Invoke(currentHealth, maxHealth);
        OnPlayerRespawn?.Invoke();

        Debug.Log("Player respawned!");
    }
}
