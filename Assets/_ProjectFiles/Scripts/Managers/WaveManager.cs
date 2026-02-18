using System;
using System.Collections;
using UnityEngine;

/// <summary>
/// Spawns enemies in configurable waves. Tracks alive enemies per wave and
/// automatically starts the next wave when all enemies in the current wave are dead.
/// Broadcasts static events so UI systems can display wave number, enemy count, etc.
///
/// <para><b>Setup:</b></para>
/// <list type="number">
///   <item>Create an empty GameObject named "WaveManager".</item>
///   <item>Attach this script.</item>
///   <item>Configure waves in the Inspector (each wave has a list of EnemySpawnEntry).</item>
///   <item>Create empty GameObjects in the scene as spawn points and assign them to <see cref="spawnPoints"/>.</item>
///   <item>If no spawn points are assigned, enemies spawn near the WaveManager with a random offset.</item>
/// </list>
///
/// <para><b>Events (static):</b></para>
/// <list type="bullet">
///   <item><see cref="OnWaveStarted"/>: (waveNumber 1-based, totalWaves) -- fires when a wave begins spawning.</item>
///   <item><see cref="OnWaveCompleted"/>: (waveNumber 1-based) -- fires when all enemies in a wave are dead.</item>
///   <item><see cref="OnAllWavesCompleted"/>: fires once after the final wave is cleared.</item>
///   <item><see cref="OnEnemyCountChanged"/>: (remainingEnemies) -- fires every time an enemy spawns or dies.</item>
/// </list>
///
/// <para><b>Dependencies:</b></para>
/// <list type="bullet">
///   <item><see cref="EnemyData"/> -- ScriptableObject providing the prefab and name for each enemy type.</item>
///   <item><see cref="EnemyHealth"/> -- Listens to <c>EnemyHealth.OnEnemyDeath</c> to track kills.</item>
/// </list>
/// </summary>
public class WaveManager : MonoBehaviour
{
    private const string LOG_PREFIX = "[WaveManager]";

    // ------------------------------------------------------------------ //
    //  Serializable data classes
    // ------------------------------------------------------------------ //

    /// <summary>
    /// Defines a single wave: which enemies to spawn and how many of each.
    /// </summary>
    [Serializable]
    public class Wave
    {
        [Tooltip("Display name shown in logs and optionally in UI.")]
        public string waveName = "Wave";

        [Tooltip("Enemy types and quantities to spawn this wave.")]
        public EnemySpawnEntry[] enemies;

        [Tooltip("Seconds to wait before this wave starts spawning (countdown / breather time).")]
        public float delayBeforeWave = 5f;
    }

    /// <summary>
    /// Pairs an <see cref="EnemyData"/> asset with a spawn count.
    /// </summary>
    [Serializable]
    public class EnemySpawnEntry
    {
        [Tooltip("EnemyData ScriptableObject that contains the prefab to spawn.")]
        public EnemyData enemyData;

        [Tooltip("Number of this enemy type to spawn in the wave.")]
        [Min(0)]
        public int count = 3;
    }

    // ------------------------------------------------------------------ //
    //  Inspector fields
    // ------------------------------------------------------------------ //

    [Header("Wave Configuration")]
    [Tooltip("Ordered list of waves. Each wave defines which enemies to spawn.")]
    [SerializeField] private Wave[] waves;

    [Header("Spawn Settings")]
    [Tooltip("Empty transforms in the scene where enemies can appear. " +
             "A random one is chosen for each spawn. " +
             "If empty, enemies spawn near the WaveManager with a random offset.")]
    [SerializeField] private Transform[] spawnPoints;

    [Tooltip("Seconds between individual enemy spawns within a single wave.")]
    [SerializeField] private float spawnInterval = 0.5f;

    [Header("Auto Start")]
    [Tooltip("If true, the first wave starts automatically after initialDelay.")]
    [SerializeField] private bool autoStartFirstWave = true;

    [Tooltip("Seconds to wait after Start() before the first wave begins.")]
    [SerializeField] private float initialDelay = 3f;

    // ------------------------------------------------------------------ //
    //  Static events
    // ------------------------------------------------------------------ //

    /// <summary>Fires when a wave begins spawning. Args: (waveNumber 1-based, totalWaves).</summary>
    public static event Action<int, int> OnWaveStarted;

    /// <summary>Fires when every enemy in a wave has been killed. Arg: waveNumber 1-based.</summary>
    public static event Action<int> OnWaveCompleted;

    /// <summary>Fires once after the last wave is cleared.</summary>
    public static event Action OnAllWavesCompleted;

    /// <summary>Fires whenever the alive-enemy count changes. Arg: remaining enemies.</summary>
    public static event Action<int> OnEnemyCountChanged;

    // ------------------------------------------------------------------ //
    //  Runtime state
    // ------------------------------------------------------------------ //

    private int currentWaveIndex = -1;
    private int aliveEnemies;
    private bool isSpawning;
    private bool allWavesComplete;
    private Coroutine activeSpawnCoroutine;

    // ------------------------------------------------------------------ //
    //  Public read-only accessors
    // ------------------------------------------------------------------ //

    /// <summary>Current wave number (1-based). 0 if no wave has started.</summary>
    public int CurrentWave => currentWaveIndex + 1;

    /// <summary>Total number of configured waves.</summary>
    public int TotalWaves => waves != null ? waves.Length : 0;

    /// <summary>Number of enemies still alive in the current wave.</summary>
    public int AliveEnemies => aliveEnemies;

    /// <summary>True while the coroutine is actively spawning enemies.</summary>
    public bool IsSpawning => isSpawning;

    /// <summary>True after every wave has been completed.</summary>
    public bool AllWavesComplete => allWavesComplete;

    // ------------------------------------------------------------------ //
    //  Unity lifecycle
    // ------------------------------------------------------------------ //

    private void OnEnable()
    {
        EnemyHealth.OnEnemyDeath += HandleEnemyDeath;
    }

    private void OnDisable()
    {
        EnemyHealth.OnEnemyDeath -= HandleEnemyDeath;
    }

    private void Start()
    {
        // Validate configuration early so designers get clear feedback.
        if (waves == null || waves.Length == 0)
        {
            Debug.LogWarning($"{LOG_PREFIX} No waves configured. Nothing will spawn.");
            return;
        }

        if (spawnPoints == null || spawnPoints.Length == 0)
        {
            Debug.LogWarning($"{LOG_PREFIX} No spawn points assigned. " +
                             "Enemies will spawn near the WaveManager position with a random offset.");
        }

        if (autoStartFirstWave)
        {
            activeSpawnCoroutine = StartCoroutine(StartFirstWaveAfterDelay());
        }
    }

    private void OnDestroy()
    {
        // Stop any running coroutine so we don't get callbacks after destruction.
        if (activeSpawnCoroutine != null)
        {
            StopCoroutine(activeSpawnCoroutine);
            activeSpawnCoroutine = null;
        }

        // Clean up static event subscribers to avoid leaks on scene reload.
        // (Each subscriber is responsible for unsubscribing, but we clear our own
        //  handler reference defensively.)
    }

    // ------------------------------------------------------------------ //
    //  Public API
    // ------------------------------------------------------------------ //

    /// <summary>
    /// Starts the next wave. Safe to call from UI buttons or other systems.
    /// No-op if currently spawning or all waves are already complete.
    /// </summary>
    public void StartNextWave()
    {
        if (allWavesComplete)
        {
            Debug.Log($"{LOG_PREFIX} All waves already completed. Ignoring StartNextWave().");
            return;
        }

        if (isSpawning)
        {
            Debug.LogWarning($"{LOG_PREFIX} Cannot start next wave while still spawning.");
            return;
        }

        currentWaveIndex++;

        if (currentWaveIndex >= waves.Length)
        {
            allWavesComplete = true;
            Debug.Log($"{LOG_PREFIX} All {waves.Length} waves completed!");
            OnAllWavesCompleted?.Invoke();
            return;
        }

        Wave wave = waves[currentWaveIndex];
        if (wave == null)
        {
            Debug.LogError($"{LOG_PREFIX} Wave {currentWaveIndex + 1} is null. Skipping.");
            StartNextWave();
            return;
        }

        activeSpawnCoroutine = StartCoroutine(SpawnWaveCoroutine(wave));
    }

    // ------------------------------------------------------------------ //
    //  Coroutines
    // ------------------------------------------------------------------ //

    private IEnumerator StartFirstWaveAfterDelay()
    {
        yield return new WaitForSeconds(initialDelay);
        StartNextWave();
    }

    private IEnumerator SpawnWaveCoroutine(Wave wave)
    {
        isSpawning = true;

        Debug.Log($"{LOG_PREFIX} Wave {currentWaveIndex + 1}/{waves.Length}: " +
                  $"\"{wave.waveName}\" starting in {wave.delayBeforeWave}s");

        if (wave.delayBeforeWave > 0f)
        {
            yield return new WaitForSeconds(wave.delayBeforeWave);
        }

        // Broadcast wave start.
        OnWaveStarted?.Invoke(currentWaveIndex + 1, waves.Length);
        Debug.Log($"{LOG_PREFIX} Wave {currentWaveIndex + 1} spawning!");

        aliveEnemies = 0;

        // Spawn each entry in the wave.
        if (wave.enemies != null)
        {
            foreach (EnemySpawnEntry entry in wave.enemies)
            {
                if (entry == null || entry.enemyData == null)
                {
                    Debug.LogWarning($"{LOG_PREFIX} Wave \"{wave.waveName}\" contains a null spawn entry. Skipping.");
                    continue;
                }

                if (entry.enemyData.enemyPrefab == null)
                {
                    Debug.LogWarning($"{LOG_PREFIX} EnemyData \"{entry.enemyData.enemyName}\" has no prefab assigned. Skipping.");
                    continue;
                }

                for (int i = 0; i < entry.count; i++)
                {
                    SpawnEnemy(entry.enemyData);
                    aliveEnemies++;
                    OnEnemyCountChanged?.Invoke(aliveEnemies);

                    if (spawnInterval > 0f)
                    {
                        yield return new WaitForSeconds(spawnInterval);
                    }
                }
            }
        }

        isSpawning = false;
        activeSpawnCoroutine = null;

        // Edge case: if the wave contained no valid enemies (all entries null / no prefabs),
        // immediately mark it as completed and proceed.
        if (aliveEnemies == 0)
        {
            Debug.LogWarning($"{LOG_PREFIX} Wave {currentWaveIndex + 1} had no valid enemies to spawn.");
            OnWaveCompleted?.Invoke(currentWaveIndex + 1);
            StartNextWave();
        }
    }

    // ------------------------------------------------------------------ //
    //  Spawning
    // ------------------------------------------------------------------ //

    private void SpawnEnemy(EnemyData enemyData)
    {
        Vector3 spawnPos = GetRandomSpawnPosition();
        Quaternion spawnRot = Quaternion.Euler(0f, UnityEngine.Random.Range(0f, 360f), 0f);

        GameObject enemy = Instantiate(enemyData.enemyPrefab, spawnPos, spawnRot);

        if (enemy == null)
        {
            Debug.LogError($"{LOG_PREFIX} Instantiate returned null for \"{enemyData.enemyName}\". " +
                           "Check that the prefab is valid.");
            return;
        }

        Debug.Log($"{LOG_PREFIX} Spawned \"{enemyData.enemyName}\" at {spawnPos}");
    }

    private Vector3 GetRandomSpawnPosition()
    {
        if (spawnPoints != null && spawnPoints.Length > 0)
        {
            // Pick a random spawn point, skipping any that may have been destroyed.
            int startIndex = UnityEngine.Random.Range(0, spawnPoints.Length);
            for (int i = 0; i < spawnPoints.Length; i++)
            {
                Transform point = spawnPoints[(startIndex + i) % spawnPoints.Length];
                if (point != null)
                {
                    return point.position;
                }
            }

            Debug.LogWarning($"{LOG_PREFIX} All spawn point references are null. Falling back to WaveManager position.");
        }

        // Fallback: spawn near the WaveManager with a random horizontal offset.
        return transform.position + new Vector3(
            UnityEngine.Random.Range(-5f, 5f),
            0f,
            UnityEngine.Random.Range(-5f, 5f));
    }

    // ------------------------------------------------------------------ //
    //  Event handlers
    // ------------------------------------------------------------------ //

    private void HandleEnemyDeath(EnemyHealth enemyHealth)
    {
        // Ignore deaths that happen while we're still spawning the wave --
        // the aliveEnemies count is being built up during spawning, so
        // decrementing mid-spawn would desync the counter.
        if (isSpawning) return;

        // Ignore if all waves are already done (late death callbacks).
        if (allWavesComplete) return;

        // Ignore if no wave has started yet.
        if (currentWaveIndex < 0) return;

        aliveEnemies = Mathf.Max(0, aliveEnemies - 1);
        OnEnemyCountChanged?.Invoke(aliveEnemies);

        Debug.Log($"{LOG_PREFIX} Enemy killed. Remaining: {aliveEnemies}");

        if (aliveEnemies <= 0)
        {
            Debug.Log($"{LOG_PREFIX} Wave {currentWaveIndex + 1} completed!");
            OnWaveCompleted?.Invoke(currentWaveIndex + 1);
            StartNextWave();
        }
    }
}
