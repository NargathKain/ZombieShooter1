using System.Collections;
using UnityEngine;
using TMPro;

/// <summary>
/// Production-quality HUD element that displays contextual reload prompts to the player.
/// Subscribes to <see cref="Inventory"/> static events for fully decoupled, event-driven updates.
/// Shows "Press R to reload" when the magazine is empty and backpack ammo is available,
/// then switches to a real-time countdown timer during active reloads.
///
/// <para><b>Setup:</b></para>
/// <list type="number">
///   <item>Create a TextMeshPro - Text (UI) element on your HUD Canvas.</item>
///   <item>Attach this script to the same GameObject (or a parent).</item>
///   <item>Assign the TMP_Text component in the Inspector via <c>reloadText</c>.</item>
///   <item>Optionally configure the color fields for your project's visual style.</item>
/// </list>
///
/// <para><b>Display States:</b></para>
/// <list type="bullet">
///   <item>Empty magazine + backpack ammo available + not reloading: "Press R to reload" in <c>promptColor</c>.</item>
///   <item>Reload in progress: countdown timer "X.Xs" in <c>countdownColor</c>.</item>
///   <item>All other states: hidden (empty text).</item>
/// </list>
///
/// <para><b>Dependencies:</b></para>
/// <list type="bullet">
///   <item><see cref="Inventory"/> -- static events for ammo changes, weapon equip, reload.</item>
///   <item><see cref="WeaponData"/> -- reload time for countdown duration.</item>
///   <item>TextMeshPro (TMPro) package.</item>
/// </list>
/// </summary>
[RequireComponent(typeof(RectTransform))]
public class ReloadPromptUI : MonoBehaviour
{
    #region Constants

    /// <summary>Prefix for all console log messages from this system.</summary>
    private const string LOG_PREFIX = "[ReloadPromptUI]";

    /// <summary>Text displayed when the player should reload.</summary>
    private const string PRESS_R_TEXT = "Press R to reload";

    #endregion

    #region Serialized Fields

    [Header("UI References")]
    [Tooltip("The TextMeshProUGUI component used to display the reload prompt and countdown.")]
    [SerializeField] private TextMeshProUGUI reloadText;

    [Header("Color Configuration")]
    [Tooltip("Color for the 'Press R to reload' prompt.")]
    [SerializeField] private Color promptColor = Color.white;

    [Tooltip("Color for the countdown timer during reload.")]
    [SerializeField] private Color countdownColor = Color.yellow;

    #endregion

    #region Private Fields

    /// <summary>Most recently received backpack ammo count for the current weapon.</summary>
    private int cachedBackpackAmmo;

    /// <summary>Most recently received magazine ammo count for the equipped weapon.</summary>
    private int cachedMagazineAmmo;

    /// <summary>Cached reload duration from the currently equipped weapon.</summary>
    private float cachedReloadTime;

    /// <summary>Whether a reload is currently in progress.</summary>
    private bool isReloading;

    /// <summary>Reference to the active countdown coroutine for safe stopping.</summary>
    private Coroutine countdownCoroutine;

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
        StopCountdownCoroutine();
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
    /// Caches the latest values and evaluates whether to show the reload prompt.
    /// </summary>
    /// <param name="ammoType">The ammo type that changed (unused for display but available for filtering).</param>
    /// <param name="backpackAmount">Current backpack ammo for this type.</param>
    /// <param name="magazineAmount">Current magazine ammo for the equipped weapon.</param>
    private void HandleAmmoChanged(AmmoType ammoType, int backpackAmount, int magazineAmount)
    {
        cachedBackpackAmmo = backpackAmount;
        cachedMagazineAmmo = magazineAmount;

        if (isReloading) return;

        EvaluatePromptVisibility();
    }

    /// <summary>
    /// Handles weapon equipment from <see cref="Inventory.OnWeaponEquipped"/>.
    /// Caches the new weapon's reload time and resets all prompt state.
    /// </summary>
    /// <param name="weaponData">The newly equipped weapon's data. May be null if unequipping.</param>
    private void HandleWeaponEquipped(WeaponData weaponData)
    {
        StopCountdownCoroutine();
        isReloading = false;

        if (weaponData != null)
        {
            cachedReloadTime = weaponData.reloadTime;
            Debug.Log($"{LOG_PREFIX} Weapon equipped: reload time {cachedReloadTime}s");
        }
        else
        {
            cachedReloadTime = 0f;
            Debug.LogWarning($"{LOG_PREFIX} Weapon unequipped (no weapon data).");
        }

        HidePrompt();
    }

    /// <summary>
    /// Handles reload initiation from <see cref="Inventory.OnReloadStarted"/>.
    /// Starts the countdown coroutine using the cached reload time.
    /// </summary>
    private void HandleReloadStarted()
    {
        isReloading = true;
        StopCountdownCoroutine();
        countdownCoroutine = StartCoroutine(ReloadCountdownRoutine());
    }

    /// <summary>
    /// Handles reload completion from <see cref="Inventory.OnReloadCompleted"/>.
    /// Stops the countdown and hides all prompt text.
    /// </summary>
    private void HandleReloadCompleted()
    {
        isReloading = false;
        StopCountdownCoroutine();
        HidePrompt();
    }

    #endregion

    #region Display Logic

    /// <summary>
    /// Evaluates whether the "Press R to reload" prompt should be shown or hidden
    /// based on current magazine and backpack ammo state.
    /// </summary>
    private void EvaluatePromptVisibility()
    {
        if (cachedMagazineAmmo == 0 && cachedBackpackAmmo > 0)
        {
            ShowPressRPrompt();
        }
        else
        {
            HidePrompt();
        }
    }

    /// <summary>
    /// Displays the "Press R to reload" prompt in the configured prompt color.
    /// </summary>
    private void ShowPressRPrompt()
    {
        if (reloadText == null) return;

        reloadText.text = PRESS_R_TEXT;
        reloadText.color = promptColor;
    }

    /// <summary>
    /// Clears all prompt text, effectively hiding the UI element.
    /// </summary>
    private void HidePrompt()
    {
        if (reloadText == null) return;

        reloadText.text = "";
    }

    /// <summary>
    /// Coroutine that displays a real-time countdown during reload.
    /// Updates every frame, showing remaining time in "X.Xs" format.
    /// </summary>
    private IEnumerator ReloadCountdownRoutine()
    {
        if (reloadText == null) yield break;

        reloadText.color = countdownColor;

        float remaining = cachedReloadTime;
        while (remaining > 0f)
        {
            reloadText.text = $"{remaining:F1}s";
            remaining -= Time.deltaTime;
            yield return null;
        }

        reloadText.text = "";
        countdownCoroutine = null;
    }

    #endregion

    #region Coroutine Management

    /// <summary>
    /// Safely stops the countdown coroutine if one is active.
    /// </summary>
    private void StopCountdownCoroutine()
    {
        if (countdownCoroutine != null)
        {
            StopCoroutine(countdownCoroutine);
            countdownCoroutine = null;
        }
    }

    #endregion

    #region Validation

    /// <summary>
    /// Validates that the required TMP text component is assigned.
    /// Logs an error and disables the component if missing.
    /// </summary>
    private void ValidateTextComponent()
    {
        if (reloadText != null) return;

        reloadText = GetComponent<TextMeshProUGUI>();

        if (reloadText == null)
        {
            Debug.LogError($"{LOG_PREFIX} No TextMeshProUGUI component assigned or found on " +
                           $"'{gameObject.name}'. Assign the reloadText field in the Inspector. " +
                           "Disabling ReloadPromptUI.");
            enabled = false;
        }
    }

    #endregion
}
