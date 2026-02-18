using UnityEngine;
using Synty.AnimationBaseLocomotion.Samples.InputSystem;

/// <summary>
/// Simple script to test all input actions from InputReader
/// Logs messages to Console when buttons are pressed
/// Works with New Input System only (no old Input class)
/// TEMPORARY - Remove this once you confirm inputs work, or just disable the component
/// </summary>
public class InputTester : MonoBehaviour
{
    private InputReader inputReader;
    private bool isAiming = false;
    private bool isShooting = false;

    void Start()
    {
        // Get the InputReader component (should be on same GameObject)
        inputReader = GetComponent<InputReader>();

        if (inputReader == null)
        {
            Debug.LogError("InputReader component not found! Make sure this script is on the same GameObject as InputReader.");
            return;
        }

        // Subscribe to shoot events (started/performed/canceled)
        inputReader.onShootStarted += OnShootStarted;
        inputReader.onShootPerformed += OnShootPerformed;
        inputReader.onShootCanceled += OnShootCanceled;
        
        // Subscribe to other gameplay events
        inputReader.onReloadPerformed += OnReloadPressed;
        inputReader.onInteractPerformed += OnInteractPressed;
        inputReader.onPausePerformed += OnPausePressed;
        inputReader.onWeaponScrollPerformed += OnWeaponScroll;
        
        // Subscribe to SYNTY's Aim events (zoom)
        inputReader.onAimActivated += OnAimStart;
        inputReader.onAimDeactivated += OnAimEnd;

        Debug.Log("=== INPUT TESTER READY ===");
        Debug.Log("Left Mouse = Shoot (watch for started/performed/canceled)");
        Debug.Log("R = Reload");
        Debug.Log("E = Interact");
        Debug.Log("Escape = Pause");
        Debug.Log("Mouse Scroll = Weapon Switch");
        Debug.Log("Right Mouse = Aim/Zoom (hold)");
    }

    void OnDestroy()
    {
        // CRITICAL: Always unsubscribe to prevent memory leaks
        if (inputReader != null)
        {
            inputReader.onShootStarted -= OnShootStarted;
            inputReader.onShootPerformed -= OnShootPerformed;
            inputReader.onShootCanceled -= OnShootCanceled;
            inputReader.onReloadPerformed -= OnReloadPressed;
            inputReader.onInteractPerformed -= OnInteractPressed;
            inputReader.onPausePerformed -= OnPausePressed;
            inputReader.onWeaponScrollPerformed -= OnWeaponScroll;
            inputReader.onAimActivated -= OnAimStart;
            inputReader.onAimDeactivated -= OnAimEnd;
        }
    }

    void Update()
    {
        // Show aiming status in real-time (comment out if too spammy)
        if (isAiming)
        {
            Debug.Log(">>> AIMING (Right Mouse held) <<<");
        }

        // Show shooting status in real-time (comment out if too spammy)
        if (isShooting)
        {
            Debug.Log("🔫🔫🔫 SHOOTING (Left Mouse held) 🔫🔫🔫");
        }
    }

    // ====== SHOOT CALLBACKS ======

    void OnShootStarted()
    {
        isShooting = true;
        Debug.Log("🔫 SHOOT STARTED (Left Mouse pressed down)");
    }

    void OnShootPerformed()
    {
        Debug.Log("✓ SHOOT PERFORMED (Shot fired!)");
    }

    void OnShootCanceled()
    {
        isShooting = false;
        Debug.Log("🔫 SHOOT CANCELED (Left Mouse released)");
    }

    // ====== OTHER CALLBACKS ======

    void OnReloadPressed()
    {
        Debug.Log("✓ RELOAD pressed (R key)");
    }

    void OnInteractPressed()
    {
        Debug.Log("✓ INTERACT pressed (E key)");
    }

    void OnPausePressed()
    {
        Debug.Log("✓ PAUSE pressed (Escape key)");
    }

    void OnWeaponScroll(float scrollDelta)
    {
        if (scrollDelta > 0)
        {
            Debug.Log("✓ WEAPON SCROLL UP (scroll up = next weapon)");
        }
        else if (scrollDelta < 0)
        {
            Debug.Log("✓ WEAPON SCROLL DOWN (scroll down = previous weapon)");
        }
    }

    void OnAimStart()
    {
        isAiming = true;
        Debug.Log("✓✓✓ AIM STARTED (Right Mouse pressed) - ZOOMING IN ✓✓✓");
    }

    void OnAimEnd()
    {
        isAiming = false;
        Debug.Log("✓✓✓ AIM ENDED (Right Mouse released) - ZOOM OUT ✓✓✓");
    }
}
