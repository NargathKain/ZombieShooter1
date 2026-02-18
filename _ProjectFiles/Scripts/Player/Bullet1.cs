using UnityEngine;

/// <summary>
/// Simple visual bullet projectile
/// </summary>
public class Bullet1 : MonoBehaviour
{
    [Header("Bullet Settings")]
    [SerializeField] private float speed = 100f;
    [SerializeField] private float lifetime = 3f;
    [Tooltip("Rotation offset to align prefab with movement direction")]
    [SerializeField] private Vector3 rotationOffset= new Vector3(90f, 0f, 0f);

    private Vector3 moveDirection;

    public void Initialize(Vector3 direction)
    {
        moveDirection = direction.normalized;

        // Point bullet toward target, then apply offset for prefab orientation
        transform.rotation = Quaternion.LookRotation(direction) * Quaternion.Euler(rotationOffset);

        Destroy(gameObject, lifetime);
    }

    void Update()
    {
        transform.position += moveDirection * speed * Time.deltaTime;
    }
}