using UnityEngine;

/// <summary>
/// MonoBehaviour attached to pickup objects in the world.
/// Auto-collects when the player walks into the trigger collider.
/// Works with any PickupData subclass (AmmoPickupData, HealthPickupData, etc.)
///
/// <para><b>Setup:</b></para>
/// <list type="number">
///   <item>Create a pickup prefab (e.g. a small glowing cube/sphere).</item>
///   <item>Add a Collider set to Is Trigger = true.</item>
///   <item>Attach this script and a concrete PickupData_Modular component
///         (HealthPickupData, AmmoPickupData, or WeaponPickupData).</item>
///   <item>Assign the PickupData reference in the Inspector (or leave blank to auto-detect).</item>
///   <item>Player must be tagged "Player".</item>
/// </list>
/// </summary>
public class PickupItem : MonoBehaviour
{
    private const string LOG_PREFIX = "[PickupItem]";

    [Header("Pickup Configuration")]
    [Tooltip("The pickup data component. If empty, auto-detects on this GameObject.")]
    [SerializeField] private PickupData_Modular pickupData;

    [Header("Visual Feedback")]
    [SerializeField] private bool rotatePickup = true;
    [SerializeField] private float rotationSpeed = 50f;
    [SerializeField] private bool bobUpDown = true;
    [SerializeField] private float bobHeight = 0.3f;
    [SerializeField] private float bobSpeed = 2f;

    private Vector3 startPosition;
    private float bobTimer;
    private bool collected;

    void Start()
    {
        if (pickupData == null)
            pickupData = GetComponent<PickupData_Modular>();

        if (pickupData == null)
        {
            Debug.LogError($"{LOG_PREFIX} No PickupData_Modular on {gameObject.name}. " +
                           "Add a HealthPickupData, AmmoPickupData, or WeaponPickupData component.", this);
            enabled = false;
            return;
        }

        startPosition = transform.position;
    }

    void Update()
    {
        AnimatePickup();
    }

    /// <summary>
    /// Auto-collects when the player enters the trigger collider.
    /// </summary>
    private void OnTriggerEnter(Collider other)
    {
        if (collected) return;
        if (other == null) return;

        // Only the player can pick things up
        if (!other.CompareTag("Player"))
        {
            // Also check root in case the trigger hit a child collider
            if (!other.transform.root.CompareTag("Player"))
                return;
        }

        CollectPickup(other.transform.root.gameObject);
    }

    private void CollectPickup(GameObject player)
    {
        if (pickupData == null || player == null) return;

        collected = true;

        pickupData.OnPickedUp(player);

        if (pickupData.pickupSound != null)
            AudioSource.PlayClipAtPoint(pickupData.pickupSound, transform.position);

        if (pickupData.pickupEffect != null)
        {
            GameObject effect = Instantiate(pickupData.pickupEffect, transform.position, Quaternion.identity);
            Destroy(effect, 3f);
        }

        Debug.Log($"{LOG_PREFIX} {pickupData.pickupName} collected by {player.name}");
        Destroy(gameObject);
    }

    private void AnimatePickup()
    {
        if (rotatePickup)
            transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);

        if (bobUpDown)
        {
            bobTimer += Time.deltaTime * bobSpeed;
            float newY = startPosition.y + Mathf.Sin(bobTimer) * bobHeight;
            transform.position = new Vector3(transform.position.x, newY, transform.position.z);
        }
    }

    public string GetPickupName()
    {
        return pickupData != null ? pickupData.pickupName : "Unknown";
    }

    public string GetPickupPromptText()
    {
        return pickupData != null ? pickupData.GetPickupPromptText() : "Pickup";
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, 0.5f);
    }
}