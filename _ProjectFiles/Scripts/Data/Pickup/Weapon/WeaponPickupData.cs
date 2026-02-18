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
    //public WeaponData weaponToGive;
    [Tooltip("Ammo to give with the weapon")]
    public int bonusAmmo = 30;

    public override void OnPickedUp(GameObject player)
    {
        //Inventory inventory = player.GetComponent<Inventory>();

        //if (inventory != null && weaponToGive != null)
        //{
        //    inventory.AddWeapon(weaponToGive);

        //    // Also give ammo for this weapon
        //    if (bonusAmmo > 0)
        //    {
        //        inventory.AddAmmo(weaponToGive.ammoType, bonusAmmo);
        //    }

        //    Debug.Log($"Picked up weapon: {weaponToGive.weaponName}");
        //}
        //else
        //{
        //    Debug.LogError("Player has no Inventory or weaponToGive is null!");
        //}
        Debug.Log("Picked up weapon (functionality not implemented in this snippet)");
    }

    public override string GetPickupPromptText()
    {
        return $"{bonusAmmo} Ammo (x{bonusAmmo})";// afaireseto auto
        //if (weaponToGive != null)
        //{
        //    return $"{weaponToGive.weaponName} (+{bonusAmmo} ammo)";
        //}
        //return "Weapon";
    }
}