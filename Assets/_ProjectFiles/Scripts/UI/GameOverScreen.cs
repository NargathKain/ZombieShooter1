using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Displays a Game Over screen when the player dies.
/// Subscribes to <see cref="PlayerHealth.OnPlayerDeath"/> and freezes the game
/// by setting Time.timeScale to 0. Shows a full-screen Game Over image/panel.
///
/// <para><b>Setup:</b></para>
/// <list type="number">
///   <item>Create a UI Panel on the HUD Canvas covering the full screen.</item>
///   <item>Add a "GAME OVER" Image or Text as a child.</item>
///   <item>Disable the panel GameObject by default (SetActive false).</item>
///   <item>Attach this script to the panel or any persistent GameObject.</item>
///   <item>Assign the panel reference in the Inspector.</item>
/// </list>
/// </summary>
public class GameOverScreen : MonoBehaviour
{
    private const string LOG_PREFIX = "[GameOverScreen]";

    [Header("UI References")]
    [Tooltip("The Game Over panel/image. Must be disabled by default in the scene.")]
    [SerializeField] private GameObject gameOverPanel;

    [Tooltip("Optional restart button. Assign a UI Button in the Inspector.")]
    [SerializeField] private Button restartButton;

    [SerializeField] private Button mainMenuButton;
    [SerializeField] private Button exitButton;

    [Header("Settings")]
    [SerializeField] private string mainMenuSceneName = "MainMenu";
    [Tooltip("Show the mouse cursor on game over so the player can click buttons.")]
    [SerializeField] private bool showCursor = true;

    private bool isGameOver;

    /// <summary>
    /// Static accessor so other systems (e.g. <see cref="PauseManager"/>) can check
    /// whether the game is over and avoid conflicting state changes.
    /// </summary>
    public static bool IsGameOver { get; private set; }

    private void OnEnable()
    {
        PlayerHealth.OnPlayerDeath += HandlePlayerDeath;
        PlayerHealth.OnPlayerRespawn += HandlePlayerRespawn;
        WinConditionManager.OnEscapeTimerExpired += HandleTimerExpired;
    }

    private void OnDisable()
    {
        PlayerHealth.OnPlayerDeath -= HandlePlayerDeath;
        PlayerHealth.OnPlayerRespawn -= HandlePlayerRespawn;
        WinConditionManager.OnEscapeTimerExpired -= HandleTimerExpired;
    }

    private void Start()
    {
        if (gameOverPanel == null)
        {
            Debug.LogError($"{LOG_PREFIX} gameOverPanel not assigned. Assign the Game Over UI panel in the Inspector.");
            return;
        }

        gameOverPanel.SetActive(false);
        isGameOver = false;

        // Wire up button listeners
        if (restartButton != null)
            restartButton.onClick.AddListener(HandleRestart);
        if (mainMenuButton != null)
            mainMenuButton.onClick.AddListener(HandleMainMenu);
        if (exitButton != null)
            exitButton.onClick.AddListener(HandleExit);
    }

    private void HandlePlayerDeath()
    {
        ShowGameOver();
    }

    private void HandleTimerExpired()
    {
        Debug.Log($"{LOG_PREFIX} Escape timer expired - showing Game Over.");
        ShowGameOver();
    }

    /// <summary>
    /// Shows the game over screen. Can be called directly as a fallback
    /// if the event subscription fails (e.g., if this GameObject was disabled).
    /// </summary>
    public void ShowGameOver()
    {
        if (isGameOver) return;

        isGameOver = true;
        IsGameOver = true;

        Time.timeScale = 0f;

        if (gameOverPanel != null)
            gameOverPanel.SetActive(true);
        else
            Debug.LogError($"{LOG_PREFIX} gameOverPanel is not assigned! Assign the Game Over UI panel in the Inspector.");

        if (showCursor)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        Debug.Log($"{LOG_PREFIX} Game Over.");
    }

    private void HandlePlayerRespawn()
    {
        if (!isGameOver) return;

        isGameOver = false;
        IsGameOver = false;

        Time.timeScale = 1f;

        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);

        Debug.Log($"{LOG_PREFIX} Game resumed after respawn.");
    }

    /// <summary>
    /// Called when the restart button is clicked. Reloads the current scene.
    /// Uses Time.timeScale = 1 first since scene loading doesn't work at timeScale 0.
    /// </summary>
    public void HandleRestart()
    {
        Time.timeScale = 1f;
        isGameOver = false;
        IsGameOver = false;
        PlayerKeys.Clear();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Debug.Log($"{LOG_PREFIX} Restarting scene.");
    }

    public void HandleMainMenu()
    {
        Time.timeScale = 1f;
        isGameOver = false;
        IsGameOver = false;
        PlayerKeys.Clear();
        Debug.Log($"{LOG_PREFIX} Loading Main Menu: {mainMenuSceneName}");
        SceneManager.LoadScene(mainMenuSceneName);
    }

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
        IsGameOver = false;

        if (restartButton != null)
            restartButton.onClick.RemoveListener(HandleRestart);
        if (mainMenuButton != null)
            mainMenuButton.onClick.RemoveListener(HandleMainMenu);
        if (exitButton != null)
            exitButton.onClick.RemoveListener(HandleExit);
    }
}
