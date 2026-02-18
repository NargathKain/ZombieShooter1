using System;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using Synty.AnimationBaseLocomotion.Samples.InputSystem;

/// <summary>
/// Production-quality weapon controller for a 3rd-person zombie shooter.
/// Manages aiming (camera FOV zoom), shooting (projectile spawning with fire-rate control),
/// reloading (delegated to <see cref="Inventory"/>), and weapon switching via scroll wheel.
///
/// <para><b>Architecture:</b></para>
/// <list type="bullet">
///   <item>Aiming: Right-mouse hold smoothly lerps camera FOV between normal and aim values.</item>
///   <item>Shooting: Left-mouse fires ONLY while aiming. Supports both single-shot and continuous fire.
///         Spawns bullet prefab at the weapon's shoot point, aimed toward camera center.</item>
///   <item>Reload: R key triggers reload via Inventory. Auto-reload when magazine empties with backpack ammo available.
///         Cannot reload while aiming (aim is released first).</item>
///   <item>Weapon Switching: Scroll wheel cycles weapons through Inventory. Shoot point is re-acquired per weapon.</item>
/// </list>
///
/// <para><b>Dependencies:</b></para>
/// <list type="bullet">
///   <item><see cref="Inventory"/> (same GameObject) -- ammo, reload, equip logic.</item>
///   <item><see cref="InputReader"/> (same GameObject) -- all player input events.</item>
///   <item><see cref="WeaponData"/> -- per-weapon configuration (SO).</item>
///   <item><see cref="Bullet"/> -- projectile with Initialize(damage, direction) and SetShooter(shooter).</item>
///   <item><see cref="HitmarkerDisplay"/> -- singleton hitmarker feedback.</item>
/// </list>
///
/// <para><b>Setup:</b></para>
/// <list type="number">
///   <item>Attach to the player GameObject alongside Inventory and InputReader.</item>
///   <item>Configure aimFOV, zoomSpeed, and hitLayers in the Inspector.</item>
///   <item>Ensure each weapon prefab has a child named per WeaponData.shootPointName.</item>
/// </list>
/// </summary>
[RequireComponent(typeof(Inventory))]
public class WeaponController : MonoBehaviour
{
    #region Constants

    private const string LOG_PREFIX = "[WeaponController]";

    /// <summary>Viewport center used for camera-based aim raycasts.</summary>
    private static readonly Vector3 ViewportCenter = new Vector3(0.5f, 0.5f, 0f);

    #endregion

    #region Serialized Fields

    [Header("Aiming")]
    [Tooltip("Main camera for aiming. If not assigned, uses Camera.main.")]
    [SerializeField] private Camera playerCameraOverride;

    [Tooltip("Camera field-of-view when aiming down sights.")]
    [SerializeField] private float aimFOV = 40f;

    [Tooltip("Speed of the FOV lerp transition (higher = faster zoom).")]
    [SerializeField] private float zoomSpeed = 10f;

    [Header("Shooting")]
    [Tooltip("Physics layers that bullets/raycasts can hit. Configure to include Enemy, Default, etc.")]
    [SerializeField] private LayerMask hitLayers = ~0;

    [Header("Animation Rigging")]
    [Tooltip("Rigs for each weapon. Index must match Inventory weaponSlots order.")]
    [SerializeField] private Rig[] weaponRigs;

    [Tooltip("Weapon GameObjects to enable/disable. Index must match Inventory weaponSlots order.")]
    [SerializeField] private GameObject[] weaponObjects;

    [Header("Audio")]
    [Tooltip("AudioSource for shooting sounds. If not assigned, one will be created.")]
    [SerializeField] private AudioSource shootAudioSource;

    [Tooltip("Volume for weapon sound effects.")]
    [Range(0f, 1f)]
    [SerializeField] private float shootVolume = 1f;

    [Tooltip("Volume for reload sound effects.")]
    [Range(0f, 1f)]
    [SerializeField] private float reloadVolume = 0.8f;

    [Tooltip("Minimum time between shot sounds to prevent overlap (0 = allow overlap).")]
    [SerializeField] private float minSoundInterval = 0.05f;

    #endregion

    #region Public Static State

    /// <summary>
    /// Global aiming state accessible by other systems (e.g., movement controller, animation).
    /// True when the player is holding the aim button and this controller is active.
    /// </summary>
    public static bool IsAiming { get; private set; }

    #endregion

    #region Private Fields

    // Component references (cached in Start/OnEnable)
    private Inventory inventory;
    private InputReader inputReader;
    private Camera playerCamera;

    // FOV state
    private float normalFOV;

    // Aiming state
    private bool isAiming;

    // Shooting state
    private bool isTriggerHeld;
    private float nextFireTime;

    // Weapon state
    private Transform shootPoint;
    private WeaponData cachedWeaponData;

    // Guard against re-entrant fire calls within the same frame
    private int lastFireFrame = -1;

    // Audio timing
    private float lastShotSoundTime;
    private bool isPlayingLoopingFire;

    #endregion

    #region Unity Lifecycle

    private void Awake()
    {
        inventory = GetComponent<Inventory>();
        if (inventory == null)
        {
            Debug.LogError($"{LOG_PREFIX} Inventory component not found on {gameObject.name}. " +
                           "WeaponController requires Inventory via [RequireComponent].");
        }
    }

    private void Start()
    {
        // Cache InputReader
        inputReader = GetComponent<InputReader>();
        if (inputReader == null)
        {
            Debug.LogError($"{LOG_PREFIX} InputReader component not found on {gameObject.name}. " +
                           "Add the InputReader component to the player GameObject. Disabling WeaponController.");
            enabled = false;
            return;
        }

        // Cache main camera (use override if assigned)
        playerCamera = playerCameraOverride != null ? playerCameraOverride : Camera.main;
        if (playerCamera == null)
        {
            Debug.LogError($"{LOG_PREFIX} No camera found. Assign Player Camera Override or tag your camera as 'MainCamera'. " +
                           "Disabling WeaponController.");
            enabled = false;
            return;
        }

        // Store the camera's starting FOV so we can return to it after aiming
        normalFOV = playerCamera.fieldOfView;

        // Initialize weapon state from whatever Inventory has equipped at start
        RefreshWeaponState(inventory != null ? inventory.EquippedWeapon : null);
        UpdateWeaponRigs(inventory != null ? inventory.EquippedSlotIndex : -1);

        // Setup shoot audio source
        if (shootAudioSource == null)
        {
            shootAudioSource = gameObject.AddComponent<AudioSource>();
            shootAudioSource.playOnAwake = false;
            shootAudioSource.spatialBlend = 0f; // 2D sound for player's own weapon
        }

        Debug.Log($"{LOG_PREFIX} Initialized. Normal FOV={normalFOV}, Aim FOV={aimFOV}, " +
                  $"Weapon={cachedWeaponData?.weaponName ?? "NONE"}");
    }

    private void OnEnable()
    {
        SubscribeInputEvents();
        Inventory.OnWeaponEquipped += HandleWeaponEquipped;
    }

    private void OnDisable()
    {
        UnsubscribeInputEvents();
        Inventory.OnWeaponEquipped -= HandleWeaponEquipped;

        // Clean up global state so other systems don't think we're still aiming
        if (isAiming)
        {
            isAiming = false;
            IsAiming = false;
        }

        isTriggerHeld = false;

        // Stop any looping fire sound
        StopLoopingFireSound();
    }

    private void Update()
    {
        UpdateCameraZoom();
        UpdateContinuousFire();
        CheckAutoReload();
    }

    #endregion

    #region Input Subscription

    /// <summary>
    /// Subscribes to all relevant InputReader events. Called in OnEnable.
    /// Null-checks inputReader because OnEnable fires before Start on first activation.
    /// </summary>
    private void SubscribeInputEvents()
    {
        // InputReader may not yet be cached if OnEnable fires before Start (first enable).
        // We lazily acquire it here; Start() will validate it fully.
        if (inputReader == null)
        {
            inputReader = GetComponent<InputReader>();
        }

        if (inputReader == null) return;

        inputReader.onAimActivated += HandleAimActivated;
        inputReader.onAimDeactivated += HandleAimDeactivated;

        inputReader.onShootStarted += HandleShootStarted;
        inputReader.onShootPerformed += HandleShootPerformed;
        inputReader.onShootCanceled += HandleShootCanceled;

        inputReader.onReloadPerformed += HandleReloadPerformed;
        inputReader.onWeaponScrollPerformed += HandleWeaponScroll;
    }

    /// <summary>
    /// Unsubscribes from all InputReader events. Called in OnDisable.
    /// </summary>
    private void UnsubscribeInputEvents()
    {
        if (inputReader == null) return;

        inputReader.onAimActivated -= HandleAimActivated;
        inputReader.onAimDeactivated -= HandleAimDeactivated;

        inputReader.onShootStarted -= HandleShootStarted;
        inputReader.onShootPerformed -= HandleShootPerformed;
        inputReader.onShootCanceled -= HandleShootCanceled;

        inputReader.onReloadPerformed -= HandleReloadPerformed;
        inputReader.onWeaponScrollPerformed -= HandleWeaponScroll;
    }

    #endregion

    #region Aiming

    private void HandleAimActivated()
    {
        isAiming = true;
        IsAiming = true;
        Debug.Log($"{LOG_PREFIX} Aim started.");
    }

    private void HandleAimDeactivated()
    {
        isAiming = false;
        IsAiming = false;
        StopLoopingFireSound();
        Debug.Log($"{LOG_PREFIX} Aim ended.");
    }

    /// <summary>
    /// Smoothly interpolates camera FOV between normalFOV and aimFOV based on current aim state.
    /// </summary>
    private void UpdateCameraZoom()
    {
        if (playerCamera == null) return;

        float targetFOV = isAiming ? aimFOV : normalFOV;
        playerCamera.fieldOfView = Mathf.Lerp(
            playerCamera.fieldOfView,
            targetFOV,
            Time.deltaTime * zoomSpeed
        );
    }

    #endregion

    #region Shooting

    private void HandleShootStarted()
    {
        isTriggerHeld = true;
    }

    private void HandleShootPerformed()
    {
        // Single-shot: attempt to fire on the performed event (tap or first press).
        TryFire();
    }

    private void HandleShootCanceled()
    {
        isTriggerHeld = false;
        StopLoopingFireSound();
    }

    /// <summary>
    /// Called every frame to support continuous/automatic fire while the trigger is held and the player is aiming.
    /// </summary>
    private void UpdateContinuousFire()
    {
        if (!isAiming || !isTriggerHeld) return;
        if (Time.time < nextFireTime) return;

        TryFire();
    }

    /// <summary>
    /// Central firing gate. Validates all preconditions before delegating to <see cref="ExecuteFire"/>.
    /// Prevents double-fire within the same frame via frame guard.
    /// </summary>
    private void TryFire()
    {
        // Must be aiming to shoot
        if (!isAiming) return;

        // Prevent double-fire in the same frame (onShootPerformed + Update continuous fire)
        if (lastFireFrame == Time.frameCount) return;

        // Need a weapon
        if (cachedWeaponData == null)
        {
            Debug.LogWarning($"{LOG_PREFIX} Cannot fire: no weapon equipped.");
            return;
        }

        // Cannot fire while reloading
        if (inventory != null && inventory.IsReloading)
        {
            return;
        }

        // Fire rate enforcement
        if (Time.time < nextFireTime) return;

        // Consume ammo from Inventory; abort if magazine is empty
        if (inventory != null && !inventory.ConsumeAmmo())
        {
            // Magazine empty -- stop looping sound and trigger auto-reload
            StopLoopingFireSound();
            TryAutoReload();
            return;
        }

        // All checks passed
        nextFireTime = Time.time + (1f / Mathf.Max(cachedWeaponData.fireRate, 0.01f));
        lastFireFrame = Time.frameCount;

        ExecuteFire();
    }

    /// <summary>
    /// Executes the actual firing logic: spawns bullet, plays sound, spawns muzzle flash.
    /// Called only after all precondition checks pass in <see cref="TryFire"/>.
    /// </summary>
    private void ExecuteFire()
    {
        Debug.Log($"{LOG_PREFIX} Firing {cachedWeaponData.weaponName}");

        // Determine aim direction from camera center
        Vector3 aimDirection = GetAimDirection();

        // Spawn bullet projectile
        SpawnBullet(aimDirection);

        // Audio
        PlayShootSound();

        // Muzzle flash VFX
        SpawnMuzzleFlash();
    }

    /// <summary>
    /// Computes the world-space aim direction from the camera's viewport center.
    /// Uses a raycast to find an actual hit point so bullets converge on the crosshair target
    /// rather than flying parallel from the barrel.
    /// </summary>
    /// <returns>Normalized direction from the shoot point toward the aim target.</returns>
    private Vector3 GetAimDirection()
    {
        if (playerCamera == null)
        {
            // Absolute fallback: forward
            return transform.forward;
        }

        Ray aimRay = playerCamera.ViewportPointToRay(ViewportCenter);

        // Raycast to find what the crosshair is pointing at.
        // If we hit something, the bullet should fly toward that point from the barrel.
        // If we miss, aim toward a far point along the ray.
        Vector3 aimTarget;
        float range = cachedWeaponData != null ? cachedWeaponData.range : 100f;

        if (Physics.Raycast(aimRay, out RaycastHit hit, range, hitLayers))
        {
            aimTarget = hit.point;

            // Hitmarker is handled by Bullet.OnTriggerEnter, not here
        }
        else
        {
            aimTarget = aimRay.origin + aimRay.direction * range;
        }

        // Direction from the barrel to the aim target
        Vector3 origin = (shootPoint != null) ? shootPoint.position : aimRay.origin;
        Vector3 direction = (aimTarget - origin).normalized;

        // Safety: if direction is nearly zero (shoot point is on top of target), use camera forward
        if (direction.sqrMagnitude < 0.001f)
        {
            direction = aimRay.direction;
        }

        return direction;
    }

    /// <summary>
    /// Instantiates the bullet prefab at the shoot point and initializes it with damage and direction.
    /// Falls back gracefully if bulletPrefab or shootPoint is missing.
    /// </summary>
    /// <param name="direction">Normalized world-space direction for the bullet.</param>
    private void SpawnBullet(Vector3 direction)
    {
        if (cachedWeaponData.bulletPrefab == null)
        {
            Debug.LogWarning($"{LOG_PREFIX} Cannot spawn bullet: WeaponData '{cachedWeaponData.weaponName}' " +
                             "has no bulletPrefab assigned.");
            return;
        }

        Vector3 spawnPosition;
        // Don't set rotation here - let Bullet.Initialize() handle it with visual offset
        Quaternion spawnRotation = Quaternion.identity;

        if (shootPoint != null)
        {
            spawnPosition = shootPoint.position;
        }
        else
        {
            // Fallback: spawn slightly in front of the camera
            Debug.LogWarning($"{LOG_PREFIX} ShootPoint is null. Spawning bullet from camera position as fallback.");
            if (playerCamera != null)
            {
                spawnPosition = playerCamera.transform.position + playerCamera.transform.forward * 1f;
            }
            else
            {
                spawnPosition = transform.position + Vector3.up * 1.5f + transform.forward * 0.5f;
            }
        }

        GameObject bulletObj = UnityEngine.Object.Instantiate(
            cachedWeaponData.bulletPrefab,
            spawnPosition,
            spawnRotation
        );

        if (bulletObj == null)
        {
            Debug.LogError($"{LOG_PREFIX} Failed to instantiate bullet prefab for '{cachedWeaponData.weaponName}'.");
            return;
        }

        // Initialize bullet with damage and direction
        Bullet bullet = bulletObj.GetComponent<Bullet>();
        if (bullet != null)
        {
            bullet.Initialize(cachedWeaponData.damage, direction);
            bullet.SetShooter(gameObject);
        }
        else
        {
            Debug.LogWarning($"{LOG_PREFIX} Spawned bullet prefab has no Bullet component. " +
                             "Bullet will not receive damage/direction data.");
        }
    }

    /// <summary>
    /// Plays the weapon's shoot sound. For automatic weapons with looping sound,
    /// starts the loop. For single-shot weapons, plays one-shot sound.
    /// </summary>
    private void PlayShootSound()
    {
        if (shootAudioSource == null) return;

        // Check if this weapon uses looping fire sound (automatic weapons)
        if (cachedWeaponData.loopingFireSound != null)
        {
            StartLoopingFireSound();
        }
        else if (cachedWeaponData.shootSound != null)
        {
            // Single-shot weapon - play one-shot sound
            shootAudioSource.PlayOneShot(cachedWeaponData.shootSound, shootVolume);
        }
    }

    /// <summary>
    /// Starts the looping fire sound for automatic weapons.
    /// </summary>
    private void StartLoopingFireSound()
    {
        if (isPlayingLoopingFire) return;
        if (cachedWeaponData == null || cachedWeaponData.loopingFireSound == null) return;

        shootAudioSource.clip = cachedWeaponData.loopingFireSound;
        shootAudioSource.volume = shootVolume;
        shootAudioSource.loop = true;
        shootAudioSource.Play();
        isPlayingLoopingFire = true;

        Debug.Log($"{LOG_PREFIX} Started looping fire sound for {cachedWeaponData.weaponName}");
    }

    /// <summary>
    /// Stops the looping fire sound.
    /// </summary>
    private void StopLoopingFireSound()
    {
        if (!isPlayingLoopingFire) return;

        shootAudioSource.Stop();
        shootAudioSource.loop = false;
        shootAudioSource.clip = null;
        isPlayingLoopingFire = false;

        Debug.Log($"{LOG_PREFIX} Stopped looping fire sound");
    }

    /// <summary>
    /// Instantiates the muzzle flash prefab at the shoot point and destroys it after a short duration.
    /// </summary>
    private void SpawnMuzzleFlash()
    {
        if (cachedWeaponData.muzzleFlashPrefab == null) return;
        if (shootPoint == null) return;

        GameObject flash = UnityEngine.Object.Instantiate(
            cachedWeaponData.muzzleFlashPrefab,
            shootPoint.position,
            shootPoint.rotation
        );

        if (flash != null)
        {
            UnityEngine.Object.Destroy(flash, 0.1f);
        }
    }

    #endregion

    #region Reloading

    /// <summary>
    /// Handles the reload input event. Cancels aim if active, then requests reload from Inventory.
    /// </summary>
    private void HandleReloadPerformed()
    {
        if (inventory == null || cachedWeaponData == null) return;

        // Cannot reload if already reloading
        if (inventory.IsReloading)
        {
            Debug.Log($"{LOG_PREFIX} Already reloading.");
            return;
        }

        // Cannot reload if magazine is full
        if (inventory.CurrentMagazineAmmo >= cachedWeaponData.magazineSize)
        {
            Debug.Log($"{LOG_PREFIX} Magazine already full for {cachedWeaponData.weaponName}.");
            return;
        }

        // Cannot reload if no backpack ammo
        if (inventory.BackpackAmmoForCurrentWeapon <= 0)
        {
            Debug.Log($"{LOG_PREFIX} No backpack ammo available for {cachedWeaponData.weaponName}.");
            return;
        }

        // If aiming, stop aiming first (release aim before reload)
        if (isAiming)
        {
            Debug.Log($"{LOG_PREFIX} Releasing aim to reload.");
            isAiming = false;
            IsAiming = false;
        }

        // Stop looping fire sound when reloading
        StopLoopingFireSound();

        Debug.Log($"{LOG_PREFIX} Reloading {cachedWeaponData.weaponName}");

        // Play reload sound
        PlayReloadSound();

        // Delegate actual reload logic to Inventory
        inventory.StartReload();
    }

    /// <summary>
    /// Attempts auto-reload when the magazine is empty and backpack ammo is available.
    /// Called when a fire attempt fails due to empty magazine.
    /// </summary>
    private void TryAutoReload()
    {
        if (inventory == null || cachedWeaponData == null) return;
        if (inventory.IsReloading) return;
        if (inventory.CurrentMagazineAmmo > 0) return;
        if (inventory.BackpackAmmoForCurrentWeapon <= 0) return;

        Debug.Log($"{LOG_PREFIX} Auto-reloading {cachedWeaponData.weaponName} (magazine empty).");

        // Release aim for auto-reload
        if (isAiming)
        {
            isAiming = false;
            IsAiming = false;
        }

        // Stop looping fire sound when auto-reloading
        StopLoopingFireSound();

        PlayReloadSound();
        inventory.StartReload();
    }

    /// <summary>
    /// Checks every frame whether an auto-reload should be triggered.
    /// Fires when magazine hits 0, not currently reloading, and backpack has ammo.
    /// Only auto-reloads when not actively firing (trigger not held) to avoid interrupting the player.
    /// </summary>
    private void CheckAutoReload()
    {
        if (inventory == null || cachedWeaponData == null) return;
        if (inventory.IsReloading) return;
        if (isTriggerHeld) return; // Don't interrupt if player is still holding trigger
        if (inventory.CurrentMagazineAmmo > 0) return;
        if (inventory.BackpackAmmoForCurrentWeapon <= 0) return;

        TryAutoReload();
    }

    /// <summary>
    /// Plays the weapon's reload sound at the player's position.
    /// </summary>
    private void PlayReloadSound()
    {
        if (cachedWeaponData == null || cachedWeaponData.reloadSound == null) return;
        AudioSource.PlayClipAtPoint(cachedWeaponData.reloadSound, transform.position, reloadVolume);
    }

    #endregion

    #region Weapon Switching

    /// <summary>
    /// Handles scroll wheel input to cycle weapons. Positive scroll = next, negative = previous.
    /// Cancels reload on the current weapon before switching.
    /// </summary>
    /// <param name="scrollDelta">Scroll wheel delta. Positive = scroll up (next weapon), negative = scroll down (previous).</param>
    private void HandleWeaponScroll(float scrollDelta)
    {
        if (inventory == null) return;

        // Stop looping fire sound when switching weapons
        StopLoopingFireSound();

        // Cancel any in-progress reload when switching weapons
        if (inventory.IsReloading)
        {
            Debug.Log($"{LOG_PREFIX} Cancelling reload due to weapon switch.");
            inventory.CancelReload();
        }

        // Reset fire timing so the new weapon can fire immediately
        nextFireTime = 0f;

        if (scrollDelta > 0f)
        {
            inventory.EquipNextWeapon();
        }
        else if (scrollDelta < 0f)
        {
            inventory.EquipPreviousWeapon();
        }
    }

    /// <summary>
    /// Callback from <see cref="Inventory.OnWeaponEquipped"/>. Updates cached weapon data and re-acquires the shoot point.
    /// </summary>
    /// <param name="weaponData">The newly equipped weapon's data. May be null if unequipping.</param>
    private void HandleWeaponEquipped(WeaponData weaponData)
    {
        RefreshWeaponState(weaponData);
        UpdateWeaponRigs(inventory.EquippedSlotIndex);

        if (weaponData != null)
        {
            Debug.Log($"{LOG_PREFIX} Equipped weapon: {weaponData.weaponName}");
        }
        else
        {
            Debug.LogWarning($"{LOG_PREFIX} Weapon unequipped (no weapon data).");
        }
    }

    /// <summary>
    /// Refreshes all cached weapon state: weapon data reference, shoot point transform.
    /// Called on initial setup and whenever the equipped weapon changes.
    /// </summary>
    /// <param name="weaponData">The weapon data to cache. Null clears the cached state.</param>
    private void RefreshWeaponState(WeaponData weaponData)
    {
        cachedWeaponData = weaponData;
        shootPoint = null;

        // Reset fire cooldown for the new weapon
        nextFireTime = 0f;

        if (weaponData == null) return;

        // Find the shoot point by name within the player's hierarchy
        string shootPointName = !string.IsNullOrEmpty(weaponData.shootPointName)
            ? weaponData.shootPointName
            : "ShootPoint";

        shootPoint = FindChildByName(transform, shootPointName);

        if (shootPoint != null)
        {
            Debug.Log($"{LOG_PREFIX} Shoot point '{shootPointName}' found at {shootPoint.position}.");
        }
        else
        {
            Debug.LogWarning($"{LOG_PREFIX} Shoot point '{shootPointName}' NOT found in hierarchy of " +
                             $"'{gameObject.name}'. Will use camera forward as fallback when firing.");
        }
    }

    /// <summary>
    /// Updates weapon rig weights. Sets the active weapon's rig weight to 1, all others to 0.
    /// </summary>
    /// <param name="activeSlotIndex">The slot index of the currently equipped weapon.</param>
    private void UpdateWeaponRigs(int activeSlotIndex)
    {
        // Update rig weights
        if (weaponRigs != null)
        {
            for (int i = 0; i < weaponRigs.Length; i++)
            {
                if (weaponRigs[i] != null)
                    weaponRigs[i].weight = (i == activeSlotIndex) ? 1f : 0f;
            }
        }

        // Enable/disable weapon objects
        if (weaponObjects != null)
        {
            for (int i = 0; i < weaponObjects.Length; i++)
            {
                if (weaponObjects[i] != null)
                    weaponObjects[i].SetActive(i == activeSlotIndex);
            }
        }
    }

    #endregion

    #region Utility

    /// <summary>
    /// Recursively searches all children (including inactive) of the given root transform
    /// for a child with the specified name (case-insensitive).
    /// </summary>
    /// <param name="root">The root transform to search from.</param>
    /// <param name="childName">The name to match (case-insensitive).</param>
    /// <returns>The first matching child transform, or null if not found.</returns>
    private static Transform FindChildByName(Transform root, string childName)
    {
        if (root == null || string.IsNullOrEmpty(childName)) return null;

        // GetComponentsInChildren includes the root and inactive children
        Transform[] allChildren = root.GetComponentsInChildren<Transform>(true);

        for (int i = 0; i < allChildren.Length; i++)
        {
            if (string.Equals(allChildren[i].name, childName, StringComparison.OrdinalIgnoreCase))
            {
                return allChildren[i];
            }
        }

        return null;
    }

    #endregion

    #region Editor Gizmos

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (shootPoint == null) return;

        // Shoot point indicator: green when aiming, red when not
        Gizmos.color = isAiming ? Color.green : Color.red;
        Gizmos.DrawWireSphere(shootPoint.position, 0.05f);

        // Forward direction from barrel
        Gizmos.color = Color.yellow;
        Gizmos.DrawRay(shootPoint.position, shootPoint.forward * 2f);
    }
#endif

    #endregion
}
