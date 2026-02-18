using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Synty.AnimationBaseLocomotion.Samples.InputSystem;

/// <summary>
/// Toggles pause state when the player presses Escape (via <see cref="InputReader.onPausePerformed"/>).
/// Shows/hides a pause panel, freezes/unfreezes time, and manages cursor lock state.
///
/// <para><b>Setup:</b></para>
/// <list type="number">
///   <item>Create a UI Panel on the HUD Canvas for the pause menu.</item>
///   <item>Disable the panel GameObject by default (SetActive false).</item>
///   <item>Attach this script to any persistent GameObject (e.g., the player or a manager).</item>
///   <item>Assign the pause panel and button references in the Inspector.</item>
/// </list>
/// </summary>
public class PauseManager : MonoBehaviour
{
    private const string LOG_PREFIX = "[PauseManager]";

    [Header("UI References")]
    [Tooltip("The pause menu panel. Must be disabled by default in the scene.")]
    [SerializeField] private GameObject pausePanel;

    [Header("Buttons")]
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button mainMenuButton;
    [SerializeField] private Button exitButton;

    [Header("Settings")]
    [Tooltip("Lock the cursor and hide it when the game resumes.")]
    [SerializeField] private bool lockCursorOnResume = true;

    [SerializeField] private string mainMenuSceneName = "MainMenu";

    private InputReader inputReader;
    private bool isPaused;

    /// <summary>
    /// Static accessor so other systems (e.g. <see cref="GameOverScreen"/>) can query pause state.
    /// </summary>
    public static bool IsPaused { get; private set; }

    private void OnEnable()
    {
        if (inputReader == null)
        {
            inputReader = GetComponentInParent<InputReader>();
            if (inputReader == null)
                inputReader = FindFirstObjectByType<InputReader>();
        }

        if (inputReader != null)
            inputReader.onPausePerformed += HandlePause;
        else
            Debug.LogWarning($"{LOG_PREFIX} No InputReader found. Pause input will not work.");
    }

    private void OnDisable()
    {
        if (inputReader != null)
            inputReader.onPausePerformed -= HandlePause;
    }

    private void Start()
    {
        if (pausePanel != null)
            pausePanel.SetActive(false);

        isPaused = false;
        IsPaused = false;

        // Wire up button listeners
        if (resumeButton != null)
            resumeButton.onClick.AddListener(Resume);
        if (mainMenuButton != null)
            mainMenuButton.onClick.AddListener(HandleMainMenu);
        if (exitButton != null)
            exitButton.onClick.AddListener(HandleExit);
    }

    private void HandlePause()
    {
        // Don't allow pause toggle if the game is already over.
        if (GameOverScreen.IsGameOver) return;

        if (isPaused)
            Resume();
        else
            Pause();
    }

    /// <summary>
    /// Pauses the game: freezes time, shows the pause panel, and unlocks the cursor.
    /// </summary>
    public void Pause()
    {
        isPaused = true;
        IsPaused = true;
        Time.timeScale = 0f;

        if (pausePanel != null)
            pausePanel.SetActive(true);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        Debug.Log($"{LOG_PREFIX} Game paused.");
    }

    /// <summary>
    /// Resumes the game: restores time, hides the pause panel, and re-locks the cursor.
    /// Can be called from a UI button's OnClick event.
    /// </summary>
    public void Resume()
    {
        isPaused = false;
        IsPaused = false;
        Time.timeScale = 1f;

        if (pausePanel != null)
            pausePanel.SetActive(false);

        if (lockCursorOnResume)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        Debug.Log($"{LOG_PREFIX} Game resumed.");
    }

    public void HandleMainMenu()
    {
        Time.timeScale = 1f;
        isPaused = false;
        IsPaused = false;
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
        if (isPaused)
        {
            IsPaused = false;
            Time.timeScale = 1f;
        }

        // Remove button listeners
        if (resumeButton != null)
            resumeButton.onClick.RemoveListener(Resume);
        if (mainMenuButton != null)
            mainMenuButton.onClick.RemoveListener(HandleMainMenu);
        if (exitButton != null)
            exitButton.onClick.RemoveListener(HandleExit);
    }
}
