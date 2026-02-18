using UnityEngine;

/// <summary>
/// Pivots a spotlight back and forth around a pivot point on the Y axis.
/// The spotlight swings like it's mounted on a rotating base.
/// </summary>
public class RotatingSpotlight : MonoBehaviour
{
    [Header("Pivot Settings")]
    [Tooltip("The pivot point to rotate around. If not set, uses this object's position.")]
    [SerializeField] private Transform pivotPoint;

    [Tooltip("Maximum rotation angle from starting rotation (degrees).")]
    [SerializeField] private float rotationAngle = 20f;

    [Tooltip("Speed of the rotation oscillation.")]
    [SerializeField] private float rotationSpeed = 1f;

    [Tooltip("Randomize start time so multiple lights don't sync.")]
    [SerializeField] private bool randomizeStart = true;

    private float startYRotation;
    private float timeOffset;
    private Vector3 pivotPos;

    private void Start()
    {
        // Use assigned pivot or self position
        pivotPos = pivotPoint != null ? pivotPoint.position : transform.position;
        startYRotation = transform.eulerAngles.y;
        timeOffset = randomizeStart ? Random.Range(0f, Mathf.PI * 2f) : 0f;
    }

    private void Update()
    {
        // Calculate target Y rotation
        float oscillation = Mathf.Sin((Time.time * rotationSpeed) + timeOffset);
        float targetY = startYRotation + (oscillation * rotationAngle);

        // Update pivot position if pivot transform moves
        if (pivotPoint != null)
            pivotPos = pivotPoint.position;

        // Rotate around the pivot point on Y axis
        Vector3 currentEuler = transform.eulerAngles;
        float deltaY = targetY - currentEuler.y;

        transform.RotateAround(pivotPos, Vector3.up, deltaY);
    }
}
