# Εικονική Πραγματικότητα - Complete Documentation Guide
**Academic Year 2025-2026**  
**Department of Informatics, University of Piraeus**  
**Instructors: E. Alépis – M. Kamítsios**

---

## PROJECT CONTEXT

This guide contains EVERYTHING needed to write complete documentation for the VR assignment without referring back to the original PDF.

### Assignment Topic (100 points)

Choose ONE of the following:

1. **Village (Χωριό)** - Ancient, medieval, modern, or sci-fi. Can represent an existing village or part of a city/community and its surrounding area (environment, roads, trees/forest, hills, sky, etc.)

2. **Tourist Destination (Τουριστικός Προορισμός)** - Ancient, modern, or sci-fi. Can represent an existing tourist destination and its surrounding area (environment, roads, trees/forest, hills, sky, cars or ships, etc.)

**OUR PROJECT**: Las Vegas Tourist Destination with zombie survival elements (fits Option 2)

---

## EVALUATION CRITERIA (How Your Project Will Be Graded)

---

## REQUIRED DELIVERABLES (What You Must Submit)

### Submission Details
- **Location**: https://thales.cs.unipi.gr/modules/work/?course=TMD117
- **If too large**: Upload to Dropbox/Google Drive, submit .txt file with link
- **Include**: Email and phone number in case of download issues

### Team Information
- Work individually or in teams of up to 3 people
- No need to declare teams to instructor in advance
- **IMPORTANT**: In documentation, clearly state who did what

---

## DELIVERABLE 1: Project Files

**What to Include**:
```
YourProjectFolder/
├── Assets/
├── Library/
├── Packages/
├── ProjectSettings/
├── UserSettings/
└── [Your Unity Project Structure]
```

**Notes**:
- Include entire Unity project folder
- Do NOT include Temp/ folder (Unity regenerates this)
- Library/ can be excluded to save space (Unity regenerates)
- If using Git, include .git folder if you want to show version history

---

## DELIVERABLE 2: Build Files

**What to Include**:
```
Builds/
├── Windows/
│   ├── YourGame.exe
│   ├── YourGame_Data/
│   ├── UnityPlayer.dll
│   └── UnityCrashHandler64.exe
├── Mac/ (if you built for Mac)
└── Linux/ (if you built for Linux)
```

**Build Settings**:
- Target Platform: Windows 64-bit (minimum)
- Development Build: **NO** (clean release build)
- Compression: Default
- Include README.txt with system requirements:
```
SYSTEM REQUIREMENTS:
- OS: Windows 10/11 64-bit
- Processor: Intel Core i5 or equivalent
- Memory: 8 GB RAM
- Graphics: NVIDIA GTX 1050 or equivalent
- Storage: 2 GB available space
```

---

## DELIVERABLE 3: PowerPoint Presentation (10-15 Slides)

### Slide Structure Template:

**Slide 1: Title Slide**
```
- Project Title: "Las Vegas Apocalypse: Tourist Destination VR Project"
- Course: Εικονική Πραγματικότητα
- Academic Year: 2025-2026
- Team Members: [Names and Student IDs]
- Date
```

**Slide 2: Project Overview**
```
- What: Las Vegas tourist destination with survival elements
- Why: Combines exploration with interactive gameplay
- Platform: Unity 6000.2.8f1 with URP
- Language: C#
```

**Slide 3: Topic Justification**
```
- Chosen Topic: Tourist Destination (Option 2)
- Las Vegas: World-famous destination
- Features: Strip, casinos, hotels, entertainment
- Interactive Element: Zombie survival adds engagement
```

**Slides 4-5: Key Features**
```
Slide 4:
- Explorable Las Vegas environment
- Multiple distinct areas
- Interactive NPCs (Medic, Shopkeeper, Tutorial)
- Dynamic combat system

Slide 5:
- Inventory and economy system
- Wave-based difficulty
- Settings and customization
- Full UI implementation
```

**Slides 6-8: Technical Implementation**
```
Slide 6 - Systems:
- Player movement and combat
- Enemy AI with NavMesh
- Interaction system
- Health and damage

Slide 7 - Architecture:
- Event-driven design
- ScriptableObjects for data
- Interface-based programming
- Component architecture

Slide 8 - Optimization:
- Object pooling
- Occlusion culling
- LOD groups
- Efficient collision detection
```

**Slides 9-11: Visual Showcase**
```
Slide 9: Environment Screenshots
- Vegas strip overview
- Casino exterior
- Interior spaces

Slide 10: Gameplay Screenshots
- Combat situation
- NPC interaction
- UI examples

Slide 11: Features Screenshots
- Weapon system
- Inventory
- Settings menu
```

**Slide 12: Development Process**
```
- Analysis phase
- Design decisions
- Implementation challenges
- Testing and iteration
```

**Slide 13: Team Contributions**
```
Member A:
- Player systems
- Weapon mechanics
- Inventory

Member B:
- Enemy AI
- Wave system
- NPC interactions

Member C:
- UI design
- Environment setup
- Optimization
```

**Slide 14: Challenges & Solutions**
```
Challenge 1: NavMesh baking for complex environment
Solution: Simplified collision meshes

Challenge 2: Performance with many zombies
Solution: Object pooling implementation

Challenge 3: Weapon switching smoothness
Solution: Animation Rigging system
```

**Slide 15: Conclusion & Future Work**
```
Achievements:
- Fully functional Vegas environment
- Complete gameplay loop
- Polished UI and interactions

Potential Improvements:
- Additional weapons
- More NPC types
- Multiplayer support
```

**Design Tips**:
- Use consistent color scheme
- Include project logo/branding
- High-quality screenshots
- Bullet points, not paragraphs
- Professional fonts
- Minimal text per slide

---

## DELIVERABLE 4: Video (Ολιγόλεπτο Video)

### Video Structure (2-4 minutes):

**0:00-0:15 - Introduction**
```
- Show title screen
- Project name
- Team members (text overlay)
```

**0:15-0:45 - Main Menu**
```
- Navigate through menu
- Show settings options
- Display controls screen
- Brief pause on each
```

**0:45-1:30 - Environment Tour**
```
- Spawn in Vegas
- Walk through strip
- Show different areas:
  * Casino exteriors
  * Street views
  * Different buildings
- Highlight landmarks
- Show skybox/atmosphere
```

**1:30-2:15 - Gameplay Features**
```
- Combat demonstration:
  * Shoot zombies
  * Show hitmarker feedback
  * Reload weapon
  * Switch weapons
  * Take damage, heal

- Interactions:
  * Pick up items
  * Open doors with keys
  * Talk to NPCs
  * Use shop
  * Use medic
```

**2:15-2:45 - UI & Systems**
```
- Show HUD elements
- Display inventory
- Demonstrate wave system
- Show point gain
- Pause menu
- Settings adjustments
```

**2:45-3:00 - Conclusion**
```
- Final scenic shot of Vegas
- Fade to credits showing:
  * Team members
  * Asset sources
  * Tools used
```

**Technical Requirements**:
- Resolution: 1920x1080 minimum (1080p)
- Format: MP4
- Framerate: 30 or 60 FPS
- Audio: Clear narration OR background music
- Length: 2-4 minutes (don't exceed 5 minutes)

**Recording Tips**:
- Use OBS Studio or Unity Recorder
- No HUD clutter (or toggle for clean shots)
- Smooth camera movements
- Show off best visual features
- Good lighting in scenes
- Background music (copyright-free)

**What to Avoid**:
- Shaky camera movements
- Long periods of nothing happening
- Poor lighting
- Laggy footage
- Audio peaking/distortion
- Showing bugs or glitches

---

## DELIVERABLE 5: Manual/Documentation (Εγχειρίδιο Παρουσίασης)

This is the MOST IMPORTANT deliverable. Must be comprehensive.

### Document Format:
- PDF format (preferred) or Word .docx
- 15-30 pages (depends on detail)
- Professional formatting
- Table of contents
- Page numbers
- Section headers

### Required Sections:

---

#### SECTION A: Introduction (Εισαγωγή)

**Length**: 1-2 pages

**What to Include**:
```
1. Project Title and Context
   - Full project name
   - Course and academic year
   - Team members with roles
   
2. Project Vision
   - What we aimed to create
   - Why we chose this topic
   - Target audience
   
3. Technology Stack
   - Platform: Unity 6000.2.8f1
   - Render Pipeline: URP
   - Language: C#
   - Additional packages/tools
   
4. Scope
   - What's included in the project
   - What's not included (intentionally)
   
5. Document Structure
   - Brief overview of what each section contains
```

**Example**:
```
Introduction

This project, titled "Las Vegas Apocalypse," represents our implementation
of a tourist destination virtual reality environment for the Virtual Reality
course (2025-2026). We chose to recreate Las Vegas, one of the world's most
iconic tourist destinations, and added interactive survival elements to
enhance user engagement.

Our team consists of three members:
- [Name A]: Player systems and combat mechanics
- [Name B]: Enemy AI and wave management
- [Name C]: UI design and environment integration

The project was developed in Unity 6000.2.8f1 using the Universal Render
Pipeline (URP) and programmed entirely in C#. We utilized Synty Studios'
Polygon asset packs to achieve a cohesive visual style while focusing our
development efforts on systems and gameplay mechanics.

This document provides comprehensive documentation of our development
process, technical implementation, and how we addressed each evaluation
criterion set forth in the assignment requirements.
```

---

#### SECTION B: Problem Description (Περιγραφή του Προβλήματος)

**Length**: 2-3 pages

**What to Include**:
```
1. Assignment Requirements
   - Restate: Create village OR tourist destination
   - Our choice: Tourist destination (Las Vegas)
   - Justification for choice
   
2. Design Challenges
   - How to represent Las Vegas authentically
   - Balancing exploration with engagement
   - Technical constraints
   - Scope management for team size
   
3. Core Questions We Addressed
   - How to create convincing Vegas atmosphere?
   - How to make exploration rewarding?
   - How to integrate interactivity naturally?
   - How to optimize for performance?
   
4. User Experience Goals
   - What we want players to feel
   - What we want players to do
   - What we want players to learn
   
5. Success Criteria
   - How we'll know if we succeeded
   - Metrics for evaluation
```

**Example**:
```
Problem Description

The assignment presented two options: create a village or a tourist
destination in a virtual environment. We selected Option 2 (tourist
destination) and chose to represent Las Vegas for several reasons:

1. VISUAL DISTINCTIVENESS
Las Vegas is immediately recognizable through its unique architecture,
neon signage, and desert setting. This visual identity would make our
environment memorable and authentic.

2. INTERACTIVITY OPPORTUNITIES
A tourist destination naturally includes numerous interactive elements:
casinos, hotels, entertainment venues, and street activities. This
aligned perfectly with the assignment's emphasis on functionality.

3. SCALE MANAGEABILITY
Unlike sprawling natural destinations, Las Vegas's concentrated strip
allowed us to create a detailed environment within our time constraints.

CORE CHALLENGES:
Our primary challenge was balancing faithful representation with engaging
gameplay. A purely explorative Vegas tour risks becoming a walking
simulator. We needed interactive elements that felt natural to the setting.

Our solution: frame the experience as "Las Vegas during a zombie outbreak."
This narrative context allowed us to:
- Add challenge and stakes to exploration
- Create meaningful player choices (risk vs. reward)
- Implement combat and survival systems naturally
- Maintain Vegas aesthetics while adding gameplay depth

TECHNICAL CHALLENGES:
- Optimizing a large urban environment for smooth performance
- Implementing AI pathfinding in complex casino interiors
- Creating intuitive interaction systems for diverse objects
- Balancing visual quality with performance

USER EXPERIENCE GOALS:
Players should feel:
- Immersed in recognizable Las Vegas environment
- Challenged by survival elements
- Rewarded for exploration
- In control through clear, responsive interactions
```

---

#### SECTION C: Development Phases (Ανάλυση-Σχεδίαση-Υλοποίηση)

**Length**: 4-6 pages

This section needs THREE subsections:

##### C1: Analysis Phase (Ανάλυση)

**What to Include**:
```
1. Requirements Gathering
   - What does the assignment require?
   - What do we want to create?
   - What are our constraints?

2. Research
   - Study of Las Vegas references
   - Analysis of similar projects
   - Unity capabilities research
   - Asset availability investigation

3. Feature Definition
   - Core features (must-have)
   - Secondary features (should-have)
   - Optional features (nice-to-have)

4. Technical Requirements
   - System architecture needs
   - Performance targets
   - Asset requirements
   - Tool requirements

5. Risk Assessment
   - What could go wrong?
   - Mitigation strategies
```

**Example**:
```
C1: Analysis Phase

REQUIREMENTS ANALYSIS:
We began by carefully reviewing the assignment criteria. The key requirements were:
- Represent a tourist destination authentically
- Include interactive functionality
- Demonstrate elements taught in class
- Implement optimization techniques
- Provide dynamic, animated world
- Well-documented, commented code

RESEARCH PHASE:
- Studied real Las Vegas reference photos and videos
- Analyzed Unity asset store for suitable environment packs
- Researched zombie survival game mechanics in similar projects
- Investigated Unity's URP capabilities and limitations
- Reviewed NavMesh system documentation for AI implementation

FEATURE PRIORITIZATION:
Must-Have (MVP):
- Navigable Vegas strip environment
- Basic player movement and shooting
- Enemy AI with pathfinding
- At least 3 interactive NPCs
- UI for health, ammo, points
- Wave spawning system

Should-Have:
- Multiple weapon types
- Inventory system
- Economy (points/shop)
- Main menu with settings
- Door/key mechanics

Nice-to-Have:
- Save system
- Multiple Vegas areas
- Day/night cycle
- Advanced particle effects

TECHNICAL REQUIREMENTS IDENTIFIED:
- NavMesh for AI pathfinding
- Event system for decoupled architecture
- ScriptableObjects for data management
- Animation Rigging for weapon handling
- Object pooling for performance
- New Input System for player controls
```

##### C2: Design Phase (Σχεδίαση)

**What to Include**:
```
1. Architecture Design
   - System diagrams
   - Component relationships
   - Data flow diagrams
   - Class hierarchy

2. Level Design
   - Environment layout
   - Player flow
   - Point of interest placement
   - Spawn locations

3. Gameplay Design
   - Player abilities
   - Enemy behaviors
   - Progression systems
   - Difficulty curve

4. UI/UX Design
   - Interface mockups
   - Information hierarchy
   - User flow diagrams

5. Technical Design Decisions
   - Why we chose certain patterns
   - Why we used certain Unity features
   - Why we structured code this way
```

**Example**:
```
C2: Design Phase

SYSTEM ARCHITECTURE:
We adopted an event-driven, component-based architecture:

[Include diagram showing]:
Player Systems ←→ Event Bus ←→ Game Systems
     ↓                              ↓
  Inventory                    Wave Manager
  Health                       Enemy Spawner
  Weapons                      UI Manager

KEY DESIGN DECISIONS:

1. EVENT-DRIVEN ARCHITECTURE
Decision: Use static events for cross-system communication
Rationale: Decouples systems, making them easier to test and maintain
Example: Inventory.OnWeaponEquipped event notifies UI and animation systems

2. SCRIPTABLEOBJECTS FOR DATA
Decision: Store weapon stats, enemy data in ScriptableObjects
Rationale: Allows designers to balance game without touching code
Example: WeaponData holds damage, fire rate, ammo capacity

3. INTERFACE-BASED INTERACTIONS
Decision: Use IInteractable interface for all interactive objects
Rationale: Polymorphism allows single interaction system for varied objects
Example: NPCs, doors, pickups all implement IInteractable

LEVEL DESIGN:
[Include top-down map/diagram of Vegas layout]

Player spawn: Center of strip
NPC locations: Strategically placed for pacing
Zombie spawns: Outside playable area, approach from all angles
Loot distribution: Sparse in safe areas, abundant in dangerous zones

PROGRESSION DESIGN:
Wave 1-3: Few slow zombies, easy difficulty
Wave 4-7: More zombies, faster movement
Wave 8+: Maximum difficulty, constant pressure

Points economy:
- Kill: 100 points
- Weapon cost: 500-2000 points
- Heal cost: 200 points
Creates risk/reward decisions: spend points or save for better weapon?
```

##### C3: Implementation Phase (Υλοποίηση)

**What to Include**:
```
1. Development Timeline
   - Week-by-week breakdown
   - Milestones achieved
   - Iterations performed

2. Implementation Challenges
   - Problems encountered
   - Solutions developed
   - Lessons learned

3. Testing & Iteration
   - Testing methodology
   - Bugs found and fixed
   - Balance adjustments

4. Integration
   - How systems were combined
   - Integration challenges
   - Final polish

5. Optimization Process
   - Performance profiling results
   - Optimization techniques applied
   - Performance improvements achieved
```

**Example**:
```
C3: Implementation Phase

DEVELOPMENT TIMELINE:

Week 1-2: Foundation
- Unity project setup with URP
- Import Synty assets
- Basic player movement (Synty controller integration)
- Camera setup

Week 3-4: Core Systems
- Weapon shooting mechanics
- Inventory system implementation
- Health/damage system
- Basic UI (health, ammo)

Week 5-6: Enemy AI
- NavMesh setup for environment
- Enemy AI with chase/attack behaviors
- Wave spawning system
- Enemy death and drops

Week 7-8: Interactions
- Interaction system (raycasting)
- NPC implementations (Medic, Shop, Tutorial)
- Door/key mechanics
- Pickup system

Week 9-10: Polish & Optimization
- Main menu creation
- Settings implementation
- Object pooling for zombies
- Occlusion culling setup
- Bug fixing and balance

Week 11-12: Documentation
- Code commenting
- Manual writing
- Video recording
- PowerPoint creation

MAJOR CHALLENGES & SOLUTIONS:

Challenge 1: Weapon Aiming Accuracy
Problem: Bullets didn't go where camera was pointing
Solution: Raycast from camera center to find aim point, then calculate
direction from weapon muzzle to that point. Provides accurate shooting.

Challenge 2: NavMesh Performance
Problem: Many zombies recalculating paths caused frame drops
Solution: Stagger path recalculation timing, reduce update frequency when
zombies far from player, use object pooling to avoid instantiation costs.

Challenge 3: Interaction System Design
Problem: Different objects (NPCs, doors, items) need different interactions
Solution: Created IInteractable interface with multiple methods
(OnReadyInteract, OnInteract, OnAbortInteract, OnEndInteract) allowing
flexible behavior while keeping system unified.

TESTING PROCESS:
- Daily playtesting by team members
- Iteration on difficulty (initially too hard, adjusted enemy damage)
- UI readability tests (increased font sizes, added contrast)
- Performance profiling (Unity Profiler identified zombie pathfinding as bottleneck)

OPTIMIZATION RESULTS:
Before optimization: 45-50 FPS with 20 zombies
After optimization: 60 FPS locked with 30+ zombies

Techniques applied:
- Object pooling for zombies (20 pre-allocated)
- Object pooling for bullets (50 pre-allocated)
- Occlusion culling for casino interiors (+15% FPS improvement)
- LOD groups for distant buildings
- Reduced NavMesh update frequency
```

---

#### SECTION D: Script Table (Πίνακας Scripts)

**Length**: 3-4 pages

**Format**: Table with columns for Script Name, Category, and Description

**What to Include**:
- Every single C# script in your project
- 4-5 line description of what each does
- Organized by category

**Example**:

```
SCRIPT TABLE

PLAYER SYSTEMS
──────────────────────────────────────────────────────────────────────
Script Name          | Description
──────────────────────────────────────────────────────────────────────
WeaponController.cs  | Manages weapon shooting, aiming (FOV zoom), and
                     | weapon switching via scroll wheel. Raycasts from
                     | camera to determine aim target and calculates
                     | bullet direction from weapon's shoot point. Handles
                     | ammo consumption and reload prompting.
──────────────────────────────────────────────────────────────────────
Inventory.cs         | Manages player's weapon collection, ammo stores
                     | (magazine and backpack), and points currency. Provides
                     | methods for adding/removing weapons, managing ammo
                     | counts, and handles weapon switching. Fires events
                     | when weapons are equipped or inventory changes.
──────────────────────────────────────────────────────────────────────
PlayerHealth.cs      | Tracks player health value and handles damage/healing.
                     | Fires events when health changes for UI updates and
                     | when player dies for game over state. Provides public
                     | methods for taking damage and healing with maximum
                     | health clamping.
──────────────────────────────────────────────────────────────────────
PlayerKeys.cs        | Static manager for player's key inventory used in
                     | door/key interaction system. Tracks which keys player
                     | has collected and provides methods to add keys and
                     | check key possession. Decoupled from player object
                     | for easy access throughout game.
──────────────────────────────────────────────────────────────────────

WEAPON SYSTEMS
──────────────────────────────────────────────────────────────────────
WeaponData.cs        | ScriptableObject defining weapon properties including
                     | damage, fire rate, magazine capacity, total ammo, ammo
                     | type, weapon prefab, and UI icon. Allows data-driven
                     | weapon balancing without code changes. Each weapon
                     | instance references one WeaponData asset.
──────────────────────────────────────────────────────────────────────
Bullet.cs            | Physics-based projectile spawned when weapons fire.
                     | Moves via Rigidbody velocity, applies damage on
                     | collision with IDamageable objects, excludes damage
                     | to shooter, spawns impact effects, and destroys
                     | itself after set lifetime or collision.
──────────────────────────────────────────────────────────────────────

ENEMY SYSTEMS
──────────────────────────────────────────────────────────────────────
EnemyAI.cs           | Controls zombie behavior using NavMeshAgent for
                     | pathfinding. Detects player within range, chases
                     | player, and attacks when in melee range. Updates
                     | Animator speed parameter for walk/idle animations.
                     | Handles death by disabling AI and playing death effects.
──────────────────────────────────────────────────────────────────────
EnemyHealth.cs       | Manages enemy health, implements IDamageable interface.
                     | Takes damage from bullets, updates health, fires death
                     | event when health reaches zero. Death event triggers
                     | RandomDrop component and awards points to player.
                     | Provides visual feedback on damage (if implemented).
──────────────────────────────────────────────────────────────────────
EnemyData.cs         | ScriptableObject storing enemy statistics including
                     | health, movement speed, attack damage, attack range,
                     | and loot drop chance. Allows balancing enemy difficulty
                     | through asset configuration rather than code modification.
                     | Referenced by EnemyAI and EnemyHealth components.
──────────────────────────────────────────────────────────────────────

INTERACTION SYSTEMS
──────────────────────────────────────────────────────────────────────
Interactor.cs        | Attached to main camera, raycasts forward to detect
                     | objects tagged "Interactable". Calls appropriate
                     | IInteractable methods (OnReadyInteract when looking at,
                     | OnInteract when E pressed, OnAbortInteract when looking
                     | away). Central component of interaction system.
──────────────────────────────────────────────────────────────────────
IInteractable.cs     | Interface defining interaction contract with methods:
                     | OnInteract (when E pressed), OnReadyInteract (when
                     | raycast first hits), OnAbortInteract (when raycast
                     | leaves), OnEndInteract (when interaction completes).
                     | Implemented by NPCs, doors, signs, pickups.
──────────────────────────────────────────────────────────────────────
InteractorUI.cs      | Displays interaction prompts to player showing what
                     | can be interacted with. Updates text/UI elements based
                     | on Interactor raycast results. Shows messages like
                     | "Press E to talk" or "Locked - need Blue Key".
                     | Provides visual feedback for interaction system.
──────────────────────────────────────────────────────────────────────

[Continue for ALL scripts...]

NPC SYSTEMS
──────────────────────────────────────────────────────────────────────
NPCBase.cs           | Abstract base class for all NPC types. Implements
                     | IInteractable interface, handles player detection,
                     | manages interaction prompts. Child classes override
                     | OnInteract() to define specific NPC behavior.
──────────────────────────────────────────────────────────────────────
MedicNPC.cs          | Extends NPCBase to provide healing service. Checks if
                     | player health is below maximum and has sufficient points
                     | (200). On interaction, restores player health to full
                     | and deducts points. Displays status messages for
                     | different conditions (full health, insufficient points).
──────────────────────────────────────────────────────────────────────

[Continue this format for every script...]
```

**Organization Categories Suggestion**:
- Player Systems
- Weapon Systems
- Enemy Systems
- Interaction Systems
- NPC Systems
- Pickup Systems
- Manager Systems
- UI Systems
- Data (ScriptableObjects)
- Utility/Helper Scripts

---

#### SECTION E: Detailed Script Analysis (4-5 Σημαντικότερα Scripts)

**Length**: 4-6 pages (about 1 page per script)

**What to Include**:
For each of 4-5 most important scripts:

```
1. Script Purpose
   - What problem does this script solve?
   - Why is it important to the project?

2. Architecture Overview
   - How does it fit in overall system?
   - What does it communicate with?

3. Key Methods Explanation
   - Detailed explanation of main methods
   - Algorithm descriptions
   - Design pattern usage

4. Code Example
   - Show actual code from the script
   - With detailed comments

5. Challenges & Solutions
   - What was difficult about implementing this?
   - How did you solve it?
```

**Example for ONE script**:

```
DETAILED SCRIPT ANALYSIS

1. WeaponController.cs
═══════════════════════════════════════════════════════════════════

PURPOSE:
WeaponController is the central component of our combat system,
managing all weapon-related player actions including shooting, aiming,
reloading, and weapon switching. It's critical because it directly
handles player input and translates it into game actions that affect
both the player's state and the world.

ARCHITECTURAL ROLE:
WeaponController sits on the Player object and acts as an intermediary
between:
- Input System (Synty's AnimationBaseLocomotion InputReader)
- Inventory (weapon and ammo data)
- Weapon GameObjects (enabling/disabling, animation rigs)
- Bullet spawning system
- UI systems (ammo display, reload prompts)

It communicates through:
- Direct component references (Inventory)
- Events (shooting, reloading)
- Method calls (spawning bullets)

KEY DESIGN DECISIONS:

Decision 1: Separate aiming and shooting raycasts
We raycast from camera to find aim point in world space, then calculate
direction from weapon's shoot point to that aim point. This ensures
bullets go where the camera is pointing while visually appearing to
come from the weapon.

Decision 2: Animation Rigging for weapon switching
Instead of animator states, we use Animation Rigging package with per-
weapon rigs. Switching weapons changes rig weights, allowing smooth
transitions and procedural aiming.

MAIN METHODS:

HandleShooting():
Called every frame from Update when fire button held. Checks fire rate
timing, ensures ammo available, raycasts to find aim target, spawns
bullet at shoot point with calculated direction, consumes ammo, triggers
effects (muzzle flash, hitmarker), and plays sounds.

SwitchWeapon(int direction):
Called when scroll wheel input detected. Disables current weapon's rig
and GameObject, calculates new weapon index with wraparound, enables
new weapon's rig and GameObject, fires OnWeaponEquipped event for other
systems to respond.

Reload():
Checks if reload is possible (backpack ammo available, magazine not full),
calculates how much ammo to transfer from backpack to magazine, updates
Inventory, plays reload animation (if implemented), and fires reload event
for UI updates.

HandleAiming():
When right-click held, smoothly reduces camera FOV to create zoom effect
for precise aiming. When released, returns FOV to normal. Simple but
effective feedback for aiming state.

CODE EXAMPLE WITH DETAILED COMMENTS:

```csharp
/// <summary>
/// Handles firing weapon when fire button is held down.
/// Διαχειρίζεται την πυροβολία όταν το κουμπί πυρός πιέζεται.
/// </summary>
private void HandleShooting()
{
    // Check if enough time has passed since last shot (fire rate limiting)
    // Έλεγχος αν πέρασε αρκετός χρόνος από την τελευταία βολή
    if (Time.time < nextFireTime) return;
    
    // Verify we have ammo in current magazine
    // Επιβεβαίωση ότι έχουμε πυρομαχικά στο τρέχον γεμιστήρα
    if (inventory.GetMagazineAmmo(currentWeaponData.ammoType) <= 0)
    {
        // No ammo - play empty click sound and show reload prompt
        // Δεν υπάρχουν πυρομαχικά - παίξε ήχο άδειου όπλου
        PlayEmptyClickSound();
        return;
    }
    
    // Step 1: Raycast from camera to find where player is aiming
    // Βήμα 1: Raycast από την κάμερα για να βρούμε που στοχεύει ο παίκτης
    Ray aimRay = mainCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
    Vector3 aimPoint;
    
    if (Physics.Raycast(aimRay, out RaycastHit aimHit, 500f))
    {
        // Hit something - use that point as aim target
        // Χτυπήσαμε κάτι - χρησιμοποίησε αυτό το σημείο ως στόχο
        aimPoint = aimHit.point;
    }
    else
    {
        // Didn't hit anything - use point far in distance
        // Δεν χτυπήσαμε τίποτα - χρησιμοποίησε μακρινό σημείο
        aimPoint = aimRay.origin + aimRay.direction * 500f;
    }
    
    // Step 2: Calculate direction from weapon muzzle to aim point
    // Βήμα 2: Υπολογισμός κατεύθυνσης από τη μύτη του όπλου προς το σημείο στόχευσης
    Vector3 shootDirection = (aimPoint - shootPoint.position).normalized;
    
    // Step 3: Spawn bullet at weapon's shoot point
    // Βήμα 3: Δημιουργία βλήματος στη θέση εκτόξευσης του όπλου
    GameObject bulletObj = BulletPool.Instance.GetBullet(); // Object pooling
    bulletObj.transform.position = shootPoint.position;
    bulletObj.transform.rotation = Quaternion.LookRotation(shootDirection);
    
    Bullet bullet = bulletObj.GetComponent<Bullet>();
    bullet.Initialize(
        currentWeaponData.damage,
        shootDirection,
        gameObject // Pass player as shooter to avoid self-damage
    );
    
    // Step 4: Consume ammo from inventory
    // Βήμα 4: Κατανάλωση πυρομαχικών από το inventory
    inventory.ConsumeMagazineAmmo(currentWeaponData.ammoType, 1);
    
    // Step 5: Visual and audio feedback
    // Βήμα 5: Οπτική και ηχητική ανατροφοδότηση
    PlayMuzzleFlash();
    PlayShootSound();
    
    // Step 6: Check if we hit an enemy for hitmarker feedback
    // Βήμα 6: Έλεγχος αν χτυπήσαμε εχθρό για hitmarker feedback
    if (Physics.Raycast(shootPoint.position, shootDirection, out RaycastHit hitInfo, 500f))
    {
        if (hitInfo.collider.CompareTag("Enemy"))
        {
            HitmarkerDisplay.Instance.ShowHitmarker();
        }
    }
    
    // Update next allowed fire time based on weapon's fire rate
    // Ενημέρωση επόμενου επιτρεπτού χρόνου πυρός βάσει fire rate
    nextFireTime = Time.time + (1f / currentWeaponData.fireRate);
}
```

CHALLENGES & SOLUTIONS:

Challenge: Initial implementation had bullets not going where camera aimed
Problem: We were using weapon's forward direction, which didn't match camera
Solution: Raycast from camera first to find aim point, then calculate
direction from weapon to that point. This decouples weapon model orientation
from actual aim direction.

Challenge: Weapon switching felt jarring with sudden changes
Problem: Instant enable/disable of weapon GameObjects looked unnatural
Solution: Implemented Animation Rigging system where each weapon has its own
rig. When switching, we smoothly transition rig weights over 0.2 seconds,
creating a smooth "equip" animation effect.

Challenge: Shooting too fast caused ammo to desync with UI
Problem: Multiple shots in one frame could consume more ammo than available
Solution: Added nextFireTime check at method start to enforce fire rate limit,
preventing multiple shots per frame. Also verify ammo availability before
each shot rather than at input detection.

WHY THIS SCRIPT MATTERS:
WeaponController is the most player-facing system - every combat interaction
goes through it. Its reliability and feel directly affect perceived game
quality. The care we took with aim accuracy, fire rate limiting, and smooth
weapon switching demonstrates understanding of both technical requirements
(raycasting, timing) and user experience (feedback, responsiveness).
```

**Continue this format for 3-4 more scripts**:
- EnemyAI.cs
- Inventory.cs or WaveManager.cs
- Interactor.cs or NPCBase.cs
- One UI script (AmmoUI.cs or PointsUI.cs)

---

#### SECTION F: Asset Sources (Πηγές Assets)

**Length**: 1-2 pages

**Format**: Categorized list with attribution

```
ASSET ATTRIBUTION

3D MODELS & ENVIRONMENTS:
═══════════════════════════════════════════════════════════════════
Asset Name: POLYGON - City Pack
Publisher: Synty Studios
Source: Unity Asset Store
URL: https://assetstore.unity.com/packages/3d/polygon-city-pack-95214
Usage: Buildings, props, vehicles for Las Vegas environment
License: Unity Asset Store EULA

Asset Name: POLYGON - Apocalypse Pack
Publisher: Synty Studios  
Source: Unity Asset Store
URL: https://assetstore.unity.com/packages/3d/polygon-apocalypse-pack-103340
Usage: Zombie characters, survival props
License: Unity Asset Store EULA

ANIMATIONS:
═══════════════════════════════════════════════════════════════════
Asset Name: Animation Base Locomotion
Publisher: Synty Studios
Source: Unity Asset Store
URL: https://assetstore.unity.com/packages/...
Usage: Player character controller and animations
License: Unity Asset Store EULA

AUDIO:
═══════════════════════════════════════════════════════════════════
Asset Name: [Specific pack name]
Publisher: [Name]
Source: [Unity Asset Store / Freesound.org / etc.]
Usage: Weapon sounds, zombie groans, ambient audio
License: [Specific license]

UI ELEMENTS:
═══════════════════════════════════════════════════════════════════
Asset Name: [UI pack name if used]
Publisher: [Name]
Source: [Source]
Usage: HUD icons, menu elements
License: [License]

FONTS:
═══════════════════════════════════════════════════════════════════
Font Name: [Font name]
Source: Google Fonts / DaFont / etc.
License: [License type]
Usage: UI text, menus

CUSTOM CREATED ASSETS:
═══════════════════════════════════════════════════════════════════
- All C# scripts (written by team)
- ScriptableObject data assets (created by team)
- Level layout and scene composition (designed by team)
- Particle system configurations (configured by team)
- Animation curves and timings (tuned by team)
- Material property adjustments (modified by team)

IN-GAME ATTRIBUTION:
Asset sources are credited in-game through:
- Credits screen in main menu
- Information board in starting area
- End credits after game over
```

---

#### SECTION G: User Manual (Παρουσίαση για Χρήστες)

**Length**: 2-3 pages

**What to Include**:

```
USER MANUAL

SYSTEM REQUIREMENTS:
═══════════════════════════════════════════════════════════════════
Minimum:
- OS: Windows 10 64-bit
- Processor: Intel Core i5-6600 / AMD Ryzen 5 1600
- Memory: 8 GB RAM
- Graphics: NVIDIA GTX 1050 Ti / AMD RX 570
- DirectX: Version 11
- Storage: 2 GB available space

Recommended:
- OS: Windows 10/11 64-bit
- Processor: Intel Core i7-8700 / AMD Ryzen 7 2700X
- Memory: 16 GB RAM
- Graphics: NVIDIA GTX 1660 Ti / AMD RX 5600 XT
- DirectX: Version 12
- Storage: 2 GB available space

INSTALLATION:
═══════════════════════════════════════════════════════════════════
1. Extract downloaded ZIP file to desired location
2. Navigate to extracted folder
3. Run "LasVegasApocalypse.exe"
4. First launch may take longer as shaders compile

CONTROLS:
═══════════════════════════════════════════════════════════════════
MOVEMENT:
W/A/S/D         - Move Forward/Left/Back/Right
Shift           - Sprint
Ctrl            - Crouch
Spacebar        - Jump
Mouse           - Look Around

COMBAT:
Left Mouse      - Shoot
Right Mouse     - Aim (zoom)
R               - Reload
Scroll Wheel    - Switch Weapon
1/2/3           - Quick select weapon (if implemented)

INTERACTION:
E               - Interact with objects/NPCs
Hold E          - Extended interaction (if applicable)

GENERAL:
ESC             - Pause Menu
Tab             - Show objectives (if implemented)
M               - Map (if implemented)

HOW TO PLAY:
═══════════════════════════════════════════════════════════════════
OBJECTIVE:
Survive waves of zombie attacks while exploring Las Vegas. Earn points
by defeating zombies, use points to purchase weapons and healing from
NPCs scattered throughout the environment.

GETTING STARTED:
1. You spawn on the Las Vegas strip with a basic pistol
2. Look for the Tutorial NPC (marked with info icon) to learn basics
3. Explore the environment to find keys, ammo, and weapons
4. Defend yourself against zombie waves

GAME MECHANICS:
- HEALTH: Your health depletes when hit by zombies. Find the Medic NPC
  to heal for 200 points.
  
- AMMO: Weapons use magazine and backpack ammo. Magazine empties first,
  press R to reload from backpack. Find ammo pickups to refill backpack.
  
- POINTS: Earn points by killing zombies (100 per kill). Use points at
  the Shop NPC to purchase better weapons.
  
- WAVES: Zombies spawn in waves. Each wave increases in difficulty with
  more and faster enemies.
  
- KEYS & DOORS: Some doors are locked and require colored keys. Find
  keys throughout the environment to access new areas.

NPCS:
═══════════════════════════════════════════════════════════════════
TUTORIAL NPC (Blue):
- Explains game mechanics
- Free information, no cost
- Usually near spawn area

MEDIC NPC (Green):
- Heals player to full health
- Cost: 200 points
- Use when health is low

SHOPKEEPER NPC (Yellow):
- Sells weapons
- Prices vary (500-2000 points)
- Browse available weapons and purchase

TIPS & STRATEGIES:
═══════════════════════════════════════════════════════════════════
- Headshots deal more damage (if implemented)
- Keep moving to avoid being surrounded
- Save points for weapon upgrades
- Explore to find free pickups before buying
- Learn zombie spawn locations and prepare
- Use environment as cover
- Reload during safe moments, not during combat
- Higher ground provides tactical advantage

SETTINGS:
═══════════════════════════════════════════════════════════════════
Access settings through Main Menu or Pause Menu:

GRAPHICS:
- Quality Preset: Low / Medium / High / Ultra
- Resolution: [Available resolutions]
- Fullscreen / Windowed
- V-Sync: On / Off
- Shadow Quality: Off / Low / Medium / High
- Texture Quality: Low / Medium / High

AUDIO:
- Master Volume: 0-100%
- Music Volume: 0-100%
- SFX Volume: 0-100%
- Mute All: Toggle

GAMEPLAY:
- Mouse Sensitivity: 1-10
- Invert Y-Axis: Toggle
- Difficulty: Easy / Normal / Hard (if implemented)
- Show FPS Counter: Toggle (if implemented)

TROUBLESHOOTING:
═══════════════════════════════════════════════════════════════════
Problem: Low FPS / Performance Issues
Solution: Lower graphics settings, close background applications,
ensure GPU drivers are updated

Problem: Game won't start
Solution: Verify DirectX 11 is installed, run as administrator,
check Windows Firewall isn't blocking

Problem: Controls not responding
Solution: Ensure no controller is plugged in (may conflict),
restart game, check control bindings in settings

Problem: Audio issues
Solution: Check master volume isn't muted, verify Windows audio
settings, update audio drivers

For additional support: [Your email or support method]
```

---

#### SECTION H: Screenshots

**Length**: 3-5 pages

**What to Include**:
At least 15-20 high-quality screenshots organized by category

```
SCREENSHOTS

ENVIRONMENT - LAS VEGAS REPRESENTATION:
═══════════════════════════════════════════════════════════════════
[Screenshot 1: Wide shot of Vegas strip with casino buildings]
Caption: Las Vegas Strip overview showing main playable area with
casino buildings and neon signage

[Screenshot 2: Close-up of casino exterior detailing]
Caption: Detailed view of casino architecture with authentic Vegas
aesthetic using Synty assets

[Screenshot 3: Street-level view with props and environment dressing]
Caption: Ground-level exploration view showing street details,
vehicles, and environmental props

[Screenshot 4: Skybox and atmospheric effects]
Caption: Vegas night sky with atmospheric lighting and post-processing
effects creating immersive atmosphere

GAMEPLAY - COMBAT SYSTEM:
═══════════════════════════════════════════════════════════════════
[Screenshot 5: Player shooting at zombies]
Caption: Combat engagement showing weapon firing, muzzle flash effects,
and enemy encounters

[Screenshot 6: Weapon switching UI]
Caption: Inventory system with multiple weapons, showing weapon
switching interface

[Screenshot 7: Reload action with UI feedback]
Caption: Reload prompt and ammo UI demonstrating magazine/backpack
ammo system

[Screenshot 8: Hitmarker and combat feedback]
Caption: Hitmarker feedback on successful hit with UI elements
displaying combat information

INTERACTION SYSTEM:
═══════════════════════════════════════════════════════════════════
[Screenshot 9: NPC interaction - Shopkeeper]
Caption: Player interacting with Shopkeeper NPC showing weapon
purchase interface

[Screenshot 10: NPC interaction - Medic]
Caption: Medic NPC interaction showing healing service option

[Screenshot 11: Door with key requirement]
Caption: Locked door showing interaction prompt and key requirement

[Screenshot 12: Pickup collection]
Caption: Item pickup interaction showing ammo/health collection

USER INTERFACE:
═══════════════════════════════════════════════════════════════════
[Screenshot 13: Main Menu]
Caption: Main menu showing Play, Settings, Controls, Credits options
with polished UI design

[Screenshot 14: Settings menu]
Caption: Settings interface displaying graphics, audio, and gameplay
customization options

[Screenshot 15: In-game HUD]
Caption: Complete HUD layout showing health bar, ammo counter, points
display, and interaction prompts

[Screenshot 16: Pause menu]
Caption: Pause menu with resume, settings, and quit options

SPECIAL FEATURES:
═══════════════════════════════════════════════════════════════════
[Screenshot 17: Wave system in action]
Caption: Multiple zombies spawning during wave event showing enemy AI
pathfinding

[Screenshot 18: Environmental storytelling]
Caption: Vegas details that enhance environment (signs, decorations,
atmosphere)

[Screenshot 19: Lighting showcase]
Caption: Neon lighting and dynamic shadows demonstrating visual quality

[Screenshot 20: Particle effects]
Caption: Visual effects including muzzle flash, blood, and impact
particles

NOTE: All screenshots captured at 1920x1080 resolution, maximum
graphics settings, showing representative gameplay moments.
```

---

#### SECTION I: Criteria Addressing (Απάντηση Κριτηρίων)

**Length**: 4-6 pages

**What to Include**:
DETAILED response for EACH of the 11 evaluation criteria

```
HOW WE ADDRESS EVALUATION CRITERIA

1. ELEMENTS TAUGHT IN CLASS
═══════════════════════════════════════════════════════════════════
We implemented numerous techniques covered in course lectures:

UNITY SYSTEMS TAUGHT:
✓ NavMesh AI for enemy pathfinding (Week 5-6 lectures)
✓ Animation system integration with Animator and Animation Rigging
✓ Particle systems for visual effects
✓ Post-processing stack for visual enhancement
✓ New Input System for player controls
✓ Raycasting for interactions and shooting
✓ Event-driven architecture
✓ ScriptableObjects for data management

OPTIMIZATION TECHNIQUES (ESPECIALLY VALUED):
✓ Object Pooling: Implemented for zombies and bullets to avoid
  instantiation costs during gameplay. Pre-allocate 20 zombies and
  50 bullets at start, reuse instead of Destroy/Instantiate.
  Result: Eliminated garbage collection spikes, improved performance by ~25%

✓ Occlusion Culling: Configured for casino interiors so cameras don't
  render objects blocked by walls.
  Result: 15% FPS improvement in dense areas

✓ LOD (Level of Detail) Groups: Applied to distant buildings with
  reduced polygon versions at far distances.
  Result: Maintained 60 FPS with large environment

✓ Efficient Collision Detection: Used layer-based collision matrix
  to prevent unnecessary collision checks (e.g., zombies don't check
  collision with other zombies).
  Result: Reduced physics overhead

✓ Reduced Update Frequency: NavMesh path recalculation for distant
  zombies happens less frequently than nearby zombies.
  Result: Better CPU utilization

PROFILING EVIDENCE:
[Include Unity Profiler screenshot showing performance metrics before/after]

2. REALISM (ΑΛΗΘΟΦΆΝΕΙΑ)
═══════════════════════════════════════════════════════════════════
Our Las Vegas environment achieves realism through multiple layers:

VISUAL REALISM:
- Accurate scale: Buildings and streets sized appropriately relative
  to human player
- Recognizable landmarks: Casino architecture reflects real Vegas style
- Authentic materials: Neon, glass, concrete textures match real-world
- Consistent art style: Synty's cohesive low-poly aesthetic maintains
  visual believability

PHYSICAL REALISM:
- Gravity: All objects affected by Unity's physics (9.81 m/s²)
- Collision: Player and objects can't pass through walls/floors
- Bullet physics: Projectiles follow ballistic trajectories
- NavMesh pathfinding: Zombies navigate realistically around obstacles,
  don't walk through walls or clip through geometry

BEHAVIORAL REALISM:
- Zombie AI: Enemies detect player, chase, and attack intelligently
- NPC reactions: Characters provide contextually appropriate dialogue
- Dynamic spawning: Zombies emerge from logical locations (not in
  player's view)
- Economy system: Points-based trading feels intuitive

ATMOSPHERIC REALISM:
- Lighting: Neon signs cast colored light, creating Vegas night ambiance
- Sound design: 3D spatial audio makes sounds come from proper directions
- Weather/atmosphere: Sky and environmental effects sell outdoor Vegas
- Post-processing: Bloom on neon, ambient occlusion adds depth

DOES NOT VIOLATE PHYSICAL LAWS:
- No floating objects (all anchored or physics-based)
- Appropriate movement speeds (player runs ~6 m/s, realistic)
- Doors open/close logically with animations
- Bullets have limited range and drop off with distance

3. CONTENT (ΠΕΡΙΕΧΌΜΕΝΟ)
═══════════════════════════════════════════════════════════════════
Our content draws from real-world Las Vegas:

REAL-WORLD INSPIRATION:
- Casino architecture styled after famous Vegas establishments
- Street layout inspired by Las Vegas Boulevard (The Strip)
- Neon signage aesthetic reflects Vegas branding culture
- Desert environment surroundings match Nevada geography
- Entertainment-focused atmosphere matches tourist destination nature

ASSET SOURCES (PROPERLY CITED):
All assets properly attributed in:
- In-game Credits menu (accessible from main menu)
- Documentation (Section F - Asset Sources)
- Information board near spawn point in-game
- README.txt file included with build

Primary sources:
- Synty Studios: POLYGON packs for 3D models
- Unity Asset Store: Various audio and effect packages
- Google Fonts: UI typography
- All properly licensed for use in student projects

APPROPRIATE INFORMATION DISPLAY:
We cited sources within the virtual world through:
- Credits screen in main menu (permanently accessible)
- Physical information board in starting area players can read
- End credits sequence that plays after game over

4. COMPLETENESS (ΠΛΗΡΌΤΗΤΑ)
═══════════════════════════════════════════════════════════════════
Our project is complete as a functional VR/3D experience:

APPEARS AS 3D WORLD:
✓ First/third-person perspective with 6DOF camera
✓ Fully navigable 3D environment
✓ Depth perception through proper camera settings and post-processing
✓ Spatial audio creates 3D sound space

HAS FUNCTIONALITY:
✓ Player movement (walk, run, jump, crouch)
✓ Combat system (shoot, reload, aim, switch weapons)
✓ Interaction system (E key for NPCs, doors, pickups)
✓ Economy system (points earning and spending)
✓ Health management (damage, healing)
✓ Inventory management (weapons, ammo tracking)

EVOLVES OVER TIME:
✓ Wave-based zombie spawning increases difficulty
✓ Animated zombies constantly moving
✓ Pickup items bob and rotate continuously
✓ Neon signs flicker and animate
✓ Particle effects play during combat
✓ UI elements animate (point gains, damage indicators)

HAS LIGHTING:
✓ Directional light (sun) for overall illumination
✓ Point lights on neon signs creating colored ambient light
✓ Spot lights for focused illumination (building entrances)
✓ Emissive materials on signs glow appropriately
✓ Real-time shadows cast by environmental geometry
✓ Post-processing bloom enhances light sources

HAS DECORATIVE ELEMENTS:
✓ Props: vehicles, trash cans, signs, decorations
✓ Vegetation: palm trees, desert plants
✓ Signage: casino names, directional signs, advertisements
✓ Atmospheric: particles, fog, lens effects
✓ Architectural details: windows, doors, facades

PROVIDES COMPLETE EXPERIENCE:
✓ Clear beginning (main menu, spawn)
✓ Middle (exploration, combat, progression)
✓ End states (game over, victory)
✓ Full UI for all game states
✓ Settings for customization
✓ Tutorial for new players

NO UNJUSTIFIED GAPS:
- Environment is consistently detailed throughout
- All buildings have collisions (no walking through walls)
- Audio plays appropriately for all actions
- UI provides feedback for all player actions
- No missing textures or placeholder graphics

5. DESIGN (ΣΧΕΔΙΑΣΜΌΣ)
═══════════════════════════════════════════════════════════════════
Our design emphasizes precision and intentionality:

ARCHITECTURAL PRECISION:
- Component-based architecture: Each script has single responsibility
- Interface-driven design: IInteractable, IDamageable define contracts
- Event-driven communication: Decoupled systems communicate via events
- Data-driven balancing: ScriptableObjects separate data from logic

CODE DESIGN PATTERNS:
- Singleton: Managers (WaveManager, PauseManager) for global access
- Observer: Event system for component communication
- State: Enemy AI behavior states (Idle, Chase, Attack)
- Strategy: Different weapon behaviors through WeaponData
- Object Pool: Reusable zombie and bullet instances
- Command: Input handling abstraction

LEVEL DESIGN PRECISION:
- Deliberate pacing: Tutorial area → open exploration → combat zones
- Visual landmarks: Distinct buildings guide navigation
- Strategic spawn points: Zombies approach from varied angles
- Resource distribution: Balanced pickup placement rewards exploration
- Sightlines: Open areas for combat, tight spaces for tension

NOT OVERLY COMPLEX:
We intentionally avoided complexity for complexity's sake:
- Could have added crafting system - decided against (scope creep)
- Could have made 50 weapon types - chose 3-5 meaningful ones
- Could have huge map - focused on dense, quality area
- Could have complex skill trees - kept progression clear

ATTENTION TO DETAIL:
- Consistent naming conventions throughout codebase
- Organized project folders (Scripts/Player, Scripts/Enemy, etc.)
- Proper layer and tag usage
- Inspector-organized components with headers and tooltips
- Thoughtful default values in ScriptableObjects

6. AESTHETICS (ΑΙΣΘΗΤΙΚΉ)
═══════════════════════════════════════════════════════════════════
Our aesthetic is presentable, cohesive, and inviting:

VISUAL LANGUAGE:
- Low-poly art style: Clean, readable, performs well
- Vibrant color palette: Neon pinks, blues, purples for Vegas vibe
- High contrast: Ensures UI and interactive elements are visible
- Consistent materials: All assets share similar shader properties

NOT CHARACTERIZED BY EXTREMES:
- Not hyper-realistic (would conflict with low-poly models)
- Not ultra-minimalist (enough detail to be interesting)
- Not garish (despite neon, maintains visual balance)
- Not dull (enough color and light to be engaging)

INVITES THE USER:
- Bright neon signs draw attention and guide exploration
- Recognizable Vegas landmarks create curiosity
- Clear visual hierarchy: Important = bright, visible
- Welcoming starting area with tutorial NPC
- Interesting silhouettes make buildings distinctive

GRABS ATTENTION:
- Dynamic lighting effects (flickering neon, shadows)
- Particle effects during combat provide spectacle
- Animated elements create visual interest
- Color contrast makes interactive objects stand out
- Post-processing bloom makes lights "pop"

UI AESTHETICS:
- Clean, modern UI design
- Readable fonts with appropriate sizing
- Consistent button styling
- Smooth animations on interactions
- Semi-transparent backgrounds don't obstruct view

7. ORIGINALITY (ΠΡΩΤΟΤΥΠΊΑ)
═══════════════════════════════════════════════════════════════════
We explored Unity's capabilities and C# extensively:

UNITY FEATURES EXPLORED:
- Universal Render Pipeline (URP): Modern rendering pipeline
- Animation Rigging: Procedural weapon handling
- New Input System: Flexible control binding
- NavMesh: AI pathfinding with dynamic obstacles
- Timeline: Could be used for cutscenes (if implemented)
- Post-Processing Stack v2: Visual effects
- Particle System: Custom effect creation
- Cinemachine: Camera behaviors (if implemented)
- Scriptable Build Pipeline: Efficient builds

C# LANGUAGE CAPABILITIES:
- Events and delegates for loose coupling
- Interfaces for polymorphic behavior
- Generics for reusable systems (could expand with generic pooling)
- Coroutines for time-based operations
- LINQ for data queries (if used in managers)
- Properties with get/set for encapsulation
- Extension methods for utility functions
- Enums for state management
- Structs for data containers

DESIGN PATTERNS:
- Singleton: Global manager access
- Observer: Event-driven architecture
- State: AI behavior management
- Strategy: Weapon behavior variation
- Object Pool: Performance optimization
- Factory: Could be used for enemy spawning
- Command: Input abstraction

UNIQUE IMPLEMENTATIONS:
- Dual ammo system (magazine + backpack) is uncommon
- Dynamic weapon rig switching for smooth transitions
- NPC economy integrated naturally with combat
- Interaction system with multiple callback methods
- Static key system decoupled from player

8. USABILITY (ΧΡΗΣΤΙΚΌΤΗΤΑ)
═══════════════════════════════════════════════════════════════════
Our project is designed for human use and enjoyment:

INTUITIVE CONTROLS:
- Industry-standard FPS controls (WASD, mouse look)
- Familiar combat (left-click shoot, right-click aim, R reload)
- Clear interaction prompt (E key with on-screen message)
- Scroll wheel weapon switching (common in shooters)
- Logical button mappings (ESC for pause, Space for jump)

EXPLORATION CAPABILITY:
- Can freely walk through entire Las Vegas environment
- Multiple distinct areas to discover
- No invisible walls blocking reasonable paths
- Clear visual indicators of traversable terrain
- Mini-map or landmarks help orientation (if implemented)

MEETING REAL USER NEEDS:
- Entertainment: Combat and exploration are engaging
- Challenge: Progressive difficulty provides satisfaction
- Mastery: Learning enemy patterns and improving aim
- Exploration: Discovering new areas and secrets
- Collection: Finding weapons and upgrades
- Social: Leaderboard/scoring for comparison (if implemented)

ACCESSIBILITY:
- Adjustable difficulty through settings
- Customizable mouse sensitivity
- Volume controls for different audio types
- Graphics settings for various PC specs
- Control remapping available (if implemented)
- Tutorial NPC explains mechanics for new players

FEEDBACK SYSTEMS:
- Visual: HUD updates, hitmarkers, damage flashes
- Audio: Weapon sounds, zombie groans, UI clicks
- Haptic: (could add controller rumble)
- Clear win/loss states
- Progress indicators (wave counter)

QUALITY OF LIFE:
- Pause menu accessible anytime
- Can adjust settings mid-game
- Clear objective indicators
- Health/ammo always visible
- Auto-save (if implemented)

9. ANIMATION (ΚΊΝΗΣΗ)
═══════════════════════════════════════════════════════════════════
Our world is dynamic and evolving:

CHARACTER ANIMATIONS:
- Player: Walk, run, jump, crouch, idle breathing, weapon handling
- Zombies: Walk cycle, run cycle, attack swing, death falls, idle fidgets
- NPCs: Idle animations, greeting gestures, talking motions

WEAPON ANIMATIONS:
- Shooting recoil animation
- Reload sequence (magazine out, new magazine in)
- Weapon switching transitions via Animation Rigging
- Procedural aim offsets (weapon points at target)
- Bobbing during movement for realism

ENVIRONMENTAL ANIMATIONS:
- Pickups: Constant bobbing (float) and rotation for visibility
- Doors: Swing open/closed with smooth easing
- Neon signs: Flickering effect, color cycling
- Slot machines: Spinning reels, flashing lights (if implemented)
- Traffic lights: State changes (red/yellow/green)
- Flags: Cloth simulation waving (if implemented)
- Vegetation: Wind movement on palm trees (if implemented)

PROPERTY ANIMATIONS (not just position):
- UI elements: Fade in/out (alpha change), scale on hover (size change)
- Damage indicators: Flash red (color change)
- Points gain: Scale up briefly on point acquisition
- Health bar: Color transitions (green → yellow → red) based on health
- Ammo counter: Color changes when low (orange/red warning)
- Hitmarker: Brief appearance and fade

PARTICLE ANIMATIONS:
- Muzzle flash: Brief burst when shooting
- Blood splatter: On zombie damage
- Impact sparks: When bullets hit surfaces
- Dust clouds: On zombie death
- Smoke trails: If applicable
- Fire effects: If environmental hazards present

DYNAMIC WORLD ELEMENTS:
- Zombies spawn periodically in waves (appearance animation)
- Random pickups spawn from dead enemies (drop animation)
- Weather particles: Light dust/fog moving (if implemented)
- Camera shake: On explosions or impacts
- Screen effects: Damage vignette pulsing

CREATES LIVING WORLD:
All these animations combine to create impression of dynamic,
time-evolving environment. Nothing is static - constant movement
at multiple scales keeps world feeling alive and reactive.

10. FUNCTIONALITY (ΛΕΙΤΟΥΡΓΙΚΌΤΗΤΑ)
═══════════════════════════════════════════════════════════════════
Extensive interactive and reactive elements:

USER-ACTIVATED INTERACTIONS:

1. Locked Doors:
   - Player attempts to open
   - If has key: Door unlocks permanently, swings open
   - If no key: Shows "Need [Color] Key" message
   - State persists: Once unlocked, stays unlocked

2. Keys:
   - Player approaches key pickup
   - Press E to collect
   - Key added to static PlayerKeys inventory
   - Key object disappears from world
   - Can now unlock corresponding colored doors

3. NPC - Shopkeeper:
   - Player approaches, prompt appears: "Press E to shop"
   - Press E: Shop interface opens
   - Browse weapons with costs displayed
   - Click purchase: If sufficient points, weapon added to inventory
   - If insufficient points: "Not enough points" message
   - Can exit shop without buying

4. NPC - Medic:
   - Player approaches, prompt: "Press E to heal (200 points)"
   - Press E: If health < max AND points >= 200:
     → Health restored to full
     → 200 points deducted
     → Healing effect plays
   - If health full: "Already at full health"
   - If insufficient points: "Need 200 points"

5. NPC - Tutorial:
   - Player approaches, prompt: "Press E to talk"
   - Press E: Information panel opens
   - Shows tutorial messages explaining mechanics
   - Can close with E or ESC
   - Free, no cost

6. Weapon System:
   - Scroll wheel: Switches between owned weapons
   - Left-click: Fires current weapon
   - Right-click: Zooms in for aiming
   - R key: Reloads from backpack to magazine
   - Each action has immediate response and feedback

7. Pickups (Auto-collect):
   - Player touches health/ammo/weapon pickup
   - OnTriggerEnter detects player
   - Item collected: Affects inventory/health
   - Pickup object disappears
   - Collection feedback (sound, UI update)

8. Menu Interactions:
   - Hover over button: Highlight/scale change
   - Click button: Navigate to screen or perform action
   - Sliders: Drag to adjust volume/sensitivity
   - Toggles: Click to enable/disable options
   - All changes apply immediately

REACTIVE ELEMENTS:

1. Zombies:
   - Proximity detection: Player enters trigger zone
   - State change: Idle → Chase
   - NavMesh pathfinding: Navigates toward player
   - Attack range: Reaches player, attacks
   - Takes damage: Health decreases
   - Death: Plays death animation, awards points, spawns loot

2. Health System:
   - Takes damage: Health value decreases
   - UI updates: Health bar shrinks, changes color
   - Critical health: Screen vignette pulses red
   - Death: Health reaches 0, game over triggered
   - Healing: Health increases when healed

3. Wave System:
   - Time-based: Every X seconds, new wave spawns
   - Kill-based: All zombies dead triggers next wave
   - Difficulty scales: More zombies, faster enemies each wave
   - UI feedback: Wave number displays
   - Automatic: No player input required

4. Points System:
   - Zombie killed: Points added (100)
   - UI animates: Points counter scales up briefly
   - Gates functionality: Need points for shop/heal
   - Persistent: Carries across waves

5. Ammo System:
   - Shooting: Magazine ammo decreases
   - Empty magazine: Weapon won't fire, prompt to reload
   - Reload: Magazine refills from backpack
   - Backpack empty: Can't reload, must find ammo
   - UI feedback: Ammo counters update in real-time

CONDITIONAL FUNCTIONALITY:
- Doors: Open ONLY IF player has matching key
- Shop: Sell ONLY IF player has enough points
- Heal: Work ONLY IF health below max AND points sufficient
- Reload: Work ONLY IF backpack has ammo AND magazine not full
- NPC interactions: Trigger ONLY WHEN player presses E while looking at NPC
- Weapon switch: Possible ONLY IF player owns multiple weapons

EXAMPLES FROM ASSIGNMENT:
The assignment gave examples like doors with switches, mechanisms
with levers, robots that activate on approach. We exceeded this with:
- Multiple NPC types with different services (commerce, healing, info)
- Economic system where purchases have costs and requirements
- Conditional state changes (doors that unlock permanently)
- Autonomous systems (wave spawner) that react to game state
- Multi-layered interactions (shop has browsing + purchasing)

11. DEVELOPMENT (ΑΝΆΠΤΥΞΗ)
═══════════════════════════════════════════════════════════════════
Code quality and professional practices:

PLATFORM & TOOLS:
- Engine: Unity 6000.2.8f1
- Render Pipeline: Universal Render Pipeline (URP)
- Language: C# (100% of gameplay code)
- IDE: Visual Studio 2022 [or Rider]
- Version Control: Git with GitHub [if used]

BEST IMPLEMENTATION TECHNIQUES:
✓ Component-based architecture (Unity paradigm)
✓ Separation of concerns (single responsibility)
✓ Interface-based programming (polymorphism)
✓ Event-driven communication (decoupling)
✓ Data-driven design (ScriptableObjects)
✓ Object-oriented principles (encapsulation, inheritance)
✓ Design patterns (Singleton, Observer, State, Strategy, Pool)
✓ Caching: Store GetComponent results, don't call repeatedly
✓ Coroutines: Time-based operations without Update loops
✓ Layer-based collision: Efficient collision detection

CODE QUALITY STANDARDS:
✓ XML documentation on every class
✓ Summary comments on all public methods
✓ Inline comments explaining complex logic
✓ Descriptive variable names (no 'x', 'temp', 'data')
✓ Consistent naming conventions (PascalCase public, camelCase private)
✓ No magic numbers (const float HEAL_COST = 200f;)
✓ Regions organize code sections
✓ No commented-out debug code in final version
✓ Proper null checks before accessing components

BILINGUAL COMMENTING:
Every significant code block commented in BOTH Greek and English:

```csharp
/// <summary>
/// Handles weapon reloading when player presses R key.
/// Διαχειρίζεται την επαναφόρτιση όπλου όταν ο παίκτης πιέζει το R.
/// </summary>
private void HandleReload()
{
    // Check if we have backpack ammo to reload from
    // Έλεγχος αν έχουμε ammo στο backpack για reload
    int backpackAmmo = inventory.GetBackpackAmmo(currentWeapon.ammoType);
    if (backpackAmmo <= 0)
    {
        // No ammo available - show feedback to player
        // Δεν υπάρχει διαθέσιμο ammo - εμφάνιση feedback στον παίκτη
        ShowNoAmmoMessage();
        return;
    }
    
    // Calculate how much ammo we need to fill magazine
    // Υπολογισμός πόσο ammo χρειαζόμαστε για να γεμίσουμε το magazine
    int currentMagAmmo = inventory.GetMagazineAmmo(currentWeapon.ammoType);
    int ammoNeeded = currentWeapon.magazineCapacity - currentMagAmmo;
    int ammoToTransfer = Mathf.Min(ammoNeeded, backpackAmmo);
    
    // Transfer ammo from backpack to magazine
    // Μεταφορά ammo από backpack σε magazine
    inventory.AddMagazineAmmo(currentWeapon.ammoType, ammoToTransfer);
    inventory.ConsumeBackpackAmmo(currentWeapon.ammoType, ammoToTransfer);
    
    // Play reload animation and sound
    // Παίξε reload animation και ήχο
    PlayReloadAnimation();
}
```

PROJECT ORGANIZATION:
```
Assets/
├── Scenes/
├── Scripts/
│   ├── Player/
│   ├── Weapons/
│   ├── Enemies/
│   ├── Interactions/
│   ├── NPCs/
│   ├── Managers/
│   ├── UI/
│   └── Data/
├── Prefabs/
├── Materials/
├── Audio/
└── [Synty Assets]/
```

WHY THIS MATTERS:
The assignment explicitly states code must be "συνολικά, αναλυτικά
και κατανοητά σχολιασμένος" (comprehensively, analytically, and
understandably commented). Our bilingual commenting demonstrates we
took this seriously. Clean architecture and best practices show
technical maturity beyond just "making it work."
```

---

## TEAM CONTRIBUTIONS (if 3-person team)

**Add this section if working in a team**:

```
TEAM MEMBER CONTRIBUTIONS

This project was developed by a team of three members. While we
collaborated on all aspects, each member took primary responsibility
for specific systems:

MEMBER A: [Name] - Player Systems Specialist (33%)
───────────────────────────────────────────────────────────────────
Primary Responsibilities:
- Player movement integration (Synty controller)
- Weapon controller implementation
- Inventory system development
- Shooting mechanics and aiming
- Ammo management (magazine/backpack system)

Scripts Authored:
- WeaponController.cs
- Inventory.cs
- PlayerHealth.cs
- PlayerKeys.cs
- Bullet.cs
- WeaponData.cs (ScriptableObject)

Contributions to Shared Tasks:
- Code review and optimization
- Testing and bug fixing
- Documentation writing (Sections C, D, E)
- Video recording

MEMBER B: [Name] - AI & Systems Specialist (33%)
───────────────────────────────────────────────────────────────────
Primary Responsibilities:
- Enemy AI development
- NavMesh pathfinding implementation
- Wave management system
- NPC interaction systems
- Pickup and drop systems

Scripts Authored:
- EnemyAI.cs
- EnemyHealth.cs
- EnemyData.cs (ScriptableObject)
- WaveManager.cs
- NPCBase.cs
- MedicNPC.cs
- ShopkeeperNPC.cs
- TutorialNPC.cs
- RandomDrop.cs
- PickupItem.cs

Contributions to Shared Tasks:
- Balancing (enemy difficulty, economy)
- Testing enemy behaviors
- Documentation writing (Sections A, B, G)
- PowerPoint creation

MEMBER C: [Name] - UI & Integration Specialist (34%)
───────────────────────────────────────────────────────────────────
Primary Responsibilities:
- All UI systems implementation
- Interaction system development
- Main menu and settings
- Environment setup and lighting
- Scene composition and polish

Scripts Authored:
- Interactor.cs
- IInteractable.cs (Interface)
- InteractorUI.cs
- AmmoUI.cs
- PointsUI.cs
- PlayerHealthUI.cs
- ReloadPromptUI.cs
- GameOverScreen.cs
- HitmarkerDisplay.cs
- PauseManager.cs
- LockedDoor.cs
- KeyPickup.cs
- TextSign.cs

Contributions to Shared Tasks:
- Visual polish and atmosphere
- Optimization (occlusion culling, LOD setup)
- Documentation writing (Sections F, H, I)
- Build configuration and testing

COLLABORATIVE WORK:
───────────────────────────────────────────────────────────────────
- Architecture planning: All members
- Design decisions: All members
- Integration testing: All members
- Code review: All members
- Bug fixing: All members
- Documentation: All members
- Final polish: All members

COMMUNICATION:
- Weekly team meetings
- Discord channel for daily communication
- Git for version control and code sharing
- Shared Google Doc for design documentation
- Playtesting sessions together

All team members contributed roughly equally to the project's success.
While we had specialized roles, we maintained collaborative approach
throughout development process.
```

---

## CONCLUSION

Add final section:

```
CONCLUSION

ACHIEVEMENTS:
This project successfully demonstrates our understanding of virtual
reality environment creation, Unity development, and software
engineering principles taught throughout the course. We created a
complete, functional Las Vegas tourist destination with engaging
interactive elements that align with all assignment criteria.

KEY ACCOMPLISHMENTS:
✓ Fully explorable Las Vegas environment
✓ Complete combat and survival gameplay loop
✓ Multiple interactive NPC types
✓ Polished UI with comprehensive settings
✓ Well-architected, commented codebase
✓ Optimized for consistent 60 FPS performance
✓ Comprehensive documentation

TECHNICAL GROWTH:
Through this project, we deepened our knowledge of:
- Unity's Universal Render Pipeline
- Advanced C# programming patterns
- Performance optimization techniques
- AI pathfinding with NavMesh
- Event-driven architecture
- ScriptableObject data management
- User interface design principles

CHALLENGES OVERCOME:
- Balancing visual quality with performance requirements
- Implementing smooth weapon switching with Animation Rigging
- Designing intuitive interaction system for diverse objects
- Creating believable AI behavior with NavMesh
- Integrating multiple complex systems cohesively

FUTURE IMPROVEMENTS:
Given more time, potential enhancements could include:
- Additional weapon variety (shotguns, explosives)
- More expansive Vegas area (additional casinos, hotel interiors)
- Quest/objective system for structured gameplay
- Multiplayer support for cooperative play
- Save/load system for progress persistence
- More environmental interactivity (slot machines, etc.)
- Day/night cycle affecting zombie behavior

LESSONS LEARNED:
- Importance of planning architecture before coding
- Value of optimization early in development
- Need for consistent testing and iteration
- Benefits of component-based design for flexibility
- Critical role of clear documentation

We believe this project successfully fulfills the assignment
requirements while demonstrating technical competence and creative
implementation. The combination of authentic Las Vegas representation
with engaging survival gameplay creates a unique, memorable experience
that invites exploration while providing meaningful interactivity.

[Team Names]
[Date]
```

---

## FORMATTING GUIDELINES

**Document Structure**:
- Title page
- Table of contents (auto-generated)
- All sections A through I
- Team contributions (if applicable)
- Conclusion
- Appendices (if needed)

**Typography**:
- Body: 11-12pt, serif font (Times New Roman, Garamond)
- Headers: Bold, larger size
- Code: Monospace font (Consolas, Courier New)
- Captions: Italic, slightly smaller

**Spacing**:
- 1.5 line spacing for body text
- Single spacing for code blocks
- Extra space before/after headers

**Page Layout**:
- Margins: 2.5cm all sides
- Page numbers: Bottom center or top right
- Headers: Section name (optional)

**Visuals**:
- High-resolution screenshots (1920x1080)
- Diagrams for architecture
- Tables for script lists
- Captions for all images

**Professional Appearance**:
- Consistent formatting throughout
- No spelling/grammar errors
- Proper punctuation
- Clear hierarchy
- Professional tone

---

## SUBMISSION CHECKLIST

Before submitting, verify you have:

□ Unity project files (all Assets, ProjectSettings, Packages)
□ Build files (Windows .exe minimum, with _Data folder)
□ PowerPoint presentation (10-15 slides)
□ Video walkthrough (2-4 minutes, MP4 format)
□ Complete manual/documentation (PDF, 15-30 pages) with:
  □ Section A: Introduction
  □ Section B: Problem Description
  □ Section C: Development Phases (Analysis, Design, Implementation)
  □ Section D: Script Table
  □ Section E: Detailed Script Analysis (4-5 scripts)
  □ Section F: Asset Sources
  □ Section G: User Manual
  □ Section H: Screenshots (15-20 quality images)
  □ Section I: Criteria Addressing (all 11 criteria)
  □ Team Contributions (if team project)
  □ Conclusion
□ README.txt with:
  □ Team member names and student IDs
  □ Brief project description
  □ System requirements
  □ How to run the build
  □ Contact information
□ All files compressed or uploaded to shared drive
□ Submission form filled out on Thales platform
□ Email/phone included in case of technical issues

FINAL CHECKS:
□ Build runs without errors
□ Video plays correctly
□ PowerPoint opens properly
□ PDF is readable and properly formatted
□ All images display correctly
□ No corrupted files
□ File naming is clear and professional
□ Total submission size noted (if large, use cloud storage)

---

## GRADING EXPECTATIONS

Based on criteria weighting:

**Elements Taught (15 points)**:
- Clear use of Unity systems: 5 points
- Optimization techniques: 10 points

**Realism (10 points)**:
- Visual believability: 4 points
- Physical accuracy: 3 points
- Atmospheric quality: 3 points

**Content (10 points)**:
- Real-world inspired: 5 points
- Proper attribution: 5 points

**Completeness (10 points)**:
- Functional systems: 5 points
- Polish and integration: 5 points

**Design (10 points)**:
- Code architecture: 5 points
- Level design: 5 points

**Aesthetics (10 points)**:
- Visual quality: 5 points
- Cohesion and appeal: 5 points

**Originality (10 points)**:
- Technical exploration: 5 points
- Creative implementation: 5 points

**Usability (10 points)**:
- User experience: 5 points
- Accessibility: 5 points

**Animation (5 points)**:
- Variety of animations: 3 points
- Dynamic world: 2 points

**Functionality (10 points)**:
- Interactive elements: 6 points
- Reactive systems: 4 points

**Development (10 points)**:
- Code quality: 5 points
- Comments and documentation: 5 points

**TOTAL: 100 points**

Target for passing: 50+ points
Target for good grade: 70+ points
Target for excellent grade: 85+ points

---

## REMEMBER

The professor wants to see:
1. **Comprehensive documentation** (most important!)
2. **Well-commented code** (bilingual preferred)
3. **Optimization techniques** (explicitly valued)
4. **Complete deliverables** (all 5 items)
5. **That you addressed all 11 criteria** (prove it in Section I)

The actual game quality matters less than documentation quality.
Focus your remaining effort on making docs perfect!

Good luck!
```
We implemented the following techniques taught in class:
- NavMesh AI for enemy pathfinding (Week X lecture)
- Event-driven architecture for decoupled systems (Week Y)
- ScriptableObjects for data management (Week Z)
- Animation rigging for weapon systems
- New Input System integration
- Raycasting for interaction and shooting mechanics

OPTIMIZATION TECHNIQUES USED:
- Object pooling for zombie spawns and bullets (reduces garbage collection)
- Occlusion culling for casino interiors (improves performance)
- LOD (Level of Detail) groups for distant buildings
- Efficient collision detection using layers
```

**Key Point**: MUST mention optimization - this is explicitly highlighted as valuable.

---

### 2. Realism (Αληθοφάνεια)
**Weight**: Medium-High  
**What They Want**:
- Space must "convince" the user
- Should not create impression of violating physical laws
- Must provide intended experience without relying on design or aesthetic extremes

**How to Address in Documentation**:
```
Our Las Vegas environment achieves realism through:
- Accurate scale and proportions of buildings and streets
- Realistic lighting (neon signs, ambient casino lighting)
- Physics-based bullet trajectories and zombie movement
- NavMesh pathfinding ensures enemies navigate realistically around obstacles
- Proper collision detection prevents players/objects from passing through walls
- Gravity and physics applied to all dynamic objects
- Sound design with 3D spatial audio for immersion
- Day/night cycle and atmospheric effects create believable environment
```

**Key Point**: Explain HOW your world feels real, not just that it looks good.

---

### 3. Content (Περιεχόμενο)
**Weight**: Medium  
**What They Want**:
- Use content reminiscent of the physical, "real" world
- **MUST cite sources** for all content (within the virtual world using appropriate information display methods)

**How to Address in Documentation**:
```
ASSET SOURCES:
- 3D Models: Synty Studios - Polygon City Pack
- Character Models: Synty Studios - Polygon Apocalypse Pack
- Textures: Unity Asset Store - [specific pack names]
- Sounds: [source names]
- Fonts: [source names]

IN-GAME ATTRIBUTION:
We included a "Credits" section in the main menu that displays all asset sources.
Additionally, we placed information boards throughout the Vegas environment that
reference real Las Vegas landmarks we represented.
```

**Key Point**: CITE EVERYTHING. They explicitly want sources referenced.

---

### 4. Completeness (Πληρότητα)
**Weight**: High  
**What They Want**:
- Space will be complete as a VR application component:
  - Appears as 3D/2D world to user
  - Has functionality at various points depending on what it represents
  - Evolves over time through moving elements and functional elements
  - Has lighting and decorative elements
  - Provides complete, multi-modal, dynamic representation experience without unjustified gaps, discontinuities, or inconsistencies

**How to Address in Documentation**:
```
Our application is complete with:

VISUAL COMPLETENESS:
- Fully navigable 3D Las Vegas strip environment
- Multiple distinct areas (casino floor, hotel exterior, parking garage, streets)
- Skybox with appropriate Vegas atmosphere
- Post-processing effects (bloom for neon, ambient occlusion)

FUNCTIONAL COMPLETENESS:
- Player movement system (walk, run, jump, crouch)
- Combat system (shooting, reloading, weapon switching)
- Interaction system (doors, NPCs, pickups)
- Economy system (points, shop, purchases)
- Health and damage system
- UI for all game states (menu, gameplay, pause, game over)

DYNAMIC ELEMENTS:
- Animated zombies with varied behaviors
- Moving pickups (bobbing, rotating)
- Weapon animations and effects
- Particle effects (muzzle flash, blood, explosions)
- Ambient animations (flickering lights, spinning signs)

AUDIO:
- Background music
- Weapon sound effects
- Zombie audio (groans, attacks)
- Ambient Vegas sounds
- UI feedback sounds
```

**Key Point**: Show your project is FINISHED, not a prototype. Cover visuals, functionality, audio, dynamics.

---

### 5. Design (Σχεδιασμός)
**Weight**: Medium  
**What They Want**:
- Design characterized by precision and detail
- **Note**: Increased design/structural complexity is NOT necessarily evaluated positively

**How to Address in Documentation**:
```
DESIGN PHILOSOPHY:
We focused on precise, detailed implementation rather than overwhelming complexity.

CODE ARCHITECTURE:
- Component-based design for modularity
- Interface pattern (IInteractable, IDamageable) for polymorphism
- Event-driven communication to decouple systems
- ScriptableObjects separate data from logic
- Single Responsibility Principle in each script

LEVEL DESIGN:
- Clear player flow through Vegas environment
- Strategic placement of zombies, pickups, and NPCs
- Visual landmarks guide navigation
- Lighting directs player attention
- Balanced difficulty progression through waves

UI/UX DESIGN:
- Clear information hierarchy
- Consistent visual language
- Intuitive controls (industry-standard FPS controls)
- Accessibility considerations (adjustable settings)
```

**Key Point**: Quality over quantity. Show thoughtful decisions, not just "we made it complex."

---

### 6. Aesthetics (Αισθητική)
**Weight**: Medium  
**What They Want**:
- Space must be presentable
- Not characterized by aesthetic extremes
- Should "invite" the user and grab their attention

**How to Address in Documentation**:
```
AESTHETIC APPROACH:
We chose a stylized, cohesive art direction that makes Las Vegas recognizable
while maintaining visual clarity for gameplay.

COLOR PALETTE:
- Neon pinks, blues, purples for Vegas nightlife atmosphere
- Warm yellow/orange lighting for interior spaces
- Dark, moody tones for post-apocalyptic elements
- High contrast for readability

VISUAL COHESION:
- Synty's low-poly art style maintains consistency
- Unified material properties across all objects
- Consistent lighting approach throughout environment
- Post-processing effects (bloom, color grading) tie scenes together

PLAYER INVITATION:
- Visually interesting landmarks draw exploration
- Neon signs and bright areas attract attention
- Clear visual differentiation between interactive and non-interactive elements
- Atmospheric effects (fog, particles) add depth and interest
```

**Key Point**: Explain your visual language and why it works. Not "it looks cool" but "here's our aesthetic strategy."

---

### 7. Originality (Πρωτοτυπία)
**Weight**: Medium  
**What They Want**:
- Structure, design, aesthetics, and functionality reflect extent of exploration of:
  - Platform's design capabilities
  - Language capabilities
  - Various design patterns and implementation practices

**How to Address in Documentation**:
```
EXPLORATION OF UNITY CAPABILITIES:
- Universal Render Pipeline (URP) for modern rendering
- New Input System for flexible control schemes
- Animation Rigging package for procedural weapon handling
- NavMesh system with dynamic obstacle avoidance
- Timeline for cutscenes/sequences
- Post-processing stack v2 for visual effects
- Cinemachine for camera behaviors

C# LANGUAGE FEATURES UTILIZED:
- Events and delegates for loose coupling
- Interfaces for polymorphic behavior
- Generics for reusable systems
- Coroutines for time-based operations
- LINQ for data queries
- Scriptable Objects as data containers

DESIGN PATTERNS IMPLEMENTED:
- Singleton pattern (managers)
- Observer pattern (event system)
- State pattern (enemy AI)
- Object pooling (performance)
- Command pattern (input handling)
- Strategy pattern (weapon behaviors)

UNIQUE FEATURES:
- Dual ammo system (magazine + backpack inventory)
- Dynamic weapon rig switching on the fly
- NPC economy integrated with combat progression
- Wave-based difficulty scaling
```

**Key Point**: Show technical depth. List specific Unity features, C# capabilities, and patterns you explored.

---

### 8. Usability (Χρηστικότητα)
**Weight**: Medium-High  
**What They Want**:
- Space intended for human use and meeting real needs
- Example: ability to move and visit various points of the 3D/2D world

**How to Address in Documentation**:
```
PLAYER MOVEMENT & EXPLORATION:
- Smooth first-person and third-person camera options
- WASD movement with intuitive sprint (Shift) and crouch (Ctrl)
- Free exploration of entire Las Vegas environment
- Clear visual cues for interactable objects
- Minimap/compass for orientation (if implemented)

USER INTERFACE:
- Clear HUD showing health, ammo, points
- Context-sensitive interaction prompts
- Tutorial messages for new players
- Pause menu accessible anytime (ESC)
- Settings menu with graphics/audio/gameplay options
- Control remapping available

ACCESSIBILITY:
- Adjustable difficulty through settings
- Visual and audio feedback for all actions
- Clear objective markers
- Tutorial NPC explains mechanics
- Multiple save slots (if implemented)

MEETING REAL NEEDS:
- Entertainment through engaging combat
- Exploration satisfaction via Vegas recreation
- Challenge through progressive difficulty
- Social aspect through leaderboard/scoring (if implemented)
```

**Key Point**: Show how USERS interact with and enjoy your world. It's not just "can they move around" but "is it pleasant and intuitive?"

---

### 9. Animation (Κίνηση)
**Weight**: Medium  
**What They Want**:
- Construction must contain representations of moving elements
- Creates impression of participating in dynamic, time-evolving world
- Examples: "inanimate" objects, objects moving on predetermined paths, animals/robots, etc.
- **Note**: Animation = ANY change in object's characteristic value over time
  - Not just movement/translation
  - Includes: color changes (traffic lights), dimension changes, etc.

**How to Address in Documentation**:
```
ANIMATED ELEMENTS IN OUR WORLD:

CHARACTER ANIMATIONS:
- Player: walk, run, jump, crouch, idle, weapon handling
- Zombies: walk, run, attack, death, idle variations
- NPCs: idle animations, greeting gestures

WEAPON ANIMATIONS:
- Shooting recoil
- Reload sequences
- Weapon switching transitions
- Procedural aim offsets via Animation Rigging

ENVIRONMENTAL ANIMATIONS:
- Pickups: bobbing and rotating continuously
- Doors: opening/closing animations
- Slot machines: spinning reels, flashing lights (if implemented)
- Neon signs: flickering, color cycling
- Traffic lights: color state changes (if implemented)
- Flags: cloth simulation waving in wind (if implemented)
- Particle systems: smoke, fire, sparks

PROPERTY ANIMATIONS (not just position):
- UI elements: fade in/out, scale on hover
- Damage indicators: color flash on hit
- Points UI: scale up on point gain
- Health bar: color change (green→yellow→red) based on health percentage
- Hitmarker: brief appearance and fade

DYNAMIC WORLD ELEMENTS:
- Zombie wave spawns at timed intervals
- Random pickup spawns from enemy deaths
- Weather particles (if implemented)
- Camera shake on explosions
```

**Key Point**: They want PROOF your world is alive. List EVERY animated element, including UI animations and property changes.

---

### 10. Functionality (Λειτουργικότητα)
**Weight**: High  
**What They Want**:
- Depending on representation, construction must contain functional components
- Components user can affect or that react to specific user actions
- Examples:
  - Door that opens/closes when user acts on a switch
  - Mechanism that starts/stops when user acts on lever
  - Robot or alarm activated when user approaches point or enters area
  - Radio whose volume user adjusts with control
  - TV device user activates/deactivates
  - Etc.

**How to Address in Documentation**:
```
INTERACTIVE ELEMENTS:

USER-ACTIVATED OBJECTS:
1. Locked Doors
   - Require specific keys to open
   - Visual feedback when attempting without key
   - Permanent state change when unlocked

2. NPC Interactions
   - Medic NPC: Heals player for points (player decision)
   - Shopkeeper NPC: Sells weapons for points (transaction system)
   - Tutorial NPC: Provides information on interaction

3. Pickups
   - Auto-collect on trigger: health, ammo, weapons
   - Visual and audio feedback on collection
   - Affects player inventory/stats immediately

4. Weapon System
   - User switches weapons (scroll wheel)
   - User aims (right-click zooms FOV)
   - User shoots (left-click)
   - User reloads (R key)
   - All actions have immediate functional response

5. Menu Interactions
   - Buttons with hover states
   - Sliders for settings (audio volume, graphics quality)
   - Toggles for gameplay options
   - All affect game state immediately

REACTIVE ELEMENTS:
1. Zombies
   - Detect player proximity (trigger zones)
   - Chase player when in range
   - Attack when close enough
   - Respond to being damaged

2. Health System
   - Reacts to damage from enemies
   - Updates UI in real-time
   - Triggers game over state when depleted

3. Wave System
   - Spawns zombies based on time/kills
   - Increases difficulty automatically
   - Responds to player performance

4. Point System
   - Awards points for kills
   - Updates UI with animations
   - Gates shop/heal functionality

CONDITIONAL FUNCTIONALITY:
- Doors only open if player has correct key
- NPCs only interact when player presses E while looking at them
- Shop only sells if player has sufficient points
- Healing only works if health below maximum
- Reloading only works if player has backpack ammo
```

**Key Point**: This is CRITICAL. List EVERY interactive thing. Show cause-and-effect relationships.

---

### 11. Development (Ανάπτυξη)
**Weight**: Very High  
**What They Want**:
- Can use: **Unity Engine**, **Godot Engine**, or **Unreal Engine**
- Functionality in: **C#**, **C++**, or **GDScript** (depending on platform)
- Code must be:
  - Well-designed
  - Incorporate best implementation techniques
  - Presentable
  - **MOST IMPORTANTLY: Comprehensively, analytically, and understandably commented**

**How to Address in Documentation**:
```
DEVELOPMENT ENVIRONMENT:
- Platform: Unity 6000.2.8f1
- Render Pipeline: Universal Render Pipeline (URP)
- Language: C#
- Version Control: Git (if used)
- IDE: Visual Studio 2022 / Rider (specify which)

CODE QUALITY STANDARDS:
✓ Every class has XML documentation comments
✓ Every public method has summary explaining purpose
✓ Complex algorithms have inline comments explaining logic
✓ Magic numbers replaced with named constants
✓ Descriptive variable and method names
✓ Consistent naming conventions (PascalCase for public, camelCase for private)
✓ Regions organize code sections
✓ No commented-out debug code in final version

BEST PRACTICES IMPLEMENTED:
- Component-based architecture
- Separation of concerns (each script has single responsibility)
- Interface-based programming for flexibility
- Event-driven communication to reduce coupling
- ScriptableObjects for data-driven design
- Object pooling for performance
- Caching of frequently accessed components
- Coroutines for time-based operations instead of Update loops where appropriate
- Layer-based collision detection for efficiency

EXAMPLE COMMENT QUALITY:
```csharp
/// <summary>
/// Handles weapon switching via mouse scroll wheel and manages active weapon state.
/// Διαχειρίζεται την εναλλαγή όπλων μέσω του scroll wheel και τη διαχείριση κατάστασης όπλων.
/// </summary>
public class WeaponController : MonoBehaviour
{
    /// <summary>
    /// Current equipped weapon index (0-based).
    /// Το τρέχον ευρετήριο εξοπλισμένου όπλου (0-based).
    /// </summary>
    private int currentWeaponIndex = 0;
    
    /// <summary>
    /// Switches to the next or previous weapon based on scroll direction.
    /// Αλλάζει στο επόμενο ή προηγούμενο όπλο με βάση την κατεύθυνση scroll.
    /// </summary>
    /// <param name="direction">1 for next, -1 for previous / 1 για επόμενο, -1 για προηγούμενο</param>
    private void SwitchWeapon(int direction)
    {
        // Disable current weapon rig before switching
        // Απενεργοποίηση του τρέχοντος weapon rig πριν την αλλαγή
        if (equippedWeapons[currentWeaponIndex] != null)
        {
            equippedWeapons[currentWeaponIndex].SetActive(false);
        }
        
        // Calculate new index with wraparound
        // Υπολογισμός νέου index με κυκλική επανάληψη
        currentWeaponIndex = (currentWeaponIndex + direction + equippedWeapons.Count) % equippedWeapons.Count;
        
        // Enable new weapon rig
        // Ενεργοποίηση νέου weapon rig
        equippedWeapons[currentWeaponIndex].SetActive(true);
        
        // Notify other systems of weapon change
        // Ειδοποίηση άλλων συστημάτων για την αλλαγή όπλου
        OnWeaponChanged?.Invoke(currentWeaponIndex);
    }
}
```
```

**Key Point**: Code comments are EXTREMELY important. Greek + English bilingual is ideal. Show examples of your commenting style.

### 1. Elements Taught in Class
- Incorporate elements taught practically in class with accessible code
- **ESPECIALLY VALUED**: Use of optimization techniques (as analyzed in class)

### 2. Realism (Αληθοφάνεια)
- Space must "convince" the user
- Should not create impression of violating physical laws
- Must provide intended experience without relying on design or aesthetic extremes

### 3. Content (Περιεχόμενο)
- Use content reminiscent of the physical, "real" world
- **MUST cite sources** for all content (within the virtual world using appropriate information display methods)

### 4. Completeness (Πληρότητα)
- Space will be complete as a VR application component:
  - Appears as 3D/2D world to user
  - Has functionality at various points depending on what it represents
  - Evolves over time through moving elements and functional elements
  - Has lighting and decorative elements
  - Provides complete, multi-modal, dynamic representation experience without unjustified gaps, discontinuities, or inconsistencies

### 5. Design (Σχεδιασμός)
- Design characterized by precision and detail
- **Note**: Increased design/structural complexity is NOT necessarily evaluated positively

### 6. Aesthetics (Αισθητική)
- Space must be presentable
- Not characterized by aesthetic extremes
- Should "invite" the user and grab their attention

### 7. Originality (Πρωτοτυπία)
- Structure, design, aesthetics, and functionality reflect extent of exploration of:
  - Platform's design capabilities
  - Language capabilities
  - Various design patterns and implementation practices

### 8. Usability (Χρηστικότητα)
- Space intended for human use and meeting real needs
- Example: ability to move and visit various points of the 3D/2D world

### 9. Animation (Κίνηση)
- Construction must contain representations of moving elements
- Creates impression of participating in dynamic, time-evolving world
- Examples: "inanimate" objects, objects moving on predetermined paths, animals/robots, etc.
- **Note**: Animation = ANY change in object's characteristic value over time
  - Not just movement/translation
  - Includes: color changes (traffic lights), dimension changes, etc.

### 10. Functionality (Λειτουργικότητα)
- Depending on representation, construction must contain functional components
- Components user can affect or that react to specific user actions
- Examples:
  - Door that opens/closes when user acts on a switch
  - Mechanism that starts/stops when user acts on lever
  - Robot or alarm activated when user approaches point or enters area
  - Radio whose volume user adjusts with control
  - TV device user activates/deactivates
  - Etc.

### 11. Development
- Can use any of 3 known game development platforms: **Unity Engine**, **Godot Engine**, or **Unreal Engine**
- Functionality implemented in one of: **C#**, **C++**, or **GDScript** (depending on platform)
- Code must be:
  - Well-designed
  - Incorporate best implementation techniques
  - Presentable
  - **MOST IMPORTANTLY: Comprehensively, analytically, and understandably commented**

---

## Implementation

### Team Structure
- Work **individually** or in **teams of up to 3 people**
- No need to declare teams to instructor
- Can import models from design programs or other sources
- Each team member participates to varying degrees in all phases and processes (analysis, general design, object design, behavior programming, testing, etc.)

### Important Note
**Even if the assignment is not fully completed, you can still get a passing grade based on individual implementations.**

---

## Submission and Evaluation

### Submission Location
Submit to: https://thales.cs.unipi.gr/modules/work/?course=TMD117

### If File Too Large
- Upload to dropbox-type service
- Write address in .txt file for instructor to download
- Include email and phone number in case download issues occur

---

## Required Deliverables

The assignment must consist of:

### 1. Project Files
- Platform project files (Unity, Godot, or Unreal)

### 2. Build Files
- Executable build files

### 3. PowerPoint Presentation
- **10-15 slides**

### 4. Video
- Short video of the application

### 5. Manual/Documentation (Εγχειρίδιο)
Must contain:

#### a. Introduction (Εισαγωγή)

#### b. Problem Description (Περιγραφή του Προβλήματος)

#### c. Detailed Presentation of All Development Phases
- **Analysis** (Ανάλυση)
- **Design** (Σχεδίαση)
- **Implementation** (Υλοποίηση)

#### d. Script Table
- Table with (C#, C++, or GDScript) scripts
- **Short description (4-5 lines)** of what each script does

#### e. Detailed Presentation of 4-5 Most Important Scripts
- In-depth analysis of your most significant scripts

#### f. Asset Sources
- Sources of all assets used

#### g. Detailed User Presentation
- Functionality, controls, etc.
- How users interact with the application

#### h. Screenshots
- Screenshots from the application

#### i. Criteria Response
- **Answer how each evaluation criterion is addressed**
- Explain how your project meets:
  - Elements taught
  - Realism
  - Content
  - Completeness
  - Design
  - Aesthetics
  - Originality
  - Usability
  - Animation
  - Functionality
  - Code quality

---

## Key Requirements Summary

### Code Comments
- Must be: "συνολικά, αναλυτικά και κατανοητά σχολιασμένος"
- Translation: "Comprehensively, analytically, and understandably commented"

### Optimization
- "θα εκτιμηθεί ιδιαίτερα η χρησιμοποίηση κάποιας τεχνικής βελτιστοποίησης"
- Translation: "Use of any optimization technique will be especially appreciated"

### Asset Attribution
- All content sources must be cited within the virtual world
- Use appropriate information display methods

### Completeness Over Complexity
- Better to have complete, polished, smaller scope
- Than incomplete, complex project
- "Even if assignment not fully completed, can still get passing grade based on individual implementations"

---

## Documentation Tips

1. **Be thorough** - Answer ALL sections
2. **Show your work** - Explain design decisions
3. **Cite everything** - Asset sources, code references
4. **Comment extensively** - Greek and/or English
5. **Screenshots matter** - Show different aspects
6. **Video walkthrough** - Demonstrate functionality
7. **Address criteria explicitly** - Don't make professor guess how you met requirements

---

## Common Mistakes to Avoid

❌ Missing deliverables (PowerPoint, video, full manual)  
❌ Uncommented code  
❌ No asset attribution  
❌ Not explaining how criteria are met  
❌ Build files don't work  
❌ Incomplete script descriptions  
❌ No screenshots showing key features
