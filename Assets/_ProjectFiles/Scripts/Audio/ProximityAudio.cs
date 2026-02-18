using UnityEngine;
using System.Collections;

/// <summary>
/// Plays audio when player gets close to this object.
/// Use for keys, collectibles, or any object that should have proximity-based sound.
///
/// SETUP:
/// 1. Attach to the key/object
/// 2. Add Sphere Collider, check "Is Trigger", set radius for detection range
/// 3. Assign audio clip
/// </summary>
[RequireComponent(typeof(Collider))]
public class ProximityAudio : MonoBehaviour
{
    [Header("Audio")]
    [SerializeField] private AudioClip audioClip;

    [Range(0f, 1f)]
    [SerializeField] private float volume = 0.5f;

    [SerializeField] private bool loop = true;

    [Header("Fade Settings")]
    [SerializeField] private float fadeInTime = 1f;
    [SerializeField] private float fadeOutTime = 1f;

    private AudioSource audioSource;
    private Coroutine fadeCoroutine;

    private void Awake()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = audioClip;
        audioSource.loop = loop;
        audioSource.playOnAwake = false;
        audioSource.volume = 0f;
        audioSource.spatialBlend = 0f; // 2D so it's consistent volume in range

        // Ensure collider is trigger
        Collider col = GetComponent<Collider>();
        if (col != null) col.isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        Debug.Log($"[ProximityAudio] Player near {gameObject.name} - playing audio");

        if (fadeCoroutine != null) StopCoroutine(fadeCoroutine);
        fadeCoroutine = StartCoroutine(FadeIn());
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        Debug.Log($"[ProximityAudio] Player left {gameObject.name} - stopping audio");

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
            audioSource.volume = Mathf.Lerp(startVol, volume, elapsed / fadeInTime);
            yield return null;
        }

        audioSource.volume = volume;
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
