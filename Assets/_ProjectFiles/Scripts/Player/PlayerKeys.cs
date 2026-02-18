using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Simple key inventory system. Tracks which keys the player has collected.
/// Uses a static HashSet so keys persist and can be checked from anywhere.
///
/// USAGE:
/// - PlayerKeys.AddKey("RedKey") to collect a key
/// - PlayerKeys.HasKey("RedKey") to check if player has a key
/// - PlayerKeys.RemoveKey("RedKey") to consume/use a key
/// - PlayerKeys.Clear() to reset all keys (e.g., on game restart)
/// </summary>
public static class PlayerKeys
{
    private static readonly HashSet<string> collectedKeys = new HashSet<string>();

    /// <summary>Fired when a new key is collected. Parameter: total key count.</summary>
    public static event Action<int> OnKeyCollected;

    /// <summary>Fired when keys are cleared (e.g., on game restart).</summary>
    public static event Action OnKeysCleared;

    /// <summary>
    /// Adds a key to the player's inventory.
    /// </summary>
    public static void AddKey(string keyID)
    {
        if (string.IsNullOrEmpty(keyID)) return;

        // Only fire event if this is a new key
        if (collectedKeys.Add(keyID))
        {
            Debug.Log($"[PlayerKeys] Collected key: {keyID}");
            OnKeyCollected?.Invoke(collectedKeys.Count);
        }
    }

    /// <summary>
    /// Checks if the player has a specific key.
    /// </summary>
    public static bool HasKey(string keyID)
    {
        if (string.IsNullOrEmpty(keyID)) return false;
        return collectedKeys.Contains(keyID);
    }

    /// <summary>
    /// Removes a key from inventory (use when unlocking a door that consumes the key).
    /// </summary>
    public static bool RemoveKey(string keyID)
    {
        if (string.IsNullOrEmpty(keyID)) return false;

        bool removed = collectedKeys.Remove(keyID);
        if (removed)
            Debug.Log($"[PlayerKeys] Used key: {keyID}");
        return removed;
    }

    /// <summary>
    /// Clears all collected keys. Call on game restart or scene reload.
    /// </summary>
    public static void Clear()
    {
        collectedKeys.Clear();
        Debug.Log("[PlayerKeys] All keys cleared.");
        OnKeysCleared?.Invoke();
    }

    /// <summary>
    /// Returns the number of keys currently held.
    /// </summary>
    public static int KeyCount => collectedKeys.Count;
}
