using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Production-quality Inventory system for managing weapons and ammunition.
/// Handles weapon ownership, equipment, ammo tracking (magazine and backpack),
/// and reload mechanics with full event support for UI and other systems.
///
/// ARCHITECTURE:
/// - Weapons are pre-attached to player model; this system only tracks ownership
/// - Magazine ammo is tracked per-weapon (stored in dictionary)
/// - Backpack ammo is shared pool per AmmoType
/// - All state changes broadcast events for decoupled UI/audio updates
///
/// USAGE:
/// 1. Assign starting weapon and ammo configuration in Inspector
/// 2. Subscribe to static events for UI updates
/// 3. Use EquipWeapon() to switch weapons, ConsumeAmmo() when firing
/// </summary>
public class Inventory : MonoBehaviour
{
    #region Constants

    /// <summary>Number of weapon slots available (mapped to number keys 1-7).</summary>
    private const int WEAPON_SLOT_COUNT = 7;

    /// <summary>Prefix for all console log messages from this system.</summary>
    private const string LOG_PREFIX = "[Inventory]";

    #endregion

    #region Events

    /// <summary>
    /// Fired when a weapon is equipped. Passes the newly equipped WeaponData.
    /// Subscribe to update UI weapon icons, animations, etc.
    /// </summary>
    public static event Action<WeaponData> OnWeaponEquipped;

    /// <summary>
    /// Fired when ammo counts change. Parameters: AmmoType, backpackAmount, magazineAmount.
    /// Subscribe to update ammo counter UI.
    /// </summary>
    public static event Action<AmmoType, int, int> OnAmmoChanged;

    /// <summary>
    /// Fired when reload begins. Use for UI feedback, animation triggers.
    /// </summary>
    public static event Action OnReloadStarted;

    /// <summary>
    /// Fired when reload completes successfully. Magazine is now refilled.
    /// </summary>
    public static event Action OnReloadCompleted;

    /// <summary>Fired when the player's point total changes. Parameter: current total.</summary>
    public static event Action<int> OnPointsChanged;

    /// <summary>Fired when points are gained. Parameters: amount gained, new total.</summary>
    public static event Action<int, int> OnPointsGained;

    #endregion

    #region Serialized Fields

    [Header("Starting Configuration")]
    [Tooltip("The weapon the player starts with. Must be in the weaponSlots array.")]
    [SerializeField] private WeaponData startingWeapon;

    [Tooltip("Weapon slots (index 0 = key 1, index 6 = key 7). Null slots are empty.")]
    [SerializeField] private WeaponData[] weaponSlots = new WeaponData[WEAPON_SLOT_COUNT];

    [Header("Starting Ammo")]
    [Tooltip("Ammo types for initial backpack ammo. Must match initialAmmoAmounts length.")]
    [SerializeField] private AmmoType[] initialAmmoTypes;

    [Tooltip("Starting amounts for each ammo type. Must match initialAmmoTypes length.")]
    [SerializeField] private int[] initialAmmoAmounts;

    [Header("Ammo Limits")]
    [Tooltip("Maximum backpack ammo per type. Must match maxAmmoTypes length.")]
    [SerializeField] private AmmoType[] maxAmmoTypes;

    [Tooltip("Max amounts for each ammo type. Must match maxAmmoTypes length.")]
    [SerializeField] private int[] maxAmmoAmounts;

    [Header("Default Limits")]
    [Tooltip("Default maximum ammo if not specified in maxAmmoTypes/maxAmmoAmounts.")]
    [SerializeField] private int defaultMaxAmmo = 999;

    #endregion

    #region Private Fields

    private Dictionary<WeaponData, bool> ownedWeapons = new Dictionary<WeaponData, bool>();
    private Dictionary<AmmoType, int> backpackAmmo = new Dictionary<AmmoType, int>();
    private Dictionary<AmmoType, int> maxBackpackAmmo = new Dictionary<AmmoType, int>();
    private Dictionary<WeaponData, int> magazineAmmo = new Dictionary<WeaponData, int>();
    private WeaponData equippedWeapon;
    private int equippedSlotIndex = -1;
    private bool isReloading = false;
    private Coroutine reloadCoroutine;
    private int currentPoints;

    #endregion

    #region Properties

    /// <summary>Gets the currently equipped weapon data. Null if none equipped.</summary>
    public WeaponData EquippedWeapon => equippedWeapon;

    /// <summary>Gets current magazine ammo for the equipped weapon.</summary>
    public int CurrentMagazineAmmo
    {
        get
        {
            if (equippedWeapon == null) return 0;
            return magazineAmmo.TryGetValue(equippedWeapon, out int ammo) ? ammo : 0;
        }
    }

    /// <summary>Gets backpack ammo for the currently equipped weapon's ammo type.</summary>
    public int BackpackAmmoForCurrentWeapon
    {
        get
        {
            if (equippedWeapon == null) return 0;
            return GetBackpackAmmo(equippedWeapon.ammoType);
        }
    }

    /// <summary>Returns true if a reload is currently in progress.</summary>
    public bool IsReloading => isReloading;

    /// <summary>Gets the index of the currently equipped weapon slot (0-6). -1 if none.</summary>
    public int EquippedSlotIndex => equippedSlotIndex;

    /// <summary>Gets the player's current point balance.</summary>
    public int CurrentPoints => currentPoints;

    #endregion

    #region Unity Lifecycle

    private void Awake()
    {
        InitializeAmmoDictionaries();
        InitializeWeaponOwnership();
    }

    private void Start()
    {
        ApplyStartingAmmo();
        EquipStartingWeapon();
    }

    private void OnEnable()
    {
        EnemyHealth.OnEnemyDeath += HandleEnemyDeath;
    }

    private void OnDisable()
    {
        EnemyHealth.OnEnemyDeath -= HandleEnemyDeath;
        CancelReload();
    }

    #endregion

    #region Initialization

    /// <summary>Initializes ammo dictionaries for all AmmoType values.</summary>
    private void InitializeAmmoDictionaries()
    {
        foreach (AmmoType ammoType in Enum.GetValues(typeof(AmmoType)))
        {
            backpackAmmo[ammoType] = 0;
            maxBackpackAmmo[ammoType] = defaultMaxAmmo;
        }

        if (maxAmmoTypes != null && maxAmmoAmounts != null)
        {
            int minLength = Mathf.Min(maxAmmoTypes.Length, maxAmmoAmounts.Length);
            for (int i = 0; i < minLength; i++)
            {
                maxBackpackAmmo[maxAmmoTypes[i]] = Mathf.Max(0, maxAmmoAmounts[i]);
            }
        }
    }

    /// <summary>Initializes weapon ownership based on configured weapon slots.</summary>
    private void InitializeWeaponOwnership()
    {
        ownedWeapons.Clear();
        magazineAmmo.Clear();

        if (weaponSlots == null)
        {
            Debug.LogWarning($"{LOG_PREFIX} weaponSlots array is null. Creating empty array.");
            weaponSlots = new WeaponData[WEAPON_SLOT_COUNT];
            return;
        }

        for (int i = 0; i < weaponSlots.Length; i++)
        {
            WeaponData weapon = weaponSlots[i];
            if (weapon != null && !ownedWeapons.ContainsKey(weapon))
            {
                ownedWeapons[weapon] = true;
                magazineAmmo[weapon] = weapon.magazineSize;
            }
        }
    }

    /// <summary>Applies starting ammo configuration from serialized arrays.</summary>
    private void ApplyStartingAmmo()
    {
        if (initialAmmoTypes == null || initialAmmoAmounts == null)
        {
            Debug.LogWarning($"{LOG_PREFIX} Initial ammo arrays not configured. Player starts with no backpack ammo.");
            return;
        }

        if (initialAmmoTypes.Length != initialAmmoAmounts.Length)
        {
            Debug.LogWarning($"{LOG_PREFIX} initialAmmoTypes length ({initialAmmoTypes.Length}) " +
                           $"does not match initialAmmoAmounts length ({initialAmmoAmounts.Length}). " +
                           "Using minimum of both.");
        }

        int minLength = Mathf.Min(initialAmmoTypes.Length, initialAmmoAmounts.Length);
        for (int i = 0; i < minLength; i++)
        {
            AmmoType type = initialAmmoTypes[i];
            int amount = Mathf.Max(0, initialAmmoAmounts[i]);
            backpackAmmo[type] = Mathf.Clamp(amount, 0, maxBackpackAmmo[type]);
            Debug.Log($"{LOG_PREFIX} Starting ammo: {type} = {backpackAmmo[type]}");
        }
    }

    /// <summary>Equips the configured starting weapon.</summary>
    private void EquipStartingWeapon()
    {
        if (startingWeapon == null)
        {
            Debug.LogWarning($"{LOG_PREFIX} No starting weapon configured. Attempting first available.");
            EquipFirstAvailableWeapon();
            return;
        }

        int slotIndex = FindWeaponSlotIndex(startingWeapon);
        if (slotIndex >= 0)
        {
            EquipWeapon(slotIndex);
        }
        else
        {
            Debug.LogWarning($"{LOG_PREFIX} Starting weapon '{startingWeapon.weaponName}' not in weapon slots.");
            EquipFirstAvailableWeapon();
        }
    }

    private void EquipFirstAvailableWeapon()
    {
        for (int i = 0; i < weaponSlots.Length; i++)
        {
            if (weaponSlots[i] != null)
            {
                EquipWeapon(i);
                return;
            }
        }
        Debug.LogWarning($"{LOG_PREFIX} No weapons available to equip.");
    }

    #endregion

    #region Weapon Management

    /// <summary>Checks if the player owns the specified weapon.</summary>
    public bool HasWeapon(WeaponData weapon)
    {
        if (weapon == null) return false;
        return ownedWeapons.TryGetValue(weapon, out bool owned) && owned;
    }

    /// <summary>Adds a weapon to the first empty slot in inventory.</summary>
    public void AddWeapon(WeaponData weapon)
    {
        if (weapon == null) return;
        if (HasWeapon(weapon))
        {
            Debug.Log($"{LOG_PREFIX} Already own weapon: {weapon.weaponName}");
            return;
        }

        int emptySlot = FindEmptyWeaponSlot();
        if (emptySlot < 0)
        {
            Debug.LogWarning($"{LOG_PREFIX} Cannot add '{weapon.weaponName}'. All slots full.");
            return;
        }

        weaponSlots[emptySlot] = weapon;
        ownedWeapons[weapon] = true;
        magazineAmmo[weapon] = weapon.magazineSize;
        Debug.Log($"{LOG_PREFIX} Added weapon: {weapon.weaponName} to slot {emptySlot + 1}");
    }

    /// <summary>Removes a weapon from inventory. Switches weapon if currently equipped.</summary>
    public void RemoveWeapon(WeaponData weapon)
    {
        if (weapon == null || !HasWeapon(weapon)) return;

        int slotIndex = FindWeaponSlotIndex(weapon);
        if (slotIndex >= 0) weaponSlots[slotIndex] = null;

        ownedWeapons[weapon] = false;
        magazineAmmo.Remove(weapon);
        Debug.Log($"{LOG_PREFIX} Removed weapon: {weapon.weaponName}");

        if (equippedWeapon == weapon)
        {
            CancelReload();
            equippedWeapon = null;
            equippedSlotIndex = -1;
            EquipFirstAvailableWeapon();
        }
    }

    /// <summary>Equips a weapon by slot index (0-6).</summary>
    public void EquipWeapon(int slotIndex)
    {
        if (slotIndex < 0 || slotIndex >= weaponSlots.Length) return;

        WeaponData weapon = weaponSlots[slotIndex];
        if (weapon == null) return;
        if (equippedWeapon == weapon && !isReloading) return;

        CancelReload();
        equippedWeapon = weapon;
        equippedSlotIndex = slotIndex;

        Debug.Log($"{LOG_PREFIX} Equipped weapon: {weapon.weaponName}");
        OnWeaponEquipped?.Invoke(weapon);
        BroadcastAmmoChanged();
    }

    /// <summary>Equips a weapon by its WeaponData reference.</summary>
    public void EquipWeapon(WeaponData weapon)
    {
        if (weapon == null || !HasWeapon(weapon)) return;
        int slotIndex = FindWeaponSlotIndex(weapon);
        if (slotIndex >= 0) EquipWeapon(slotIndex);
    }

    /// <summary>Equips the next weapon in slot order (scroll wheel up).</summary>
    public void EquipNextWeapon()
    {
        int nextSlot = FindNextOccupiedSlot(equippedSlotIndex, 1);
        if (nextSlot >= 0 && nextSlot != equippedSlotIndex) EquipWeapon(nextSlot);
    }

    /// <summary>Equips the previous weapon in slot order (scroll wheel down).</summary>
    public void EquipPreviousWeapon()
    {
        int prevSlot = FindNextOccupiedSlot(equippedSlotIndex, -1);
        if (prevSlot >= 0 && prevSlot != equippedSlotIndex) EquipWeapon(prevSlot);
    }

    #endregion

    #region Ammo Management

    /// <summary>Gets backpack ammo count for the specified ammo type.</summary>
    public int GetBackpackAmmo(AmmoType type)
    {
        return backpackAmmo.TryGetValue(type, out int amount) ? amount : 0;
    }

    /// <summary>Gets maximum backpack ammo capacity for the specified type.</summary>
    public int GetMaxBackpackAmmo(AmmoType type)
    {
        return maxBackpackAmmo.TryGetValue(type, out int max) ? max : defaultMaxAmmo;
    }

    /// <summary>Adds ammo to the backpack pool, clamped to max capacity.</summary>
    public void AddAmmo(AmmoType type, int amount)
    {
        if (amount <= 0) return;

        int currentAmount = GetBackpackAmmo(type);
        int maxAmount = GetMaxBackpackAmmo(type);
        int newAmount = Mathf.Clamp(currentAmount + amount, 0, maxAmount);

        backpackAmmo[type] = newAmount;
        Debug.Log($"{LOG_PREFIX} Added {newAmount - currentAmount} {type} ammo. Total: {newAmount}/{maxAmount}");

        if (equippedWeapon != null && equippedWeapon.ammoType == type)
            BroadcastAmmoChanged();
    }

    /// <summary>Consumes one round from the current weapon's magazine.</summary>
    /// <returns>True if ammo was consumed, false if magazine empty or reloading.</returns>
    public bool ConsumeAmmo()
    {
        if (equippedWeapon == null || isReloading) return false;

        int currentMag = CurrentMagazineAmmo;
        if (currentMag <= 0) return false;

        magazineAmmo[equippedWeapon] = currentMag - 1;
        BroadcastAmmoChanged();
        return true;
    }

    /// <summary>Checks if a reload is possible for the current weapon.</summary>
    public bool CanReload()
    {
        if (equippedWeapon == null) return false;
        if (isReloading) return false;
        if (CurrentMagazineAmmo >= equippedWeapon.magazineSize) return false;
        if (GetBackpackAmmo(equippedWeapon.ammoType) <= 0) return false;
        return true;
    }

    /// <summary>Begins the reload process for the current weapon.</summary>
    public void StartReload()
    {
        if (!CanReload())
        {
            if (equippedWeapon != null)
            {
                if (isReloading) Debug.LogWarning($"{LOG_PREFIX} Already reloading.");
                else if (CurrentMagazineAmmo >= equippedWeapon.magazineSize) Debug.LogWarning($"{LOG_PREFIX} Magazine already full.");
                else if (GetBackpackAmmo(equippedWeapon.ammoType) <= 0) Debug.LogWarning($"{LOG_PREFIX} No {equippedWeapon.ammoType} ammo in backpack.");
            }
            return;
        }
        reloadCoroutine = StartCoroutine(ReloadCoroutine());
    }

    /// <summary>Cancels any active reload operation.</summary>
    public void CancelReload()
    {
        if (reloadCoroutine != null)
        {
            StopCoroutine(reloadCoroutine);
            reloadCoroutine = null;
        }
        if (isReloading)
        {
            isReloading = false;
            Debug.Log($"{LOG_PREFIX} Reload cancelled.");
        }
    }

    #endregion

    #region Points Management

    /// <summary>
    /// Adds points from any source (kills, pickups, debug).
    /// Broadcasts OnPointsGained and OnPointsChanged.
    /// </summary>
    public void AddPoints(int amount)
    {
        if (amount <= 0) return;

        currentPoints += amount;
        OnPointsGained?.Invoke(amount, currentPoints);
        OnPointsChanged?.Invoke(currentPoints);
        Debug.Log($"{LOG_PREFIX} +{amount} points (Total: {currentPoints})");
    }

    /// <summary>
    /// Attempts to spend points. Returns false if insufficient balance.
    /// Broadcasts OnPointsChanged on success.
    /// </summary>
    public bool SpendPoints(int amount)
    {
        if (amount <= 0) return false;

        if (currentPoints < amount)
        {
            Debug.Log($"{LOG_PREFIX} Cannot spend {amount} pts. Current: {currentPoints}");
            return false;
        }

        currentPoints -= amount;
        OnPointsChanged?.Invoke(currentPoints);
        Debug.Log($"{LOG_PREFIX} Spent {amount} pts. Remaining: {currentPoints}");
        return true;
    }

    private void HandleEnemyDeath(EnemyHealth enemyHealth)
    {
        EnemyData data = enemyHealth.GetEnemyData();
        int points = (data != null) ? data.pointsOnDeath : 100;

        if (data == null)
            Debug.LogWarning($"{LOG_PREFIX} EnemyData null on {enemyHealth.gameObject.name}. Using default {points} pts.");

        AddPoints(points);
    }

    #endregion

    #region Private Helpers

    private IEnumerator ReloadCoroutine()
    {
        if (equippedWeapon == null) yield break;

        isReloading = true;
        WeaponData weaponBeingReloaded = equippedWeapon;

        Debug.Log($"{LOG_PREFIX} Reloading {weaponBeingReloaded.weaponName} ({weaponBeingReloaded.reloadTime}s)");
        OnReloadStarted?.Invoke();

        yield return new WaitForSeconds(weaponBeingReloaded.reloadTime);

        if (equippedWeapon != weaponBeingReloaded)
        {
            isReloading = false;
            yield break;
        }

        int currentMag = magazineAmmo.TryGetValue(weaponBeingReloaded, out int mag) ? mag : 0;
        int ammoNeeded = weaponBeingReloaded.magazineSize - currentMag;
        int backpack = GetBackpackAmmo(weaponBeingReloaded.ammoType);
        int ammoToTransfer = Mathf.Min(ammoNeeded, backpack);

        magazineAmmo[weaponBeingReloaded] = currentMag + ammoToTransfer;
        backpackAmmo[weaponBeingReloaded.ammoType] = backpack - ammoToTransfer;

        isReloading = false;
        reloadCoroutine = null;

        Debug.Log($"{LOG_PREFIX} Reload completed for {weaponBeingReloaded.weaponName}. " +
                 $"Magazine: {magazineAmmo[weaponBeingReloaded]}/{weaponBeingReloaded.magazineSize}");

        OnReloadCompleted?.Invoke();
        BroadcastAmmoChanged();
    }

    private void BroadcastAmmoChanged()
    {
        if (equippedWeapon == null) return;
        OnAmmoChanged?.Invoke(equippedWeapon.ammoType, GetBackpackAmmo(equippedWeapon.ammoType), CurrentMagazineAmmo);
    }

    private int FindWeaponSlotIndex(WeaponData weapon)
    {
        if (weapon == null || weaponSlots == null) return -1;
        for (int i = 0; i < weaponSlots.Length; i++)
            if (weaponSlots[i] == weapon) return i;
        return -1;
    }

    private int FindEmptyWeaponSlot()
    {
        if (weaponSlots == null) return -1;
        for (int i = 0; i < weaponSlots.Length; i++)
            if (weaponSlots[i] == null) return i;
        return -1;
    }

    private int FindNextOccupiedSlot(int currentSlot, int direction)
    {
        if (weaponSlots == null || weaponSlots.Length == 0) return -1;
        direction = direction >= 0 ? 1 : -1;
        int searchStart = currentSlot + direction;
        for (int i = 0; i < weaponSlots.Length; i++)
        {
            int checkIndex = (searchStart + (i * direction) + weaponSlots.Length * 2) % weaponSlots.Length;
            if (weaponSlots[checkIndex] != null) return checkIndex;
        }
        return -1;
    }

    #endregion

    #region Editor Helpers

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (weaponSlots == null || weaponSlots.Length != WEAPON_SLOT_COUNT)
            System.Array.Resize(ref weaponSlots, WEAPON_SLOT_COUNT);
    }
#endif

    #endregion
}
