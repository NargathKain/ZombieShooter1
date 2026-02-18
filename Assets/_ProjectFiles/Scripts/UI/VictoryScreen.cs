using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Displays a Victory screen when the player successfully escapes.
/// Subscribes to <see cref="WinConditionManager.OnVictory"/> and freezes the game
/// by setting Time.timeScale to 0. Shows a full-screen Victory image/panel.
///
/// <para><b>Setup:</b></para>
/// <list type="number">
///   <item>Create a UI Panel on the HUD Canvas covering the full screen.</item>
///   <item>Add a "VICTORY" Image or Text as a child.</item>
///   <item>Add "Main Menu" and "Exit" buttons as children.</item>
///   <item>Disable the panel GameObject by default (SetActive false).</item>
///   <item>Attach this script to the panel or any persistent GameObject.</item>
///   <item>Assign the panel and button references in the Inspector.</item>
/// </list>
/// </summary>
public class VictoryScreen : MonoBehaviour
{
    private const string LOG_PREFIX = "[VictoryScreen]";

    [Header("UI References")]
    [Tooltip("The Victory panel/image. Must be disabled by default in the scene.")]
    [SerializeField] private GameObject victoryPanel;

    [Tooltip("Button to return to Main Menu.")]
    [SerializeField] private Button mainMenuButton;

    [Tooltip("Button to exit the game.")]
    [SerializeField] private Button exitButton;

    [Header("Settings")]
    [Tooltip("Scene name to load when Main Menu button is clicked.")]
    [SerializeField] private string mainMenuSceneName = "MainMenu";

    [Tooltip("Show the mouse cursor on victory so the player can click buttons.")]
    [SerializeField] private bool showCursor = true;

    private bool isVictory;

    /// <summary>
    /// Static accessor so other systems can check whether victory has been achieved.
    /// </summary>
    public static bool IsVictory { get; private set; }

    private void OnEnable()
    {
        WinConditionManager.OnVictory += HandleVictory;
        WinConditionManager.OnEscapeTimerExpired += HandleTimerExpired;
    }

    private void OnDisable()
    {
        WinConditionManager.OnVictory -= HandleVictory;
        WinConditionManager.OnEscapeTimerExpired -= HandleTimerExpired;
    }

    private void Start()
    {
        if (victoryPanel == null)
        {
            Debug.LogError($"{LOG_PREFIX} victoryPanel not assigned. Assign the Victory UI panel in the Inspector.");
            return;
        }

        victoryPanel.SetActive(false);
        isVictory = false;

        // Wire up button listeners
        if (mainMenuButton != null)
            mainMenuButton.onClick.AddListener(HandleMainMenu);

        if (exitButton != null)
            exitButton.onClick.AddListener(HandleExit);
    }

    private void HandleVictory()
    {
        ShowVictory();
    }

    private void HandleTimerExpired()
    {
        // Timer expired = Game Over, handled by GameOverScreen
        // We just need to make sure we don't show victory
        Debug.Log($"{LOG_PREFIX} Timer expired - deferring to GameOverScreen.");
    }

    /// <summary>
    /// Shows the victory screen. Can be called directly as a fallback.
    /// </summary>
    public void ShowVictory()
    {
        if (isVictory) return;

        isVictory = true;
        IsVictory = true;

        Time.timeScale = 0f;

        if (victoryPanel != null)
            victoryPanel.SetActive(true);
        else
            Debug.LogError($"{LOG_PREFIX} victoryPanel is not assigned!");

        if (showCursor)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        Debug.Log($"{LOG_PREFIX} VICTORY! Player escaped successfully.");
    }

    /// <summary>
    /// Called when the Main Menu button is clicked. Loads the main menu scene.
    /// </summary>
    public void HandleMainMenu()
    {
        Time.timeScale = 1f;
        isVictory = false;
        IsVictory = false;

        // Clear keys for fresh start
        PlayerKeys.Clear();

        Debug.Log($"{LOG_PREFIX} Loading Main Menu: {mainMenuSceneName}");
        SceneManager.LoadScene(mainMenuSceneName);
    }

    /// <summary>
    /// Called when the Exit button is clicked. Quits the application.
    /// </summary>
    public void HandleExit()
    {
        Debug.Log($"{LOG_PREFIX} Exiting game.");

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    private void OnDestroy()
    {
        // Clean up static state so scene reloads start fresh.
        IsVictory = false;

        if (mainMenuButton != null)
            mainMenuButton.onClick.RemoveListener(HandleMainMenu);

        if (exitButton != null)
            exitButton.onClick.RemoveListener(HandleExit);
    }
}
