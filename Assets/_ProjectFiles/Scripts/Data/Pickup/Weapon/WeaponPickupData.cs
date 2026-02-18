using UnityEngine;
// ============================================================================
// WEAPON PICKUP
// ============================================================================

/// <summary>
/// Gives the player a new weapon when picked up
/// Create via: Right Click → Create → Game/Pickups/Weapon Pickup
/// </summary>
[CreateAssetMenu(fileName = "NewWeaponPickup", menuName = "Game/Pickups/Weapon Pickup")]
public class WeaponPickupData : PickupData_Modular
{
    [Header("Weapon Settings")]
    public WeaponData weaponToGive;
    [Tooltip("Ammo to give with the weapon")]
    public int bonusAmmo = 30;

    public override void OnPickedUp(GameObject player)
    {
        Inventory inventory = player.GetComponent<Inventory>();
        if (inventory == null)
            inventory = player.GetComponentInParent<Inventory>();
        if (inventory == null)
            inventory = player.GetComponentInChildren<Inventory>();

        if (inventory != null && weaponToGive != null)
        {
            inventory.AddWeapon(weaponToGive);

            if (bonusAmmo > 0)
            {
                inventory.AddAmmo(weaponToGive.ammoType, bonusAmmo);
            }

            Debug.Log($"[WeaponPickup] Picked up {weaponToGive.weaponName} (+{bonusAmmo} ammo)");
        }
        else
        {
            Debug.LogWarning("[WeaponPickup] No Inventory or weaponToGive is null.");
        }
    }

    public override string GetPickupPromptText()
    {
        if (weaponToGive != null)
            return $"{weaponToGive.weaponName} (+{bonusAmmo} ammo)";
        return "Weapon";
    }
}