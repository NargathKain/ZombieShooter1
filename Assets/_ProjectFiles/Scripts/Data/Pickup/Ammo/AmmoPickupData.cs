// ============================================================================
// AMMO PICKUP
// ============================================================================

using UnityEngine;


/// <summary>
/// Gives the player ammunition when picked up
/// Create via: Right Click → Create → Game/Pickups/Ammo Pickup
/// </summary>
[CreateAssetMenu(fileName = "NewAmmoPickup", menuName = "Game/Pickups/Ammo Pickup")]
public class AmmoPickupData : PickupData_Modular
{
    [Header("Ammo Settings")]
    public AmmoType ammoType = AmmoType.Pistol;
    public int ammoAmount = 20;

    public override void OnPickedUp(GameObject player)
    {
        Inventory inventory = player.GetComponent<Inventory>();
        if (inventory == null)
            inventory = player.GetComponentInParent<Inventory>();
        if (inventory == null)
            inventory = player.GetComponentInChildren<Inventory>();

        if (inventory != null)
        {
            inventory.AddAmmo(ammoType, ammoAmount);
            Debug.Log($"[AmmoPickup] +{ammoAmount} {ammoType} ammo");
        }
        else
        {
            Debug.LogWarning("[AmmoPickup] No Inventory found on player.");
        }
    }

    public override string GetPickupPromptText()
    {
        return $"{ammoType} Ammo (x{ammoAmount})";
    }
}