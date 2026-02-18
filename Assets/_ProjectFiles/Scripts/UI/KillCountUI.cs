using UnityEngine;
using TMPro;

/// <summary>
/// Displays the current kill count progress (e.g., "12/25").
/// Subscribes to WinConditionManager.OnKillCountChanged.
///
/// SETUP:
/// 1. Create TextMeshPro - Text (UI) element on HUD Canvas
/// 2. Attach this script to the text or a parent
/// 3. Assign the TMP_Text reference in Inspector
/// </summary>
public class KillCountUI : MonoBehaviour
{
    private const string LOG_PREFIX = "[KillCountUI]";

    [Header("References")]
    [Tooltip("TextMeshPro component to display kill count")]
    [SerializeField] private TMP_Text killText;

    [Header("Display Settings")]
    [Tooltip("Format string for kill count. {0} = current, {1} = required")]
    [SerializeField] private string format = "{0}/{1}";

    [Tooltip("Optional label prefix (e.g., 'Kills: ')")]
    [SerializeField] private string labelPrefix = "";

    [Header("Colors")]
    [SerializeField] private Color normalColor = Color.white;
    [SerializeField] private Color completeColor = Color.green;

    private void Awake()
    {
        if (killText == null)
        {
            killText = GetComponent<TMP_Text>();
        }

        if (killText == null)
        {
            Debug.LogError($"{LOG_PREFIX} No TMP_Text assigned or found. Disabling.");
            enabled = false;
        }
    }

    private void OnEnable()
    {
        WinConditionManager.OnKillCountChanged += HandleKillCountChanged;

        // Initialize with current values if WinConditionManager exists
        if (WinConditionManager.Instance != null)
        {
            UpdateDisplay(
                WinConditionManager.Instance.CurrentKills,
                WinConditionManager.Instance.RequiredKills
            );
        }
    }

    private void OnDisable()
    {
        WinConditionManager.OnKillCountChanged -= HandleKillCountChanged;
    }

    private void HandleKillCountChanged(int current, int required)
    {
        UpdateDisplay(current, required);
    }

    private void UpdateDisplay(int current, int required)
    {
        if (killText == null) return;

        killText.text = labelPrefix + string.Format(format, current, required);
        killText.color = current >= required ? completeColor : normalColor;
    }
}
