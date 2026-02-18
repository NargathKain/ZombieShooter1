# Documentation Blueprint - Las Vegas Zombie Shooter

**Course**: Virtual Reality (Εικονική Πραγματικότητα)
**Academic Year**: 2025-2026
**University of Piraeus - Department of Informatics**

---

## DELIVERABLES OVERVIEW

| Deliverable | Format | Size/Length |
|-------------|--------|-------------|
| 1. Project Files | Unity Project Folder | Full project |
| 2. Build Files | Windows .exe + Data | ~2GB |
| 3. PowerPoint | .pptx | 10-15 slides |
| 4. Video | .mp4 (1080p) | 2-4 minutes |
| 5. Documentation | .pdf | 15-30 pages |

---

## DOCUMENTATION STRUCTURE (15-30 pages)

### SECTION A: Introduction (1-2 pages)

**Content to write:**
- Project title: "Las Vegas Apocalypse: Zombie Survival"
- Course context and academic year
- Team members with student IDs and roles
- Technology stack:
  - Unity 6000.2.8f1
  - Universal Render Pipeline (URP)
  - C# programming language
  - Synty AnimationBaseLocomotion package
- Project scope and vision
- Document structure overview

---

### SECTION B: Problem Description (2-3 pages)

**Content to write:**
- Assignment requirement: Tourist Destination (Option 2)
- Why Las Vegas was chosen:
  - Iconic, recognizable destination
  - Rich visual identity (neon, casinos)
  - Natural interactive elements
- Design challenges addressed:
  - Making exploration engaging (added zombie survival)
  - Balancing combat with environment appreciation
  - Performance optimization for complex scenes
- User experience goals:
  - Immersion in Vegas atmosphere
  - Challenging but fair gameplay
  - Clear progression and rewards

---

### SECTION C: Development Phases (4-6 pages)

#### C1: Analysis Phase
- Requirements from assignment criteria
- Research conducted (Vegas references, similar games)
- Feature prioritization:
  - **Must-have**: Player movement, combat, enemies, UI, NPCs
  - **Should-have**: Multiple weapons, inventory, economy, keys/doors
  - **Nice-to-have**: Win conditions, audio zones, victory screen

#### C2: Design Phase
- System architecture diagram showing:
  - Player systems (Health, Inventory, WeaponController)
  - Enemy systems (EnemyAI, EnemyHealth, WaveManager)
  - Interaction systems (Interactor, IInteractable, NPCs)
  - UI systems (AmmoUI, PointsUI, GameOverScreen)
  - Manager systems (PauseManager, WinConditionManager)
- Event-driven communication diagram
- Level layout sketch/map

#### C3: Implementation Phase
- Development timeline (week by week)
- Major challenges and solutions:
  - Weapon aiming accuracy (raycast from camera)
  - NPC ground sticking (raycast grounding)
  - Button click issues (onClick listeners in code)
  - Win condition flow (events + timer)
- Testing and iteration process
- Optimization techniques applied

---

### SECTION D: Script Table (3-4 pages)

**Format**: Table with Script Name, Category, Description (4-5 lines each)

**Categories to organize by:**

#### Player Systems
| Script | Description |
|--------|-------------|
| `WeaponController.cs` | Manages shooting, aiming (FOV zoom), weapon switching. Raycasts from camera to find aim target, spawns bullets from weapon muzzle. Handles ammo consumption and reload input. |
| `Inventory.cs` | Manages weapons, magazine/backpack ammo, and points. Fires events for UI updates. Provides methods for equipping weapons and spending points. |
| `PlayerHealth.cs` | Tracks health, handles damage/healing. Fires OnHealthChanged and OnPlayerDeath events for UI and game over state. |
| `PlayerKeys.cs` | Static class tracking collected keys. Used by VaultDoor and KeyCountUI. Fires OnKeyCollected event. |
| `Bullet.cs` | Projectile spawned when shooting. Moves via Rigidbody, applies damage to IDamageable targets, destroys on collision. |

#### Enemy Systems
| Script | Description |
|--------|-------------|
| `EnemyAI.cs` | Controls zombie behavior using NavMeshAgent. Detects player, chases, attacks in melee range. Updates Animator parameters. |
| `EnemyHealth.cs` | Manages zombie health, implements IDamageable. Fires OnEnemyDeath event for points and loot drops. |
| `EnemyData.cs` | ScriptableObject storing enemy stats (health, speed, damage, detection range). |

#### Interaction Systems
| Script | Description |
|--------|-------------|
| `Interactor.cs` | Attached to camera, raycasts to detect "Interactable" tagged objects. Calls IInteractable methods on look and E key press. |
| `IInteractable.cs` | Interface defining OnInteract, OnReadyInteract, OnAbortInteract, OnEndInteract methods. |
| `InteractorUI.cs` | Displays interaction prompts based on Interactor raycast results. |

#### NPC Systems
| Script | Description |
|--------|-------------|
| `NPCBase.cs` | Abstract base class implementing IInteractable. Handles player detection and prompts. |
| `MedicNPC.cs` | Heals player to full for 200 points. Shows status messages. |
| `ShopkeeperNPC.cs` | Sells weapons to player for points. Displays available weapons. |
| `TutorialNPC.cs` | Displays tutorial information panels to teach mechanics. |
| `SimplePatrolNPC.cs` | Patrols between waypoints without NavMesh. Includes ground sticking via raycast. |

#### Manager Systems
| Script | Description |
|--------|-------------|
| `PauseManager.cs` | Handles pause state via Escape key. Shows pause panel, freezes time, manages cursor. |
| `WinConditionManager.cs` | Tracks kills and keys. Starts 90-second escape timer when conditions met. Triggers victory or game over. |
| `WaveManager.cs` | Spawns enemy waves with configurable timing and counts. |
| `GameInitializer.cs` | Resets static game state on scene load. |

#### UI Systems
| Script | Description |
|--------|-------------|
| `AmmoUI.cs` | Displays magazine/backpack ammo counts. Subscribes to Inventory events. |
| `PointsUI.cs` | Shows player points with animated gain feedback. |
| `PlayerHealthUI.cs` | Health bar that updates on PlayerHealth.OnHealthChanged. |
| `GameOverScreen.cs` | Shows on player death or timer expiry. Restart, Main Menu, Exit buttons. |
| `VictoryScreen.cs` | Shows when player escapes through vault door. Main Menu, Exit buttons. |
| `KillCountUI.cs` | Displays X/25 kill progress for win conditions. |
| `KeyCountUI.cs` | Displays X/3 key progress for win conditions. |
| `EscapeTimerUI.cs` | Shows 90-second countdown when conditions are met. |
| `ButtonSound.cs` | Plays click sound when button is pressed. |

#### Pickup Systems
| Script | Description |
|--------|-------------|
| `AmmoPickupData.cs` | ScriptableObject defining ammo pickup behavior and amounts. |
| `HealthPickupData.cs` | ScriptableObject defining health pickup behavior and heal amount. |
| `WeaponPickupData.cs` | ScriptableObject defining weapon pickup behavior. |
| `KeyPickup.cs` | Adds key to PlayerKeys when collected. |

#### Audio Systems
| Script | Description |
|--------|-------------|
| `AudioZone.cs` | Trigger-based ambient audio that fades in/out when player enters/exits. |
| `ProximityAudio.cs` | Plays audio when player approaches (used for key proximity music). |

#### Other
| Script | Description |
|--------|-------------|
| `VaultDoor.cs` | Escape door that opens when kills + keys conditions are met. Triggers victory. |
| `MainMenuController.cs` | Main menu scene navigation (New Game, Settings, Exit). |
| `WeaponData.cs` | ScriptableObject for weapon stats (damage, fire rate, ammo type, capacity). |

---

### SECTION E: Detailed Script Analysis (4-6 pages)

**Analyze 4-5 of the most important scripts in depth:**

#### 1. WeaponController.cs
- Purpose: Central combat system managing all weapon actions
- Architecture: Intermediary between Input, Inventory, and Bullets
- Key methods: HandleShooting(), SwitchWeapon(), HandleAiming(), Reload()
- Code example with bilingual comments (Greek + English)
- Challenges: Aiming accuracy solved with dual raycast approach

#### 2. EnemyAI.cs
- Purpose: Zombie behavior and pathfinding
- Architecture: Uses NavMeshAgent for movement, state-based behavior
- Key methods: DetectPlayer(), ChasePlayer(), AttackPlayer()
- Code example with comments
- Challenges: Performance with many zombies

#### 3. WinConditionManager.cs
- Purpose: Tracks win conditions and escape timer
- Architecture: Singleton with static events
- Key methods: HandleEnemyDeath(), HandleKeyCollected(), StartEscapeTimer()
- Code example with comments
- Challenges: Event flow coordination

#### 4. Interactor.cs
- Purpose: Enables player to interact with world objects
- Architecture: Raycast-based detection with interface callbacks
- Key methods: PerformRaycast(), TriggerInteraction()
- Code example with comments
- Challenges: Multiple interaction types through single system

#### 5. Inventory.cs
- Purpose: Manages all player resources (weapons, ammo, points)
- Architecture: Dictionary-based storage with events
- Key methods: AddWeapon(), SpendPoints(), GetAmmo()
- Code example with comments
- Challenges: Dual ammo system (magazine + backpack)

---

### SECTION F: Asset Sources (1-2 pages)

**Format**: Categorized list with proper attribution

#### 3D Models & Environments
- Synty Studios POLYGON packs (Unity Asset Store)
- Specific pack names and URLs

#### Character Controller & Animations
- Synty AnimationBaseLocomotion package

#### Audio
- List all audio asset sources
- Sound effects and music attribution

#### Fonts
- TextMeshPro defaults or custom fonts used

#### Custom Created
- All C# scripts (written by team)
- Level design and composition
- ScriptableObject configurations

---

### SECTION G: User Manual (2-3 pages)

#### System Requirements
- Minimum: Windows 10, i5, 8GB RAM, GTX 1050
- Recommended: Windows 10/11, i7, 16GB RAM, GTX 1660

#### Installation
1. Extract ZIP to desired location
2. Run LasVegasApocalypse.exe

#### Controls
| Action | Key |
|--------|-----|
| Move | W/A/S/D |
| Look | Mouse |
| Sprint | Shift |
| Jump | Space |
| Shoot | Left Mouse |
| Aim | Right Mouse |
| Reload | R |
| Switch Weapon | Scroll Wheel |
| Interact | E |
| Pause | ESC |

#### How to Play
- Objective: Kill 25 zombies + collect 3 keys, then escape through vault door within 90 seconds
- NPCs: Medic (heal for 200 points), Shopkeeper (buy weapons), Tutorial (learn controls)
- Tips and strategies

#### Settings
- Graphics options
- Audio options
- Gameplay options

#### Troubleshooting
- Common issues and solutions

---

### SECTION H: Screenshots (3-5 pages)

**Minimum 15-20 screenshots organized by category:**

#### Environment (4-5 screenshots)
- Vegas strip overview
- Casino exteriors
- Interior spaces
- Atmospheric/lighting showcase

#### Gameplay (4-5 screenshots)
- Combat with zombies
- Weapon switching
- Reload action
- Hitmarker feedback

#### Interactions (3-4 screenshots)
- NPC dialogue (Medic, Shopkeeper)
- Door/key system
- Pickup collection

#### UI (4-5 screenshots)
- Main menu
- In-game HUD
- Pause menu
- Game over screen
- Victory screen

---

### SECTION I: Evaluation Criteria Response (4-6 pages)

**Address each of the 11 criteria with specific examples:**

#### 1. Elements Taught in Class
- NavMesh AI for pathfinding
- Event-driven architecture
- ScriptableObjects for data
- Optimization: Object pooling, occlusion culling, LOD

#### 2. Realism
- Physical laws respected (gravity, collisions)
- Authentic Vegas architecture
- Believable NPC behaviors

#### 3. Content
- Real Las Vegas inspiration
- All assets properly cited
- In-game credits

#### 4. Completeness
- Full 3D navigable environment
- Complete game loop (start, play, end)
- Lighting, decorations, UI all present

#### 5. Design
- Clean architecture with single responsibility
- Design patterns: Singleton, Observer, State, Strategy
- Organized project structure

#### 6. Aesthetics
- Cohesive low-poly visual style
- Vegas neon atmosphere
- Clean, readable UI

#### 7. Originality
- Unity features explored (URP, Animation Rigging, New Input System)
- C# capabilities used (events, interfaces, coroutines)
- Unique dual ammo system

#### 8. Usability
- Industry-standard FPS controls
- Full environment exploration
- Tutorial NPC for onboarding

#### 9. Animation
- Character animations (walk, run, attack, death)
- Environmental animations (pickups bob, doors swing)
- UI animations (point gains, damage flashes)

#### 10. Functionality
- User-activated: Doors, keys, NPCs, weapons, pickups
- Reactive: Zombies chase, health updates, waves spawn
- Conditional: Door opens only with key, heal only with points

#### 11. Development
- Unity 6000.2.8f1 + URP + C#
- Best practices: component-based, events, interfaces
- Well-commented code with XML documentation

---

## POWERPOINT STRUCTURE (10-15 slides)

1. **Title Slide** - Project name, course, team
2. **Project Overview** - What, why, platform
3. **Topic Justification** - Why Las Vegas, why zombies
4. **Key Features 1** - Environment, NPCs, combat
5. **Key Features 2** - Inventory, economy, win conditions
6. **Technical - Systems** - Player, enemy, interaction
7. **Technical - Architecture** - Events, ScriptableObjects, interfaces
8. **Technical - Optimization** - Pooling, culling, LOD
9. **Screenshots - Environment** - 3-4 images
10. **Screenshots - Gameplay** - 3-4 images
11. **Screenshots - UI** - 3-4 images
12. **Development Process** - Timeline, challenges
13. **Team Contributions** - Who did what
14. **Challenges & Solutions** - Major problems solved
15. **Conclusion** - Achievements, future improvements

---

## VIDEO STRUCTURE (2-4 minutes)

| Time | Content |
|------|---------|
| 0:00-0:15 | Title screen, team members |
| 0:15-0:45 | Main menu navigation, settings |
| 0:45-1:30 | Environment tour (Vegas strip, casinos) |
| 1:30-2:15 | Gameplay (combat, NPCs, pickups) |
| 2:15-2:45 | UI showcase, win condition progress |
| 2:45-3:00 | Victory/ending, credits |

**Technical**: 1080p, 30/60 FPS, MP4 format, clear audio

---

## BUILD CHECKLIST

- [ ] File > Build Settings > Windows 64-bit
- [ ] Development Build: OFF
- [ ] Include all scenes in build
- [ ] Test .exe runs without Unity
- [ ] Create README.txt with system requirements
- [ ] Package in ZIP file

---

## SUBMISSION CHECKLIST

- [ ] Project Files (Unity folder, exclude Temp/)
- [ ] Build Files (Windows .exe + Data folder)
- [ ] PowerPoint (10-15 slides, .pptx)
- [ ] Video (2-4 min, 1080p, .mp4)
- [ ] Documentation (15-30 pages, .pdf)
- [ ] Team member contributions clearly stated
- [ ] All asset sources credited
- [ ] Contact info included

---

## NOTES

- Upload to: https://thales.cs.unipi.gr/modules/work/?course=TMD117
- If too large: Use Dropbox/Google Drive, submit link in .txt file
- Include email and phone for download issues
- Clearly state who did what in team projects
