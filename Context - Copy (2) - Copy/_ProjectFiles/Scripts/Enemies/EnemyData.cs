using UnityEngine;

/// <summary>
/// ScriptableObject that defines an enemy's properties.
/// Designer can create different enemy types by creating multiple assets.
/// Create via: Right-click → Create → Game/Enemy Data
/// 
/// USAGE:
/// 1. Create asset: Right-click in Project → Create → Game/Enemy Data
/// 2. Name it (e.g., "BasicZombie", "FastZombie", "TankZombie")
/// 3. Configure values in Inspector
/// 4. Assign to EnemyHealth component on enemy prefab
/// </summary>
[CreateAssetMenu(fileName = "New Enemy", menuName = "Game/Enemy Data")]
public class EnemyData : ScriptableObject
{
    [Header("Basic Info")]
    [Tooltip("Display name of this enemy type")]
    public string enemyName = "Zombie";

    [Tooltip("Prefab used for spawning (optional, for wave system)")]
    public GameObject enemyPrefab;

    [Header("Combat Stats")]
    [Tooltip("Maximum health this enemy starts with")]
    public float maxHealth = 50f;

    [Tooltip("Damage dealt to player per attack")]
    public float attackDamage = 10f;

    [Tooltip("Time between attacks (seconds)")]
    public float attackCooldown = 1.5f;

    [Tooltip("How close enemy must be to attack player")]
    public float attackRange = 2f;

    [Header("Movement")]
    [Tooltip("NavMesh agent movement speed")]
    public float moveSpeed = 3f;

    [Tooltip("How far enemy can detect player")]
    public float detectionRange = 15f;

    [Tooltip("How close to get before stopping (personal space)")]
    public float stoppingDistance = 1.5f;

    [Header("Rewards")]
    [Tooltip("Points awarded to player when this enemy dies")]
    public int pointsOnDeath = 100;

    [Tooltip("Chance to drop pickup (0.0 = never, 1.0 = always)")]
    [Range(0f, 1f)]
    public float dropChance = 0.3f;

    [Header("Visual Effects")]
    [Tooltip("Sound played when enemy attacks")]
    public AudioClip attackSound;

    [Tooltip("Sound played when enemy dies")]
    public AudioClip deathSound;

    [Tooltip("Particle effect spawned on death")]
    public GameObject deathEffect;

    [Header("Audio")]
    [Tooltip("Random idle sounds (groans, moans)")]
    public AudioClip[] idleSounds;

    [Tooltip("Time between idle sounds")]
    public float idleSoundInterval = 5f;
}