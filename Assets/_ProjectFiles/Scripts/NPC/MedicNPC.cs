using UnityEngine;

/// <summary>
/// NPC that heals the player in exchange for points.
/// Subscribes to <see cref="PlayerHealth.OnHealthChanged"/> to track whether
/// the player actually needs healing before allowing the transaction.
/// </summary>
public class MedicNPC : NPCBase
{
    private const string LOG_PREFIX = "[MedicNPC]";

    [SerializeField] private int healCost = 50;
    [SerializeField] private float healAmount = 50f;

    private Inventory cachedInventory;
    private float lastKnownHealth;
    private float lastKnownMaxHealth;

    private void OnEnable()
    {
        PlayerHealth.OnHealthChanged += HandleHealthChanged;
    }

    private void OnDisable()
    {
        PlayerHealth.OnHealthChanged -= HandleHealthChanged;
    }

    private void HandleHealthChanged(float current, float max)
    {
        lastKnownHealth = current;
        lastKnownMaxHealth = max;
    }

    public override void OnInteract(Interactor interactor)
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
            cachedInventory = player.GetComponent<Inventory>();
        base.OnInteract(interactor);
    }

    public override void OnEndInteract()
    {
        cachedInventory = null;
        base.OnEndInteract();
    }

    protected override string GetInteractionMessage()
    {
        if (lastKnownMaxHealth > 0f && lastKnownHealth >= lastKnownMaxHealth)
            return "MEDIC: You look healthy! No need for treatment.";

        if (cachedInventory == null)
            return "MEDIC: My supplies aren't available right now.";

        if (cachedInventory.CurrentPoints < healCost)
            return $"MEDIC: Treatment costs {healCost} points. You only have {cachedInventory.CurrentPoints}. Kill more zombies!";

        return $"MEDIC: I can patch you up for {healCost} points.";
    }

    protected override void PerformInteraction(Interactor interactor)
    {
        if (interactor == null)
        {
            Debug.LogWarning($"{LOG_PREFIX} Interactor is null.");
            return;
        }

        // Find player by tag since Interactor may be on Camera, not Player
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player == null)
        {
            Debug.LogWarning($"{LOG_PREFIX} No Player found in scene.");
            SendMessage("MEDIC: I can't seem to find you.");
            return;
        }

        cachedInventory = player.GetComponent<Inventory>();
        if (cachedInventory == null)
        {
            Debug.LogWarning($"{LOG_PREFIX} No Inventory found on Player.");
            SendMessage("MEDIC: My supplies aren't available right now.");
            return;
        }

        PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
        if (playerHealth == null)
        {
            Debug.LogWarning($"{LOG_PREFIX} No PlayerHealth found on Player.");
            SendMessage("MEDIC: I can't seem to help you right now.");
            return;
        }

        if (playerHealth.IsDead)
        {
            Debug.Log($"{LOG_PREFIX} Player is dead. Cannot heal.");
            return;
        }

        if (lastKnownMaxHealth > 0f && lastKnownHealth >= lastKnownMaxHealth)
        {
            Debug.Log($"{LOG_PREFIX} Player is at full health. No healing needed.");
            return;
        }

        if (!cachedInventory.SpendPoints(healCost))
        {
            Debug.Log($"{LOG_PREFIX} Player cannot afford healing. Cost: {healCost}, Has: {cachedInventory.CurrentPoints}");
            return;
        }

        playerHealth.Heal(healAmount);
        SendMessage($"MEDIC: Healed you for {healAmount} HP! Cost: {healCost} points.");
        Debug.Log($"{LOG_PREFIX} Healed player for {healAmount} HP. Charged {healCost} points.");
    }
}
