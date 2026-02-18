// Copyright (c) 2024 Synty Studios Limited. All rights reserved.
//
// Use of this software is subject to the terms and conditions of the Synty Studios End User Licence Agreement (EULA)
// available at: https://syntystore.com/pages/end-user-licence-agreement
//
// Sample scripts are included only as examples and are not intended as production-ready.

using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

namespace Synty.AnimationBaseLocomotion.Samples.InputSystem
{
    public class InputReader : MonoBehaviour, Controls.IPlayerActions
    {
        public Vector2 _mouseDelta;
        public Vector2 _moveComposite;

        public float _movementInputDuration;
        public bool _movementInputDetected;

        private Controls _controls;

        public Action onAimActivated;
        public Action onAimDeactivated;

        public Action onCrouchActivated;
        public Action onCrouchDeactivated;

        public Action onJumpPerformed;

        public Action onLockOnToggled;

        public Action onSprintActivated;
        public Action onSprintDeactivated;

        public Action onWalkToggled;

        // ====== YOUR GAMEPLAY ACTIONS ======
        // Shoot actions - started, performed (held), and canceled (released)
        public Action onShootStarted;   // When button first pressed
        public Action onShootPerformed; // When button triggers (can fire repeatedly if held)
        public Action onShootCanceled;  // When button released
        public Action onReloadPerformed;
        public Action onInteractPerformed;
        public Action onPausePerformed;
        public Action<float> onWeaponScrollPerformed; // Passes scroll delta as float

        /// <inheritdoc cref="OnEnable" />
        private void OnEnable()
        {
            if (_controls == null)
            {
                _controls = new Controls();
                _controls.Player.SetCallbacks(this);
            }

            _controls.Player.Enable();
        }

        /// <inheritdoc cref="OnDisable" />
        public void OnDisable()
        {
            _controls.Player.Disable();
        }

        /// <summary>
        ///     Defines the action to perform when the OnLook callback is called.
        /// </summary>
        /// <param name="context">The context of the callback.</param>
        public void OnLook(InputAction.CallbackContext context)
        {
            _mouseDelta = context.ReadValue<Vector2>();
        }

        /// <summary>
        ///     Defines the action to perform when the OnMove callback is called.
        /// </summary>
        /// <param name="context">The context of the callback.</param>
        public void OnMove(InputAction.CallbackContext context)
        {
            _moveComposite = context.ReadValue<Vector2>();
            _movementInputDetected = _moveComposite.magnitude > 0;
        }

        /// <summary>
        ///     Defines the action to perform when the OnJump callback is called.
        /// </summary>
        /// <param name="context">The context of the callback.</param>
        public void OnJump(InputAction.CallbackContext context)
        {
            if (!context.performed)
            {
                return;
            }

            onJumpPerformed?.Invoke();
        }

        /// <summary>
        ///     Defines the action to perform when the OnToggleWalk callback is called.
        /// </summary>
        /// <param name="context">The context of the callback.</param>
        public void OnToggleWalk(InputAction.CallbackContext context)
        {
            if (!context.performed)
            {
                return;
            }

            onWalkToggled?.Invoke();
        }

        /// <summary>
        ///     Defines the action to perform when the OnSprint callback is called.
        /// </summary>
        /// <param name="context">The context of the callback.</param>
        public void OnSprint(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                onSprintActivated?.Invoke();
            }
            else if (context.canceled)
            {
                onSprintDeactivated?.Invoke();
            }
        }

        /// <summary>
        ///     Defines the action to perform when the OnCrouch callback is called.
        /// </summary>
        /// <param name="context">The context of the callback.</param>
        public void OnCrouch(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                onCrouchActivated?.Invoke();
            }
            else if (context.canceled)
            {
                onCrouchDeactivated?.Invoke();
            }
        }

        /// <summary>
        ///     Defines the action to perform when the OnAim callback is called.
        /// </summary>
        /// <param name="context">The context of the callback.</param>
        public void OnAim(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                onAimActivated?.Invoke();
            }

            if (context.canceled)
            {
                onAimDeactivated?.Invoke();
            }
        }

        /// <summary>
        ///     Defines the action to perform when the OnLockOn callback is called.
        /// </summary>
        /// <param name="context">The context of the callback.</param>
        public void OnLockOn(InputAction.CallbackContext context)
        {
            if (!context.performed)
            {
                return;
            }

            onLockOnToggled?.Invoke();
            onSprintDeactivated?.Invoke();
        }


        // ====== YOUR NEW CALLBACK IMPLEMENTATIONS ======

        /// <summary>
        /// Called when the Shoot action is performed (Left Mouse Button)
        /// </summary>
        public void OnShoot(InputAction.CallbackContext context)
        {
            // Started = button first pressed down
            if (context.started)
            {
                onShootStarted?.Invoke();
            }

            // Performed = button action triggered (happens once per click)
            if (context.performed)
            {
                onShootPerformed?.Invoke();
            }

            // Canceled = button released
            if (context.canceled)
            {
                onShootCanceled?.Invoke();
            }
        }

        /// <summary>
        /// Called when the Reload action is performed (R key)
        /// </summary>
        public void OnReload(InputAction.CallbackContext context)
        {
            if (!context.performed)
            {
                return;
            }

            onReloadPerformed?.Invoke();
        }

        /// <summary>
        /// Called when the Interact action is performed (E key)
        /// </summary>
        public void OnInteract(InputAction.CallbackContext context)
        {
            if (!context.performed)
            {
                return;
            }

            onInteractPerformed?.Invoke();
        }

        /// <summary>
        /// Called when the Pause action is performed (Escape key)
        /// </summary>
        public void OnPause(InputAction.CallbackContext context)
        {
            if (!context.performed)
            {
                return;
            }

            onPausePerformed?.Invoke();
        }

        /// <summary>
        /// Called when mouse scroll wheel is moved (weapon switching)
        /// Passes the scroll delta as a float value
        /// </summary>
        public void OnWeaponScroll(InputAction.CallbackContext context)
        {
            float scrollDelta = context.ReadValue<float>();

            // Only invoke if there's actual scrolling (ignore tiny noise)
            if (Mathf.Abs(scrollDelta) > 0.01f)
            {
                onWeaponScrollPerformed?.Invoke(scrollDelta);
            }
        }
    }
}
