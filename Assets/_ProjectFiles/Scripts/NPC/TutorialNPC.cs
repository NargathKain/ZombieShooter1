using UnityEngine;

/// <summary>
/// NPC that cycles through an array of gameplay tips on each interaction.
/// Wraps back to the first tip after all tips have been shown.
/// <para>
/// Example tips:
/// <list type="bullet">
/// <item>"Use RIGHT MOUSE to aim, LEFT MOUSE to shoot. You can only fire while aiming!"</item>
/// <item>"Press R to reload. Your weapon auto-reloads when the magazine empties."</item>
/// <item>"Use SCROLL WHEEL to switch weapons. Make sure you've unlocked them first!"</item>
/// <item>"Kill zombies to earn points. Visit the Shopkeeper to buy new weapons."</item>
/// <item>"The Medic can heal you if you're low on health. It costs points though!"</item>
/// </list>
/// </para>
/// </summary>
public class TutorialNPC : NPCBase
{
    [SerializeField, TextArea(2, 5)]
    private string[] tips;

    private int currentTipIndex;

    protected override string GetInteractionMessage()
    {
        if (tips == null || tips.Length == 0)
        {
            Debug.LogWarning("[TutorialNPC] No tips configured.");
            return "...";
        }

        return tips[currentTipIndex];
    }

    protected override void PerformInteraction(Interactor interactor)
    {
        if (tips == null || tips.Length == 0) return;

        Debug.Log($"[TutorialNPC] Showed tip {currentTipIndex + 1}/{tips.Length}: \"{tips[currentTipIndex]}\"");

        currentTipIndex = (currentTipIndex + 1) % tips.Length;
    }
}
