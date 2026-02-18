using UnityEngine;
using Synty.AnimationBaseLocomotion.Samples.InputSystem;

/// <summary>
/// PHASE 2 WEAPON CONTROLLER - WITH AIMING
/// - Player MUST aim (Right Mouse) before shooting (Left Mouse)
/// - Uses SYNTY's InputReader events for aim + shoot
/// - Exposes static IsAiming bool for movement controller
/// - Optional camera zoom when aiming
/// - Shoots raycast from camera center through crosshair
/// 
/// TESTING VERSION - Later upgrade to full version with inventory/ammo
/// </summary>
public class WeaponController_Phase2_Aiming : MonoBehaviour
{
    [Header("Weapon Setup")]
    [Tooltip("The weapon data asset (create from WeaponData ScriptableObject)")]
    [SerializeField] private WeaponData currentWeaponData;

    [Tooltip("The ShootPoint GameObject at the barrel tip")]
    [SerializeField] private Transform shootPoint;

    [Header("Shooting Settings")]
    [Tooltip("Max distance the raycast can hit")]
    [SerializeField] private float maxShootDistance = 100f;

    [Tooltip("Layer mask for what can be hit (Default + Enemy)")]
    [SerializeField] private LayerMask hitLayers;

    [Header("Aiming Settings")]
    [Tooltip("Camera zooms to this FOV when aiming (default 60, aim 40)")]
    [SerializeField] private float aimFOV = 40f;

    [Tooltip("Normal FOV when not aiming")]
    [SerializeField] private float normalFOV = 60f;

    [Tooltip("How fast camera zooms in/out")]
    [SerializeField] private float zoomSpeed = 10f;

    [Header("Visual Feedback")]
    [Tooltip("Optional: muzzle flash particle effect")]
    [SerializeField] private ParticleSystem muzzleFlash;

    [Tooltip("Optional: bullet trail line renderer")]
    [SerializeField] private LineRenderer bulletTrail;

    [Tooltip("How long bullet trail stays visible")]
    [SerializeField] private float trailDuration = 0.05f;

    // References
    private InputReader inputReader;
    private Camera playerCamera;

    // Aiming state
    private bool isAiming = false;

    /// <summary>
    /// Public static accessor for other scripts (e.g., movement controller)
    /// Returns true when player is holding Right Mouse (aiming)
    /// </summary>
    public static bool IsAiming { get; private set; } = false;

    // Shooting state
    private bool canShoot = true;
    private float nextFireTime = 0f;
    private bool triggerHeld = false; // Track if left mouse is held

    void Start()
    {
        // Get InputReader (SYNTY's input system)
        inputReader = GetComponent<InputReader>();
        if (inputReader == null)
        {
            Debug.LogError("WeaponController: InputReader not found! Add InputReader component to Player.");
            enabled = false;
            return;
        }

        // Get main camera for aiming and raycasting
        playerCamera = Camera.main;
        if (playerCamera == null)
        {
            Debug.LogError("WeaponController: Main camera not found! Tag your camera as 'MainCamera'.");
            enabled = false;
            return;
        }

        // Subscribe to INPUT EVENTS
        // Aiming (Right Mouse)
        inputReader.onAimActivated += OnAimStart;
        inputReader.onAimDeactivated += OnAimEnd;

        // Shooting (Left Mouse)
        inputReader.onShootStarted += OnShootStarted;
        inputReader.onShootPerformed += OnShootPerformed;
        inputReader.onShootCanceled += OnShootCanceled;

        // Validate setup
        if (currentWeaponData == null)
        {
            Debug.LogError("WeaponController: No WeaponData assigned! Create a WeaponData asset and assign it.");
        }

        if (shootPoint == null)
        {
            Debug.LogError("WeaponController: No ShootPoint assigned! Assign the empty GameObject at barrel tip.");
        }

        // Hide bullet trail initially
        if (bulletTrail != null)
        {
            bulletTrail.enabled = false;
        }

        Debug.Log($"WeaponController initialized with weapon: {currentWeaponData?.weaponName ?? "NONE"}");
        Debug.Log("Controls: Right Mouse = Aim | Left Mouse (while aiming) = Shoot");
    }

    void OnDestroy()
    {
        // Unsubscribe to prevent memory leaks
        if (inputReader != null)
        {
            inputReader.onAimActivated -= OnAimStart;
            inputReader.onAimDeactivated -= OnAimEnd;
            inputReader.onShootStarted -= OnShootStarted;
            inputReader.onShootPerformed -= OnShootPerformed;
            inputReader.onShootCanceled -= OnShootCanceled;
        }
    }

    void Update()
    {
        // Smoothly zoom camera FOV when aiming
        UpdateCameraZoom();

        // If holding trigger while aiming, keep shooting (auto-fire)
        if (triggerHeld && isAiming)
        {
            TryShoot();
        }
    }

    // ============================================
    // AIMING CALLBACKS
    // ============================================

    /// <summary>
    /// Called when Right Mouse pressed (start aiming)
    /// </summary>
    void OnAimStart()
    {
        Debug.Log("OnAimStart FIRED"); // Debug line
        isAiming = true;
        IsAiming = true; // Update static property
        Debug.Log(">>> AIM START - Now you can shoot!");
    }

    /// <summary>
    /// Called when Right Mouse released (stop aiming)
    /// </summary>
    void OnAimEnd()
    {
        isAiming = false;
        IsAiming = false; // Update static property
        Debug.Log(">>> AIM END - Cannot shoot until aiming again");
    }

    /// <summary>
    /// Smoothly zoom camera FOV based on aiming state
    /// </summary>
    void UpdateCameraZoom()
    {
        if (playerCamera == null) return;

        float targetFOV = isAiming ? aimFOV : normalFOV;
        playerCamera.fieldOfView = Mathf.Lerp(playerCamera.fieldOfView, targetFOV, Time.deltaTime * zoomSpeed);
    }

    // ============================================
    // SHOOTING CALLBACKS
    // ============================================

    /// <summary>
    /// Called when Left Mouse pressed down
    /// </summary>
    void OnShootStarted()
    {
        triggerHeld = true;
        Debug.Log("🔫 TRIGGER PRESSED");
    }

    /// <summary>
    /// Called when shot is triggered (performed phase)
    /// </summary>
    void OnShootPerformed()
    {
        // Attempt to shoot (will check if aiming inside TryShoot)
        TryShoot();
    }

    /// <summary>
    /// Called when Left Mouse released
    /// </summary>
    void OnShootCanceled()
    {
        triggerHeld = false;
        Debug.Log("🔫 TRIGGER RELEASED");
    }

    /// <summary>
    /// Attempt to fire the weapon
    /// SAFETY CHECK: Can only shoot if aiming (Right Mouse held)
    /// </summary>
    void TryShoot()
    {
        // CRITICAL: Must be aiming to shoot
        if (!isAiming)
        {
            Debug.LogWarning("⚠️ Cannot shoot - You must aim (Right Mouse) first!");
            return;
        }

        // Validation checks
        if (currentWeaponData == null)
        {
            Debug.LogWarning("Cannot shoot: No weapon data assigned!");
            return;
        }

        if (shootPoint == null)
        {
            Debug.LogWarning("Cannot shoot: No shoot point assigned!");
            return;
        }

        // Fire rate check (prevent shooting too fast)
        if (Time.time < nextFireTime)
        {
            // Too soon - cooldown active
            return;
        }

        // Update next fire time based on weapon's fire rate
        nextFireTime = Time.time + (1f / currentWeaponData.fireRate);

        // Perform the shot
        FireWeapon();
    }

    /// <summary>
    /// Execute the actual shooting logic
    /// - Raycast from camera through crosshair
    /// - Check if we hit something
    /// - Apply damage if target is IDamageable
    /// - Show visual effects
    /// </summary>
    void FireWeapon()
    {
        Debug.Log($"🔫 FIRING {currentWeaponData.weaponName}!");

        // Play muzzle flash effect
        if (muzzleFlash != null)
        {
            muzzleFlash.Play();
        }

        // Get ray from camera center (where player is looking)
        // This shoots through the crosshair in the middle of the screen
        Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

        // Perform raycast
        if (Physics.Raycast(ray, out hit, maxShootDistance, hitLayers))
        {
            // We hit something!
            Debug.Log($"✓ Hit: {hit.collider.gameObject.name} at distance {hit.distance:F2}m");

            // Check if target can take damage
            IDamageable damageable = hit.collider.GetComponent<IDamageable>();
            if (damageable != null)
            {
                // Apply damage from weapon data
                damageable.TakeDamage(currentWeaponData.damage);
                Debug.Log($"💥 Dealt {currentWeaponData.damage} damage to {hit.collider.gameObject.name}");
            }
            else
            {
                Debug.Log($"⚠️ Hit {hit.collider.gameObject.name} but it has no IDamageable component");
            }

            // Show bullet trail to hit point
            ShowBulletTrail(shootPoint.position, hit.point);
        }
        else
        {
            // Missed - raycast hit nothing
            Debug.Log("✗ Shot missed (hit nothing)");

            // Show bullet trail to max distance
            Vector3 endPoint = ray.origin + (ray.direction * maxShootDistance);
            ShowBulletTrail(shootPoint.position, endPoint);
        }
    }

    /// <summary>
    /// Display visual bullet trail from shoot point to hit location
    /// </summary>
    void ShowBulletTrail(Vector3 startPoint, Vector3 endPoint)
    {
        if (bulletTrail != null)
        {
            bulletTrail.SetPosition(0, startPoint);
            bulletTrail.SetPosition(1, endPoint);
            bulletTrail.enabled = true;

            // Hide trail after short duration
            Invoke(nameof(HideBulletTrail), trailDuration);
        }

        // Draw debug line in Scene view
        Debug.DrawLine(startPoint, endPoint, Color.yellow, 1f);
    }

    /// <summary>
    /// Hide the bullet trail after duration
    /// </summary>
    void HideBulletTrail()
    {
        if (bulletTrail != null)
        {
            bulletTrail.enabled = false;
        }
    }

    /// <summary>
    /// DEBUG: Draw shootpoint position in Scene view
    /// </summary>
    void OnDrawGizmos()
    {
        if (shootPoint != null)
        {
            // Red sphere at shoot point
            Gizmos.color = isAiming ? Color.green : Color.red;
            Gizmos.DrawWireSphere(shootPoint.position, 0.05f);

            // Yellow line showing shoot direction
            Gizmos.color = Color.yellow;
            Gizmos.DrawRay(shootPoint.position, shootPoint.forward * 2f);
        }
    }
}