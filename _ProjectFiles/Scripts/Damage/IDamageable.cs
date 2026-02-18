using UnityEngine;

/// <summary>
/// Interface for anything that can take damage (Player, Enemies, Destructibles).
/// This allows weapons to damage anything without knowing what it is.
/// </summary>
public interface IDamageable
{
    void TakeDamage(float damage);
}

