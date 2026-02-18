# PDF Documentation Blueprint - Detailed Guide
## Las Vegas Zombie Shooter - Virtual Reality Course

**Target Length**: 15-30 pages
**Format**: PDF (export from Word or Google Docs)

---

# TABLE OF CONTENTS

1. Introduction (1-2 pages)
2. Problem Description (2-3 pages)
3. Development Phases (4-6 pages)
   - 3.1 Analysis
   - 3.2 Design
   - 3.3 Implementation
4. Script Table (3-4 pages)
5. Detailed Script Analysis (4-6 pages)
6. Asset Sources (1-2 pages)
7. User Manual (2-3 pages)
8. Screenshots (3-5 pages)
9. Evaluation Criteria Response (4-6 pages)

---

# 1. INTRODUCTION (Εισαγωγή)
**Length: 1-2 pages**

## Content to Write:

### 1.1 Project Title and Context

```
Las Vegas Apocalypse: Zombie Survival

This project was developed for the Virtual Reality course (Εικονική Πραγματικότητα)
at the University of Piraeus, Department of Informatics, during the academic year
2025-2026. The project implements a tourist destination virtual environment as
specified in Option 2 of the assignment requirements.
```

### 1.2 Team Members

```
Team Members:
- [Name 1] (AM: XXXXX) - Role: [e.g., Player systems, combat mechanics]
- [Name 2] (AM: XXXXX) - Role: [e.g., Enemy AI, wave system]
- [Name 3] (AM: XXXXX) - Role: [e.g., UI design, environment]

Contact: [email] | [phone number]
```

### 1.3 Project Vision

```
Our vision was to create an immersive Las Vegas experience that combines
exploration of an iconic tourist destination with engaging survival gameplay.
Players explore a stylized recreation of the Las Vegas Strip while defending
against zombie hordes, interacting with helpful NPCs, and working toward
escape objectives.

The project demonstrates proficiency in Unity development, C# programming,
and application of virtual reality concepts taught throughout the course.
```

### 1.4 Technology Stack

```
Development Environment:
- Game Engine: Unity 6000.2.8f1
- Render Pipeline: Universal Render Pipeline (URP)
- Programming Language: C#
- IDE: Visual Studio 2022 / Visual Studio Code
- Version Control: Git (optional)

Key Packages & Assets:
- Synty AnimationBaseLocomotion: Player controller and input handling
- Synty POLYGON Packs: 3D models and environment assets
- TextMeshPro: UI text rendering
- Unity Animation Rigging: Procedural weapon handling
- Unity AI Navigation: NavMesh pathfinding for enemies
```

### 1.5 Project Scope

```
Included Features:
- Explorable Las Vegas Strip environment
- Third-person shooter combat with multiple weapons
- Zombie enemy AI with NavMesh pathfinding
- Wave-based enemy spawning system
- Interactive NPC system (Medic, Shopkeeper, Tutorial)
- Inventory and economy system (points-based)
- Key collection and locked door mechanics
- Win condition system (25 kills + 3 keys + escape)
- Complete UI (HUD, menus, game states)
- Audio system with ambient zones

Intentionally Excluded (scope management):
- Multiplayer functionality
- Save/load system
- Crafting mechanics
- Day/night cycle
```

### 1.6 Document Structure

```
This document is organized into nine sections:
- Section 1 (Introduction): Project overview and context
- Section 2 (Problem Description): Design challenges and goals
- Section 3 (Development Phases): Analysis, design, and implementation details
- Section 4 (Script Table): Complete listing of all C# scripts
- Section 5 (Script Analysis): In-depth examination of 5 key scripts
- Section 6 (Asset Sources): Attribution for all external assets
- Section 7 (User Manual): Player guide and controls
- Section 8 (Screenshots): Visual documentation
- Section 9 (Criteria Response): How we address each evaluation criterion
```

---

# 2. PROBLEM DESCRIPTION (Περιγραφή του Προβλήματος)
**Length: 2-3 pages**

## Content to Write:

### 2.1 Assignment Requirements

```
The assignment presented two options for creating a virtual environment:
1. Village (Χωριό) - Ancient, medieval, modern, or sci-fi
2. Tourist Destination (Τουριστικός Προορισμός) - With surrounding environment

We selected Option 2: Tourist Destination, choosing to recreate Las Vegas,
Nevada - one of the world's most recognizable entertainment destinations.
```

### 2.2 Why Las Vegas?

```
Las Vegas was chosen for several strategic reasons:

1. INSTANT RECOGNITION
Las Vegas has a unique visual identity recognized worldwide - neon lights,
casino architecture, desert setting, and entertainment culture. This
recognition helps players immediately connect with the environment.

2. RICH INTERACTIVE POTENTIAL
A tourist destination naturally contains numerous interactive elements:
- Casinos with games and machines
- Hotels and buildings to explore
- Street entertainment and attractions
- Shops and services
This aligns perfectly with the assignment's emphasis on functionality.

3. MANAGEABLE SCOPE
Unlike sprawling natural destinations, Las Vegas's famous "Strip" is a
concentrated area. This allowed us to create a detailed, quality environment
within our development timeline rather than a sparse, expansive one.

4. VISUAL COHESION WITH ASSETS
The Synty POLYGON asset packs provide stylized urban and apocalyptic content
that matches the Vegas aesthetic while maintaining consistent art style.
```

### 2.3 The Zombie Survival Concept

```
DESIGN CHALLENGE:
A pure exploration experience of Las Vegas risks becoming a passive "walking
simulator" without meaningful player engagement. We needed interactive elements
that feel natural to the setting while providing gameplay depth.

OUR SOLUTION:
We framed the experience as "Las Vegas during a zombie apocalypse." This
narrative context provides:

- STAKES: Players have something to lose (health, progress)
- CHALLENGE: Combat requires skill and resource management
- PROGRESSION: Kill counts and key collection create clear goals
- ECONOMY: Points from kills enable purchases from NPCs
- URGENCY: The 90-second escape timer creates tension
- EXPLORATION REWARD: Finding keys, weapons, and ammo throughout the environment

This approach transforms passive sightseeing into active survival while
preserving the Vegas atmosphere and exploration opportunities.
```

### 2.4 Core Design Questions

```
During initial planning, we identified key questions our design must answer:

Q1: How do we make exploration rewarding?
A: Hidden keys, weapon pickups, and ammo scattered throughout encourage
   thorough exploration. Finding resources before buying from shops saves points.

Q2: How do we balance combat difficulty?
A: Wave-based spawning with progressive difficulty. Early waves are manageable;
   later waves challenge even prepared players. NPCs provide healing and upgrades.

Q3: How do we guide players without being heavy-handed?
A: Tutorial NPC near spawn explains mechanics. Visual landmarks guide navigation.
   UI shows objective progress (kills, keys) without explicit waypoints.

Q4: How do we create satisfying progression?
A: Multiple weapon types with different characteristics. Point accumulation for
   purchases. Clear win conditions with visible progress tracking.

Q5: How do we handle performance with many enemies?
A: Object pooling for zombies and bullets. Staggered NavMesh updates.
   Occlusion culling for indoor areas. LOD groups for distant objects.
```

### 2.5 User Experience Goals

```
We want players to experience:

IMMERSION
- Feel present in a recognizable Las Vegas environment
- Atmospheric lighting and audio enhance believability
- Consistent visual style maintains suspension of disbelief

ENGAGEMENT
- Combat is responsive and satisfying
- Clear feedback for all actions (hitmarkers, sounds, UI updates)
- Meaningful choices (spend points now or save for better weapon?)

ACCOMPLISHMENT
- Clear objectives with visible progress
- Escalating challenge provides sense of mastery
- Victory feels earned after meeting all conditions

AGENCY
- Freedom to explore environment
- Multiple valid strategies (aggressive vs. defensive play)
- Player choices affect outcomes
```

### 2.6 Success Criteria

```
We defined success as:

TECHNICAL SUCCESS:
✓ Stable 60 FPS performance with 20+ active enemies
✓ No game-breaking bugs or crashes
✓ All systems function as designed
✓ Clean, maintainable codebase

DESIGN SUCCESS:
✓ Players understand objectives without external guidance
✓ Combat feels responsive and fair
✓ Environment is interesting to explore
✓ Difficulty curve is challenging but not frustrating

ACADEMIC SUCCESS:
✓ Demonstrates techniques taught in course
✓ Addresses all 11 evaluation criteria
✓ Well-documented and commented code
✓ Complete deliverables (build, video, presentation, documentation)
```

---

# 3. DEVELOPMENT PHASES (Ανάλυση-Σχεδίαση-Υλοποίηση)
**Length: 4-6 pages**

## 3.1 Analysis Phase (Ανάλυση)

### 3.1.1 Requirements Analysis

```
ASSIGNMENT REQUIREMENTS BREAKDOWN:

From the evaluation criteria, we identified these requirements:

1. Elements Taught in Class
   - Must use: NavMesh, animations, particles, post-processing
   - Optimization techniques especially valued

2. Realism (Αληθοφάνεια)
   - Physical laws must be respected
   - No floating objects or impossible movement

3. Content (Περιεχόμενο)
   - Based on real-world reference (Las Vegas)
   - Proper asset attribution

4. Completeness (Πληρότητα)
   - Full 3D navigable environment
   - Lighting, decorations, functionality
   - Dynamic/animated elements

5. Design (Σχεδιασμός)
   - Clean architecture
   - Appropriate complexity

6. Aesthetics (Αισθητική)
   - Visually appealing
   - Inviting to users

7. Originality (Πρωτοτυπία)
   - Explore Unity and C# capabilities

8. Usability (Χρηστικότητα)
   - Intuitive controls
   - Serves user needs

9. Animation (Κίνηση)
   - Dynamic, evolving world
   - Multiple animation types

10. Functionality (Λειτουργικότητα)
    - Interactive elements
    - Reactive systems

11. Development (Ανάπτυξη)
    - Professional coding practices
    - Well-commented code
```

### 3.1.2 Feature Prioritization

```
MUST-HAVE (Minimum Viable Product):
- Player movement and camera control
- Basic shooting mechanics
- At least one weapon type
- Zombie enemies with AI
- Player health system
- Basic HUD (health, ammo)
- Game over on death
- Main menu

SHOULD-HAVE (Core Experience):
- Multiple weapon types
- Weapon switching
- Reload mechanics
- Point/economy system
- NPC interactions (Medic, Shop)
- Wave spawning system
- Pause menu
- Settings (audio, graphics)

NICE-TO-HAVE (Polish):
- Win conditions (kills + keys)
- Escape timer mechanic
- Victory screen
- Key/door system
- Audio zones
- Tutorial NPC
- Button sound effects
- Patrol NPCs
```

### 3.1.3 Technical Research

```
RESEARCH CONDUCTED:

Unity Systems:
- NavMesh documentation for AI pathfinding
- New Input System for flexible controls
- Animation Rigging for procedural weapon handling
- URP post-processing for visual effects
- Audio system for 3D spatial sound

Design Patterns:
- Singleton pattern for managers
- Observer pattern for events
- State pattern for AI behavior
- Object pooling for performance

Similar Projects Analysis:
- Studied zombie survival games for mechanics inspiration
- Analyzed shooter UI conventions
- Reviewed NPC interaction patterns
```

## 3.2 Design Phase (Σχεδίαση)

### 3.2.1 System Architecture

```
ARCHITECTURE OVERVIEW:

Our architecture follows an event-driven, component-based design:

┌─────────────────────────────────────────────────────────────┐
│                      INPUT LAYER                            │
│  InputReader (Synty) → Player Input Events                  │
└─────────────────────────────────────────────────────────────┘
                              ↓
┌─────────────────────────────────────────────────────────────┐
│                    PLAYER SYSTEMS                           │
│  ┌─────────────┐  ┌─────────────┐  ┌─────────────┐        │
│  │ Inventory   │  │WeaponControl│  │PlayerHealth │        │
│  │ - Weapons   │  │ - Shooting  │  │ - Damage    │        │
│  │ - Ammo      │  │ - Aiming    │  │ - Healing   │        │
│  │ - Points    │  │ - Switching │  │ - Death     │        │
│  └─────────────┘  └─────────────┘  └─────────────┘        │
└─────────────────────────────────────────────────────────────┘
                              ↓ Events
┌─────────────────────────────────────────────────────────────┐
│                    GAME SYSTEMS                             │
│  ┌─────────────┐  ┌─────────────┐  ┌─────────────┐        │
│  │ WaveManager │  │WinCondition │  │PauseManager │        │
│  │ - Spawning  │  │ - Kills     │  │ - Time stop │        │
│  │ - Waves     │  │ - Keys      │  │ - Menu      │        │
│  │ - Difficulty│  │ - Timer     │  │ - Cursor    │        │
│  └─────────────┘  └─────────────┘  └─────────────┘        │
└─────────────────────────────────────────────────────────────┘
                              ↓ Events
┌─────────────────────────────────────────────────────────────┐
│                      UI LAYER                               │
│  AmmoUI │ PointsUI │ HealthUI │ KillCountUI │ KeyCountUI  │
│  GameOverScreen │ VictoryScreen │ EscapeTimerUI           │
└─────────────────────────────────────────────────────────────┘
```

### 3.2.2 Event System Design

```
EVENT-DRIVEN COMMUNICATION:

We use C# events for decoupled communication between systems:

INVENTORY EVENTS:
- OnWeaponEquipped(WeaponData) → WeaponController, AmmoUI
- OnAmmoChanged(AmmoType, backpack, magazine) → AmmoUI, ReloadPromptUI
- OnPointsChanged(total) → PointsUI
- OnPointsGained(amount, total) → PointsUI (animated feedback)

HEALTH EVENTS:
- PlayerHealth.OnHealthChanged(current, max) → PlayerHealthUI
- PlayerHealth.OnPlayerDeath → GameOverScreen
- PlayerHealth.OnPlayerRespawn → GameOverScreen (hide)
- EnemyHealth.OnEnemyDeath(EnemyHealth) → Inventory (points), RandomDrop

WIN CONDITION EVENTS:
- WinConditionManager.OnKillCountChanged(current, required) → KillCountUI
- WinConditionManager.OnKeyCountChanged(current, required) → KeyCountUI
- WinConditionManager.OnWinConditionsMet → EscapeTimerUI (show timer)
- WinConditionManager.OnEscapeTimerTick(remaining) → EscapeTimerUI
- WinConditionManager.OnEscapeTimerExpired → GameOverScreen
- WinConditionManager.OnVictory → VictoryScreen

KEY EVENTS:
- PlayerKeys.OnKeyCollected(count) → WinConditionManager, KeyCountUI

This architecture allows:
- UI components to update without knowing about game logic
- Systems to be tested in isolation
- Easy addition of new features without modifying existing code
```

### 3.2.3 Win Condition Flow Design

```
WIN CONDITION SYSTEM FLOW:

┌─────────────┐     ┌─────────────┐
│ Kill Zombie │     │ Collect Key │
└──────┬──────┘     └──────┬──────┘
       ↓                   ↓
┌──────────────────────────────────┐
│      WinConditionManager         │
│  - Track kills (0/25)            │
│  - Track keys (0/3)              │
└──────────────┬───────────────────┘
               ↓
        ┌──────────────┐
        │ Both Complete│
        │ 25 kills AND │
        │ 3 keys?      │
        └──────┬───────┘
               ↓ YES
┌──────────────────────────────────┐
│     Start 90-Second Timer        │
│  - Show EscapeTimerUI            │
│  - "Find the Vault Door!"        │
└──────────────┬───────────────────┘
               ↓
     ┌─────────┴─────────┐
     ↓                   ↓
┌─────────┐        ┌─────────┐
│ Timer   │        │ Player  │
│ Expires │        │ Opens   │
│         │        │ Vault   │
└────┬────┘        └────┬────┘
     ↓                  ↓
┌─────────┐        ┌─────────┐
│ GAME    │        │ VICTORY │
│ OVER    │        │         │
└─────────┘        └─────────┘
```

### 3.2.4 Interaction System Design

```
INTERACTION SYSTEM:

We designed a flexible interaction system using interfaces:

┌─────────────────────────────────────────────────┐
│              IInteractable Interface            │
├─────────────────────────────────────────────────┤
│ + OnReadyInteract(Interactor) : string          │
│   Called when player looks at object            │
│   Returns prompt text to display                │
│                                                 │
│ + OnInteract(Interactor)                        │
│   Called when player presses E                  │
│                                                 │
│ + OnAbortInteract(Interactor)                   │
│   Called when player looks away                 │
│                                                 │
│ + OnEndInteract(Interactor)                     │
│   Called when interaction completes             │
└─────────────────────────────────────────────────┘
                      ↑
                 Implemented by:
    ┌─────────┬─────────┬─────────┬─────────┐
    │ NPCBase │VaultDoor│ KeyPickup│TextSign │
    │ -Medic  │         │         │         │
    │ -Shop   │         │         │         │
    │ -Tutorial│        │         │         │
    └─────────┴─────────┴─────────┴─────────┘

The Interactor component (on camera) raycasts forward, detects
"Interactable" tagged objects, and calls appropriate interface methods.
```

## 3.3 Implementation Phase (Υλοποίηση)

### 3.3.1 Development Timeline

```
WEEK 1-2: PROJECT FOUNDATION
- Unity project setup with URP
- Import Synty assets and configure
- Integrate AnimationBaseLocomotion player controller
- Basic scene setup with Vegas environment
- Camera configuration

WEEK 3-4: CORE PLAYER SYSTEMS
- WeaponController implementation
- Shooting mechanics with raycasting
- Bullet spawning and physics
- Inventory system for weapons and ammo
- PlayerHealth with damage/healing
- Basic HUD (health, ammo display)

WEEK 5-6: ENEMY SYSTEMS
- EnemyAI with NavMeshAgent
- Detection, chase, and attack behaviors
- EnemyHealth with IDamageable
- Death handling and point rewards
- WaveManager for spawning

WEEK 7-8: INTERACTIONS & NPCs
- IInteractable interface design
- Interactor raycasting system
- NPCBase abstract class
- MedicNPC, ShopkeeperNPC, TutorialNPC
- Pickup system (ammo, health, weapons, keys)
- Door/key mechanics

WEEK 9-10: WIN CONDITIONS & POLISH
- WinConditionManager implementation
- Kill and key tracking
- Escape timer system
- VaultDoor interaction
- VictoryScreen and GameOverScreen
- Audio zones and proximity audio
- Button sounds and menu polish

WEEK 11-12: TESTING & DOCUMENTATION
- Bug fixing and balance tuning
- Performance optimization
- Code commenting and cleanup
- Documentation writing
- Video recording
- PowerPoint creation
```

### 3.3.2 Major Challenges and Solutions

```
CHALLENGE 1: WEAPON AIMING ACCURACY

Problem:
Initially, bullets didn't go where the camera was aiming. Using the weapon's
forward direction caused shots to miss the crosshair target.

Solution:
Implemented dual raycast approach:
1. Raycast from camera center to find world aim point
2. Calculate direction from weapon muzzle to that aim point
3. Spawn bullet with calculated direction

Code concept:
Ray aimRay = camera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
Vector3 aimPoint = Physics.Raycast(aimRay, out hit) ? hit.point : aimRay.GetPoint(500f);
Vector3 shootDirection = (aimPoint - weaponMuzzle.position).normalized;

─────────────────────────────────────────────────────────────

CHALLENGE 2: NPC GROUND STICKING

Problem:
SimplePatrolNPC characters would sink into the ground over time while walking
between waypoints. Without NavMesh (for simple patrol), no automatic grounding.

Solution:
Added raycast-based ground detection:
- Cast ray downward from NPC position
- Snap Y position to hit point plus offset
- Runs every frame during movement

Code concept:
if (Physics.Raycast(transform.position + Vector3.up, Vector3.down, out hit, 2f))
{
    Vector3 pos = transform.position;
    pos.y = hit.point.y + groundOffset;
    transform.position = pos;
}

─────────────────────────────────────────────────────────────

CHALLENGE 3: UI PANELS NOT APPEARING

Problem:
EscapeTimerUI and VictoryScreen wouldn't show when events fired. Scripts
weren't receiving events.

Root Cause:
Parent GameObjects were disabled, so OnEnable() never ran and events
weren't subscribed.

Solution:
Restructured UI hierarchy:
- Script's GameObject stays ENABLED
- Child Panel object is disabled by default
- Script subscribes to events in OnEnable
- Script enables/disables the child Panel, not itself

─────────────────────────────────────────────────────────────

CHALLENGE 4: MENU BUTTONS NOT CLICKABLE

Problem:
Buttons in Pause, GameOver, and Victory screens couldn't be clicked.
OnClick events in Inspector weren't working reliably.

Solution:
Wired button listeners through code instead of Inspector:

private void Start()
{
    if (resumeButton != null)
        resumeButton.onClick.AddListener(Resume);
    if (mainMenuButton != null)
        mainMenuButton.onClick.AddListener(HandleMainMenu);
    if (exitButton != null)
        exitButton.onClick.AddListener(HandleExit);
}

private void OnDestroy()
{
    // Clean up listeners
    if (resumeButton != null)
        resumeButton.onClick.RemoveListener(Resume);
    // ... etc
}

─────────────────────────────────────────────────────────────

CHALLENGE 5: AMMO PICKUP NOT FINDING INVENTORY

Problem:
AmmoPickupData couldn't find Inventory component on player. GetComponent
returned null.

Root Cause:
Inventory was on a child object of the player, not the root.

Solution:
Added fallback component search:

Inventory inventory = player.GetComponent<Inventory>();
if (inventory == null)
    inventory = player.GetComponentInParent<Inventory>();
if (inventory == null)
    inventory = player.GetComponentInChildren<Inventory>();

Applied same pattern to HealthPickupData and WeaponPickupData.
```

### 3.3.3 Optimization Techniques

```
OPTIMIZATIONS IMPLEMENTED:

1. OBJECT POOLING (Planned/Partial)
   - Pre-allocate zombie and bullet objects
   - Reuse instead of Instantiate/Destroy
   - Eliminates garbage collection spikes

2. NAVMESH OPTIMIZATION
   - Stagger path recalculation timing
   - Reduce update frequency for distant enemies
   - Use simpler paths when possible

3. OCCLUSION CULLING
   - Configured for casino interior areas
   - Objects behind walls not rendered
   - Significant FPS improvement indoors

4. LAYER-BASED COLLISION
   - Configured collision matrix
   - Enemies don't collide with each other
   - Bullets only check relevant layers

5. COMPONENT CACHING
   - GetComponent results stored in variables
   - No repeated GetComponent calls in Update

6. EVENT-BASED UPDATES
   - UI updates only when values change
   - No polling in Update loops
   - Reduces unnecessary processing
```

---

# 4. SCRIPT TABLE (Πίνακας Scripts)
**Length: 3-4 pages**

```
Complete table of all C# scripts in Assets/_ProjectFiles/Scripts/
```

## Player Systems

| Script | File Path | Description |
|--------|-----------|-------------|
| **WeaponController.cs** | Player/ | Central combat manager handling shooting, aiming (FOV zoom), weapon switching via scroll wheel, and reload input. Raycasts from camera to determine aim target, spawns bullets from weapon muzzle position. Subscribes to InputReader events for fire, aim, reload, and weapon scroll inputs. |
| **Inventory.cs** | Player/ | Manages player's weapon collection, dual ammo system (magazine per-weapon, backpack per-ammo-type), and points currency. Provides methods for adding/removing weapons, consuming and adding ammo, spending and gaining points. Fires events for UI updates when any value changes. |
| **PlayerHealth.cs** | Damage/ | Tracks player health value with maximum cap. Handles damage application and healing. Fires OnHealthChanged event for UI updates and OnPlayerDeath event for game over state. Implements damage cooldown to prevent instant death from multiple hits. |
| **PlayerKeys.cs** | Player/ | Static class managing collected keys for door/key system. Tracks which key IDs player has collected. Fires OnKeyCollected event for WinConditionManager and KeyCountUI. Provides Clear() method for scene reset. |
| **Bullet.cs** | Player/ | Physics-based projectile spawned when weapons fire. Moves via Rigidbody velocity, applies damage to objects implementing IDamageable interface. Excludes damage to shooter GameObject. Spawns impact effects on collision and destroys itself after set lifetime or on hit. |

## Enemy Systems

| Script | File Path | Description |
|--------|-----------|-------------|
| **EnemyAI.cs** | Enemies/ | Controls zombie behavior using Unity's NavMeshAgent for pathfinding. Implements detection (player enters range), chase (navigate toward player), and attack (deal damage when close) states. Updates Animator parameters for walk/idle/attack animations. Handles death state by disabling AI components. |
| **EnemyHealth.cs** | Damage/ | Manages enemy health pool and implements IDamageable interface. Receives damage from bullets, updates internal health value. When health reaches zero, fires static OnEnemyDeath event consumed by Inventory (awards points) and RandomDrop (spawns loot). Disables collider on death. |
| **EnemyData.cs** | Enemies/ | ScriptableObject defining enemy statistics including max health, movement speed, attack damage, attack cooldown, detection range, and attack range. Allows balancing enemy difficulty through Inspector without code changes. Referenced by EnemyAI and EnemyHealth at runtime. |
| **RandomDrop.cs** | Enemies/ | Attached to enemies, subscribes to EnemyHealth death. On death, rolls random chance to spawn pickup item from configured loot table. Supports weighted drop chances for different pickup types. |

## Interaction Systems

| Script | File Path | Description |
|--------|-----------|-------------|
| **Interactor.cs** | Interaction/ | Attached to main camera, performs raycast each frame to detect objects tagged "Interactable". When raycast hits interactable, calls OnReadyInteract to get prompt text. When player presses E (via InputReader), calls OnInteract. When raycast leaves object, calls OnAbortInteract. Central hub of interaction system. |
| **IInteractable.cs** | Data/ | Interface defining contract for all interactive objects. Methods: OnReadyInteract(Interactor) returns prompt string, OnInteract(Interactor) performs interaction, OnAbortInteract(Interactor) cancels pending interaction, OnEndInteract(Interactor) signals completion. |
| **InteractorUI.cs** | UI/ | Displays interaction prompts to player. Listens to Interactor for current target, shows/hides prompt panel, updates prompt text (e.g., "Press E to talk", "Locked - need 3 keys"). Positioned at screen center or near crosshair. |

## NPC Systems

| Script | File Path | Description |
|--------|-----------|-------------|
| **NPCBase.cs** | NPC/ | Abstract base class for all NPC types implementing IInteractable. Handles common functionality: player detection, interaction prompts, look-at-player behavior. Child classes override OnInteract() to define specific NPC services. Provides ReceiveInteract() for sending messages back to player. |
| **MedicNPC.cs** | NPC/ | Extends NPCBase to provide healing service. On interaction, checks if player health is below maximum and player has sufficient points (200). If conditions met, restores health to full and deducts points. Displays contextual messages: "Healed!", "Already at full health", "Need 200 points". |
| **ShopkeeperNPC.cs** | NPC/ | Extends NPCBase for weapon sales. Displays available weapons with point costs. On purchase interaction, verifies player has enough points, adds weapon to Inventory, deducts cost. Shows feedback for successful purchase or insufficient funds. |
| **TutorialNPC.cs** | NPC/ | Extends NPCBase to display tutorial information. On interaction, shows panel explaining game mechanics: controls, objectives, NPC locations. Free service, no point cost. Helps onboard new players. |
| **SimplePatrolNPC.cs** | NPC/ | Non-combat NPC that patrols between waypoints without NavMesh. Supports Loop, PingPong, and Random patrol modes. Includes ground-sticking via raycast to prevent sinking. Optional Animator integration for walk/idle states. Configurable movement speed and wait time at waypoints. |

## Manager Systems

| Script | File Path | Description |
|--------|-----------|-------------|
| **PauseManager.cs** | Managers/ | Handles game pause state triggered by Escape key (via InputReader.onPausePerformed). Shows pause panel, sets Time.timeScale to 0, unlocks and shows cursor. Resume button restores time and hides cursor. Main Menu button loads MainMenu scene. Exit button quits application. |
| **WinConditionManager.cs** | Managers/ | Singleton tracking win condition progress. Subscribes to EnemyHealth.OnEnemyDeath to count kills (target: 25). Subscribes to PlayerKeys.OnKeyCollected to count keys (target: 3). When both conditions met, starts 90-second escape countdown. Fires events for UI updates and victory/game-over states. |
| **WaveManager.cs** | Managers/ | Controls enemy wave spawning. Configurable wave count, enemies per wave, spawn interval, and difficulty scaling. Spawns enemies at designated spawn points. Tracks active enemy count. Can trigger next wave on timer or when all enemies defeated. |
| **GameInitializer.cs** | Managers/ | Runs on scene load to reset static game state. Clears PlayerKeys, resets WinConditionManager counters, ensures Time.timeScale is 1. Prevents state bleeding between scene loads. |

## UI Systems

| Script | File Path | Description |
|--------|-----------|-------------|
| **AmmoUI.cs** | UI/ | Displays current weapon's ammo status. Shows magazine count and backpack reserve. Subscribes to Inventory.OnAmmoChanged and OnWeaponEquipped events. Format: "30 / 90" (magazine / backpack). Changes color when low ammo. |
| **PointsUI.cs** | UI/ | Shows player's point total. Subscribes to Inventory.OnPointsChanged for updates. Optional animated feedback on OnPointsGained showing "+100" floating text. Points used for shop purchases and healing. |
| **PlayerHealthUI.cs** | UI/ | Health bar display subscribing to PlayerHealth.OnHealthChanged. Shows current/max health as filled bar. Color transitions from green (healthy) to yellow (wounded) to red (critical). Optional damage vignette effect. |
| **KillCountUI.cs** | UI/ | Displays kill progress toward win condition. Format: "15/25". Subscribes to WinConditionManager.OnKillCountChanged. Text turns green when target reached. |
| **KeyCountUI.cs** | UI/ | Displays key collection progress. Format: "2/3". Subscribes to WinConditionManager.OnKeyCountChanged (via PlayerKeys.OnKeyCollected). Text turns green when all keys collected. |
| **EscapeTimerUI.cs** | UI/ | Shows 90-second countdown after win conditions met. Hidden by default, appears on WinConditionManager.OnWinConditionsMet. Updates each second via OnEscapeTimerTick. Format: "1:30" (MM:SS). Turns red when under 10 seconds. |
| **GameOverScreen.cs** | UI/ | Full-screen panel shown on player death or timer expiry. Subscribes to PlayerHealth.OnPlayerDeath and WinConditionManager.OnEscapeTimerExpired. Freezes time, shows cursor. Buttons: Restart (reload scene), Main Menu, Exit. |
| **VictoryScreen.cs** | UI/ | Full-screen panel shown when player escapes through vault door. Subscribes to WinConditionManager.OnVictory. Freezes time, shows cursor. Displays victory message. Buttons: Main Menu, Exit. |
| **ReloadPromptUI.cs** | UI/ | Shows "Press R to Reload" prompt when magazine is empty but backpack has ammo. Subscribes to Inventory ammo events. Helps players know when reload is needed and possible. |
| **HitmarkerDisplay.cs** | UI/ | Singleton displaying brief crosshair hitmarker when bullets hit enemies. Called by WeaponController on confirmed hit. Fades in/out quickly for feedback without obstruction. |
| **ButtonSound.cs** | UI/ | Plays click sound when UI button is pressed. Attach to any Button GameObject, assign AudioClip. Uses AudioSource.PlayClipAtPoint for simplicity. |

## Pickup Systems

| Script | File Path | Description |
|--------|-----------|-------------|
| **IPickupable.cs** | Data/ | Interface for pickup behavior. Method: OnPickup(GameObject player) called when player collects item. Implemented by pickup data ScriptableObjects. |
| **AmmoPickupData.cs** | Data/ | ScriptableObject defining ammo pickup behavior. Specifies AmmoType and amount to add. OnPickup finds player's Inventory and calls AddBackpackAmmo(). Searches parent and children for Inventory component. |
| **HealthPickupData.cs** | Data/ | ScriptableObject defining health pickup behavior. Specifies heal amount. OnPickup finds PlayerHealth and calls Heal(). Searches component hierarchy for PlayerHealth. |
| **WeaponPickupData.cs** | Data/ | ScriptableObject defining weapon pickup behavior. References WeaponData to add. OnPickup finds Inventory and calls AddWeapon(). Optionally includes starting ammo. |
| **KeyPickup.cs** | Interaction/ | Implements IInteractable for key collection. On interact, adds keyID to PlayerKeys static collection. Plays pickup sound and destroys self. Each key has unique ID matching door requirements. |
| **PickupData_Modular.cs** | Data/ | Abstract ScriptableObject base class for modular pickup system. Defines pickup icon, name, and abstract ApplyPickup method. Extended by specific pickup types. |

## Audio Systems

| Script | File Path | Description |
|--------|-----------|-------------|
| **AudioZone.cs** | Audio/ or Scripts/ | Trigger-based ambient audio for room areas. When player enters trigger collider, fades in assigned AudioSource. When player exits, fades out. Used for vault room buzzing, casino ambiance. Configurable fade duration. |
| **ProximityAudio.cs** | Audio/ or Scripts/ | Distance-based audio that plays when player approaches. Used for key proximity music hints. Calculates distance to player, adjusts volume accordingly. Can loop or play once. |

## Door Systems

| Script | File Path | Description |
|--------|-----------|-------------|
| **VaultDoor.cs** | Interaction/ | Implements IInteractable for the escape vault door. On interact, checks WinConditionManager.ConditionsMet (25 kills + 3 keys). If not met, displays progress message. If met, plays open animation (90° pivot), waits, then triggers WinConditionManager.TriggerVictory(). |

## Data / ScriptableObjects

| Script | File Path | Description |
|--------|-----------|-------------|
| **WeaponData.cs** | Data/ | ScriptableObject storing weapon configuration: damage per shot, fire rate (shots/second), magazine capacity, ammo type (enum), weapon prefab reference, UI icon sprite, reload time. One asset per weapon type. |
| **AmmoType.cs** | Data/ | Enum defining ammunition categories: Pistol, Rifle, Shotgun, etc. Used by WeaponData and Inventory to track ammo pools. Allows weapons to share ammo types. |
| **IDamageable.cs** | Data/ | Interface for objects that can receive damage. Method: TakeDamage(float amount, GameObject source). Implemented by PlayerHealth, EnemyHealth, and any destructible objects. |

## Menu Systems

| Script | File Path | Description |
|--------|-----------|-------------|
| **MainMenuController.cs** | UI/ or Managers/ | Main menu scene controller. New Game button loads game scene. Settings button opens settings panel. Exit button quits application. Handles cursor visibility and button navigation. |

---

# 5. DETAILED SCRIPT ANALYSIS (4-5 Σημαντικότερα Scripts)
**Length: 4-6 pages (approximately 1 page per script)**

## 5.1 WeaponController.cs

### Purpose
WeaponController is the central combat system managing all weapon-related player actions. It handles shooting, aiming, reloading, and weapon switching - the core gameplay mechanics that players interact with most frequently.

### Architectural Role
```
INPUT (InputReader)
       ↓
WEAPON CONTROLLER ←→ INVENTORY (ammo, weapons)
       ↓
    BULLETS → ENEMIES (damage)
       ↓
    UI EVENTS → AmmoUI, HitmarkerDisplay
```

WeaponController bridges input, inventory, and world systems. It receives input events, queries inventory for ammo/weapon state, spawns bullets into the world, and fires events for UI feedback.

### Key Design Decisions

**Decision 1: Camera-based aiming with weapon-origin bullets**
Bullets must visually come from the weapon but hit where the camera aims. We raycast from camera center to find the aim point, then calculate bullet direction from weapon muzzle to that point.

**Decision 2: Event-driven ammo updates**
Rather than polling inventory each frame, we subscribe to ammo change events and cache current values. This reduces coupling and improves performance.

**Decision 3: Fire rate limiting**
We track `nextFireTime` and compare against `Time.time` to prevent firing faster than weapon's fire rate, even if player clicks rapidly.

### Key Methods Explained

```csharp
/// <summary>
/// Handles weapon firing when fire input is active.
/// Ελέγχει την πυροδότηση όπλου όταν το input πυρός είναι ενεργό.
/// </summary>
private void HandleShooting()
{
    // Check fire rate timing - prevent firing too fast
    // Έλεγχος χρονισμού ρυθμού πυρός
    if (Time.time < nextFireTime) return;

    // Check magazine has ammo
    // Έλεγχος αν το γεμιστήρα έχει πυρομαχικά
    int magazineAmmo = inventory.GetMagazineAmmo(currentWeapon);
    if (magazineAmmo <= 0)
    {
        // Play empty click, prompt reload
        return;
    }

    // Step 1: Find where camera is aiming in world space
    // Βήμα 1: Εύρεση σημείου στόχευσης κάμερας
    Ray aimRay = mainCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
    Vector3 aimPoint;

    if (Physics.Raycast(aimRay, out RaycastHit hit, maxAimDistance))
        aimPoint = hit.point;
    else
        aimPoint = aimRay.GetPoint(maxAimDistance);

    // Step 2: Calculate direction from weapon to aim point
    // Βήμα 2: Υπολογισμός κατεύθυνσης από όπλο σε στόχο
    Vector3 shootDirection = (aimPoint - shootPoint.position).normalized;

    // Step 3: Spawn and initialize bullet
    // Βήμα 3: Δημιουργία και αρχικοποίηση βλήματος
    GameObject bulletObj = Instantiate(bulletPrefab, shootPoint.position,
                                       Quaternion.LookRotation(shootDirection));
    Bullet bullet = bulletObj.GetComponent<Bullet>();
    bullet.Initialize(currentWeaponData.damage, shootDirection, gameObject);

    // Step 4: Consume ammo and update fire timing
    // Βήμα 4: Κατανάλωση πυρομαχικών και ενημέρωση χρονισμού
    inventory.ConsumeMagazineAmmo(currentWeaponData.ammoType, 1);
    nextFireTime = Time.time + (1f / currentWeaponData.fireRate);

    // Step 5: Effects and feedback
    // Βήμα 5: Εφέ και ανατροφοδότηση
    PlayMuzzleFlash();
    PlayShootSound();
}
```

### Challenges Solved
- **Aiming accuracy**: Dual raycast approach ensures bullets go where crosshair points
- **Smooth weapon switching**: Disable old weapon, enable new weapon, fire equip event
- **Reload timing**: Coroutine handles reload duration, prevents shooting during reload

---

## 5.2 WinConditionManager.cs

### Purpose
WinConditionManager tracks player progress toward victory: 25 zombie kills and 3 key collections. When both conditions are met, it starts a 90-second countdown for the player to reach and open the vault door.

### Architectural Role
```
EnemyHealth.OnEnemyDeath → HandleEnemyDeath() → OnKillCountChanged
PlayerKeys.OnKeyCollected → HandleKeyCollected() → OnKeyCountChanged
                                    ↓
                         CheckWinConditions()
                                    ↓
                         OnWinConditionsMet → Start Timer
                                    ↓
                    ┌───────────────┴───────────────┐
                    ↓                               ↓
            OnEscapeTimerExpired            VaultDoor.TriggerVictory()
                    ↓                               ↓
              GameOverScreen                  OnVictory
                                                    ↓
                                             VictoryScreen
```

### Key Design Decisions

**Decision 1: Singleton pattern**
Only one WinConditionManager should exist. Singleton allows easy access from VaultDoor and UI without dependency injection complexity.

**Decision 2: Event-based updates**
Rather than UI polling for values, manager fires events when counts change. UI subscribes and updates only when needed.

**Decision 3: Separate timer coroutine**
Timer runs as coroutine, firing tick events each second. Allows UI to update countdown display without tight coupling.

### Key Methods Explained

```csharp
/// <summary>
/// Called when an enemy dies. Increments kill count.
/// Καλείται όταν πεθαίνει εχθρός. Αυξάνει τον αριθμό θανατώσεων.
/// </summary>
private void HandleEnemyDeath(EnemyHealth enemy)
{
    currentKills++;
    OnKillCountChanged?.Invoke(currentKills, requiredKills);

    Debug.Log($"[WinConditionManager] Kills: {currentKills}/{requiredKills}");

    CheckWinConditions();
}

/// <summary>
/// Checks if both kill and key conditions are met.
/// Ελέγχει αν πληρούνται και οι δύο συνθήκες.
/// </summary>
private void CheckWinConditions()
{
    if (conditionsMet) return; // Already triggered

    bool killsComplete = currentKills >= requiredKills;
    bool keysComplete = currentKeys >= requiredKeys;

    if (killsComplete && keysComplete)
    {
        conditionsMet = true;
        ConditionsMet = true; // Public static property

        Debug.Log("[WinConditionManager] Win conditions met! Starting escape timer.");
        OnWinConditionsMet?.Invoke();

        StartCoroutine(EscapeTimerCoroutine());
    }
}

/// <summary>
/// 90-second countdown coroutine.
/// Κορουτίνα αντίστροφης μέτρησης 90 δευτερολέπτων.
/// </summary>
private IEnumerator EscapeTimerCoroutine()
{
    float timeRemaining = escapeTimerDuration; // 90 seconds

    while (timeRemaining > 0)
    {
        OnEscapeTimerTick?.Invoke(timeRemaining);
        yield return new WaitForSeconds(1f);
        timeRemaining -= 1f;
    }

    // Timer expired - game over
    // Ο χρόνος έληξε - τέλος παιχνιδιού
    Debug.Log("[WinConditionManager] Escape timer expired!");
    OnEscapeTimerExpired?.Invoke();
}

/// <summary>
/// Called by VaultDoor when player successfully escapes.
/// Καλείται από το VaultDoor όταν ο παίκτης διαφεύγει επιτυχώς.
/// </summary>
public void TriggerVictory()
{
    if (timerCoroutine != null)
        StopCoroutine(timerCoroutine);

    Debug.Log("[WinConditionManager] Victory!");
    OnVictory?.Invoke();
}
```

### Challenges Solved
- **Event subscription timing**: Subscribe in OnEnable, unsubscribe in OnDisable
- **State persistence**: Static ConditionsMet property for VaultDoor access
- **Timer cancellation**: Store coroutine reference to stop on victory

---

## 5.3 Interactor.cs

### Purpose
Interactor enables the player to interact with world objects (NPCs, doors, pickups) using the E key. It raycasts from the camera to detect interactable objects and calls appropriate interface methods.

### Architectural Role
```
Camera (Interactor attached)
         ↓ Raycast
    "Interactable" tagged objects
         ↓
    IInteractable interface
         ↓
┌────────┼────────┬────────┐
↓        ↓        ↓        ↓
NPCs   Doors   Pickups  Signs
```

### Key Design Decisions

**Decision 1: Interface-based detection**
Using IInteractable interface allows any object type to be interactable. New interactable types don't require Interactor modifications.

**Decision 2: Tag-based filtering**
Raycast only considers objects tagged "Interactable". This improves performance by not checking every collider.

**Decision 3: State tracking for abort**
Track current target to call OnAbortInteract when looking away. Provides clean interaction feedback.

### Key Methods Explained

```csharp
/// <summary>
/// Performs raycast each frame to find interactable objects.
/// Εκτελεί raycast κάθε frame για εύρεση αλληλεπιδραστικών αντικειμένων.
/// </summary>
private void Update()
{
    PerformInteractionRaycast();

    // Check for interact input (E key)
    // Έλεγχος για input αλληλεπίδρασης (πλήκτρο E)
    if (interactPressed && currentTarget != null)
    {
        currentTarget.OnInteract(this);
        interactPressed = false;
    }
}

private void PerformInteractionRaycast()
{
    Ray ray = new Ray(transform.position, transform.forward);

    if (Physics.Raycast(ray, out RaycastHit hit, interactionRange))
    {
        // Check if hit object is tagged Interactable
        // Έλεγχος αν το αντικείμενο έχει tag Interactable
        if (hit.collider.CompareTag("Interactable"))
        {
            IInteractable interactable = hit.collider.GetComponent<IInteractable>();

            if (interactable != null)
            {
                // New target or same target?
                // Νέος στόχος ή ίδιος στόχος;
                if (interactable != currentTarget)
                {
                    // Abort previous target if exists
                    // Ακύρωση προηγούμενου στόχου αν υπάρχει
                    if (currentTarget != null)
                        currentTarget.OnAbortInteract(this);

                    // Set new target and get prompt
                    // Ορισμός νέου στόχου και λήψη prompt
                    currentTarget = interactable;
                    string prompt = currentTarget.OnReadyInteract(this);
                    ShowPrompt(prompt);
                }
                return; // Valid target found
            }
        }
    }

    // No valid target - clear current
    // Κανένας έγκυρος στόχος - καθαρισμός
    if (currentTarget != null)
    {
        currentTarget.OnAbortInteract(this);
        currentTarget = null;
        HidePrompt();
    }
}
```

### Challenges Solved
- **Multiple interaction types**: Interface allows NPCs, doors, signs to share system
- **Clean state management**: Track current target, abort when looking away
- **UI integration**: Pass prompt string to InteractorUI for display

---

## 5.4 EnemyAI.cs

### Purpose
EnemyAI controls zombie behavior: detecting the player, chasing them using NavMesh pathfinding, and attacking when in range. It's the core of enemy behavior that creates gameplay challenge.

### Architectural Role
```
EnemyAI
    ├── NavMeshAgent (movement)
    ├── Animator (animations)
    ├── EnemyHealth (subscribes to death)
    └── EnemyData (stats reference)
         ↓
    Player Detection (distance check)
         ↓
    State: IDLE → CHASE → ATTACK
```

### Key Design Decisions

**Decision 1: Distance-based detection**
Simple distance check rather than complex vision cones. Efficient and predictable.

**Decision 2: NavMeshAgent for pathfinding**
Unity's built-in NavMesh handles obstacle avoidance and path calculation.

**Decision 3: State-based behavior**
Clear states (Idle, Chase, Attack) make behavior predictable and debuggable.

### Key Methods Explained

```csharp
/// <summary>
/// Main update loop handling AI state transitions.
/// Κύριος βρόχος ενημέρωσης για μεταβάσεις κατάστασης AI.
/// </summary>
private void Update()
{
    if (isDead) return;

    float distanceToPlayer = Vector3.Distance(transform.position, player.position);

    // State transitions based on distance
    // Μεταβάσεις κατάστασης βάσει απόστασης
    if (distanceToPlayer <= attackRange)
    {
        // In attack range - stop and attack
        // Σε εμβέλεια επίθεσης - στάση και επίθεση
        currentState = AIState.Attack;
        agent.isStopped = true;
        HandleAttack();
    }
    else if (distanceToPlayer <= detectionRange)
    {
        // Detected player - chase
        // Ανίχνευση παίκτη - καταδίωξη
        currentState = AIState.Chase;
        agent.isStopped = false;
        agent.SetDestination(player.position);
    }
    else
    {
        // Player out of range - idle
        // Παίκτης εκτός εμβέλειας - αδράνεια
        currentState = AIState.Idle;
        agent.isStopped = true;
    }

    UpdateAnimator();
}

/// <summary>
/// Handles attack behavior with cooldown.
/// Διαχείριση συμπεριφοράς επίθεσης με cooldown.
/// </summary>
private void HandleAttack()
{
    // Face the player
    // Στροφή προς τον παίκτη
    Vector3 lookDirection = (player.position - transform.position).normalized;
    lookDirection.y = 0;
    transform.rotation = Quaternion.LookRotation(lookDirection);

    // Check attack cooldown
    // Έλεγχος cooldown επίθεσης
    if (Time.time >= nextAttackTime)
    {
        // Deal damage to player
        // Πρόκληση ζημιάς στον παίκτη
        PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
        if (playerHealth != null)
        {
            playerHealth.TakeDamage(enemyData.attackDamage, gameObject);
        }

        // Play attack animation
        animator.SetTrigger("Attack");

        nextAttackTime = Time.time + enemyData.attackCooldown;
    }
}
```

### Challenges Solved
- **Performance with many enemies**: Stagger NavMesh updates, use simple distance checks
- **Animation synchronization**: Update Animator parameters based on agent velocity
- **Death handling**: Disable agent and collider, play death animation

---

## 5.5 Inventory.cs

### Purpose
Inventory manages all player resources: owned weapons, ammunition (dual magazine/backpack system), and points currency. It's the central data store that other systems query and modify.

### Architectural Role
```
         Inventory
    ┌────────┼────────┐
    ↓        ↓        ↓
 Weapons    Ammo    Points
    ↓        ↓        ↓
 Events →  UI Components
```

### Key Design Decisions

**Decision 1: Dual ammo system**
Magazine ammo (per-weapon, limited) and backpack ammo (per-type, reserve). Creates reload mechanic and resource management.

**Decision 2: Event-driven updates**
Fire events when values change rather than exposing mutable state. UI subscribes without tight coupling.

**Decision 3: Dictionary storage**
Dictionaries map WeaponData→magazine ammo and AmmoType→backpack ammo. Flexible for any number of weapons/types.

### Key Methods Explained

```csharp
/// <summary>
/// Consumes ammo from current weapon's magazine.
/// Καταναλώνει πυρομαχικά από το γεμιστήρα του τρέχοντος όπλου.
/// </summary>
public bool ConsumeMagazineAmmo(AmmoType type, int amount)
{
    WeaponData weapon = GetWeaponForAmmoType(type);
    if (weapon == null) return false;

    if (!magazineAmmo.ContainsKey(weapon)) return false;

    if (magazineAmmo[weapon] >= amount)
    {
        magazineAmmo[weapon] -= amount;

        // Fire event for UI update
        // Εκπομπή event για ενημέρωση UI
        int backpack = GetBackpackAmmo(type);
        OnAmmoChanged?.Invoke(type, backpack, magazineAmmo[weapon]);

        return true;
    }
    return false;
}

/// <summary>
/// Reloads weapon from backpack ammo reserve.
/// Επαναγεμίζει όπλο από το απόθεμα πυρομαχικών.
/// </summary>
public void Reload(WeaponData weapon)
{
    if (weapon == null) return;

    AmmoType type = weapon.ammoType;
    int currentMag = magazineAmmo.ContainsKey(weapon) ? magazineAmmo[weapon] : 0;
    int maxMag = weapon.magazineCapacity;
    int needed = maxMag - currentMag;

    if (needed <= 0) return; // Magazine full

    int available = GetBackpackAmmo(type);
    int toTransfer = Mathf.Min(needed, available);

    if (toTransfer > 0)
    {
        // Transfer from backpack to magazine
        // Μεταφορά από σακίδιο σε γεμιστήρα
        backpackAmmo[type] -= toTransfer;
        magazineAmmo[weapon] += toTransfer;

        OnAmmoChanged?.Invoke(type, backpackAmmo[type], magazineAmmo[weapon]);
        OnReloadCompleted?.Invoke();

        Debug.Log($"[Inventory] Reloaded {toTransfer} rounds into {weapon.weaponName}");
    }
}

/// <summary>
/// Adds points from kills or pickups.
/// Προσθέτει πόντους από θανατώσεις ή pickups.
/// </summary>
public void AddPoints(int amount)
{
    int previousPoints = currentPoints;
    currentPoints += amount;

    OnPointsGained?.Invoke(amount, currentPoints);
    OnPointsChanged?.Invoke(currentPoints);

    Debug.Log($"[Inventory] +{amount} points. Total: {currentPoints}");
}
```

### Challenges Solved
- **Ammo type sharing**: Multiple weapons can use same AmmoType (e.g., all pistols share pistol ammo)
- **Magazine vs backpack**: Separate tracking allows reload mechanic
- **Point validation**: SpendPoints returns bool for shop/medic to check affordability

---

# 6. ASSET SOURCES (Πηγές Assets)
**Length: 1-2 pages**

## 6.1 3D Models and Environment

| Asset | Publisher | Source | Usage |
|-------|-----------|--------|-------|
| POLYGON City Pack | Synty Studios | Unity Asset Store | Casino buildings, urban props, vehicles |
| POLYGON Apocalypse Pack | Synty Studios | Unity Asset Store | Zombie characters, survival props |
| POLYGON Western Pack | Synty Studios | Unity Asset Store | Desert environment elements |

## 6.2 Character Controller and Animations

| Asset | Publisher | Source | Usage |
|-------|-----------|--------|-------|
| Animation Base Locomotion | Synty Studios | Unity Asset Store | Player character controller, movement animations, input handling |

## 6.3 Audio

| Asset | Publisher | Source | Usage |
|-------|-----------|--------|-------|
| [Audio Pack Name] | [Publisher] | [Source] | Weapon sounds, zombie groans |
| [Music Pack Name] | [Publisher] | [Source] | Background music, ambient audio |

*Note: Replace with actual audio assets used*

## 6.4 Fonts and UI

| Asset | Publisher | Source | Usage |
|-------|-----------|--------|-------|
| TextMeshPro | Unity Technologies | Built-in Package | UI text rendering |
| [Font Name] | [Source] | Google Fonts / DaFont | Menu and HUD text |

## 6.5 Custom Created Content

The following were created by our team:

- **All C# Scripts** (35+ scripts in Assets/_ProjectFiles/Scripts/)
- **Scene Composition** - Layout and placement of all environment objects
- **ScriptableObject Configurations** - WeaponData, EnemyData, PickupData assets
- **UI Layout** - All canvas configurations and panel arrangements
- **Prefab Configurations** - Player, enemy, NPC, pickup prefabs
- **Material Adjustments** - Custom material property modifications
- **Audio Configurations** - AudioSource settings, zone triggers

## 6.6 In-Game Attribution

Asset sources are credited in-game through:
- **Credits Screen** - Accessible from main menu
- **Information Sign** - Near spawn point in game scene
- **README.txt** - Included with build files

---

# 7. USER MANUAL (Παρουσίαση για Χρήστες)
**Length: 2-3 pages**

## 7.1 System Requirements

### Minimum Requirements
- **OS**: Windows 10 64-bit
- **Processor**: Intel Core i5-6600 / AMD Ryzen 5 1600
- **Memory**: 8 GB RAM
- **Graphics**: NVIDIA GTX 1050 Ti / AMD RX 570
- **DirectX**: Version 11
- **Storage**: 2 GB available space

### Recommended Requirements
- **OS**: Windows 10/11 64-bit
- **Processor**: Intel Core i7-8700 / AMD Ryzen 7 2700X
- **Memory**: 16 GB RAM
- **Graphics**: NVIDIA GTX 1660 Ti / AMD RX 5600 XT
- **DirectX**: Version 12
- **Storage**: 2 GB available space

## 7.2 Installation

1. Extract the downloaded ZIP file to your desired location
2. Navigate to the extracted folder
3. Double-click `LasVegasApocalypse.exe` to launch
4. First launch may take longer as shaders compile

## 7.3 Controls

### Movement
| Action | Key |
|--------|-----|
| Move Forward | W |
| Move Left | A |
| Move Backward | S |
| Move Right | D |
| Sprint | Left Shift |
| Jump | Spacebar |
| Look Around | Mouse |

### Combat
| Action | Key |
|--------|-----|
| Shoot | Left Mouse Button |
| Aim (Zoom) | Right Mouse Button |
| Reload | R |
| Switch Weapon | Mouse Scroll Wheel |

### Interaction
| Action | Key |
|--------|-----|
| Interact | E |
| Pause Menu | Escape |

## 7.4 How to Play

### Objective
**Kill 25 zombies, collect 3 keys, then escape through the vault door within 90 seconds.**

### Getting Started
1. You spawn on the Las Vegas Strip with a basic pistol
2. Find the **Tutorial NPC** (blue marker) to learn the basics
3. Explore the environment to find keys, ammo, and weapons
4. Kill zombies to earn points (100 points per kill)
5. Use points at NPCs for upgrades and healing

### Win Conditions
1. Kill **25 zombies** (progress shown in top-left UI)
2. Collect **3 keys** (scattered throughout the map)
3. When both conditions are met, a **90-second timer** starts
4. Find and open the **Vault Door** before time runs out
5. **Victory!**

### NPCs

**Medic (Green Cross)**
- Heals you to full health
- Cost: 200 points
- Use when health is low

**Shopkeeper (Dollar Sign)**
- Sells weapons
- Prices: 500-2000 points
- Better weapons deal more damage

**Tutorial (Question Mark)**
- Explains game mechanics
- Free, no cost
- Recommended for first playthrough

### Tips & Strategies
- **Explore early** - Find free pickups before buying from shop
- **Manage ammo** - Don't spray; aim carefully
- **Reload safely** - Find cover before reloading
- **Save points** - Better weapons make later waves easier
- **Learn spawn points** - Zombies come from predictable locations
- **Keep moving** - Standing still gets you surrounded

## 7.5 Settings

Access settings from Main Menu or Pause Menu:

### Graphics
- Quality Preset: Low / Medium / High / Ultra
- Resolution: Select from available options
- Fullscreen / Windowed mode
- V-Sync: On / Off

### Audio
- Master Volume: 0-100%
- Music Volume: 0-100%
- SFX Volume: 0-100%

### Gameplay
- Mouse Sensitivity: Adjustable slider
- Invert Y-Axis: Toggle

## 7.6 Troubleshooting

**Problem: Low FPS / Stuttering**
- Lower graphics quality preset
- Close background applications
- Update graphics drivers

**Problem: Game won't start**
- Verify DirectX 11 is installed
- Run as Administrator
- Check antivirus isn't blocking

**Problem: No sound**
- Check Windows audio settings
- Verify in-game volume isn't muted
- Update audio drivers

**Problem: Controls not responding**
- Disconnect any gamepads
- Restart the game
- Check settings for control bindings

---

# 8. SCREENSHOTS (Στιγμιότυπα)
**Length: 3-5 pages**

*Include 15-20 high-quality screenshots (1920x1080) organized as follows:*

## 8.1 Environment Screenshots (4-5 images)

**Screenshot 1: Vegas Strip Overview**
*Caption: Panoramic view of the Las Vegas Strip showing casino buildings, neon signage, and desert atmosphere.*

**Screenshot 2: Casino Exterior**
*Caption: Detailed view of casino architecture with authentic Vegas styling using Synty assets.*

**Screenshot 3: Street Level View**
*Caption: Ground-level exploration showing street props, vehicles, and environmental details.*

**Screenshot 4: Night Lighting**
*Caption: Neon lighting effects with post-processing bloom creating Vegas atmosphere.*

## 8.2 Gameplay Screenshots (4-5 images)

**Screenshot 5: Combat Engagement**
*Caption: Player fighting zombies showing weapon firing and muzzle flash effects.*

**Screenshot 6: Aiming System**
*Caption: Right-click aim zoom with crosshair targeting an enemy.*

**Screenshot 7: Reload Action**
*Caption: Reload prompt UI showing magazine/backpack ammo system.*

**Screenshot 8: Hitmarker Feedback**
*Caption: Hitmarker display confirming successful hit on enemy.*

## 8.3 Interaction Screenshots (3-4 images)

**Screenshot 9: Medic NPC**
*Caption: Player interacting with Medic NPC showing healing service option.*

**Screenshot 10: Shopkeeper NPC**
*Caption: Weapon shop interface displaying available weapons and prices.*

**Screenshot 11: Key Pickup**
*Caption: Key pickup interaction showing collection prompt.*

**Screenshot 12: Vault Door**
*Caption: Vault door showing locked status with kill/key progress requirements.*

## 8.4 UI Screenshots (4-5 images)

**Screenshot 13: Main Menu**
*Caption: Main menu with New Game, Settings, and Exit options.*

**Screenshot 14: In-Game HUD**
*Caption: Complete HUD showing health, ammo, points, kill count, and key count.*

**Screenshot 15: Pause Menu**
*Caption: Pause menu with Resume, Main Menu, and Exit buttons.*

**Screenshot 16: Win Condition Progress**
*Caption: UI showing 25/25 kills and 3/3 keys with escape timer countdown.*

**Screenshot 17: Victory Screen**
*Caption: Victory screen displayed after successful escape through vault door.*

**Screenshot 18: Game Over Screen**
*Caption: Game over screen with Restart, Main Menu, and Exit options.*

---

# 9. EVALUATION CRITERIA RESPONSE (Απάντηση Κριτηρίων)
**Length: 4-6 pages**

## Criterion 1: Elements Taught in Class (Στοιχεία Μαθήματος)

### Unity Systems Implemented

**NavMesh AI Pathfinding**
- EnemyAI uses NavMeshAgent for zombie movement
- Automatic obstacle avoidance
- Dynamic path recalculation when player moves

**Animation System**
- Animator controllers for player and enemies
- Animation parameters driven by gameplay state
- Blend trees for smooth movement transitions

**Particle Systems**
- Muzzle flash effects on weapon fire
- Blood splatter on enemy damage
- Impact particles on bullet collision

**Post-Processing**
- URP post-processing stack
- Bloom for neon light enhancement
- Color grading for Vegas atmosphere

**New Input System**
- Synty InputReader integration
- Event-based input handling
- Support for keyboard/mouse

### Optimization Techniques

**Object Pooling** (Planned/Partial)
- Reuse zombie and bullet instances
- Avoid runtime instantiation costs

**Occlusion Culling**
- Configured for casino interiors
- Objects behind walls not rendered

**Layer-Based Collision**
- Collision matrix prevents unnecessary checks
- Enemies don't collide with each other

**Component Caching**
- GetComponent results stored in Awake
- No repeated lookups in Update

---

## Criterion 2: Realism (Αληθοφάνεια)

### Physical Laws Respected

- **Gravity**: All objects affected by physics
- **Collision**: Player cannot pass through walls/floors
- **Projectile physics**: Bullets follow realistic trajectories
- **NavMesh pathfinding**: Zombies navigate around obstacles naturally

### No Violations

- No floating objects in environment
- Player movement speed is realistic (~6 m/s)
- Doors open with proper animations
- Zombies cannot walk through walls

### Authentic Representation

- Vegas architecture styled after real casinos
- Neon lighting matches Vegas aesthetic
- Desert environment surroundings appropriate

---

## Criterion 3: Content (Περιεχόμενο)

### Real-World Inspiration
- Las Vegas Strip as primary reference
- Casino architecture from famous establishments
- Entertainment-focused atmosphere

### Proper Attribution
All external assets credited in:
- In-game Credits screen
- Documentation Section 6
- README.txt with build
- Information sign in game scene

---

## Criterion 4: Completeness (Πληρότητα)

### 3D Navigable Environment
- Full Vegas Strip explorable
- Multiple interior and exterior areas
- 6DOF camera movement

### Lighting
- Directional light (sun)
- Point lights on neon signs
- Spot lights at building entrances
- Emissive materials on signs
- Real-time shadows

### Decorations
- Vehicles, props, signs throughout
- Palm trees and desert plants
- Casino facades and details
- Atmospheric particles

### Functionality
- Complete gameplay loop (start → play → win/lose)
- All systems functional and interconnected
- Full UI for all game states

---

## Criterion 5: Design (Σχεδιασμός)

### Clean Architecture
- Component-based design
- Single responsibility principle
- Interface-based polymorphism
- Event-driven communication

### Design Patterns Used
- **Singleton**: WinConditionManager, PauseManager
- **Observer**: Event system for UI updates
- **State**: Enemy AI behavior states
- **Strategy**: Different weapon behaviors via WeaponData

### Not Overly Complex
- Focused scope on core mechanics
- Avoided unnecessary features (crafting, skill trees)
- Clear, maintainable code structure

---

## Criterion 6: Aesthetics (Αισθητική)

### Visual Appeal
- Cohesive low-poly art style
- Vibrant neon color palette
- High contrast for readability
- Consistent materials throughout

### Inviting to Users
- Recognizable Vegas landmarks
- Clear visual hierarchy
- Welcoming starting area with tutorial
- Interesting environment to explore

---

## Criterion 7: Originality (Πρωτοτυπία)

### Unity Capabilities Explored
- Universal Render Pipeline (URP)
- Animation Rigging (if used)
- New Input System
- NavMesh AI Navigation
- Post-Processing Stack

### C# Capabilities Used
- Events and delegates
- Interfaces for polymorphism
- Coroutines for timing
- Properties with encapsulation
- Enums for type safety

### Unique Implementations
- Dual ammo system (magazine + backpack)
- Win condition flow with timed escape
- Multiple NPC service types
- Flexible interaction system

---

## Criterion 8: Usability (Χρηστικότητα)

### Intuitive Controls
- Industry-standard FPS controls (WASD + mouse)
- Familiar shooter mechanics
- Clear interaction prompts (Press E)

### Exploration Capability
- Entire environment freely explorable
- No invisible barriers blocking reasonable paths
- Visual landmarks for navigation

### Meeting User Needs
- Entertainment through combat and exploration
- Challenge through progressive difficulty
- Accomplishment through clear objectives
- Tutorial NPC for onboarding

---

## Criterion 9: Animation (Κίνηση)

### Character Animations
- Player: Walk, run, jump, idle
- Zombies: Walk, run, attack, death
- NPCs: Idle, talking gestures

### Environmental Animations
- Pickups bob and rotate
- Doors swing open/closed
- Neon signs flicker

### Property Animations
- UI fade in/out
- Health bar color transitions
- Point gain scale effects
- Hitmarker appearance/fade

---

## Criterion 10: Functionality (Λειτουργικότητα)

### User-Activated Interactions

**Doors**: Check key requirement → unlock/deny
**NPCs**: Display service → perform action (heal/sell/inform)
**Pickups**: Add to inventory → remove from world
**Weapons**: Fire → consume ammo → damage enemies

### Reactive Elements

**Zombies**: Detect player → chase → attack → die → drop loot
**Health**: Take damage → update UI → trigger death
**Waves**: Time/kill trigger → spawn enemies → increase difficulty
**Win Conditions**: Track kills/keys → start timer → victory/defeat

### Conditional Functionality
- Doors open only with matching key
- Healing only if health < max AND points >= 200
- Reload only if backpack has ammo AND magazine not full
- Victory only if conditions met AND vault door opened

---

## Criterion 11: Development (Ανάπτυξη)

### Technology Stack
- Unity 6000.2.8f1
- Universal Render Pipeline
- C# with modern features
- Visual Studio 2022

### Best Practices

**Code Quality**
- XML documentation on all classes
- Summary comments on public methods
- Descriptive variable names
- Consistent naming conventions
- No magic numbers (constants used)

**Architecture**
- Component-based design
- Event-driven communication
- Interface-based polymorphism
- Data-driven configuration (ScriptableObjects)

**Organization**
- Logical folder structure
- Scripts organized by domain
- Consistent file naming
- Clear prefab hierarchy

---

# DOCUMENT FORMATTING CHECKLIST

Before exporting to PDF:

- [ ] Professional title page with project name, course, team
- [ ] Table of contents with page numbers
- [ ] Consistent heading styles (H1, H2, H3)
- [ ] Page numbers on all pages
- [ ] Code blocks with syntax highlighting
- [ ] Tables formatted consistently
- [ ] Screenshots captioned and numbered
- [ ] Diagrams clear and readable
- [ ] Spell check completed
- [ ] Greek terms included where appropriate
- [ ] 15-30 page target met
- [ ] Export as PDF
