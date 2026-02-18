using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manager that listens to all enemy deaths and spawns pickup drops.
/// Subscribes to EnemyHealth.OnEnemyDeath and rolls against each enemy's
/// EnemyData.dropChance to decide whether to spawn a health or ammo pickup.
///
/// SETUP:
/// 1. Place on a single manager GameObject in the scene
/// 2. Assign health drop prefab (must have PickupItem + HealthPickupData)
/// 3. Configure ammo drop configs — one entry per AmmoType you want to support
/// 4. Each AmmoDropConfig links an AmmoType to a weapon (for ownership check) and a prefab
///
/// DEPENDENCIES:
/// - EnemyHealth (static event OnEnemyDeath)
/// - EnemyData (dropChance field)
/// - Inventory (HasWeapon for filtering ammo drops to owned weapons)
/// </summary>
public class RandomDrop : MonoBehaviour
{
    #region Nested Types

    [System.Serializable]
    public class AmmoDropConfig
    {
        [Tooltip("The ammo type this config provides.")]
        public AmmoType ammoType;

        [Tooltip("The weapon that uses this ammo type. Used to check player ownership.")]
        public WeaponData weapon;

        [Tooltip("Pickup prefab to instantiate. Must have PickupItem + AmmoPickupData.")]
        public GameObject dropPrefab;
    }

    #endregion

    #region Constants

    private const string LOG_PREFIX = "[RandomDrop]";

    #endregion

    #region Serialized Fields

    [Header("Drop Prefabs")]
    [Tooltip("Health pickup prefab. Must have PickupItem + HealthPickupData.")]
    [SerializeField] private GameObject healthDropPrefab;

    [Header("Ammo Drop Configs")]
    [Tooltip("One entry per supported AmmoType. Links ammo type, weapon, and prefab.")]
    [SerializeField] private AmmoDropConfig[] ammoDropConfigs;

    [Header("Drop Settings")]
    [Tooltip("Y offset above enemy death position when spawning drops.")]
    [SerializeField] private float dropHeightOffset = 1f;

    [Tooltip("Probability of choosing health over ammo when a drop occurs (0-1).")]
    [Range(0f, 1f)]
    [SerializeField] private float healthDropChanceWeight = 0.5f;

    [Header("Ammo Drop Settings")]
    [Tooltip("Minimum ammo amount for ammo drops (currently reserved for future runtime configuration).")]
    [SerializeField] private int minAmmoDrop = 10;

    [Tooltip("Maximum ammo amount for ammo drops (currently reserved for future runtime configuration).")]
    [SerializeField] private int maxAmmoDrop = 30;

    #endregion

    #region Private Fields

    private Inventory playerInventory;
    private readonly List<AmmoDropConfig> validAmmoConfigs = new List<AmmoDropConfig>();

    #endregion

    #region Unity Lifecycle

    private void Start()
    {
        playerInventory = FindFirstObjectByType<Inventory>();

        if (playerInventory == null)
            Debug.LogWarning($"{LOG_PREFIX} No Inventory found in scene. Ammo drops will use random ammo type.");

        ValidateConfiguration();
    }

    private void OnEnable()
    {
        EnemyHealth.OnEnemyDeath += HandleEnemyDeath;
    }

    private void OnDisable()
    {
        EnemyHealth.OnEnemyDeath -= HandleEnemyDeath;
    }

    #endregion

    #region Event Handler

    private void HandleEnemyDeath(EnemyHealth enemyHealth)
    {
        if (enemyHealth == null)
        {
            Debug.LogWarning($"{LOG_PREFIX} Received null EnemyHealth in death event.");
            return;
        }

        EnemyData enemyData = enemyHealth.GetEnemyData();
        if (enemyData == null)
        {
            Debug.LogWarning($"{LOG_PREFIX} EnemyData is null on {enemyHealth.gameObject.name}. Skipping drop.");
            return;
        }

        if (Random.value > enemyData.dropChance)
            return;

        Vector3 dropPosition = GetGroundedDropPosition(enemyHealth.transform.position);

        bool chooseHealth = Random.value < healthDropChanceWeight;

        if (chooseHealth)
        {
            SpawnHealthDrop(dropPosition);
        }
        else
        {
            SpawnAmmoDrop(dropPosition);
        }
    }

    #endregion

    #region Drop Spawning

    private void SpawnHealthDrop(Vector3 position)
    {
        if (healthDropPrefab == null)
        {
            Debug.LogWarning($"{LOG_PREFIX} healthDropPrefab is not assigned. Cannot spawn health drop.");
            return;
        }

        Instantiate(healthDropPrefab, position, Quaternion.identity);
        Debug.Log($"{LOG_PREFIX} Spawned health drop at {position}");
    }

    private void SpawnAmmoDrop(Vector3 position)
    {
        if (ammoDropConfigs == null || ammoDropConfigs.Length == 0)
        {
            Debug.LogWarning($"{LOG_PREFIX} No ammo drop configs. Falling back to health drop.");
            SpawnHealthDrop(position);
            return;
        }

        AmmoDropConfig selectedConfig = SelectAmmoConfig();

        if (selectedConfig == null || selectedConfig.dropPrefab == null)
        {
            Debug.LogWarning($"{LOG_PREFIX} No valid ammo config for player's weapons. Falling back to health drop.");
            SpawnHealthDrop(position);
            return;
        }

        Instantiate(selectedConfig.dropPrefab, position, Quaternion.identity);
        Debug.Log($"{LOG_PREFIX} Spawned {selectedConfig.ammoType} ammo drop at {position}");
    }

    private AmmoDropConfig SelectAmmoConfig()
    {
        validAmmoConfigs.Clear();

        if (playerInventory != null)
        {
            for (int i = 0; i < ammoDropConfigs.Length; i++)
            {
                AmmoDropConfig config = ammoDropConfigs[i];
                if (config == null || config.dropPrefab == null) continue;

                if (config.weapon != null && playerInventory.HasWeapon(config.weapon))
                {
                    validAmmoConfigs.Add(config);
                }
            }
        }

        if (validAmmoConfigs.Count > 0)
            return validAmmoConfigs[Random.Range(0, validAmmoConfigs.Count)];

        // Fallback: no inventory or no matching weapons — pick from all configs with valid prefabs
        for (int i = 0; i < ammoDropConfigs.Length; i++)
        {
            AmmoDropConfig config = ammoDropConfigs[i];
            if (config != null && config.dropPrefab != null)
                validAmmoConfigs.Add(config);
        }

        if (validAmmoConfigs.Count > 0)
            return validAmmoConfigs[Random.Range(0, validAmmoConfigs.Count)];

        return null;
    }

    #endregion

    #region Position Helpers

    /// <summary>
    /// Finds a valid drop position above the ground using a raycast.
    /// </summary>
    private Vector3 GetGroundedDropPosition(Vector3 enemyPosition)
    {
        // Start raycast from above the enemy position
        Vector3 rayStart = enemyPosition + Vector3.up * 2f;

        if (Physics.Raycast(rayStart, Vector3.down, out RaycastHit hit, 10f))
        {
            // Spawn slightly above the ground
            return hit.point + Vector3.up * dropHeightOffset;
        }

        // Fallback if no ground found
        return enemyPosition + Vector3.up * dropHeightOffset;
    }

    #endregion

    #region Validation

    private void ValidateConfiguration()
    {
        if (healthDropPrefab == null)
            Debug.LogWarning($"{LOG_PREFIX} healthDropPrefab is not assigned.");

        if (ammoDropConfigs == null || ammoDropConfigs.Length == 0)
        {
            Debug.LogWarning($"{LOG_PREFIX} No ammo drop configs configured.");
            return;
        }

        for (int i = 0; i < ammoDropConfigs.Length; i++)
        {
            AmmoDropConfig config = ammoDropConfigs[i];
            if (config == null)
            {
                Debug.LogWarning($"{LOG_PREFIX} ammoDropConfigs[{i}] is null.");
                continue;
            }

            if (config.dropPrefab == null)
                Debug.LogWarning($"{LOG_PREFIX} ammoDropConfigs[{i}] ({config.ammoType}) has no dropPrefab.");

            if (config.weapon == null)
                Debug.LogWarning($"{LOG_PREFIX} ammoDropConfigs[{i}] ({config.ammoType}) has no weapon assigned. " +
                                 "Ownership check will be skipped for this entry.");
        }

        if (minAmmoDrop > maxAmmoDrop)
            Debug.LogWarning($"{LOG_PREFIX} minAmmoDrop ({minAmmoDrop}) > maxAmmoDrop ({maxAmmoDrop}). Values may need correcting.");
    }

    #endregion
}
