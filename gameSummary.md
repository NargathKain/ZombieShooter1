# Zombie Shooter - Project Summary

## Overview
3rd-person zombie shooter built in **Unity 6000.2.8f1** with **Universal Render Pipeline (URP)**. Uses Synty AnimationBaseLocomotion package for player controller and **New Input System**.

## Core Systems

### Player
- **WeaponController**: Handles shooting, aiming (FOV zoom), weapon switching via scroll wheel
- **Inventory**: Manages weapons, ammo (dual system: magazine + backpack), and points
- **PlayerHealth**: Health system with events for UI/NPC integration
- **PlayerKeys**: Static key inventory for door/key interactions

### Weapons
- **WeaponData** (ScriptableObject): Defines weapon stats, prefabs, ammo type
- **Bullet**: Physics-based projectile with damage, shooter exclusion, impact effects
- **Animation Rigging**: Each weapon has a rig; switching weapons changes rig weights and enables/disables weapon GameObjects

### Enemies
- **EnemyAI**: NavMeshAgent-based AI that chases and attacks player
- **EnemyHealth**: Health system with death events
- **EnemyData** (ScriptableObject): Enemy stats and drop chance
- **Animator driven by AI**: Speed parameter controls walk/idle transitions

### Interaction System (E key)
- **Interactor**: On camera, raycasts for "Interactable" tagged objects
- **IInteractable**: Interface with OnInteract, OnReadyInteract, OnAbortInteract, OnEndInteract
- **InteractorUI**: Shows interaction messages
- Implementations: TextSign, KeyPickup, LockedDoor, NPCs

### NPCs
- **NPCBase**: Abstract base implementing IInteractable
- **MedicNPC**: Heals player for points
- **ShopkeeperNPC**: Sells weapons for points
- **TutorialNPC**: Displays tutorial messages

### Pickups (auto-collect on trigger)
- **PickupItem**: Base pickup behavior (bob, rotate, collect on trigger)
- **HealthPickupData**, **AmmoPickupData**, **WeaponPickupData**: Pickup types
- **RandomDrop**: Spawns pickups when enemies die (raycasts to ground)

### Managers
- **WaveManager**: Spawns enemy waves (configurable delays, or instant spawn)
- **PauseManager**: Handles game pause

### UI
- **AmmoUI**: Displays magazine/backpack ammo
- **PointsUI**: Shows points with gain animations
- **PlayerHealthUI**: Health bar
- **ReloadPromptUI**: Shows reload prompt when low/empty
- **GameOverScreen**: Death screen
- **HitmarkerDisplay**: Singleton hitmarker feedback

## Key Architecture Patterns
- **Static events** for decoupled communication (Inventory.OnWeaponEquipped, PlayerHealth.OnHealthChanged, etc.)
- **Interfaces** for polymorphism (IDamageable, IInteractable, IPickupable)
- **ScriptableObjects** for data (WeaponData, EnemyData)
- **New Input System** via Synty's InputReader

## Required Tags
- `"Player"` - Player root GameObject
- `"MainCamera"` - Main camera
- `"Interactable"` - Interactive objects (NPCs, doors, keys, signs)

## Important Notes
- **Interactor is on the Camera**, not the player body (for correct raycast direction)
- NPCs find player components via `GameObject.FindGameObjectWithTag("Player")` since Interactor isn't a child of Player
- Weapon aiming raycasts from camera center to find aim target, then calculates direction from shootPoint to that target
