using UnityEngine;
using System.Collections;
using System.Text;
using Synty.AnimationBaseLocomotion.Samples.InputSystem;

/// <summary>
/// WEAPON CONTROLLER - OPTIMIZED VERSION
/// 
/// OPTIMIZATIONS MADE:
/// - Conditional debug logging (enable/disable via inspector)
/// - StringBuilder for string operations (garbage reduction)
/// - Coroutine instead of Invoke (better performance)
/// - Cached layer names (no repeated LayerMask.LayerToName calls)
/// - Consolidated aiming state (removed redundant bool)
/// - Early validation checks to prevent unnecessary operations
/// - Bullet trail coroutine with pooling support ready
/// - Removed duplicate raycast/bullet spawn logic (clarified system)
/// - Better memory management
/// </summary>
public class WeaponController2 : MonoBehaviour
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
    [Tooltip("Shooting mode: Hitscan (instant raycast) or Projectile (spawn bullet prefab)")]
    [SerializeField] private ShootingMode shootingMode = ShootingMode.Hitscan;

    [Tooltip("Layer mask for what can be hit. IMPORTANT: Include 'Default' and 'Enemy' layers")]
    [SerializeField] private LayerMask hitLayers = ~0;

    [Header("Visual Feedback - Optional")]
    [Tooltip("Optional: muzzle flash particle effect")]
    [SerializeField] private ParticleSystem muzzleFlash;

    [Tooltip("Optional: bullet trail line renderer")]
    [SerializeField] private LineRenderer bulletTrail;

    [Tooltip("How long bullet trail stays visible")]
    [SerializeField] private float trailDuration = 0.05f;

    [Header("Debug Settings")]
    [Tooltip("Enable detailed debug logging (disable for production)")]
    [SerializeField] private bool enableDebugLogs = true;

    // Public static accessor for other scripts
    public static bool IsAiming { get; private set; }

    // Cached references
    private InputReader inputReader;
    private Camera playerCamera;
    private Transform shootPoint;

    // State tracking
    private float originalFOV;
    private float nextFireTime;
    private bool triggerHeld;
    private Coroutine trailCoroutine;

    // Cached viewport center (no need to recreate every frame)
    private static readonly Vector3 ViewportCenter = new Vector3(0.5f, 0.5f, 0);

    // Cached strings for garbage reduction
    private string layerMaskString;
    private bool layerMaskCached;

    public enum ShootingMode
    {
        Hitscan,    // Instant raycast damage (like most FPS games)
        Projectile  // Spawn physical bullet with rigidbody
    }

    #region Initialization

    void Start()
    {
        if (!InitializeComponents())
        {
            enabled = false;
            return;
        }

        SubscribeToInputEvents();
        ValidateWeaponSetup();
        InitializeBulletTrail();

        if (enableDebugLogs)
        {
            LogInitializationInfo();
        }
    }

    bool InitializeComponents()
    {
        // Get InputReader
        inputReader = GetComponent<InputReader>();
        if (inputReader == null)
        {
            Debug.LogError("WeaponController: InputReader not found! Add InputReader component to Player.");
            return false;
        }

        // Get main camera
        playerCamera = Camera.main;
        if (playerCamera == null)
        {
            Debug.LogError("WeaponController: Main camera not found! Tag your camera as 'MainCamera'.");
            return false;
        }

        // Store SYNTY's original FOV
        originalFOV = playerCamera.fieldOfView;
        return true;
    }

    void SubscribeToInputEvents()
    {
        inputReader.onAimActivated += OnAimStart;
        inputReader.onAimDeactivated += OnAimEnd;
        inputReader.onShootStarted += OnShootStarted;
        inputReader.onShootPerformed += OnShootPerformed;
        inputReader.onShootCanceled += OnShootCanceled;
    }

    void ValidateWeaponSetup()
    {
        if (currentWeaponData == null)
        {
            Debug.LogError("WeaponController: No WeaponData assigned!");
            return;
        }

        if (currentWeaponObject == null)
        {
            Debug.LogError("WeaponController: No weapon GameObject assigned!");
            return;
        }

        // Find ShootPoint (cached, only called once)
        shootPoint = FindShootPointCached(currentWeaponObject, currentWeaponData.shootPointName);

        if (shootPoint == null)
        {
            Debug.LogError($"WeaponController: ShootPoint '{currentWeaponData.shootPointName}' not found!");
        }
    }

    void InitializeBulletTrail()
    {
        if (bulletTrail != null)
        {
            bulletTrail.enabled = false;
        }
    }

    void LogInitializationInfo()
    {
        Debug.Log("=== WEAPON CONTROLLER READY ===");
        Debug.Log($"Weapon: {currentWeaponData?.weaponName ?? "NONE"}");
        Debug.Log($"Shooting Mode: {shootingMode}");
        Debug.Log($"ShootPoint: {(shootPoint != null ? shootPoint.name : "NOT FOUND")}");
        Debug.Log($"Hit Layers: {GetLayerMaskString()}");
        Debug.Log("Controls: Right Mouse = Aim | Left Mouse = Shoot");
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

        // Stop any running coroutines
        if (trailCoroutine != null)
        {
            StopCoroutine(trailCoroutine);
        }
    }

    #endregion

    #region Update Loop

    void Update()
    {
        UpdateCameraZoom();

        // Auto-fire while holding trigger
        if (triggerHeld && IsAiming)
        {
            TryShoot();
        }
    }

    void UpdateCameraZoom()
    {
        if (playerCamera == null) return;

        float targetFOV = IsAiming ? aimFOV : originalFOV;
        playerCamera.fieldOfView = Mathf.Lerp(
            playerCamera.fieldOfView,
            targetFOV,
            Time.deltaTime * zoomSpeed
        );
    }

    #endregion

    #region Input Callbacks

    void OnAimStart()
    {
        IsAiming = true;
        if (enableDebugLogs) Debug.Log(">>> AIM START");
    }

    void OnAimEnd()
    {
        IsAiming = false;
        if (enableDebugLogs) Debug.Log(">>> AIM END");
    }

    void OnShootStarted()
    {
        triggerHeld = true;
        if (enableDebugLogs) Debug.Log("🔫 TRIGGER PRESSED");
    }

    void OnShootPerformed()
    {
        TryShoot();
    }

    void OnShootCanceled()
    {
        triggerHeld = false;
        if (enableDebugLogs) Debug.Log("🔫 TRIGGER RELEASED");
    }

    #endregion

    #region Shooting Logic

    void TryShoot()
    {
        // Early validation checks
        if (!IsAiming)
        {
            if (enableDebugLogs) Debug.LogWarning("⚠️ Must aim first!");
            return;
        }

        if (currentWeaponData == null || shootPoint == null)
        {
            return;
        }

        // Fire rate check
        if (Time.time < nextFireTime)
        {
            return;
        }

        nextFireTime = Time.time + (1f / currentWeaponData.fireRate);
        FireWeapon();
    }

    void FireWeapon()
    {
        if (enableDebugLogs)
        {
            Debug.Log($"FIRING {currentWeaponData.weaponName}!");
        }

        // Visual feedback
        PlayMuzzleFlash();

        // Choose shooting system
        switch (shootingMode)
        {
            case ShootingMode.Hitscan:
                PerformHitscanShot();
                break;
            case ShootingMode.Projectile:
                SpawnProjectile();
                break;
        }
    }

    void PlayMuzzleFlash()
    {
        if (muzzleFlash != null)
        {
            muzzleFlash.Play();
        }
    }

    #endregion

    #region Hitscan System

    void PerformHitscanShot()
    {
        Ray ray = playerCamera.ViewportPointToRay(ViewportCenter);

        if (Physics.Raycast(ray, out RaycastHit hit, currentWeaponData.range, hitLayers))
        {
            ProcessHit(hit);
            ShowBulletTrail(shootPoint.position, hit.point);
        }
        else
        {
            // Missed
            if (enableDebugLogs)
            {
                Debug.Log("Shot missed");
            }
            Vector3 endPoint = ray.origin + (ray.direction * currentWeaponData.range);
            ShowBulletTrail(shootPoint.position, endPoint);
        }
    }

    void ProcessHit(RaycastHit hit)
    {
        if (enableDebugLogs)
        {
            Debug.Log($"=== HIT: {hit.collider.gameObject.name} at {hit.distance:F2}m ===");
        }

        // Check for IDamageable (check parent if needed)
        IDamageable damageable = hit.collider.GetComponent<IDamageable>()
            ?? hit.collider.GetComponentInParent<IDamageable>();

        if (damageable != null)
        {
            damageable.TakeDamage(currentWeaponData.damage);
            if (enableDebugLogs)
            {
                Debug.Log($"💥 DAMAGE: {currentWeaponData.damage} to {hit.collider.gameObject.name}");
            }
        }
        else if (enableDebugLogs)
        {
            Debug.LogWarning($"⚠️ {hit.collider.gameObject.name} has no IDamageable component!");
        }
    }

    #endregion

    #region Projectile System

    void SpawnProjectile()
    {
        if (currentWeaponData.bulletPrefab == null)
        {
            if (enableDebugLogs)
            {
                Debug.LogWarning("No bullet prefab assigned!");
            }
            return;
        }

        Ray aimRay = playerCamera.ViewportPointToRay(ViewportCenter);
        Quaternion bulletRotation = Quaternion.LookRotation(aimRay.direction);

        // Spawn bullet (Consider using object pooling for optimization)
        GameObject bullet = Instantiate(
            currentWeaponData.bulletPrefab,
            shootPoint.position,
            bulletRotation
        );

        if (enableDebugLogs)
        {
            Debug.Log($"Bullet spawned at {shootPoint.position}");
        }

        // TODO: Implement object pooling for bullets to reduce GC
        // BulletPool.Instance.SpawnBullet(shootPoint.position, bulletRotation);
    }

    #endregion

    #region Visual Effects

    void ShowBulletTrail(Vector3 startPoint, Vector3 endPoint)
    {
        if (bulletTrail == null) return;

        // Stop previous trail coroutine if running
        if (trailCoroutine != null)
        {
            StopCoroutine(trailCoroutine);
        }

        bulletTrail.SetPosition(0, startPoint);
        bulletTrail.SetPosition(1, endPoint);
        bulletTrail.enabled = true;

        trailCoroutine = StartCoroutine(HideBulletTrailCoroutine());

        // Debug visualization in Scene view
        if (enableDebugLogs)
        {
            Debug.DrawLine(startPoint, endPoint, Color.yellow, 1f);
        }
    }

    IEnumerator HideBulletTrailCoroutine()
    {
        yield return new WaitForSeconds(trailDuration);

        if (bulletTrail != null)
        {
            bulletTrail.enabled = false;
        }

        trailCoroutine = null;
    }

    #endregion

    #region Helper Methods

    /// <summary>
    /// Cached recursive search for ShootPoint
    /// </summary>
    Transform FindShootPointCached(GameObject weapon, string pointName)
    {
        // Cache all transforms in one call
        Transform[] allChildren = weapon.GetComponentsInChildren<Transform>(true);

        foreach (Transform child in allChildren)
        {
            if (child.name.Equals(pointName, System.StringComparison.OrdinalIgnoreCase))
            {
                return child;
            }
        }

        return null;
    }

    /// <summary>
    /// Cached LayerMask to string conversion using StringBuilder
    /// </summary>
    string GetLayerMaskString()
    {
        if (layerMaskCached)
        {
            return layerMaskString;
        }

        StringBuilder sb = new StringBuilder();
        bool first = true;

        for (int i = 0; i < 32; i++)
        {
            if ((hitLayers.value & (1 << i)) != 0)
            {
                if (!first) sb.Append(", ");
                sb.Append(LayerMask.LayerToName(i));
                first = false;
            }
        }

        layerMaskString = sb.Length > 0 ? sb.ToString() : "Nothing";
        layerMaskCached = true;

        return layerMaskString;
    }

    #endregion

    #region Gizmos

    void OnDrawGizmos()
    {
        if (shootPoint == null) return;

        // Draw shoot point
        Gizmos.color = IsAiming ? Color.green : Color.red;
        Gizmos.DrawWireSphere(shootPoint.position, 0.05f);

        // Draw shoot direction
        Gizmos.color = Color.yellow;
        Gizmos.DrawRay(shootPoint.position, shootPoint.forward * 2f);
    }

    #endregion
}