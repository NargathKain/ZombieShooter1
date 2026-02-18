# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview

3rd-person zombie shooter game built in Unity 6000.2.8f1 with Universal Render Pipeline (URP). Uses Synty AnimationBaseLocomotion package for player controller and input handling.

## Build & Run

- Open in Unity 6000.2.8f1 or compatible version
- The project uses URP - render pipeline assets are in ProjectSettings
- Play mode: Open a scene containing the player prefab and press Play in Unity Editor
- No external build tools required; standard Unity build process via File > Build Settings

## Project Structure

All game scripts are in `Assets/_ProjectFiles/Scripts/` organized by domain:
- `Data/` - ScriptableObjects and interfaces (WeaponData, AmmoType enum, IPickupable)
- `Damage/` - Health systems and damage interfaces (PlayerHealth, EnemyHealth, IDamageable)
- `Player/` - Player systems (Inventory, WeaponController, Bullet)
- `Enemies/` - Enemy AI and data (EnemyAI, EnemyData, RandomDrop)
- `NPC/` - Interactive NPCs (NPCBase, TutorialNPC, ShopkeeperNPC, MedicNPC)
- `UI/` - HUD components (AmmoUI, PointsUI, ReloadPromptUI, GameOverScreen)
- `Managers/` - Game managers (PauseManager, WaveManager)
- `Interaction/` - Interaction system (IInteractable, Interactor, TextSign)

Synty input system scripts are at `Assets/Synty/AnimationBaseLocomotion/Samples/Scripts/InputSystem/`.

## Architecture

### Event-Driven Communication

The codebase uses static events for decoupled communication between systems:

**Inventory Events:**
- `OnWeaponEquipped(WeaponData)` - Consumed by WeaponController, AmmoUI, ReloadPromptUI
- `OnAmmoChanged(AmmoType, backpack, magazine)` - Consumed by AmmoUI, ReloadPromptUI
- `OnReloadStarted/OnReloadCompleted` - Consumed by AmmoUI, ReloadPromptUI
- `OnPointsChanged(total)` / `OnPointsGained(amount, total)` - Consumed by PointsUI

**Health Events:**
- `PlayerHealth.OnHealthChanged(current, max)` - Consumed by PlayerHealthUI, MedicNPC
- `PlayerHealth.OnPlayerDeath` - Consumed by GameOverScreen
- `EnemyHealth.OnEnemyDeath(EnemyHealth)` - Consumed by Inventory (points), RandomDrop (loot)

**Input Events (from Synty InputReader):**
- onAimActivated/Deactivated, onShootPerformed/Started/Canceled
- onReloadPerformed, onWeaponScrollPerformed, onInteractPerformed, onPausePerformed

### Dual Ammo System

Ammunition is tracked in two pools:
1. **Magazine ammo** - Per-weapon, stored in `Dictionary<WeaponData, int>`
2. **Backpack ammo** - Per-AmmoType, shared pool stored in `Dictionary<AmmoType, int>`

Reload transfers ammo from backpack to magazine over time (coroutine).

### Dependency Layers

Scripts depend only on layers above them:
```
Layer 0 (Foundation): Interfaces (IDamageable, IInteractable, IPickupable), Enums (AmmoType)
Layer 1: EnemyData, WeaponData (ScriptableObjects)
Layer 2: Health systems, Inventory, Interactor
Layer 3: NPCs, WeaponController, EnemyAI
Layer 4: UI components (subscribe to events only)
```

### Key Patterns

- **ScriptableObjects for data**: WeaponData and EnemyData define weapon/enemy stats
- **Interfaces for polymorphism**: IDamageable for anything that takes damage, IInteractable for NPCs/objects
- **Static events for UI**: UI components subscribe in OnEnable, unsubscribe in OnDisable
- **Singleton for HitmarkerDisplay**: Accessed via `HitmarkerDisplay.Instance`

## Required Unity Tags

- `"Player"` - On player root GameObject (required for enemy AI targeting and pickups)
- `"MainCamera"` - On main camera (required for WeaponController aiming)
- `"Interactable"` - On NPCs and interactive objects (required for Interactor raycast detection)

## Key Components on Player

The player GameObject requires:
- InputReader (from Synty)
- PlayerHealth
- Inventory (manages weapons, ammo, points)
- WeaponController (handles shooting, aiming, reload input)
- Interactor (handles E-key interactions)

## Console Log Prefixes

Each system uses a prefix for filtering: `[Inventory]`, `[WeaponController]`, `[Bullet]`, `[RandomDrop]`, `[PauseManager]`, `[GameOverScreen]`, `[NPC]`, `[ShopkeeperNPC]`, `[MedicNPC]`, `[PointsUI]`, `[ReloadPromptUI]`

## Incomplete Systems

- `WaveManager.cs` - Planned but not fully implemented. Intended for configurable enemy wave spawning.
