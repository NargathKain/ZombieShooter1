using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Production-quality physical projectile for a 3rd-person zombie shooter.
/// Moves forward using Rigidbody velocity for accurate physics interactions,
/// detects collisions via trigger events, and applies damage through the
/// <see cref="IDamageable"/> interface. Supports shooter exclusion to prevent
/// self-damage, parent-hierarchy damage lookup for child hitboxes on enemies,
/// and configurable impact effects.
///
/// <para><b>Setup:</b></para>
/// <list type="number">
///   <item>Create a small sphere/capsule prefab (scale ~0.05-0.1).</item>
///   <item>Add a Rigidbody (Use Gravity = false, Is Kinematic = false).</item>
///   <item>Add a Collider set to Is Trigger = true.</item>
///   <item>Attach this script.</item>
///   <item>Optional: Add a Trail Renderer for a visual tail effect.</item>
/// </list>
///
/// <para><b>Usage (from WeaponController):</b></para>
/// <code>
/// Bullet bullet = bulletObj.GetComponent&lt;Bullet&gt;();
/// bullet.Initialize(weaponData.damage, aimDirection);
/// bullet.SetShooter(playerGameObject);
/// </code>
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class Bullet : MonoBehaviour
{
    #region Constants

    /// <summary>Prefix for all console log messages from this system.</summary>
    private const string LOG_PREFIX = "[Bullet]";

    #endregion

    #region Serialized Fields

    [Header("Projectile Settings")]
    [Tooltip("Speed of the bullet in units per second.")]
    [SerializeField] private float speed = 100f;

    [Tooltip("Time in seconds before the bullet self-destructs if it hits nothing.")]
    [SerializeField] private float lifetime = 3f;

    [Header("Visual Orientation")]
    [Tooltip("Rotation offset applied to align the bullet mesh correctly. Default (90,0,0) works for capsules with long axis on Y.")]
    [SerializeField] private Vector3 visualRotationOffset = new Vector3(90f, 0f, 0f);

    [Header("Impact Effects")]
    [Tooltip("Optional prefab spawned at the impact point on collision (e.g., spark/dust particle).")]
    [SerializeField] private GameObject impactEffectPrefab;

    [Tooltip("How long the impact effect lives before being destroyed.")]
    [SerializeField] private float impactEffectLifetime = 2f;

    #endregion

    #region Private Fields

    /// <summary>Cached Rigidbody component. Null if missing (fallback to transform movement).</summary>
    private Rigidbody rb;

    /// <summary>The damage this bullet deals on hit.</summary>
    private float damage;

    /// <summary>The direction this bullet travels (normalized).</summary>
    private Vector3 moveDirection;

    /// <summary>The root GameObject of whoever fired this bullet, used to prevent self-hits.</summary>
    private GameObject shooterRoot;

    /// <summary>Whether the Rigidbody was found and is usable for velocity-based movement.</summary>
    private bool hasRigidbody;

    /// <summary>Whether Initialize() has been called with an explicit direction override.</summary>
    private bool isInitialized;

    /// <summary>
    /// Tracks root GameObjects already damaged this frame/collision sequence to prevent
    /// double-hits when a bullet overlaps multiple child colliders on the same entity.
    /// </summary>
    private readonly HashSet<GameObject> damagedTargets = new HashSet<GameObject>();

    #endregion

    #region Unity Lifecycle

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        hasRigidbody = rb != null;

        if (hasRigidbody)
        {
            // Configure Rigidbody for projectile behavior
            rb.useGravity = false;
            rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
            rb.interpolation = RigidbodyInterpolation.Interpolate;
        }
        else
        {
            Debug.LogWarning($"{LOG_PREFIX} No Rigidbody found on {gameObject.name}. " +
                             "Falling back to transform-based movement. Add a Rigidbody for proper physics.");
        }
    }

    private void Start()
    {
        // Apply velocity if Rigidbody is available.
        // If Initialize() was called before Start, use the overridden direction.
        // Otherwise, default to the bullet's current forward direction.
        if (!isInitialized)
        {
            moveDirection = transform.forward;
        }

        ApplyVelocity();

        // Self-destruct safety net -- prevents orphaned bullets from leaking
        Destroy(gameObject, lifetime);
    }

    private void Update()
    {
        // Fallback movement when Rigidbody is missing or kinematic
        if (!hasRigidbody)
        {
            transform.position += moveDirection * speed * Time.deltaTime;
        }
    }

    #endregion

    #region Public API

    /// <summary>
    /// Initializes the bullet with damage and an explicit travel direction.
    /// Must be called after instantiation and before the first physics step for accurate aim.
    /// Overrides the bullet's forward direction so it flies toward the camera crosshair target.
    /// </summary>
    /// <param name="damage">The amount of damage to deal on hit.</param>
    /// <param name="direction">Normalized world-space direction for the bullet to travel.</param>
    public void Initialize(float damage, Vector3 direction)
    {
        this.damage = Mathf.Max(0f, damage);

        if (direction.sqrMagnitude > 0.0001f)
        {
            moveDirection = direction.normalized;
            // Apply look rotation then add visual offset to correct mesh orientation
            transform.rotation = Quaternion.LookRotation(moveDirection) * Quaternion.Euler(visualRotationOffset);
        }
        else
        {
            Debug.LogWarning($"{LOG_PREFIX} Initialize called with near-zero direction. Using transform.forward.");
            moveDirection = transform.forward;
        }

        isInitialized = true;

        // If Rigidbody is already available (Awake has run), apply velocity immediately.
        // Otherwise, Start() will handle it.
        ApplyVelocity();
    }

    /// <summary>
    /// Registers the shooter's GameObject so the bullet will not damage its own source.
    /// Uses the root GameObject for comparison to handle hierarchical collider setups.
    /// </summary>
    /// <param name="shooter">The GameObject that fired this bullet (typically the player root).</param>
    public void SetShooter(GameObject shooter)
    {
        if (shooter != null)
        {
            shooterRoot = shooter.transform.root.gameObject;
        }
        else
        {
            Debug.LogWarning($"{LOG_PREFIX} SetShooter called with null shooter.");
            shooterRoot = null;
        }
    }

    /// <summary>
    /// Alternative method to set the bullet's damage after instantiation.
    /// Useful when damage is determined separately from direction (e.g., damage modifiers).
    /// </summary>
    /// <param name="damage">The amount of damage to deal on hit. Clamped to >= 0.</param>
    public void SetDamage(float damage)
    {
        this.damage = Mathf.Max(0f, damage);
    }

    #endregion

    #region Collision Handling

    private void OnTriggerEnter(Collider other)
    {
        if (other == null) return;

        // --- Shooter exclusion ---
        // Compare against the root GameObject so child colliders on the shooter are also ignored.
        GameObject otherRoot = other.transform.root.gameObject;
        if (shooterRoot != null && otherRoot == shooterRoot)
        {
            return;
        }

        // --- Skip other triggers (e.g., pickup zones, interaction volumes) ---
        // Only react to solid colliders. If the other collider is also a trigger, skip it.
        if (other.isTrigger)
        {
            return;
        }

        // --- Double-hit prevention ---
        // If we already damaged this root entity in the same collision sequence, skip.
        if (!damagedTargets.Add(otherRoot))
        {
            return;
        }

        // --- Damage application ---
        // Search for IDamageable on the hit object first, then walk up the parent hierarchy.
        // This handles both direct hits on enemy roots and hits on child hitbox colliders.
        IDamageable damageable = other.GetComponent<IDamageable>();
        if (damageable == null)
        {
            damageable = other.GetComponentInParent<IDamageable>();
        }

        if (damageable != null)
        {
            damageable.TakeDamage(damage);
            Debug.Log($"{LOG_PREFIX} Hit {other.name}, dealt {damage} damage");

            // Hitmarker feedback via singleton (null-safe)
            HitmarkerDisplay.Instance?.ShowHitmarker();
        }

        // --- Impact VFX ---
        SpawnImpactEffect(other);

        // --- Destroy bullet on solid hit ---
        Destroy(gameObject);
    }

    #endregion

    #region Private Helpers

    /// <summary>
    /// Applies the current moveDirection * speed as Rigidbody velocity.
    /// Safe to call multiple times; only applies if Rigidbody is available and active.
    /// </summary>
    private void ApplyVelocity()
    {
        if (hasRigidbody && rb != null)
        {
            rb.linearVelocity = moveDirection * speed;
        }
    }

    /// <summary>
    /// Spawns the impact effect prefab at the collision point if configured.
    /// Orients the effect along the surface normal for correct particle alignment.
    /// </summary>
    /// <param name="hitCollider">The collider that was hit, used to approximate the contact point.</param>
    private void SpawnImpactEffect(Collider hitCollider)
    {
        if (impactEffectPrefab == null) return;

        // Use the bullet's current position as a reasonable impact point.
        // For more precision, a raycast along moveDirection could be used,
        // but for fast projectiles the bullet position at trigger time is close enough.
        Vector3 impactPosition = transform.position;
        Quaternion impactRotation = Quaternion.LookRotation(-moveDirection);

        GameObject effect = Instantiate(impactEffectPrefab, impactPosition, impactRotation);
        if (effect != null)
        {
            Destroy(effect, impactEffectLifetime);
        }
    }

    #endregion
}
