using UnityEngine;

/// <summary>
/// Simple visual bullet projectile
/// Attach this to your bullet prefab
/// Moves forward and destroys itself after lifetime
/// 
/// SETUP:
/// 1. Create small sphere/capsule as bullet prefab
/// 2. Scale it small (0.1, 0.1, 0.1)
/// 3. Add this script
/// 4. Add Rigidbody (Is Kinematic = true, Use Gravity = false)
/// 5. Optional: Add Trail Renderer for cool effect
/// </summary>
public class Bullet : MonoBehaviour
{
    [Header("Bullet Settings")]
    [Tooltip("How fast the bullet moves")]
    [SerializeField] private float speed = 100f;

    [Tooltip("How long before bullet destroys itself")]
    [SerializeField] private float lifetime = 3f;

    void Start()
    {
        // Destroy bullet after lifetime (in case it doesn't hit anything)
        Destroy(gameObject, lifetime);
    }

    void Update()
    {
        // Move bullet forward
        transform.position += transform.forward * speed * Time.deltaTime;
    }
}