using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// MonoBehaviour attached to pickup objects in the world.
/// Handles detection, interaction, and collection of pickups.
/// Works with any PickupData subclass (AmmoPickupData, HealthPickupData, etc.)
/// </summary>
public class PickupItem : MonoBehaviour
{
    [Header("Pickup Configuration")]
    [SerializeField] private PickupData_Modular pickupData;

    [Header("Detection Settings")]
    [SerializeField] private float detectionRadius = 2f;
    [SerializeField] private LayerMask playerLayer;

    [Header("Visual Feedback")]
    [SerializeField] private bool rotatePickup = true;
    [SerializeField] private float rotationSpeed = 50f;
    [SerializeField] private bool bobUpDown = true;
    [SerializeField] private float bobHeight = 0.3f;
    [SerializeField] private float bobSpeed = 2f;

    // Runtime state
    private GameObject nearbyPlayer;
    private bool isPlayerNearby;
    private Vector3 startPosition;
    private float bobTimer;

    // addition for queuing pickups to prevent multiple pickups at once
    private static Queue<System.Action> pickupQueue = new Queue<System.Action>();
    private static bool isProcessing = false;

    void Start()
    {
        if (pickupData == null)
        {
            Debug.LogError($"PickupItem on {gameObject.name}: No PickupData assigned!", this);
            enabled = false;
            return;
        }

        startPosition = transform.position;
    }

    void Update()
    {
        // Visual effects
        AnimatePickup();

        // Player detection
        CheckForNearbyPlayer();

        // Handle pickup input
        //if (isPlayerNearby && GameInputReader.Instance != null && GameInputReader.Instance.InteractPressed)
        //{
        //    CollectPickup();
        //}
    }

    /// <summary>
    /// Checks if player is within detection radius using OverlapSphere
    /// </summary>
    private void CheckForNearbyPlayer()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, detectionRadius, playerLayer);

        if (hits.Length > 0)
        {
            nearbyPlayer = hits[0].gameObject;
            isPlayerNearby = true;
        }
        else
        {
            nearbyPlayer = null;
            isPlayerNearby = false;
        }
    }

    /// <summary>
    /// Called when player presses the interact key while nearby
    /// </summary>
    private void CollectPickup()
    {
        if (pickupData == null || nearbyPlayer == null) return;

        // Queue the pickup instead of executing immediately
        pickupQueue.Enqueue(() => {
            pickupData.OnPickedUp(nearbyPlayer);

            // Effects
            if (pickupData.pickupSound != null)
                AudioSource.PlayClipAtPoint(pickupData.pickupSound, transform.position);

            if (pickupData.pickupEffect != null)
            {
                GameObject effect = Instantiate(pickupData.pickupEffect, transform.position, Quaternion.identity);
                Destroy(effect, 3f);
            }
        });

        // Destroy pickup immediately
        Destroy(gameObject);

        // Start processing if not already
        if (!isProcessing)
        {
            StartCoroutine(ProcessPickupQueue());
        }
    }

    private static IEnumerator ProcessPickupQueue()
    {
        isProcessing = true;

        while (pickupQueue.Count > 0)
        {
            System.Action pickup = pickupQueue.Dequeue();
            pickup.Invoke();

            yield return new WaitForSeconds(0.1f); // Small delay between pickups
        }

        isProcessing = false;
    }

    /// <summary>
    /// Makes the pickup rotate and bob up and down
    /// </summary>
    private void AnimatePickup()
    {
        // Rotation
        if (rotatePickup)
        {
            transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
        }

        // Bob up and down
        if (bobUpDown)
        {
            bobTimer += Time.deltaTime * bobSpeed;
            float newY = startPosition.y + Mathf.Sin(bobTimer) * bobHeight;
            transform.position = new Vector3(transform.position.x, newY, transform.position.z);
        }
    }

    /// <summary>
    /// Returns the pickup name for UI display
    /// </summary>
    public string GetPickupName()
    {
        return pickupData != null ? pickupData.pickupName : "Unknown";
    }

    /// <summary>
    /// Returns the pickup prompt text (e.g., "Pistol Ammo (x20)")
    /// </summary>
    public string GetPickupPromptText()
    {
        return pickupData != null ? pickupData.GetPickupPromptText() : "Press E to pick up";
    }

    /// <summary>
    /// Returns true if player is nearby (for UI to show prompt)
    /// </summary>
    public bool IsPlayerNearby()
    {
        return isPlayerNearby;
    }

    // Visualize detection radius in Scene view
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}