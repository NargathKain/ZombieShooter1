using UnityEngine;

/// <summary>
/// Interactable key pickup. When the player interacts with it,
/// the key is added to PlayerKeys and the pickup is destroyed.
///
/// SETUP:
/// 1. Attach to key GameObject
/// 2. Add a Collider (can be trigger)
/// 3. Tag as "Interactable"
/// 4. Set keyID to match the door's requiredKeyID
/// 5. Optional: assign indicator for visual feedback
/// </summary>
public class KeyPickup : MonoBehaviour, IInteractable
{
    [Header("Key Settings")]
    [Tooltip("Unique identifier for this key. Must match the door's requiredKeyID.")]
    [SerializeField] private string keyID = "Key";

    [Tooltip("Message shown when player looks at the key.")]
    [SerializeField] private string pickupMessage = "Press E to pick up key";

    [Header("Visual Feedback")]
    [Tooltip("Optional indicator object to show when player is looking at the key.")]
    [SerializeField] private GameObject indicator;

    [Header("Audio")]
    [Tooltip("Sound played when key is picked up.")]
    [SerializeField] private AudioClip pickupSound;

    [Range(0f, 1f)]
    [SerializeField] private float pickupVolume = 1f;

    public void OnInteract(Interactor interactor)
    {
        // Add key to player's inventory
        PlayerKeys.AddKey(keyID);

        // Play pickup sound
        if (pickupSound != null)
        {
            AudioSource.PlayClipAtPoint(pickupSound, transform.position, pickupVolume);
        }

        // Show confirmation message
        interactor.ReceiveInteract($"Picked up: {keyID}");

        // Destroy the pickup
        Destroy(gameObject);
    }

    public void OnReadyInteract()
    {
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
        // Nothing needed here - object is destroyed on pickup
    }
}
