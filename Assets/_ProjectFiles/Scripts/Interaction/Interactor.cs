using UnityEngine;
using Synty.AnimationBaseLocomotion.Samples.InputSystem;

/// <summary>
/// Handles player interaction with IInteractable objects using raycasting.
/// Uses the New Input System via InputReader.
///
/// SETUP:
/// 1. Attach to the Main Camera (so raycast originates from player's view)
/// 2. Assign InputReader reference (from player)
/// 3. Assign InteractorUI and Hint references
/// 4. Interactable objects need tag "Interactable" and implement IInteractable
/// </summary>
public class Interactor : MonoBehaviour
{
    [Header("Output")]
    public IInteractable currentInteractable;
    private IInteractable lastInteractable;

    [Header("Setup")]
    [SerializeField] private string targetTag = "Interactable";
    [SerializeField] private float rayMaxDistance = 20f;
    [SerializeField] private LayerMask layerMask = ~0;
    [SerializeField] private InteractorUI interactorUI;
    [SerializeField] private GameObject hint;

    [Header("Input (New Input System)")]
    [Tooltip("InputReader component from the player. Required for interaction input.")]
    [SerializeField] private InputReader inputReader;

    [Header("Cancel Settings")]
    [Tooltip("If true, moving will cancel the current interaction.")]
    [SerializeField] private bool cancelOnMove = true;
    [Tooltip("Movement threshold to trigger cancel (0-1).")]
    [SerializeField] private float moveCancelThreshold = 0.1f;

    // State
    private bool readyInteract;
    private bool interacting;
    private bool interactInputTriggered;

    void Start()
    {
        currentInteractable = null;
        interacting = false;

        if (interactorUI == null)
            Debug.LogWarning("[Interactor] InteractorUI component was not set.");
        if (hint == null)
            Debug.LogWarning("[Interactor] Hint GameObject was not set.");
        if (inputReader == null)
        {
            inputReader = FindFirstObjectByType<InputReader>();
            if (inputReader == null)
                Debug.LogError("[Interactor] InputReader not found! Interaction will not work.");
        }

        AbortInteract();
    }

    void OnEnable()
    {
        if (inputReader == null)
            inputReader = FindFirstObjectByType<InputReader>();

        if (inputReader != null)
            inputReader.onInteractPerformed += HandleInteractInput;
    }

    void OnDisable()
    {
        if (inputReader != null)
            inputReader.onInteractPerformed -= HandleInteractInput;
    }

    void HandleInteractInput()
    {
        interactInputTriggered = true;
    }

    void Update()
    {
        currentInteractable = null;

        // Raycast to find interactable
        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, rayMaxDistance, layerMask))
        {
            if (hit.collider.CompareTag(targetTag))
            {
                IInteractable interactable = hit.collider.GetComponent<IInteractable>();
                if (interactable != null)
                {
                    currentInteractable = interactable;
                }
            }
        }

        // State transitions
        if (currentInteractable != null && !readyInteract)
        {
            ReadyInteract();
        }
        else if (currentInteractable == null && readyInteract)
        {
            AbortInteract();
        }

        if (!interacting)
        {
            if (readyInteract && interactInputTriggered)
            {
                Interact();
            }
        }
        else
        {
            // Check for cancel conditions
            if (ShouldCancelInteraction())
            {
                EndInteract();
            }
            else if (currentInteractable == null || currentInteractable != lastInteractable)
            {
                EndInteract();
            }
        }

        if (currentInteractable != null)
            lastInteractable = currentInteractable;

        // Reset input flag
        interactInputTriggered = false;
    }

    bool ShouldCancelInteraction()
    {
        if (!cancelOnMove || inputReader == null)
            return false;

        // Check if player is moving using InputReader's move composite
        return inputReader._moveComposite.magnitude > moveCancelThreshold;
    }

    void ReadyInteract()
    {
        readyInteract = true;

        if (hint != null)
            hint.SetActive(true);

        currentInteractable.OnReadyInteract();
    }

    void AbortInteract()
    {
        readyInteract = false;

        if (hint != null)
            hint.SetActive(false);

        if (lastInteractable != null)
            lastInteractable.OnAbortInteract();
    }

    void Interact()
    {
        interacting = true;
        readyInteract = true;

        if (hint != null)
            hint.SetActive(false);

        currentInteractable.OnInteract(this);
    }

    void EndInteract()
    {
        interacting = false;
        readyInteract = false;

        if (lastInteractable != null)
            lastInteractable.OnEndInteract();

        if (interactorUI != null)
            interactorUI.HideTextMessage();
    }

    public void EndInteract(IInteractable requester)
    {
        if (requester == lastInteractable)
            EndInteract();
    }

    public void ReceiveInteract(string message)
    {
        if (interactorUI != null)
            interactorUI.ShowTextMessage(message);
    }

    public void ReceiveInteract(IInteractable interactable)
    {
        Debug.Log(interactable);
    }
}
