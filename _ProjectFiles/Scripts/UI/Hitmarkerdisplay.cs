using UnityEngine;
using UnityEngine.UI;
using System.Collections;

/// <summary>
/// Displays hitmarker (crosshair feedback) when successfully hitting enemies.
/// Plays hit sound and shows brief visual indicator.
/// Attach to Canvas or UI GameObject.
/// 
/// SETUP:
/// 1. Create UI Image for hitmarker (white X or crosshair symbol)
/// 2. Position at center of screen
/// 3. Start with alpha = 0 (invisible)
/// 4. Attach this script to the Image or parent
/// 5. Assign hit sound in Inspector
/// 
/// USAGE:
/// Other scripts call: HitmarkerDisplay.Instance.ShowHitmarker();
/// </summary>
public class Hitmarkerdisplay : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private Image hitmarkerImage;

    [Header("Settings")]
    [SerializeField] private float displayDuration = 0.4f;
    [SerializeField] private Color hitColor = Color.white;
    [SerializeField] private Color criticalHitColor = Color.red; // For future: headshots

    [Header("Audio")]
    [SerializeField] private AudioClip hitSound;
    [SerializeField] private AudioClip criticalHitSound;
    [SerializeField] private float hitSoundVolume = 0.5f;

    [Header("Animation")]
    [SerializeField] private AnimationCurve fadeInCurve = AnimationCurve.Linear(0, 1, 1, 1);
    [SerializeField] private AnimationCurve fadeOutCurve = AnimationCurve.Linear(0, 1, 1, 0);

    // Singleton pattern for easy access
    public static Hitmarkerdisplay Instance { get; private set; }

    private AudioSource audioSource;
    private Coroutine currentCoroutine;

    private void Awake()
    {
        // Singleton setup
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        // Get or add AudioSource
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        audioSource.playOnAwake = false;

        // Validate
        if (hitmarkerImage == null)
        {
            Debug.LogError("HitmarkerDisplay: No hitmarker image assigned!");
            enabled = false;
            return;
        }

        // Start invisible
        SetAlpha(0f);
    }

    /// <summary>
    /// Show normal hit feedback (called by weapon scripts)
    /// </summary>
    public void ShowHitmarker()
    {
        ShowHitmarker(false);
    }

    /// <summary>
    /// Show hit feedback with optional critical hit styling
    /// </summary>
    public void ShowHitmarker(bool isCritical)
    {
        // Stop previous animation if still running
        if (currentCoroutine != null)
        {
            StopCoroutine(currentCoroutine);
        }

        // Start new animation
        currentCoroutine = StartCoroutine(AnimateHitmarker(isCritical));

        // Play sound
        AudioClip soundToPlay = isCritical ? criticalHitSound : hitSound;
        if (soundToPlay != null)
        {
            audioSource.PlayOneShot(soundToPlay, hitSoundVolume);
        }
    }

    /// <summary>
    /// Animate hitmarker: fade in quickly, stay briefly, fade out
    /// </summary>
    private IEnumerator AnimateHitmarker(bool isCritical)
    {
        Color targetColor = isCritical ? criticalHitColor : hitColor;

        // Fade in (25% of duration)
        float fadeInTime = displayDuration * 0.25f;
        float timer = 0f;

        while (timer < fadeInTime)
        {
            timer += Time.deltaTime;
            float alpha = fadeInCurve.Evaluate(timer / fadeInTime);
            SetColor(targetColor, alpha);
            yield return null;
        }

        // Stay visible (50% of duration)
        float stayTime = displayDuration * 0.5f;
        SetColor(targetColor, 1f);
        yield return new WaitForSeconds(stayTime);

        // Fade out (25% of duration)
        float fadeOutTime = displayDuration * 0.25f;
        timer = 0f;

        while (timer < fadeOutTime)
        {
            timer += Time.deltaTime;
            float alpha = fadeOutCurve.Evaluate(1f - (timer / fadeOutTime));
            SetColor(targetColor, alpha);
            yield return null;
        }

        // Ensure fully transparent
        SetAlpha(0f);
        currentCoroutine = null;
    }

    /// <summary>
    /// Set hitmarker color and alpha
    /// </summary>
    private void SetColor(Color color, float alpha)
    {
        if (hitmarkerImage != null)
        {
            hitmarkerImage.color = new Color(color.r, color.g, color.b, alpha);
        }
    }

    /// <summary>
    /// Set hitmarker alpha only
    /// </summary>
    private void SetAlpha(float alpha)
    {
        if (hitmarkerImage != null)
        {
            Color current = hitmarkerImage.color;
            hitmarkerImage.color = new Color(current.r, current.g, current.b, alpha);
        }
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }
}