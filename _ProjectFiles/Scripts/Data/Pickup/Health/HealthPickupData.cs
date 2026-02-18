using UnityEngine;

/// <summary>
/// Restores player health when picked up
/// Create via: Right Click → Create → Game/Pickups/Health Pickup
/// </summary>
[CreateAssetMenu(fileName = "NewHealthPickup", menuName = "Game/Pickups/Health Pickup")]
public class HealthPickupData : PickupData_Modular
{
    [Header("Health Settings")]
    public float healthAmount = 25f;

    public override void OnPickedUp(GameObject player)
    {
        // Find the PlayerHealth component on the player
        //PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();

        //if (playerHealth != null)
        //{
        //    playerHealth.Heal(healthAmount);
        //    Debug.Log($"Picked up Health Pack: +{healthAmount} HP");
        //}
        //else
        //{
        //    Debug.LogError("Player has no PlayerHealth component!");
        //}
    }

    public override string GetPickupPromptText()
    {
        return $"Health Pack (+{healthAmount} HP)";
    }
}