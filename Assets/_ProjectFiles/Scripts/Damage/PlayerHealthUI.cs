using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Displays player health on UI.
/// Subscribes to PlayerHealth events (no direct reference needed!).
/// Attach to Canvas GameObject, assign Slider in Inspector.
/// 
/// SETUP:
/// 1. Attach this script to PlayerUI Canvas (NOT to the Healthbar)
/// 2. Drag Healthbar slider to "Health Slider" field
/// 3. Optional: Enable color gradient for green→yellow→red effect
/// 
/// EVENT-DRIVEN DESIGN:
/// - PlayerHealth broadcasts → This script listens
/// - No tight coupling between health logic and UI
/// </summary>
public class PlayerHealthUI : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private Slider healthSlider;
    [SerializeField] private Image damageFlash; // Optional: red screen flash

    [Header("Health Bar Color")]
    [SerializeField] private bool useColorGradient = true;
    [SerializeField] private Color fullHealthColor = Color.green;
    [SerializeField] private Color halfHealthColor = Color.yellow;
    [SerializeField] private Color lowHealthColor = Color.red;

    [Header("Flash Settings")]
    [SerializeField] private float flashDuration = 0.2f;
    [SerializeField] private Color flashColor = new Color(1f, 0f, 0f, 0.3f);

    private float flashTimer;
    private float previousHealth;
    private Image healthBarFill; // Reference to the Fill image

    private void OnEnable()
    {
        // Subscribe to PlayerHealth events
        PlayerHealth.OnHealthChanged += UpdateHealthBar;
        PlayerHealth.OnPlayerDeath += OnPlayerDied;
    }

    private void OnDisable()
    {
        // CRITICAL: Always unsubscribe to prevent memory leaks
        PlayerHealth.OnHealthChanged -= UpdateHealthBar;
        PlayerHealth.OnPlayerDeath -= OnPlayerDied;
    }

    private void Start()
    {
        // Validate references
        if (healthSlider == null)
        {
            Debug.LogError("PlayerHealthUI: Health Slider not assigned!");
            enabled = false;
            return;
        }

        // Get the Fill image from the slider
        if (healthSlider.fillRect != null)
        {
            healthBarFill = healthSlider.fillRect.GetComponent<Image>();

            if (healthBarFill == null)
            {
                Debug.LogWarning("PlayerHealthUI: Fill Rect doesn't have an Image component. Color gradient won't work.");
            }
        }
        else
        {
            Debug.LogWarning("PlayerHealthUI: Slider has no Fill Rect assigned!");
        }

        // Initialize flash to transparent
        if (damageFlash != null)
        {
            damageFlash.color = new Color(flashColor.r, flashColor.g, flashColor.b, 0f);
        }
    }

    private void Update()
    {
        // Handle damage flash fade
        if (flashTimer > 0f && damageFlash != null)
        {
            flashTimer -= Time.deltaTime;
            float alpha = Mathf.Lerp(0f, flashColor.a, flashTimer / flashDuration);
            damageFlash.color = new Color(flashColor.r, flashColor.g, flashColor.b, alpha);
        }
    }

    /// <summary>
    /// Called whenever player health changes (via event)
    /// </summary>
    private void UpdateHealthBar(float currentHealth, float maxHealth)
    {
        if (healthSlider == null) return;

        // Update slider value
        healthSlider.maxValue = maxHealth;
        healthSlider.value = currentHealth;

        // Update fill color based on health percentage
        if (useColorGradient && healthBarFill != null)
        {
            float healthPercent = currentHealth / maxHealth;
            Color targetColor;

            if (healthPercent > 0.5f)
            {
                // Interpolate between full (green) and half (yellow)
                float t = (healthPercent - 0.5f) / 0.5f; // 0 to 1
                targetColor = Color.Lerp(halfHealthColor, fullHealthColor, t);
            }
            else
            {
                // Interpolate between low (red) and half (yellow)
                float t = healthPercent / 0.5f; // 0 to 1
                targetColor = Color.Lerp(lowHealthColor, halfHealthColor, t);
            }

            healthBarFill.color = targetColor;
        }

        // Trigger damage flash if took damage
        if (currentHealth < previousHealth && damageFlash != null)
        {
            flashTimer = flashDuration;
        }

        previousHealth = currentHealth;
    }

    /// <summary>
    /// Called when player dies (via event)
    /// </summary>
    private void OnPlayerDied()
    {
        Debug.Log("PlayerHealthUI: Player died - should show death screen");
        // Future: Show death screen, disable UI, etc.
    }
}