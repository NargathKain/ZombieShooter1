using UnityEngine;
using TMPro;

/// <summary>
/// Displays the current key count progress (e.g., "2/3").
/// Subscribes to WinConditionManager.OnKeyCountChanged.
///
/// SETUP:
/// 1. Create TextMeshPro - Text (UI) element on HUD Canvas
/// 2. Attach this script to the text or a parent
/// 3. Assign the TMP_Text reference in Inspector
/// </summary>
public class KeyCountUI : MonoBehaviour
{
    private const string LOG_PREFIX = "[KeyCountUI]";

    [Header("References")]
    [Tooltip("TextMeshPro component to display key count")]
    [SerializeField] private TMP_Text keyText;

    [Header("Display Settings")]
    [Tooltip("Format string for key count. {0} = current, {1} = required")]
    [SerializeField] private string format = "{0}/{1}";

    [Tooltip("Optional label prefix (e.g., 'Keys: ')")]
    [SerializeField] private string labelPrefix = "";

    [Header("Colors")]
    [SerializeField] private Color normalColor = Color.white;
    [SerializeField] private Color completeColor = Color.green;

    private void Awake()
    {
        if (keyText == null)
        {
            keyText = GetComponent<TMP_Text>();
        }

        if (keyText == null)
        {
            Debug.LogError($"{LOG_PREFIX} No TMP_Text assigned or found. Disabling.");
            enabled = false;
        }
    }

    private void OnEnable()
    {
        WinConditionManager.OnKeyCountChanged += HandleKeyCountChanged;

        // Initialize with current values if WinConditionManager exists
        if (WinConditionManager.Instance != null)
        {
            UpdateDisplay(
                WinConditionManager.Instance.CurrentKeys,
                WinConditionManager.Instance.RequiredKeys
            );
        }
    }

    private void OnDisable()
    {
        WinConditionManager.OnKeyCountChanged -= HandleKeyCountChanged;
    }

    private void HandleKeyCountChanged(int current, int required)
    {
        UpdateDisplay(current, required);
    }

    private void UpdateDisplay(int current, int required)
    {
        if (keyText == null) return;

        keyText.text = labelPrefix + string.Format(format, current, required);
        keyText.color = current >= required ? completeColor : normalColor;
    }
}
