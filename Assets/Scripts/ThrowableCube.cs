using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

/// <summary>
/// Makes a cube grabbable and throwable with the XR Interaction Toolkit.
/// Add it to any cube: Rigidbody and XRGrabInteractable are added automatically.
/// On release, the cube keeps the linear and angular velocity of the hand.
/// </summary>
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(XRGrabInteractable))]
public class ThrowableCube : MonoBehaviour
{
    [Header("Throw tuning")]
    [Tooltip("Multiplies the hand's linear velocity at release.")]
    [SerializeField] private float throwVelocityScale = 1.5f;

    [Tooltip("Multiplies the hand's angular velocity at release (spin).")]
    [SerializeField] private float throwAngularVelocityScale = 1.5f;

    [Tooltip("Seconds of hand motion averaged at release. Lower = more reactive.")]
    [SerializeField] private float throwSmoothingDuration = 0.1f;

    private void Awake()
    {
        // Rigidbody: no damping, so the cube keeps its motion until it hits something.
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.mass = 0.3f;
        rb.useGravity = true;
        rb.linearDamping = 0f;
        rb.angularDamping = 0f;
        rb.maxAngularVelocity = 50f; // Unity's default (7 rad/s) cuts spin short.
        rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
        rb.interpolation = RigidbodyInterpolation.Interpolate;

        // XRGrabInteractable: follow the hand with physics and apply throw velocity on release.
        XRGrabInteractable grab = GetComponent<XRGrabInteractable>();
        grab.movementType = XRBaseInteractable.MovementType.VelocityTracking;
        grab.useDynamicAttach = true; // grab the cube where you touch it
        grab.throwOnDetach = true;
        grab.throwSmoothingDuration = throwSmoothingDuration;
        grab.throwVelocityScale = throwVelocityScale;
        grab.throwAngularVelocityScale = throwAngularVelocityScale;
    }
}
