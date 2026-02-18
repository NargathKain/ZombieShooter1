using UnityEngine;
using TMPro;

/// <summary>
/// Displays the escape countdown timer when win conditions are met.
/// Hidden by default, shows when OnWinConditionsMet fires.
///
/// SETUP:
/// 1. Create a Panel on HUD Canvas (center-top recommended)
/// 2. Add TextMeshPro child for timer display
/// 3. Disable the panel by default in Inspector
/// 4. Attach this script to the panel or a parent
/// 5. Assign references in Inspector
/// </summary>
public class EscapeTimerUI : MonoBehaviour
{
    private const string LOG_PREFIX = "[EscapeTimerUI]";

    [Header("References")]
    [Tooltip("Panel/container to show/hide")]
    [SerializeField] private GameObject timerPanel;

    [Tooltip("TextMeshPro component to display countdown")]
    [SerializeField] private TMP_Text timerText;

    [Header("Display Settings")]
    [Tooltip("Message shown above or before the timer")]
    [SerializeField] private string escapeMessage = "ESCAPE NOW!";

    [Tooltip("Show message with timer (true) or just timer (false)")]
    [SerializeField] private bool showMessage = true;

    [Header("Colors")]
    [SerializeField] private Color normalColor = Color.white;
    [SerializeField] private Color warningColor = Color.red;

    [Tooltip("Time remaining (in seconds) when warning color activates")]
    [SerializeField] private float warningThreshold = 10f;

    [Header("Warning Flash")]
    [Tooltip("Enable flashing effect when time is low")]
    [SerializeField] private bool enableFlash = true;

    [Tooltip("Flash speed in flashes per second")]
    [SerializeField] private float flashSpeed = 2f;

    private bool isWarning;

    private void Awake()
    {
        if (timerText == null)
        {
            timerText = GetComponentInChildren<TMP_Text>();
        }

        if (timerText == null)
        {
            Debug.LogError($"{LOG_PREFIX} No TMP_Text assigned or found. Disabling.");
            enabled = false;
            return;
        }

        // Ensure panel is hidden at start
        if (timerPanel != null)
        {
            timerPanel.SetActive(false);
        }
    }

    private void OnEnable()
    {
        WinConditionManager.OnWinConditionsMet += HandleConditionsMet;
        WinConditionManager.OnEscapeTimerTick += HandleTimerTick;
        WinConditionManager.OnEscapeTimerExpired += HandleTimerExpired;
        WinConditionManager.OnVictory += HandleVictory;
    }

    private void OnDisable()
    {
        WinConditionManager.OnWinConditionsMet -= HandleConditionsMet;
        WinConditionManager.OnEscapeTimerTick -= HandleTimerTick;
        WinConditionManager.OnEscapeTimerExpired -= HandleTimerExpired;
        WinConditionManager.OnVictory -= HandleVictory;
    }

    private void HandleConditionsMet()
    {
        Debug.Log($"{LOG_PREFIX} Conditions met - showing escape timer");
        if (timerPanel != null)
        {
            timerPanel.SetActive(true);
        }
    }

    private void HandleTimerTick(float remaining)
    {
        if (timerText == null) return;

        // Format as MM:SS
        int minutes = Mathf.FloorToInt(remaining / 60f);
        int seconds = Mathf.FloorToInt(remaining % 60f);
        string timeString = string.Format("{0:00}:{1:00}", minutes, seconds);

        if (showMessage)
        {
            timerText.text = $"{escapeMessage}\n{timeString}";
        }
        else
        {
            timerText.text = timeString;
        }

        // Handle warning state
        isWarning = remaining <= warningThreshold;

        if (isWarning && enableFlash)
        {
            // Flash between normal and warning color
            float flash = Mathf.PingPong(Time.unscaledTime * flashSpeed, 1f);
            timerText.color = Color.Lerp(normalColor, warningColor, flash);
        }
        else
        {
            timerText.color = isWarning ? warningColor : normalColor;
        }
    }

    private void HandleTimerExpired()
    {
        // Timer expired - panel will be hidden by GameOverScreen taking over
        Debug.Log($"{LOG_PREFIX} Timer expired");
    }

    private void HandleVictory()
    {
        // Hide timer on victory
        if (timerPanel != null)
        {
            timerPanel.SetActive(false);
        }
    }
}
