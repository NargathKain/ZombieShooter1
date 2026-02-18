using UnityEngine;
using System.Collections;

/// <summary>
/// Vault door that opens when win conditions are met (25 kills + 3 keys).
/// Pivots 90 degrees and triggers victory.
///
/// SETUP:
/// 1. Attach to door GameObject
/// 2. Make sure the door's pivot point is at the hinge (edge of door)
/// 3. Add a Collider
/// 4. Tag as "Interactable"
/// </summary>
public class VaultDoor : MonoBehaviour, IInteractable
{
    [Header("Rotation Settings")]
    [SerializeField] private float openAngle = 90f;
    [SerializeField] private Vector3 rotationAxis = Vector3.up;
    [SerializeField] private float rotationDuration = 0.5f;

    [Header("Audio")]
    [SerializeField] private AudioClip openSound;
    [SerializeField] private AudioClip lockedSound;
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
        if (isAnimating || isOpen) return;

        // Check if win conditions are met (kills + keys)
        if (WinConditionManager.Instance == null)
        {
            Debug.LogError("[VaultDoor] WinConditionManager not found!");
            return;
        }

        if (!WinConditionManager.Instance.ConditionsMet)
        {
            // Show progress
            int kills = WinConditionManager.Instance.CurrentKills;
            int reqKills = WinConditionManager.Instance.RequiredKills;
            int keys = WinConditionManager.Instance.CurrentKeys;
            int reqKeys = WinConditionManager.Instance.RequiredKeys;

            interactor.ReceiveInteract($"Locked - Kills: {kills}/{reqKills}, Keys: {keys}/{reqKeys}");

            if (lockedSound != null)
            {
                AudioSource.PlayClipAtPoint(lockedSound, transform.position, audioVolume);
            }
            return;
        }

        // Conditions met - open the door and trigger victory
        isOpen = true;
        StartCoroutine(OpenAndTriggerVictory());

        if (openSound != null)
        {
            AudioSource.PlayClipAtPoint(openSound, transform.position, audioVolume);
        }
    }

    private IEnumerator OpenAndTriggerVictory()
    {
        // Rotate door open
        isAnimating = true;

        Quaternion startRot = transform.localRotation;
        float elapsed = 0f;

        while (elapsed < rotationDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.SmoothStep(0f, 1f, elapsed / rotationDuration);
            transform.localRotation = Quaternion.Slerp(startRot, openRotation, t);
            yield return null;
        }

        transform.localRotation = openRotation;
        isAnimating = false;

        Debug.Log("[VaultDoor] Door opened - Victory!");

        // Trigger victory immediately
        WinConditionManager.Instance.TriggerVictory();
    }

    public void OnReadyInteract() { }
    public void OnAbortInteract() { }
    public void OnEndInteract() { }
}
