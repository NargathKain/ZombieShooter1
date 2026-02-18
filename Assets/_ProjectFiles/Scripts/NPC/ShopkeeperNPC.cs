using UnityEngine;

/// <summary>
/// NPC shopkeeper that sells weapons to the player in exchange for points.
/// Cycles through available (unowned) weapon offerings on each interaction.
/// If the player can afford the current offering, it is purchased immediately.
/// <para>
/// Setup:
/// <list type="bullet">
/// <item>Populate weaponOfferings in the Inspector with WeaponData, cost, and description.</item>
/// <item>Set bonusAmmoOnPurchase for the ammo amount granted with each weapon.</item>
/// <item>Requires Inventory on the player root (points are stored in Inventory).</item>
/// </list>
/// </para>
/// </summary>
public class ShopkeeperNPC : NPCBase
{
    private const string LOG_PREFIX = "[ShopkeeperNPC]";

    [System.Serializable]
    public class WeaponOffering
    {
        public WeaponData weapon;
        public int cost;
        [TextArea(1, 3)]
        public string description;
    }

    [SerializeField] private WeaponOffering[] weaponOfferings;
    [SerializeField] private int bonusAmmoOnPurchase = 60;

    private int browseIndex;
    private Inventory cachedInventory;

    protected override string GetInteractionMessage()
    {
        if (!ValidateSetup())
        {
            return "SHOPKEEPER: Sorry, I'm not set up for business right now.";
        }

        WeaponOffering offering = FindNextUnownedOffering();

        if (offering == null)
        {
            return "SHOPKEEPER: You already own everything! Come back when I get new stock.";
        }

        int currentPoints = (cachedInventory != null) ? cachedInventory.CurrentPoints : 0;

        if (currentPoints >= offering.cost)
        {
            return $"SHOPKEEPER: {offering.weapon.weaponName} - {offering.description}\n" +
                   $"Cost: {offering.cost} points (You have: {currentPoints})\n" +
                   $"Purchasing...";
        }

        return $"SHOPKEEPER: {offering.weapon.weaponName} - {offering.description}\n" +
               $"Cost: {offering.cost} points (You have: {currentPoints})\n" +
               $"Not enough points! Kill more zombies!";
    }

    protected override void PerformInteraction(Interactor interactor)
    {
        if (!ValidateSetup()) return;

        // Find player by tag since Interactor may be on Camera, not Player
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
            cachedInventory = player.GetComponent<Inventory>();

        if (cachedInventory == null)
        {
            Debug.LogError($"{LOG_PREFIX} No Inventory found on Player.");
            SendMessage("SHOPKEEPER: I can't seem to find your inventory...");
            return;
        }

        WeaponOffering offering = FindNextUnownedOffering();

        if (offering == null)
        {
            Debug.Log($"{LOG_PREFIX} Player owns all weapons.");
            AdvanceBrowseIndex();
            return;
        }

        AttemptPurchase(offering);
        AdvanceBrowseIndex();
    }

    private void AttemptPurchase(WeaponOffering offering)
    {
        if (offering == null || offering.weapon == null) return;

        if (!cachedInventory.SpendPoints(offering.cost))
        {
            Debug.Log($"{LOG_PREFIX} Purchase failed: cannot afford {offering.weapon.weaponName} " +
                      $"(Cost: {offering.cost}, Has: {cachedInventory.CurrentPoints})");
            return;
        }

        cachedInventory.AddWeapon(offering.weapon);
        cachedInventory.AddAmmo(offering.weapon.ammoType, bonusAmmoOnPurchase);

        string successMessage = $"SHOPKEEPER: Sold! {offering.weapon.weaponName} is yours.\n" +
                                $"+{bonusAmmoOnPurchase} {offering.weapon.ammoType} ammo included.\n" +
                                $"Remaining points: {cachedInventory.CurrentPoints}";

        SendMessage(successMessage);

        Debug.Log($"{LOG_PREFIX} Sold {offering.weapon.weaponName} for {offering.cost} pts. " +
                  $"Bonus ammo: +{bonusAmmoOnPurchase} {offering.weapon.ammoType}.");
    }

    /// <summary>
    /// Finds the next unowned weapon offering starting from browseIndex.
    /// Wraps around the array once to check all offerings.
    /// Returns null if the player owns every offered weapon.
    /// </summary>
    private WeaponOffering FindNextUnownedOffering()
    {
        if (weaponOfferings == null || weaponOfferings.Length == 0) return null;
        if (cachedInventory == null) return null;

        int count = weaponOfferings.Length;

        for (int i = 0; i < count; i++)
        {
            int index = (browseIndex + i) % count;
            WeaponOffering offering = weaponOfferings[index];

            if (offering == null || offering.weapon == null)
            {
                Debug.LogWarning($"{LOG_PREFIX} Null offering or weapon at index {index}. Skipping.");
                continue;
            }

            if (!cachedInventory.HasWeapon(offering.weapon))
            {
                browseIndex = index;
                return offering;
            }
        }

        return null;
    }

    private void AdvanceBrowseIndex()
    {
        if (weaponOfferings != null && weaponOfferings.Length > 0)
        {
            browseIndex = (browseIndex + 1) % weaponOfferings.Length;
        }
    }

    /// <summary>
    /// Validates that all required dependencies and configuration are present.
    /// Called at the start of both GetInteractionMessage and PerformInteraction
    /// because GetInteractionMessage runs before PerformInteraction can cache the inventory.
    /// </summary>
    private bool ValidateSetup()
    {
        if (weaponOfferings == null || weaponOfferings.Length == 0)
        {
            Debug.LogWarning($"{LOG_PREFIX} No weapon offerings configured.");
            return false;
        }

        return true;
    }

    /// <summary>
    /// Called by NPCBase when interaction starts, before GetInteractionMessage.
    /// We cache the inventory here so GetInteractionMessage can check weapon ownership.
    /// </summary>
    public override void OnInteract(Interactor interactor)
    {
        // Find player by tag since Interactor may be on Camera, not Player
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
            cachedInventory = player.GetComponent<Inventory>();

        if (cachedInventory == null)
        {
            Debug.LogError($"{LOG_PREFIX} No Inventory found on Player.");
        }

        base.OnInteract(interactor);
    }

    public override void OnEndInteract()
    {
        cachedInventory = null;
        base.OnEndInteract();
    }
}
