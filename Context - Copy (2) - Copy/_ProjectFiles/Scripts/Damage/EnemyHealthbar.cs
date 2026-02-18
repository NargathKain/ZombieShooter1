using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Individual health bar displayed above each enemy.
/// Uses World Space Canvas that billboards to camera.
/// Subscribes to its parent EnemyHealth's instance events.
/// 
/// SETUP (Matching Your Player Setup):
/// 1. Create World Space Canvas as child of enemy prefab
/// 2. Set Canvas Render Mode to "World Space"
/// 3. Set Canvas scale to (0.01, 0.01, 0.01)
/// 4. Create child GameObject "HealthBar" with Slider component
/// 5. Inside HealthBar: Create "Fill" (Image, white color)
/// 6. Configure Slider:
///    - Min Value: 0
///    - Max Value: 100
///    - Fill Rect: Drag Fill image here
///    - Interactable: OFF
///    - Transition: None
///    - Navigation: None
/// 7. Attach this script to Canvas (or HealthBar GameObject)
/// 8. Assign Slider in Inspector
/// 
/// NOTE: No Border needed for enemy health bars (cleaner look)
/// 
/// EVENT-DRIVEN DESIGN:
/// - EnemyHealth (on parent) broadcasts → This script listens
/// - Each enemy has its own health bar that follows it
/// </summary>
public class EnemyHealthBar : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private Slider healthSlider;

    [Header("Settings")]
    [SerializeField] private Vector3 offset = new Vector3(0f, 2.5f, 0f);
    [SerializeField] private bool hideWhenFull = true;
    [SerializeField] private bool hideWhenDead = true;
    [SerializeField] private bool changeColor = true;

    [Header("Colors (Optional)")]
    [SerializeField] private Color highHealthColor = Color.green;
    [SerializeField] private Color mediumHealthColor = Color.yellow;
    [SerializeField] private Color lowHealthColor = Color.red;

    private EnemyHealth enemyHealth;
    private Transform enemyTransform;
    private Camera mainCamera;
    private Canvas canvas;
    private Image fillImage;

    private void Start()
    {
        // Find camera
        mainCamera = Camera.main;

        // Get Canvas component (either on this object or parent)
        canvas = GetComponentInParent<Canvas>();
        if (canvas != null)
        {
            canvas.renderMode = RenderMode.WorldSpace;
        }
        else
        {
            Debug.LogError("EnemyHealthBar: No Canvas found! This script needs a Canvas component.");
            enabled = false;
            return;
        }

        // Find parent EnemyHealth
        enemyHealth = GetComponentInParent<EnemyHealth>();
        if (enemyHealth == null)
        {
            Debug.LogError("EnemyHealthBar: No EnemyHealth found in parent! This script must be child of enemy.");
            enabled = false;
            return;
        }

        enemyTransform = enemyHealth.transform;

        // Subscribe to THIS enemy's health changes (instance event)
        enemyHealth.OnHealthChanged += UpdateHealthBar;

        // Subscribe to death event to hide bar
        EnemyHealth.OnEnemyDeath += OnAnyEnemyDied;

        // Validate slider
        if (healthSlider == null)
        {
            Debug.LogError("EnemyHealthBar: Health Slider not assigned!");
            enabled = false;
            return;
        }

        // Get fill image for color changes (if enabled)
        if (changeColor && healthSlider.fillRect != null)
        {
            fillImage = healthSlider.fillRect.GetComponent<Image>();
        }

        // Start hidden if full health
        if (hideWhenFull)
        {
            canvas.enabled = false;
        }
    }

    private void OnDestroy()
    {
        // Unsubscribe
        if (enemyHealth != null)
        {
            enemyHealth.OnHealthChanged -= UpdateHealthBar;
        }
        EnemyHealth.OnEnemyDeath -= OnAnyEnemyDied;
    }

    private void LateUpdate()
    {
        if (enemyTransform == null || mainCamera == null) return;

        // Position: Follow enemy + offset
        canvas.transform.position = enemyTransform.position + offset;

        // Billboard: Always face camera
        canvas.transform.LookAt(canvas.transform.position + mainCamera.transform.rotation * Vector3.forward,
                                mainCamera.transform.rotation * Vector3.up);
    }

    /// <summary>
    /// Called when THIS enemy's health changes
    /// </summary>
    private void UpdateHealthBar(float currentHealth, float maxHealth)
    {
        if (healthSlider == null) return;

        // Update slider value (matches your player setup!)
        healthSlider.maxValue = maxHealth;
        healthSlider.value = currentHealth;

        // Calculate percentage
        float healthPercent = currentHealth / maxHealth;

        // Optional: Change color based on health percentage
        if (changeColor && fillImage != null)
        {
            if (healthPercent > 0.5f)
            {
                fillImage.color = highHealthColor;
            }
            else if (healthPercent > 0.25f)
            {
                fillImage.color = mediumHealthColor;
            }
            else
            {
                fillImage.color = lowHealthColor;
            }
        }

        // Show bar when damaged
        if (hideWhenFull && canvas != null)
        {
            canvas.enabled = (healthPercent < 1f);
        }
    }

    /// <summary>
    /// Called when ANY enemy dies (to hide THIS enemy's bar if it's the one that died)
    /// </summary>
    private void OnAnyEnemyDied(EnemyHealth deadEnemy)
    {
        // Check if it's THIS enemy that died
        if (deadEnemy == enemyHealth && hideWhenDead && canvas != null)
        {
            canvas.enabled = false;
        }
    }
}