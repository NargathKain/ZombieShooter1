using UnityEngine;

/// <summary>
/// Interactable door that requires a specific key to open.
/// Can optionally consume the key when used.
///
/// SETUP:
/// 1. Attach to door GameObject
/// 2. Add a Collider (on the door or a separate interaction zone)
/// 3. Tag as "Interactable"
/// 4. Set requiredKeyID to match the key's keyID
/// 5. Configure open behavior (animation, rotation, or disable)
/// </summary>
public class LockedDoor : MonoBehaviour, IInteractable
{
    [Header("Key Settings")]
    [Tooltip("Key ID required to open this door. Must match a KeyPickup's keyID.")]
    [SerializeField] private string requiredKeyID = "Key";

    [Tooltip("If true, the key is removed from inventory when the door opens.")]
    [SerializeField] private bool consumeKey = false;

    [Header("Messages")]
    [Tooltip("Message shown when player has the key.")]
    [SerializeField] private string unlockMessage = "Press E to unlock";

    [Tooltip("Message shown when player doesn't have the key.")]
    [SerializeField] private string lockedMessage = "Requires key";

    [Header("Visual Feedback")]
    [Tooltip("Optional indicator object shown when player looks at the door.")]
    [SerializeField] private GameObject indicator;

    [Header("Open Behavior")]
    [Tooltip("If assigned, this GameObject is disabled when door opens (e.g., the door itself).")]
    [SerializeField] private GameObject objectToDisable;

    [Tooltip("If assigned, this GameObject is enabled when door opens (e.g., an open door model).")]
    [SerializeField] private GameObject objectToEnable;

    [Tooltip("If assigned, this Animator triggers 'Open' when door opens.")]
    [SerializeField] private Animator doorAnimator;

    [Tooltip("Animator trigger parameter name for opening.")]
    [SerializeField] private string openTrigger = "Open";

    [Header("Audio")]
    [Tooltip("Sound played when door unlocks and opens.")]
    [SerializeField] private AudioClip unlockSound;

    [Tooltip("Sound played when player tries to open without key.")]
    [SerializeField] private AudioClip lockedSound;

    [Range(0f, 1f)]
    [SerializeField] private float audioVolume = 1f;

    // State
    private bool isOpen = false;

    public void OnInteract(Interactor interactor)
    {
        if (isOpen)
        {
            interactor.ReceiveInteract("Door is already open");
            return;
        }

        if (PlayerKeys.HasKey(requiredKeyID))
        {
            OpenDoor(interactor);
        }
        else
        {
            // Player doesn't have the key
            if (lockedSound != null)
            {
                AudioSource.PlayClipAtPoint(lockedSound, transform.position, audioVolume);
            }
            interactor.ReceiveInteract(lockedMessage);
        }
    }

    private void OpenDoor(Interactor interactor)
    {
        isOpen = true;

        // Consume key if configured
        if (consumeKey)
        {
            PlayerKeys.RemoveKey(requiredKeyID);
        }

        // Play unlock sound
        if (unlockSound != null)
        {
            AudioSource.PlayClipAtPoint(unlockSound, transform.position, audioVolume);
        }

        // Handle door opening visuals
        if (objectToDisable != null)
        {
            objectToDisable.SetActive(false);
        }

        if (objectToEnable != null)
        {
            objectToEnable.SetActive(true);
        }

        if (doorAnimator != null)
        {
            doorAnimator.SetTrigger(openTrigger);
        }

        // Hide indicator since door is now open
        if (indicator != null)
        {
            indicator.SetActive(false);
        }

        interactor.ReceiveInteract("Door unlocked!");

        Debug.Log($"[LockedDoor] Opened with key: {requiredKeyID}");
    }

    public void OnReadyInteract()
    {
        if (isOpen) return;

        if (indicator != null)
            indicator.SetActive(true);
    }

    public void OnAbortInteract()
    {
        if (indicator != null)
            indicator.SetActive(false);
    }

    public void OnEndInteract()
    {
        // Nothing needed
    }
}
