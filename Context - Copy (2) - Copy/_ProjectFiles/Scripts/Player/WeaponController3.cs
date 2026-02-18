using UnityEngine;
using Synty.AnimationBaseLocomotion.Samples.InputSystem;

/// <summary>
/// WEAPON CONTROLLER - FIXED VERSION
/// - Dynamically finds ShootPoint from active weapon using WeaponData.shootPointName
/// - Uses WeaponData.range for max distance (not hardcoded)
/// - Respects SYNTY's default FOV (only changes aimFOV)
/// - Better debugging for enemy detection issues
/// - Proper layer mask configuration
/// 
/// HOW TO USE:
/// 1. Assign WeaponData asset for current weapon
/// 2. Assign the weapon GameObject itself (parent of ShootPoint)
/// 3. Configure aimFOV and layer mask
/// 4. ShootPoint will be found automatically by name
/// </summary>
public class WeaponController3 : MonoBehaviour
{
    [Header("Weapon Setup")]
    [Tooltip("The weapon data asset (create from WeaponData ScriptableObject)")]
    [SerializeField] private WeaponData currentWeaponData;

    [Tooltip("The active weapon GameObject (parent of ShootPoint child)")]
    [SerializeField] private GameObject currentWeaponObject;

    [Header("Aiming Settings")]
    [Tooltip("Camera FOV when aiming (SYNTY manages normal FOV, we only change this)")]
    [SerializeField] private float aimFOV = 40f;

    [Tooltip("How fast camera zooms in/out")]
    [SerializeField] private float zoomSpeed = 10f;

    [Header("Shooting Settings")]
    [Tooltip("Layer mask for what can be hit. IMPORTANT: Include 'Default' and 'Enemy' layers")]
    [SerializeField] private LayerMask hitLayers = ~0; // Default: Everything

    [Header("Visual Feedback - Optional")]
    [Tooltip("Optional: muzzle flash particle effect")]
    [SerializeField] private ParticleSystem muzzleFlash;

    [Tooltip("Optional: bullet trail line renderer")]
    [SerializeField] private LineRenderer bulletTrail;

    [Tooltip("How long bullet trail stays visible")]
    [SerializeField] private float trailDuration = 0.05f;

    // References
    private InputReader inputReader;
    private Camera playerCamera;
    private float originalFOV; // Store SYNTY's original FOV
    private Transform shootPoint; // Found dynamically from weapon

    // Aiming state
    private bool isAiming = false;

    /// <summary>
    /// Public static accessor for other scripts (e.g., movement controller)
    /// Returns true when player is holding Right Mouse (aiming)
    /// </summary>
    public static bool IsAiming { get; private set; } = false;

    // Shooting state
    private float nextFireTime = 0f;
    private bool triggerHeld = false;

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

        // Get main camera
        playerCamera = Camera.main;
        if (playerCamera == null)
        {
            Debug.LogError("WeaponController: Main camera not found! Tag your camera as 'MainCamera'.");
            enabled = false;
            return;
        }

        // Store SYNTY's original FOV (don't modify it, only use aimFOV)
        originalFOV = playerCamera.fieldOfView;
        Debug.Log($"WeaponController: Stored original FOV = {originalFOV}");

        // Subscribe to INPUT EVENTS
        inputReader.onAimActivated += OnAimStart;
        inputReader.onAimDeactivated += OnAimEnd;
        inputReader.onShootStarted += OnShootStarted;
        inputReader.onShootPerformed += OnShootPerformed;
        inputReader.onShootCanceled += OnShootCanceled;

        // Validate weapon setup
        ValidateWeaponSetup();

        // Hide bullet trail initially
        if (bulletTrail != null)
        {
            bulletTrail.enabled = false;
        }

        Debug.Log("=== WEAPON CONTROLLER READY ===");
        Debug.Log($"Weapon: {currentWeaponData?.weaponName ?? "NONE"}");
        Debug.Log($"ShootPoint: {(shootPoint != null ? shootPoint.name : "NOT FOUND")}");
        Debug.Log($"Hit Layers: {LayerMaskToString(hitLayers)}");
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

    /// <summary>
    /// Validate weapon setup and find ShootPoint
    /// </summary>
    void ValidateWeaponSetup()
    {
        if (currentWeaponData == null)
        {
            Debug.LogError("WeaponController: No WeaponData assigned! Create a WeaponData asset and assign it.");
            return;
        }

        if (currentWeaponObject == null)
        {
            Debug.LogError("WeaponController: No weapon GameObject assigned! Assign the active weapon (e.g., Pistol GameObject).");
            return;
        }

        // Find ShootPoint by name from WeaponData
        shootPoint = FindShootPoint(currentWeaponObject, currentWeaponData.shootPointName);

        if (shootPoint == null)
        {
            Debug.LogError($"WeaponController: ShootPoint '{currentWeaponData.shootPointName}' not found in {currentWeaponObject.name}! Make sure weapon has child GameObject named '{currentWeaponData.shootPointName}'.");
        }
        else
        {
            Debug.Log($"✓ ShootPoint found: {shootPoint.name} at position {shootPoint.position}");
        }
    }

    /// <summary>
    /// Recursively search for ShootPoint by name in weapon hierarchy
    /// </summary>
    Transform FindShootPoint(GameObject weapon, string pointName)
    {
        // Check all children recursively
        Transform[] allChildren = weapon.GetComponentsInChildren<Transform>();
        foreach (Transform child in allChildren)
        {
            if (child.name.Equals(pointName, System.StringComparison.OrdinalIgnoreCase))
            {
                return child;
            }
        }
        return null;
    }

    // ============================================
    // AIMING CALLBACKS
    // ============================================

    void OnAimStart()
    {
        isAiming = true;
        IsAiming = true;
        Debug.Log(">>> AIM START - Now you can shoot!");
    }

    void OnAimEnd()
    {
        isAiming = false;
        IsAiming = false;
        Debug.Log(">>> AIM END - Cannot shoot until aiming again");
    }

    void UpdateCameraZoom()
    {
        if (playerCamera == null) return;

        // Target FOV: aim FOV when aiming, otherwise return to SYNTY's original FOV
        float targetFOV = isAiming ? aimFOV : originalFOV;
        playerCamera.fieldOfView = Mathf.Lerp(playerCamera.fieldOfView, targetFOV, Time.deltaTime * zoomSpeed);
    }

    // ============================================
    // SHOOTING CALLBACKS
    // ============================================

    void OnShootStarted()
    {
        triggerHeld = true;
        Debug.Log("TRIGGER PRESSED");
    }

    void OnShootPerformed()
    {
        TryShoot();
    }

    void OnShootCanceled()
    {
        triggerHeld = false;
        Debug.Log("TRIGGER RELEASED");
    }

    void TryShoot()
    {
        // CRITICAL: Must be aiming to shoot
        if (!isAiming)
        {
            Debug.LogWarning("Cannot shoot - You must aim (Right Mouse) first!");
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
            Debug.LogWarning("Cannot shoot: No shoot point found! Check weapon setup.");
            return;
        }

        // Fire rate check
        if (Time.time < nextFireTime)
        {
            return; // Cooldown active
        }

        // Update next fire time
        nextFireTime = Time.time + (1f / currentWeaponData.fireRate);

        // Perform the shot
        FireWeapon();
    }

    void FireWeapon()
    {
        Debug.Log($"FIRING {currentWeaponData.weaponName}!");

        // Play muzzle flash
        if (muzzleFlash != null)
        {
            muzzleFlash.Play();
        }

        // Spawn bullet prefab (visual only - damage handled by raycast)
        if (currentWeaponData.bulletPrefab != null && shootPoint != null)
        {
            Ray aimRay = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));

            // Visual rotation: rotate prefab to look horizontal along aim direction
            Quaternion bulletRotation = Quaternion.LookRotation(aimRay.direction) * Quaternion.Euler(1f, 0f, 0f);

            GameObject bullet1 = Instantiate(
                currentWeaponData.bulletPrefab,
                shootPoint.position,
                bulletRotation
            );

            //// Set movement direction (separate from visual rotation)
            //Bullet1 bullet1Script = bullet1.GetComponent<Bullet1>();
            //if (bullet1Script != null)
            //{
            //    bullet1Script.SetDirection(aimRay.direction);
            //}

            Debug.Log($"Bullet spawned at {shootPoint.position}");
        }
        else if (currentWeaponData.bulletPrefab == null)
        {
            Debug.LogWarning("No bulletPrefab assigned in WeaponData!");
        }

        // Get ray from camera center (where crosshair would be)
        Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

        Debug.Log($"Raycast from camera position: {ray.origin}, direction: {ray.direction}");
        Debug.Log($"Max distance: {currentWeaponData.range}m, Layers: {LayerMaskToString(hitLayers)}");

        // Perform raycast using weapon's range
        if (Physics.Raycast(ray, out hit, currentWeaponData.range, hitLayers))
        {
            // We hit something!
            Debug.Log($"=== HIT DETECTED ===");
            Debug.Log($"  - Object: {hit.collider.gameObject.name}");
            Debug.Log($"  - Layer: {LayerMask.LayerToName(hit.collider.gameObject.layer)}");
            Debug.Log($"  - Distance: {hit.distance:F2}m");
            Debug.Log($"  - Position: {hit.point}");
            Debug.Log($"  - Has Collider: {hit.collider != null}");

            // Check for IDamageable component
            IDamageable damageable = hit.collider.GetComponent<IDamageable>();

            Debug.Log($"  - Has IDamageable: {damageable != null}");

            if (damageable != null)
            {
                // Apply damage
                damageable.TakeDamage(currentWeaponData.damage);
                Debug.Log($"💥💥💥 DAMAGE APPLIED: {currentWeaponData.damage} to {hit.collider.gameObject.name}");
            }
            else
            {
                Debug.LogWarning($"⚠️ Hit {hit.collider.gameObject.name} but it has NO IDamageable component!");
                Debug.LogWarning($"   Make sure enemy has EnemyHealth script attached.");
            }

            // Show bullet trail
            ShowBulletTrail(shootPoint.position, hit.point);
        }
        else
        {
            // Missed
            Debug.Log("Shot missed (raycast hit nothing)");
            Debug.LogWarning("If you're aiming at enemy and missing:");
            Debug.LogWarning("1. Check enemy Layer is included in Hit Layers");
            Debug.LogWarning("2. Check enemy has Collider component");
            Debug.LogWarning("3. Check you're within range");
            Debug.LogWarning($"4. Current range: {currentWeaponData.range}m");

            // Show bullet trail to max distance
            Vector3 endPoint = ray.origin + (ray.direction * currentWeaponData.range);
            ShowBulletTrail(shootPoint.position, endPoint);
        }
    }

    void ShowBulletTrail(Vector3 startPoint, Vector3 endPoint)
    {
        if (bulletTrail != null)
        {
            bulletTrail.SetPosition(0, startPoint);
            bulletTrail.SetPosition(1, endPoint);
            bulletTrail.enabled = true;
            Invoke(nameof(HideBulletTrail), trailDuration);
        }

        // Draw in Scene view
        Debug.DrawLine(startPoint, endPoint, Color.yellow, 1f);
    }

    void HideBulletTrail()
    {
        if (bulletTrail != null)
        {
            bulletTrail.enabled = false;
        }
    }

    /// <summary>
    /// Convert LayerMask to readable string for debugging
    /// </summary>
    string LayerMaskToString(LayerMask mask)
    {
        string result = "";
        for (int i = 0; i < 32; i++)
        {
            if ((mask.value & (1 << i)) != 0)
            {
                result += LayerMask.LayerToName(i) + ", ";
            }
        }
        return result.Length > 0 ? result.Substring(0, result.Length - 2) : "Nothing";
    }

    void OnDrawGizmos()
    {
        if (shootPoint != null)
        {
            // Draw shoot point
            Gizmos.color = isAiming ? Color.green : Color.red;
            Gizmos.DrawWireSphere(shootPoint.position, 0.05f);

            // Draw shoot direction
            Gizmos.color = Color.yellow;
            Gizmos.DrawRay(shootPoint.position, shootPoint.forward * 2f);
        }
    }
}