using UnityEngine;

/// BASE CLASS: Abstract pickup data that all specific pickups inherit from.
/// This enforces a common interface while allowing specialized behavior.
/// 
/// DO NOT create instances of this - create AmmoPickupData, HealthPickupData, etc.

public abstract class PickupData_Modular : MonoBehaviour
{
    [Header("Common Properties")]
    public string pickupName = "Pickup";

    [Header("Audio/Visual")]
    public AudioClip pickupSound;
    public GameObject pickupEffect;

    /// <summary>
    /// Each pickup type implements its own collection logic
    /// This is called when the player picks up the item
    /// </summary>
    /// <param name="player">The player GameObject that picked this up</param>
    public abstract void OnPickedUp(GameObject player);

    /// <summary>
    /// Returns the display text for the pickup prompt
    /// Example: "Pistol Ammo (x20)" or "Health Pack (+25 HP)"
    /// </summary>
    public abstract string GetPickupPromptText();
}
