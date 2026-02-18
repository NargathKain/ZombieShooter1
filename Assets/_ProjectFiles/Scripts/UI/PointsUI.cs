using UnityEngine;
using System.Collections;
using TMPro;

/// <summary>
/// HUD element that displays the player's current point total.
/// Subscribes to <see cref="Inventory"/> static events for fully decoupled, event-driven updates.
/// Flashes the text color on point gain for visual feedback.
///
/// <para><b>Setup:</b></para>
/// <list type="number">
///   <item>Create a TextMeshPro - Text (UI) element on your HUD Canvas.</item>
///   <item>Attach this script to the same GameObject (or a parent with a RectTransform).</item>
///   <item>Assign the TMP_Text component in the Inspector via <c>pointsText</c>.</item>
///   <item>Optionally configure the format string, colors, and flash duration.</item>
/// </list>
///
/// <para><b>Dependencies:</b></para>
/// <list type="bullet">
///   <item><see cref="Inventory"/> -- static events for point changes and gains.</item>
///   <item>TextMeshPro (TMPro) package.</item>
/// </list>
/// </summary>
[RequireComponent(typeof(RectTransform))]
public class PointsUI : MonoBehaviour
{
    #region Constants

    private const string LOG_PREFIX = "[PointsUI]";

    #endregion

    #region Serialized Fields

    [Header("UI References")]
    [Tooltip("The TextMeshProUGUI component used to display the points count.")]
    [SerializeField] private TextMeshProUGUI pointsText;

    [Header("Display Configuration")]
    [Tooltip("Format string for the points display. {0} is replaced with the current total.")]
    [SerializeField] private string format = "Points: {0}";

    [Header("Color Configuration")]
    [Tooltip("Default text color during normal display.")]
    [SerializeField] private Color normalColor = Color.white;

    [Tooltip("Flash color applied briefly when points are gained.")]
    [SerializeField] private Color gainColor = Color.yellow;

    [Tooltip("Duration in seconds for the gain flash effect to lerp back to normal.")]
    [SerializeField] private float flashDuration = 0.5f;

    #endregion

    #region Private Fields

    private Coroutine flashCoroutine;

    #endregion

    #region Unity Lifecycle

    private void Awake()
    {
        ValidateTextComponent();
    }

    private void Start()
    {
        if (pointsText == null) return;

        Inventory inventory = FindFirstObjectByType<Inventory>();
        if (inventory != null)
        {
            UpdateDisplay(inventory.CurrentPoints);
        }
        else
        {
            UpdateDisplay(0);
            Debug.LogWarning($"{LOG_PREFIX} Inventory not found in scene at Start. " +
                             "Displaying default value.");
        }
    }

    private void OnEnable()
    {
        SubscribeToEvents();
    }

    private void OnDisable()
    {
        UnsubscribeFromEvents();
        StopFlash();
    }

    #endregion

    #region Event Subscription

    /// <summary>
    /// Subscribes to all relevant <see cref="Inventory"/> static events.
    /// Called in <see cref="OnEnable"/> so the UI reconnects after being toggled.
    /// </summary>
    private void SubscribeToEvents()
    {
        Inventory.OnPointsChanged += HandlePointsChanged;
        Inventory.OnPointsGained += HandlePointsGained;
    }

    /// <summary>
    /// Unsubscribes from all <see cref="Inventory"/> static events.
    /// Called in <see cref="OnDisable"/> to prevent stale references and memory leaks.
    /// </summary>
    private void UnsubscribeFromEvents()
    {
        Inventory.OnPointsChanged -= HandlePointsChanged;
        Inventory.OnPointsGained -= HandlePointsGained;
    }

    #endregion

    #region Event Handlers

    /// <summary>
    /// Handles point total changes from <see cref="Inventory.OnPointsChanged"/>.
    /// Updates the displayed text to reflect the new total.
    /// </summary>
    /// <param name="currentTotal">The player's new point total.</param>
    private void HandlePointsChanged(int currentTotal)
    {
        UpdateDisplay(currentTotal);
    }

    /// <summary>
    /// Handles point gain events from <see cref="Inventory.OnPointsGained"/>.
    /// Triggers the flash effect to provide visual feedback on gain.
    /// </summary>
    /// <param name="gained">The number of points gained.</param>
    /// <param name="newTotal">The player's new point total after the gain.</param>
    private void HandlePointsGained(int gained, int newTotal)
    {
        StartFlash();
    }

    #endregion

    #region Display Logic

    /// <summary>
    /// Updates the points text with the formatted current total and applies the normal color
    /// if no flash is in progress.
    /// </summary>
    /// <param name="points">The current point total to display.</param>
    private void UpdateDisplay(int points)
    {
        if (pointsText == null) return;

        pointsText.text = string.Format(format, points);
    }

    #endregion

    #region Flash Effect

    /// <summary>
    /// Initiates the gain flash effect. Cancels any in-progress flash before starting a new one.
    /// </summary>
    private void StartFlash()
    {
        if (pointsText == null) return;

        StopFlash();
        flashCoroutine = StartCoroutine(FlashRoutine());
    }

    /// <summary>
    /// Stops any in-progress flash coroutine and resets the text to its normal color.
    /// </summary>
    private void StopFlash()
    {
        if (flashCoroutine != null)
        {
            StopCoroutine(flashCoroutine);
            flashCoroutine = null;
        }

        if (pointsText != null)
        {
            pointsText.color = normalColor;
        }
    }

    /// <summary>
    /// Coroutine that sets the text to the gain color and lerps it back to normal over
    /// <see cref="flashDuration"/> seconds.
    /// </summary>
    private IEnumerator FlashRoutine()
    {
        pointsText.color = gainColor;

        float elapsed = 0f;

        while (elapsed < flashDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / flashDuration);
            pointsText.color = Color.Lerp(gainColor, normalColor, t);
            yield return null;
        }

        pointsText.color = normalColor;
        flashCoroutine = null;
    }

    #endregion

    #region Validation

    /// <summary>
    /// Validates that the required TMP text component is assigned.
    /// Logs an error and disables the component if missing.
    /// </summary>
    private void ValidateTextComponent()
    {
        if (pointsText != null) return;

        pointsText = GetComponent<TextMeshProUGUI>();

        if (pointsText == null)
        {
            Debug.LogError($"{LOG_PREFIX} No TextMeshProUGUI component assigned or found on " +
                           $"'{gameObject.name}'. Assign the pointsText field in the Inspector. " +
                           "Disabling PointsUI.");
            enabled = false;
        }
    }

    #endregion
}
