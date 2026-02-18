using System;
using UnityEngine;

/// <summary>
/// Abstract base class for all NPC types (Tutorial, Shopkeeper, Medic).
/// Implements IInteractable following the same pattern as TextSign.
/// Provides common indicator logic, interactor caching, and helper methods.
/// Subclasses must override <see cref="GetInteractionMessage"/> and <see cref="PerformInteraction"/>.
/// <para>
/// Setup: Tag the NPC GameObject as "Interactable" and assign the indicator in the Inspector.
/// </para>
/// </summary>
public abstract class NPCBase : MonoBehaviour, IInteractable
{
    [SerializeField] private GameObject indicator;
    [SerializeField] private string npcName;

    protected Interactor cachedInteractor;

    // --- IInteractable Implementation (mirrors TextSign pattern) ---

    public virtual void OnInteract(Interactor interactor)
    {
        indicator.SetActive(false);
        cachedInteractor = interactor;

        string message = GetInteractionMessage();
        interactor.ReceiveInteract(message);

        PerformInteraction(interactor);

        Debug.Log($"[NPC] {npcName} interaction started.");
    }

    public virtual void OnEndInteract()
    {
        Debug.Log($"[NPC] {npcName} interaction ended.");
        cachedInteractor = null;
    }

    public void OnReadyInteract()
    {
        indicator.SetActive(true);
    }

    public void OnAbortInteract()
    {
        indicator.SetActive(false);
    }

    // --- Abstract Methods (each NPC type implements these) ---

    protected abstract string GetInteractionMessage();

    protected abstract void PerformInteraction(Interactor interactor);

    // --- Helper Methods ---

    protected void SendMessage(string message)
    {
        if (cachedInteractor != null)
        {
            cachedInteractor.ReceiveInteract(message);
        }
        else
        {
            Debug.LogWarning($"[NPC] {npcName}: SendMessage called with no cached interactor.");
        }
    }

    protected void EndInteraction()
    {
        if (cachedInteractor != null)
        {
            cachedInteractor.EndInteract(this);
        }
        else
        {
            Debug.LogWarning($"[NPC] {npcName}: EndInteraction called with no cached interactor.");
        }
    }
}
