using System;
using UnityEngine;

/// <summary>
/// Manages win conditions for the game. Tracks kills and keys,
/// starts escape timer when conditions are met, and triggers victory/failure.
///
/// SETUP:
/// 1. Create empty GameObject named "WinConditionManager"
/// 2. Attach this script
/// 3. Configure required kills, keys, and timer duration in Inspector
/// </summary>
public class WinConditionManager : MonoBehaviour
{
    private const string LOG_PREFIX = "[WinConditionManager]";

    public static WinConditionManager Instance { get; private set; }

    [Header("Win Requirements")]
    [Tooltip("Number of zombies the player must kill")]
    [SerializeField] private int requiredKills = 25;

    [Tooltip("Number of keys the player must collect")]
    [SerializeField] private int requiredKeys = 3;

    [Header("Escape Timer")]
    [Tooltip("Time in seconds the player has to reach the vault door after conditions are met")]
    [SerializeField] private float escapeTimerDuration = 90f;

    // Events
    /// <summary>Fired when kill count changes. Parameters: current kills, required kills.</summary>
    public static event Action<int, int> OnKillCountChanged;

    /// <summary>Fired when key count changes. Parameters: current keys, required keys.</summary>
    public static event Action<int, int> OnKeyCountChanged;

    /// <summary>Fired when both kill and key requirements are met.</summary>
    public static event Action OnWinConditionsMet;

    /// <summary>Fired every frame while escape timer is running. Parameter: remaining seconds.</summary>
    public static event Action<float> OnEscapeTimerTick;

    /// <summary>Fired when escape timer reaches zero.</summary>
    public static event Action OnEscapeTimerExpired;

    /// <summary>Fired when player successfully opens the vault door.</summary>
    public static event Action OnVictory;

    // Runtime state
    private int currentKills;
    private int currentKeys;
    private bool conditionsMet;
    private bool timerRunning;
    private float timerRemaining;
    private bool victoryTriggered;

    /// <summary>Returns true if both kill and key requirements are met.</summary>
    public bool ConditionsMet => conditionsMet;

    /// <summary>Returns true if victory has been triggered.</summary>
    public bool IsVictory => victoryTriggered;

    /// <summary>Current kill count.</summary>
    public int CurrentKills => currentKills;

    /// <summary>Required kill count.</summary>
    public int RequiredKills => requiredKills;

    /// <summary>Current key count.</summary>
    public int CurrentKeys => currentKeys;

    /// <summary>Required key count.</summary>
    public int RequiredKeys => requiredKeys;

    private void Awake()
    {
        // Singleton setup
        if (Instance != null && Instance != this)
        {
            Debug.LogWarning($"{LOG_PREFIX} Duplicate WinConditionManager found. Destroying this one.");
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void OnEnable()
    {
        EnemyHealth.OnEnemyDeath += HandleEnemyDeath;
        PlayerKeys.OnKeyCollected += HandleKeyCollected;
        PlayerKeys.OnKeysCleared += HandleKeysCleared;
    }

    private void OnDisable()
    {
        EnemyHealth.OnEnemyDeath -= HandleEnemyDeath;
        PlayerKeys.OnKeyCollected -= HandleKeyCollected;
        PlayerKeys.OnKeysCleared -= HandleKeysCleared;
    }

    private void Start()
    {
        // Initialize key count from PlayerKeys in case keys were collected before we subscribed
        currentKeys = PlayerKeys.KeyCount;

        // Broadcast initial state
        OnKillCountChanged?.Invoke(currentKills, requiredKills);
        OnKeyCountChanged?.Invoke(currentKeys, requiredKeys);

        Debug.Log($"{LOG_PREFIX} Initialized. Required: {requiredKills} kills, {requiredKeys} keys. Escape time: {escapeTimerDuration}s");
    }

    private void Update()
    {
        if (!timerRunning || victoryTriggered) return;

        // Timer ticks with Time.deltaTime (pauses when Time.timeScale = 0)
        timerRemaining -= Time.deltaTime;
        OnEscapeTimerTick?.Invoke(timerRemaining);

        if (timerRemaining <= 0f)
        {
            timerRemaining = 0f;
            timerRunning = false;
            Debug.Log($"{LOG_PREFIX} Escape timer expired! Game Over.");
            OnEscapeTimerExpired?.Invoke();
        }
    }

    private void HandleEnemyDeath(EnemyHealth enemyHealth)
    {
        if (conditionsMet) return; // Stop counting after conditions are met

        currentKills++;
        Debug.Log($"{LOG_PREFIX} Kill count: {currentKills}/{requiredKills}");
        OnKillCountChanged?.Invoke(currentKills, requiredKills);

        CheckWinConditions();
    }

    private void HandleKeyCollected(int totalKeys)
    {
        if (conditionsMet) return; // Stop counting after conditions are met

        currentKeys = totalKeys;
        Debug.Log($"{LOG_PREFIX} Key count: {currentKeys}/{requiredKeys}");
        OnKeyCountChanged?.Invoke(currentKeys, requiredKeys);

        CheckWinConditions();
    }

    private void HandleKeysCleared()
    {
        currentKeys = 0;
        OnKeyCountChanged?.Invoke(currentKeys, requiredKeys);
    }

    private void CheckWinConditions()
    {
        if (conditionsMet) return;

        if (currentKills >= requiredKills && currentKeys >= requiredKeys)
        {
            conditionsMet = true;
            Debug.Log($"{LOG_PREFIX} WIN CONDITIONS MET! Starting escape timer ({escapeTimerDuration}s)");

            OnWinConditionsMet?.Invoke();
            StartEscapeTimer();
        }
    }

    private void StartEscapeTimer()
    {
        timerRemaining = escapeTimerDuration;
        timerRunning = true;
        OnEscapeTimerTick?.Invoke(timerRemaining);
    }

    /// <summary>
    /// Called by VaultDoor when player successfully opens it.
    /// Triggers victory sequence.
    /// </summary>
    public void TriggerVictory()
    {
        if (victoryTriggered) return;
        if (!conditionsMet)
        {
            Debug.LogWarning($"{LOG_PREFIX} TriggerVictory called but conditions not met!");
            return;
        }

        victoryTriggered = true;
        timerRunning = false;
        Debug.Log($"{LOG_PREFIX} VICTORY! Player escaped with {timerRemaining:F1}s remaining.");
        OnVictory?.Invoke();
    }

    /// <summary>
    /// Resets the win condition state. Call on game restart.
    /// </summary>
    public void ResetState()
    {
        currentKills = 0;
        currentKeys = 0;
        conditionsMet = false;
        timerRunning = false;
        timerRemaining = 0f;
        victoryTriggered = false;

        OnKillCountChanged?.Invoke(currentKills, requiredKills);
        OnKeyCountChanged?.Invoke(currentKeys, requiredKeys);

        Debug.Log($"{LOG_PREFIX} State reset.");
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }
}
