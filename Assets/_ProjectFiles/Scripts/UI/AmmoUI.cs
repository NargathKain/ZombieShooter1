using UnityEngine;
using TMPro;

/// <summary>
/// Production-quality UI controller that displays the player's current ammunition status.
/// Subscribes to <see cref="Inventory"/> static events for fully decoupled, event-driven updates.
/// Supports color-coded states (normal, low ammo, empty, reloading) and responds to
/// weapon switches, ammo consumption, and reload cycles.
///
/// <para><b>Setup:</b></para>
/// <list type="number">
///   <item>Create a TextMeshPro - Text (UI) element on your HUD Canvas.</item>
///   <item>Attach this script to the same GameObject (or a parent).</item>
///   <item>Assign the TMP_Text component in the Inspector via <c>ammoText</c>.</item>
///   <item>Optionally configure the color fields for your project's visual style.</item>
/// </list>
///
/// <para><b>Display Format:</b></para>
/// <c>"{backpack} / {magazine}"</c> -- e.g., "60 / 12".
/// During reload: "RELOADING..." in gray.
///
/// <para><b>Color States:</b></para>
/// <list type="bullet">
///   <item>White: Normal (magazine > 25% of capacity).</item>
///   <item>Yellow: Low ammo (magazine at or below 25% but not empty).</item>
///   <item>Red: Empty (magazine = 0 rounds).</item>
///   <item>Gray: Currently reloading.</item>
/// </list>
///
/// <para><b>Dependencies:</b></para>
/// <list type="bullet">
///   <item><see cref="Inventory"/> -- static events for ammo changes, weapon equip, reload.</item>
///   <item><see cref="WeaponData"/> -- magazine size for capacity calculations.</item>
///   <item>TextMeshPro (TMPro) package.</item>
/// </list>
/// </summary>
[RequireComponent(typeof(RectTransform))]
public class AmmoUI : MonoBehaviour
{
    #region Constants

    /// <summary>Prefix for all console log messages from this system.</summary>
    private const string LOG_PREFIX = "[AmmoUI]";

    /// <summary>Low-ammo threshold as a fraction of magazine capacity (25%).</summary>
    private const float LOW_AMMO_THRESHOLD = 0.25f;

    /// <summary>Text displayed during an active reload cycle.</summary>
    private const string RELOADING_TEXT = "RELOADING...";

    #endregion

    #region Serialized Fields

    [Header("UI References")]
    [Tooltip("The TextMeshProUGUI component used to display the ammo count.")]
    [SerializeField] private TextMeshProUGUI ammoText;

    [Header("Color Configuration")]
    [Tooltip("Color when magazine is above 25% capacity.")]
    [SerializeField] private Color normalColor = Color.white;

    [Tooltip("Color when magazine is at or below 25% capacity but not empty.")]
    [SerializeField] private Color lowAmmoColor = Color.yellow;

    [Tooltip("Color when magazine is completely empty (0 rounds).")]
    [SerializeField] private Color emptyColor = Color.red;

    [Tooltip("Color displayed during reload (shown alongside RELOADING... text).")]
    [SerializeField] private Color reloadingColor = Color.gray;

    #endregion

    #region Private Fields

    /// <summary>Magazine capacity of the currently equipped weapon, cached on equip.</summary>
    private int currentMagazineCapacity;

    /// <summary>Most recently received backpack ammo count for the current weapon.</summary>
    private int cachedBackpackAmmo;

    /// <summary>Most recently received magazine ammo count for the current weapon.</summary>
    private int cachedMagazineAmmo;

    /// <summary>Whether a reload is currently in progress.</summary>
    private bool isReloading;

    #endregion

    #region Unity Lifecycle

    private void Awake()
    {
        ValidateTextComponent();
    }

    private void OnEnable()
    {
        SubscribeToEvents();
    }

    private void OnDisable()
    {
        UnsubscribeFromEvents();
    }

    #endregion

    #region Event Subscription

    /// <summary>
    /// Subscribes to all relevant <see cref="Inventory"/> static events.
    /// Called in <see cref="OnEnable"/> so the UI reconnects after being toggled.
    /// </summary>
    private void SubscribeToEvents()
    {
        Inventory.OnAmmoChanged += HandleAmmoChanged;
        Inventory.OnWeaponEquipped += HandleWeaponEquipped;
        Inventory.OnReloadStarted += HandleReloadStarted;
        Inventory.OnReloadCompleted += HandleReloadCompleted;
    }

    /// <summary>
    /// Unsubscribes from all <see cref="Inventory"/> static events.
    /// Called in <see cref="OnDisable"/> to prevent stale references and memory leaks.
    /// </summary>
    private void UnsubscribeFromEvents()
    {
        Inventory.OnAmmoChanged -= HandleAmmoChanged;
        Inventory.OnWeaponEquipped -= HandleWeaponEquipped;
        Inventory.OnReloadStarted -= HandleReloadStarted;
        Inventory.OnReloadCompleted -= HandleReloadCompleted;
    }

    #endregion

    #region Event Handlers

    /// <summary>
    /// Handles ammo count changes from <see cref="Inventory.OnAmmoChanged"/>.
    /// Caches the latest values and refreshes the display unless a reload is active.
    /// </summary>
    /// <param name="ammoType">The ammo type that changed (unused for display but available for filtering).</param>
    /// <param name="backpackAmount">Current backpack ammo for this type.</param>
    /// <param name="magazineAmount">Current magazine ammo for the equipped weapon.</param>
    private void HandleAmmoChanged(AmmoType ammoType, int backpackAmount, int magazineAmount)
    {
        cachedBackpackAmmo = backpackAmount;
        cachedMagazineAmmo = magazineAmount;

        // During reload, keep showing the reload text; ammo display restores on completion.
        if (!isReloading)
        {
            UpdateAmmoDisplay();
        }
    }

    /// <summary>
    /// Handles weapon equipment from <see cref="Inventory.OnWeaponEquipped"/>.
    /// Caches the new weapon's magazine capacity and immediately refreshes the display.
    /// </summary>
    /// <param name="weaponData">The newly equipped weapon's data. May be null if unequipping.</param>
    private void HandleWeaponEquipped(WeaponData weaponData)
    {
        if (weaponData != null)
        {
            currentMagazineCapacity = weaponData.magazineSize;
            Debug.Log($"{LOG_PREFIX} Weapon equipped: {weaponData.weaponName} " +
                      $"(magazine capacity: {currentMagazineCapacity})");
        }
        else
        {
            currentMagazineCapacity = 0;
            Debug.LogWarning($"{LOG_PREFIX} Weapon unequipped (no weapon data).");
        }

        // Reset reload state on weapon switch
        isReloading = false;

        // The Inventory broadcasts OnAmmoChanged after OnWeaponEquipped,
        // but we update immediately for instant feedback on weapon switch.
        // The subsequent OnAmmoChanged will refresh with accurate values.
        UpdateAmmoDisplay();
    }

    /// <summary>
    /// Handles reload initiation from <see cref="Inventory.OnReloadStarted"/>.
    /// Switches the display to reloading state.
    /// </summary>
    private void HandleReloadStarted()
    {
        isReloading = true;
        ShowReloadingState();
    }

    /// <summary>
    /// Handles reload completion from <see cref="Inventory.OnReloadCompleted"/>.
    /// Restores the normal ammo display with the updated magazine count.
    /// </summary>
    private void HandleReloadCompleted()
    {
        isReloading = false;
        UpdateAmmoDisplay();
    }

    #endregion

    #region Display Logic

    /// <summary>
    /// Updates the ammo text display with the current backpack and magazine values.
    /// Applies color coding based on the current magazine fill level relative to capacity.
    /// </summary>
    private void UpdateAmmoDisplay()
    {
        if (ammoText == null) return;

        ammoText.text = $"{cachedBackpackAmmo} / {cachedMagazineAmmo}";
        ammoText.color = DetermineAmmoColor();
    }

    /// <summary>
    /// Switches the display to the reloading state: gray text reading "RELOADING...".
    /// </summary>
    private void ShowReloadingState()
    {
        if (ammoText == null) return;

        ammoText.text = RELOADING_TEXT;
        ammoText.color = reloadingColor;
    }

    /// <summary>
    /// Determines the appropriate display color based on the current magazine fill level.
    /// </summary>
    /// <returns>
    /// <see cref="emptyColor"/> if magazine is 0,
    /// <see cref="lowAmmoColor"/> if magazine is at or below 25% of capacity,
    /// <see cref="normalColor"/> otherwise.
    /// </returns>
    private Color DetermineAmmoColor()
    {
        if (cachedMagazineAmmo <= 0)
        {
            return emptyColor;
        }

        // Guard against division by zero if capacity is not yet set
        if (currentMagazineCapacity <= 0)
        {
            return normalColor;
        }

        float fillRatio = (float)cachedMagazineAmmo / currentMagazineCapacity;

        if (fillRatio <= LOW_AMMO_THRESHOLD)
        {
            return lowAmmoColor;
        }

        return normalColor;
    }

    #endregion

    #region Validation

    /// <summary>
    /// Validates that the required TMP text component is assigned.
    /// Logs an error and disables the component if missing.
    /// </summary>
    private void ValidateTextComponent()
    {
        if (ammoText != null) return;

        // Attempt auto-detection on the same GameObject
        ammoText = GetComponent<TextMeshProUGUI>();

        if (ammoText == null)
        {
            Debug.LogError($"{LOG_PREFIX} No TextMeshProUGUI component assigned or found on " +
                           $"'{gameObject.name}'. Assign the ammoText field in the Inspector. " +
                           "Disabling AmmoUI.");
            enabled = false;
        }
    }

    #endregion
}
