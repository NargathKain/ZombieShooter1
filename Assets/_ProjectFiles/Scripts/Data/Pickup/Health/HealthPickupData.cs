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
        PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
        if (playerHealth == null)
            playerHealth = player.GetComponentInParent<PlayerHealth>();
        if (playerHealth == null)
            playerHealth = player.GetComponentInChildren<PlayerHealth>();

        if (playerHealth != null)
        {
            playerHealth.Heal(healthAmount);
            Debug.Log($"[HealthPickup] +{healthAmount} HP");
        }
        else
        {
            Debug.LogWarning("[HealthPickup] No PlayerHealth found on player.");
        }
    }

    public override string GetPickupPromptText()
    {
        return $"Health Pack (+{healthAmount} HP)";
    }
}