using UnityEngine;
using System.Collections;

/// <summary>
/// Plays ambient audio when player enters an area (vault room, casino, etc.)
///
/// SETUP:
/// 1. Create empty GameObject covering the area
/// 2. Add Box Collider, check "Is Trigger", size it to the room
/// 3. Attach this script
/// 4. Assign ambient audio clip (buzzing lights, casino sounds, etc.)
/// </summary>
[RequireComponent(typeof(Collider))]
public class AudioZone : MonoBehaviour
{
    [Header("Ambient Sound")]
    [Tooltip("The ambient sound to play in this zone (loops)")]
    [SerializeField] private AudioClip ambientClip;

    [Range(0f, 1f)]
    [SerializeField] private float ambientVolume = 0.5f;

    [Header("Fade Settings")]
    [SerializeField] private float fadeInTime = 1f;
    [SerializeField] private float fadeOutTime = 1f;

    private AudioSource audioSource;
    private Coroutine fadeCoroutine;

    private void Awake()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = ambientClip;
        audioSource.loop = true;
        audioSource.playOnAwake = false;
        audioSource.volume = 0f;
        audioSource.spatialBlend = 0f; // 2D sound fills the room

        Collider col = GetComponent<Collider>();
        if (col != null) col.isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        Debug.Log($"[AudioZone] Player entered {gameObject.name}");

        if (fadeCoroutine != null) StopCoroutine(fadeCoroutine);
        fadeCoroutine = StartCoroutine(FadeIn());
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        Debug.Log($"[AudioZone] Player exited {gameObject.name}");

        if (fadeCoroutine != null) StopCoroutine(fadeCoroutine);
        fadeCoroutine = StartCoroutine(FadeOut());
    }

    private IEnumerator FadeIn()
    {
        audioSource.Play();
        float elapsed = 0f;
        float startVol = audioSource.volume;

        while (elapsed < fadeInTime)
        {
            elapsed += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(startVol, ambientVolume, elapsed / fadeInTime);
            yield return null;
        }

        audioSource.volume = ambientVolume;
    }

    private IEnumerator FadeOut()
    {
        float elapsed = 0f;
        float startVol = audioSource.volume;

        while (elapsed < fadeOutTime)
        {
            elapsed += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(startVol, 0f, elapsed / fadeOutTime);
            yield return null;
        }

        audioSource.volume = 0f;
        audioSource.Stop();
    }
}
