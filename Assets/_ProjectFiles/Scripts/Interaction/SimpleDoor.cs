using UnityEngine;
using System.Collections;

/// <summary>
/// Simple door that pivots 90 degrees when the player interacts with it.
///
/// SETUP:
/// 1. Attach to door GameObject
/// 2. Make sure the door's pivot point is at the hinge (edge of door)
/// 3. Add a Collider
/// 4. Tag as "Interactable"
/// </summary>
public class SimpleDoor : MonoBehaviour, IInteractable
{
    [Header("Rotation Settings")]
    [Tooltip("Degrees to rotate when opened")]
    [SerializeField] private float openAngle = 90f;

    [Tooltip("Axis to rotate around (local space)")]
    [SerializeField] private Vector3 rotationAxis = Vector3.up;

    [Tooltip("Time in seconds to complete rotation")]
    [SerializeField] private float rotationDuration = 0.5f;

    [Header("Audio")]
    [SerializeField] private AudioClip openSound;
    [SerializeField] private AudioClip closeSound;
    [Range(0f, 1f)]
    [SerializeField] private float audioVolume = 1f;

    private bool isOpen;
    private bool isAnimating;
    private Quaternion closedRotation;
    private Quaternion openRotation;

    private void Start()
    {
        closedRotation = transform.localRotation;
        openRotation = closedRotation * Quaternion.AngleAxis(openAngle, rotationAxis);
    }

    public void OnInteract(Interactor interactor)
    {
        if (isAnimating) return;

        isOpen = !isOpen;
        StartCoroutine(RotateDoor(isOpen));

        AudioClip clip = isOpen ? openSound : closeSound;
        if (clip != null)
        {
            AudioSource.PlayClipAtPoint(clip, transform.position, audioVolume);
        }
    }

    private IEnumerator RotateDoor(bool opening)
    {
        isAnimating = true;

        Quaternion startRot = transform.localRotation;
        Quaternion endRot = opening ? openRotation : closedRotation;

        float elapsed = 0f;
        while (elapsed < rotationDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.SmoothStep(0f, 1f, elapsed / rotationDuration);
            transform.localRotation = Quaternion.Slerp(startRot, endRot, t);
            yield return null;
        }

        transform.localRotation = endRot;
        isAnimating = false;
    }

    public void OnReadyInteract() { }
    public void OnAbortInteract() { }
    public void OnEndInteract() { }
}
