using UnityEngine;

/// <summary>
/// Resets all persistent game state when the Game scene loads.
/// Ensures a fresh start when coming from Main Menu or restarting.
///
/// SETUP:
/// 1. Create empty GameObject named "GameInitializer" in Game scene
/// 2. Attach this script
/// 3. Make sure it runs before other game scripts (default is fine)
/// </summary>
public class GameInitializer : MonoBehaviour
{
    private const string LOG_PREFIX = "[GameInitializer]";

    private void Awake()
    {
        ResetGameState();
    }

    private void ResetGameState()
    {
        // Reset collected keys (static class persists between scenes)
        PlayerKeys.Clear();

        // Ensure time is running (may be paused from Game Over or Pause menu)
        Time.timeScale = 1f;

        Debug.Log($"{LOG_PREFIX} Game state reset. Keys cleared, time resumed.");
    }
}
