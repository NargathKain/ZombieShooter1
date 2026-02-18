# AGENTS.md - Unity Zombie Shooter Project

This document provides essential information for AI agents working on this Unity zombie shooter project.

---

## Project Overview

**Type:** 3rd person zombie wave shooter game
**Engine:** Unity 6000.2.8f1 with URP (Universal Render Pipeline)
**Assets:**
- SYNTY Animation Base Locomotion (player movement, camera)
- SimpleZombies (enemy models)
- SYNTY environment packs

**Assignment Requirements:**
- Events, NavMesh, Animation Rigging must be used
- Object pooling required for optimization
- Proper physics and lighting
- Animation for enemies and weapons
- Interactive objects and pickups
- Heavily commented code (critical for grading)
- Good architecture (ScriptableObjects, Managers, Events)
- Deliverables: Unity project, build files, PowerPoint, video demonstration, manual

---

## Critical: Input System

### MUST USE: New Input System Package
- **NEVER** use `Input.GetKey()`, `Input.GetMouseButton()`, or `UnityEngine.Input` class
- This causes "InvalidOperationException: You are trying to read Input using UnityEngine.Input class, but you have switched active Input handling to Input System package"

### Input Reader Location
**File:** `Assets/Synty/AnimationBaseLocomotion/Samples/Scripts/InputSystem/InputReader.cs`
**Namespace:** `Synty.AnimationBaseLocomotion.Samples.InputSystem`

### Using Input in Your Scripts
```csharp
using Synty.AnimationBaseLocomotion.Samples.InputSystem;

public class YourScript : MonoBehaviour
{
    private InputReader inputReader;

    void Start()
    {
        inputReader = GetComponent<InputReader>();

        // Subscribe to events (REQUIRED pattern)
        inputReader.onShootPerformed += Fire;
        inputReader.onReloadPerformed += Reload;
        inputReader.onInteractPerformed += Interact;
        inputReader.onPausePerformed += TogglePause;
        inputReader.onWeaponScrollPerformed += SwitchWeapon;
    }

    void OnDestroy()
    {
        // CRITICAL: Always unsubscribe to prevent memory leaks
        if (inputReader != null)
        {
            inputReader.onShootPerformed -= Fire;
            inputReader.onReloadPerformed -= Reload;
            inputReader.onInteractPerformed -= Interact;
            inputReader.onPausePerformed -= TogglePause;
            inputReader.onWeaponScrollPerformed -= SwitchWeapon;
        }
    }

    void Fire() { /* shooting logic */ }
    void Reload() { /* reload logic */ }
    void Interact() { /* interact logic */ }
    void TogglePause() { /* pause logic */ }
    void SwitchWeapon(float delta) { /* weapon switching */ }
}
```

### Available Input Actions
From InputReader.cs (extended SYNTY version):
- `onShootPerformed` - Left Mouse Button (fires on click)
- `onReloadPerformed` - R key
- `onInteractPerformed` - E key
- `onPausePerformed` - Escape key
- `onWeaponScrollPerformed` - Mouse Scroll Y (passes float delta)
- SYNTY's original actions (Move, Look, Jump, Sprint, Crouch, Aim, etc.)

### Checking Button Hold State
```csharp
bool isShooting = false;

void Start()
{
    inputReader.onShootStarted += () => isShooting = true;
    inputReader.onShootCanceled += () => isShooting = false;
}

void Update()
{
    if (isShooting)
    {
        // Button is being held down
        // Good for full-auto weapons
    }
}
```

---

## Project Structure

```
Assets/
├── Synty/                        (Third-party asset - DO NOT MODIFY except InputReader)
│   └── AnimationBaseLocomotion/
│       └── Samples/
│           └── Scripts/
│               └── InputSystem/
│                   ├── Controls.inputactions      (Input actions asset)
│                   ├── Controls.cs             (Auto-generated from .inputactions)
│                   └── InputReader.cs         (EXTENDED - added gameplay inputs)
│
└── _Project/                      (Your code goes here)
    ├── Scripts/
    │   ├── Data/                     (Foundation: enums, interfaces)
    │   │   ├── AmmoType.cs            (enum: Rifle, Pistol, Shotgun, etc.)
    │   │   ├── IPickupable.cs        (interface for pickups)
    │   │   └── IDamageable.cs        (interface for damageable objects)
    │   │
    │   ├── Player/                   (Player systems)
    │   │   ├── PlayerHealth.cs        (manages player HP, death, respawn)
    │   │   ├── WeaponController.cs     (shooting system - multiple versions exist)
    │   │   ├── Bullet.cs             (projectile movement)
    │   │   └── InputTester.cs        (debug script - KEEP THIS)
    │   │
    │   ├── Enemies/                  (Enemy systems)
    │   │   ├── EnemyAI.cs            (NavMesh chasing + attacking)
    │   │   ├── EnemyData.cs          (ScriptableObject: enemy stats)
    │   │   └── EnemyHealth.cs        (enemy HP, death, events)
    │   │
    │   ├── Damage/                   (Damage system components)
    │   │   ├── EnemyHealthbar.cs      (World Space UI above enemies)
    │   │   ├── PlayerHealthUI.cs      (Screen Space UI for player)
    │   │   └── Hitmarkerdisplay.cs   (visual/audio feedback on hit)
    │   │
    │   ├── Interaction/              (Interaction system from course)
    │   │   ├── IInteractable.cs       (complex interaction interface)
    │   │   ├── Interactor.cs         (raycast + interaction logic)
    │   │   ├── InteractorUI.cs       (UI for interactions)
    │   │   └── TextSign.cs          (example interactable)
    │   │
    │   └── ScriptableObjects/        (Data assets)
    │       ├── Enemies/              (EnemyData ScriptableObjects)
    │       │   └── BasicZombie.asset
    │       └── Weapons/              (WeaponData ScriptableObjects)
    │
    ├── Prefabs/                    (Reusable game objects)
    │   ├── Weapons/                 (weapon prefabs)
    │   ├── Enemies/                 (enemy prefabs)
    │   └── Pickups/                (pickup prefabs)
    │
    ├── Scenes/                     (Unity scenes)
    │   └── MainGame.unity           (Main gameplay scene)
    │
    └── UI/                        (UI prefabs)
        └── PlayerUI/               (Player UI canvas)
```

---

## Build Order (CRITICAL)

### NEVER Skip This Order
Previous attempts created 20 scripts at once → everything broke due to circular dependencies.

### Dependency Layers (Bottom-Up)

**Layer 0: Foundation (No dependencies)**
- AmmoType.cs (enum)
- IPickupable.cs (interface)
- IDamageable.cs (interface)
- IInteractable.cs (interface)

**Layer 1: Data Templates (Depend on Layer 0 only)**
- WeaponData.cs (ScriptableObject)
- EnemyData.cs (ScriptableObject)
- PickupData.cs (ScriptableObject)

**Layer 2: Input System**
- Already implemented (InputReader.cs extended)

**Layer 3: Core Systems (Depend on Layers 0-2)**
- PlayerHealth.cs
- EnemyHealth.cs

**Layer 4: UI Systems (Depend on Layer 3)**
- PlayerHealthUI.cs
- EnemyHealthbar.cs
- Hitmarkerdisplay.cs

**Layer 5: Combat Systems (Depend on Layers 0-4)**
- WeaponController.cs
- EnemyAI.cs

**Layer 6: Advanced Systems (Depend on everything)**
- Inventory.cs
- PickupItem.cs
- WaveManager.cs

### Golden Rule
> Never create a script that references another script that doesn't exist yet.

Always check:
1. What other scripts does this need?
2. Do those scripts exist yet?
3. Are those scripts working and tested?

If NO to 2 or 3 → don't create this script yet.

---

## Code Patterns & Conventions

### 1. Event-Driven Architecture (Observer Pattern)

**Broadcasting Events (Publisher):**
```csharp
public class PlayerHealth : MonoBehaviour
{
    // Static events (all player health changes)
    public static event Action<float, float> OnHealthChanged; // current, max
    public static event Action OnPlayerDeath;

    void TakeDamage(float damage)
    {
        currentHealth -= damage;

        // Broadcast to all listeners
        OnHealthChanged?.Invoke(currentHealth, maxHealth);

        if (currentHealth <= 0)
        {
            OnPlayerDeath?.Invoke();
        }
    }
}
```

**Listening to Events (Subscriber):**
```csharp
public class PlayerHealthUI : MonoBehaviour
{
    void OnEnable()
    {
        // Subscribe when component enabled
        PlayerHealth.OnHealthChanged += UpdateHealthBar;
        PlayerHealth.OnPlayerDeath += OnPlayerDeath;
    }

    void OnDisable()
    {
        // CRITICAL: Unsubscribe when disabled
        PlayerHealth.OnHealthChanged -= UpdateHealthBar;
        PlayerHealth.OnPlayerDeath -= OnPlayerDeath;
    }

    void UpdateHealthBar(float current, float max)
    {
        // Update UI
    }
}
```

**Benefits:**
- Loose coupling (UI doesn't need reference to PlayerHealth)
- Easy to add more listeners without modifying health scripts
- Clean separation of concerns

### 2. Interface-Based Polymorphism

**IDamageable Interface:**
```csharp
public interface IDamageable
{
    void TakeDamage(float damage);
}
```

**Implemented by PlayerHealth:**
```csharp
public class PlayerHealth : MonoBehaviour, IDamageable
{
    public void TakeDamage(float damage)
    {
        // Player-specific damage handling
    }
}
```

**Implemented by EnemyHealth:**
```csharp
public class EnemyHealth : MonoBehaviour, IDamageable
{
    public void TakeDamage(float damage)
    {
        // Enemy-specific damage handling
    }
}
```

**Used by WeaponController:**
```csharp
// Weapon doesn't care WHAT it hits, just that it can be damaged
IDamageable target = hit.collider.GetComponent<IDamageable>();
if (target != null)
{
    target.TakeDamage(damage); // Polymorphic call - works for player OR enemy
}
```

### 3. Data-Driven Design (ScriptableObjects)

**EnemyData ScriptableObject:**
```csharp
[CreateAssetMenu(fileName = "New Enemy", menuName = "Game/Enemy Data")]
public class EnemyData : ScriptableObject
{
    public string enemyName;
    public float maxHealth;
    public float attackDamage;
    public float moveSpeed;
    public AudioClip attackSound;
    // ... etc
}
```

**Creating Asset:**
1. Right-click in Project window
2. Create → Game → Enemy Data
3. Name it (e.g., "BasicZombie")
4. Configure values in Inspector
5. Assign to EnemyHealth component

**Used by EnemyHealth:**
```csharp
public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private EnemyData enemyData;

    void Start()
    {
        currentHealth = enemyData.maxHealth;
        // ... configure other stats from enemyData
    }
}
```

**Benefits:**
- Designers create enemies without coding
- Easy balancing (just change numbers)
- Data separated from logic
- Reusable across multiple prefabs

### 4. Interaction System (Two Types)

**Simple Instant Pickup (IPickupable):**
```csharp
public interface IPickupable
{
    void Collect(GameObject player);
}

public class AmmoPickup : MonoBehaviour, IPickupable
{
    public void Collect(GameObject player)
    {
        // Instant collection - no interaction delay
        // Add ammo to player inventory
        Destroy(gameObject);
    }
}
```

**Complex Interaction (IInteractable from course):**
```csharp
public interface IInteractable
{
    void OnInteract(Interactor interactor);    // Start interaction
    void OnEndInteract();                    // End interaction
    void OnReadyInteract();                  // Player looking at object
    void OnAbortInteract();                  // Player stopped looking
}

public class TextSign : MonoBehaviour, IInteractable
{
    public void OnInteract(Interactor interactor)
    {
        // Show text, start reading, etc.
        interactor.ReceiveInteract("Hello World!");
    }

    public void OnReadyInteract()
    {
        // Show "Press E to read" hint
    }

    public void OnEndInteract()
    {
        // Hide text, stop reading
    }

    public void OnAbortInteract()
    {
        // Hide "Press E to read" hint
    }
}
```

**Interactor.cs (from course) handles raycasting:**
- Casts ray from camera
- Finds IInteractable component
- Calls appropriate interface methods based on state
- Shows/hides interaction hints
- Uses OLD Input System (`Input.GetKeyDown()`) - needs fixing

**Fixing Interactor for New Input System:**
```csharp
// REPLACE THIS:
if (Input.GetKeyDown(interactKey))
{
    Interact();
}

// WITH THIS:
private InputReader inputReader;

void Start()
{
    inputReader = GetComponent<InputReader>();
    inputReader.onInteractPerformed += Interact;
}

void OnDestroy()
{
    if (inputReader != null)
    {
        inputReader.onInteractPerformed -= Interact;
    }
}
```

---

## Important Patterns & Gotchas

### 1. Component Lifecycle Management

**Enemy Death Sequence Example:**
```csharp
private void Die()
{
    IsDead = true;

    // Disable AI and movement (stop chasing)
    if (enemyAI != null) enemyAI.enabled = false;
    if (navMeshAgent != null) navMeshAgent.isStopped = true;

    // Disable collider (player walks through corpse)
    if (collider != null) collider.enabled = false;

    // Start death animation (rotate -90°)
    StartCoroutine(FallOver());

    // Play effects
    if (enemyData.deathSound != null) AudioSource.PlayClipAtPoint(...);
    if (enemyData.deathEffect != null) Instantiate(...);

    // Destroy after delay
    Destroy(gameObject, 2f);
}
```

**Why This Order Matters:**
- AI must stop before animation starts
- Collider must disable before player can walk through
- Renderer stays active to show death animation
- Destroy delayed to allow animation to complete

### 2. NavMesh Enemy AI

**Setup Requirements:**
1. Scene must have NavMesh baked (Window → AI → Navigation → Bake)
2. Enemy must have NavMeshAgent component
3. Terrain/environment must be marked "Navigation Static"
4. Player GameObject must be tagged "Player"

**EnemyAI.cs Pattern:**
```csharp
[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(EnemyHealth))]
public class EnemyAI : MonoBehaviour
{
    private NavMeshAgent agent;
    private Transform player;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player").transform;

        // Configure agent from ScriptableObject
        agent.speed = enemyData.moveSpeed;
        agent.stoppingDistance = enemyData.stoppingDistance;
    }

    void Update()
    {
        if (health.IsDead)
        {
            agent.isStopped = true;
            return;
        }

        // Chase player
        agent.SetDestination(player.position);

        // Attack if in range
        if (Vector3.Distance(transform.position, player.position) <= enemyData.attackRange)
        {
            AttackPlayer();
        }
    }
}
```

**Debug Gizmos:**
```csharp
private void OnDrawGizmos()
{
    // Detection range (yellow)
    Gizmos.color = Color.yellow;
    Gizmos.DrawWireSphere(transform.position, enemyData.detectionRange);

    // Attack range (red)
    Gizmos.color = Color.red;
    Gizmos.DrawWireSphere(transform.position, enemyData.attackRange);
}
```

### 3. UI Implementation Patterns

**Player UI (Screen Space Overlay):**
```
PlayerUI (Canvas - Screen Space Overlay)
└── Healthbar (GameObject)
    ├── Slider component
    │   ├── Min Value: 0
    │   ├── Max Value: 100
    │   └── Fill Rect: Fill image
    ├── Border (Image - outline sprite)
    └── Fill (Image - white)
```

**Enemy UI (World Space):**
```
Enemy Prefab
└── HealthBarCanvas (Canvas - World Space)
    ├── Render Mode: World Space
    ├── Scale: (0.01, 0.01, 0.01)
    └── HealthBar (GameObject)
        └── Fill (Image - white, NO border)
```

**Billboarding Enemy Health Bar:**
```csharp
void LateUpdate()
{
    // Always face camera
    transform.LookAt(Camera.main.transform);

    // Maintain position offset above enemy
    transform.position = enemy.position + offset;
}
```

### 4. Weapon Shooting Patterns

**Raycast Shooting (Instant Hit):**
```csharp
void Shoot()
{
    Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));

    if (Physics.Raycast(ray, out RaycastHit hit, weaponData.range, hitLayers))
    {
        // Visual feedback
        Debug.DrawRay(ray.origin, ray.direction * weaponData.range, Color.yellow, 1f);

        // Damage target
        IDamageable target = hit.collider.GetComponent<IDamageable>();
        if (target != null)
        {
            target.TakeDamage(weaponData.damage);
        }

        // Spawn bullet hole
        if (weaponData.bulletHolePrefab != null)
        {
            GameObject hole = Instantiate(weaponData.bulletHolePrefab,
                                     hit.point,
                                     Quaternion.LookRotation(hit.normal));
            Destroy(hole, 10f);
        }
    }

    // Muzzle flash
    if (weaponData.muzzleFlashPrefab != null)
    {
        GameObject flash = Instantiate(weaponData.muzzleFlashPrefab,
                                       shootPoint.position,
                                       shootPoint.rotation);
        Destroy(flash, 0.1f);
    }

    // Sound
    if (weaponData.shootSound != null)
    {
        AudioSource.PlayClipAtPoint(weaponData.shootSound, shootPoint.position);
    }
}
```

**Projectile Shooting (Physical Bullets):**
```csharp
void Shoot()
{
    GameObject bullet = Instantiate(bulletPrefab, shootPoint.position, shootPoint.rotation);
    Bullet bulletScript = bullet.GetComponent<Bullet>();

    // Configure bullet (if needed)
    bulletScript.SetDamage(weaponData.damage);
}

// Bullet.cs
public class Bullet : MonoBehaviour
{
    void Update()
    {
        transform.position += transform.forward * speed * Time.deltaTime;
    }

    void OnTriggerEnter(Collider other)
    {
        IDamageable target = other.GetComponent<IDamageable>();
        if (target != null)
        {
            target.TakeDamage(damage);
            Destroy(gameObject);
        }
    }
}
```

---

## Essential Commands

### Unity Editor
No external build commands. Development happens entirely within Unity Editor.

### Testing in Unity
1. **Enter Play Mode:** Top toolbar → Play button
2. **Stop Play Mode:** Play button or Ctrl+Shift+P
3. **Console Window:** Window → General → Console (CRITICAL for debugging)
4. **Bake NavMesh:** Window → AI → Navigation → Bake button
5. **Generate C# Class from Input Actions:** Select `.inputactions` asset → Inspector → Check "Generate C# Class"

### Scene Setup
1. **Create Scene:** File → New Scene → Save as "MainGame" in `_Project/Scenes/`
2. **Add SYNTY Character:** Drag from Project window to Hierarchy
3. **Position Player:** Set to (0, 0, 0)
4. **Tag Player:** Inspector → Tag dropdown → "Player"
5. **Add Lighting:** GameObject → Light → Directional Light
6. **Save Scene:** Ctrl+S

### Baking NavMesh
1. Select terrain/ground objects
2. Inspector → Static dropdown → Navigation Static
3. Window → AI → Navigation
4. Bake tab → click "Bake" button
5. Wait for bake (green areas = walkable)

---

## Assignment-Specific Requirements

### 1. Heavily Commented Code
Every method, class, and complex logic MUST have XML comments:
```csharp
/// <summary>
/// Manages enemy health and death behavior.
/// Uses EnemyData ScriptableObject for stats.
/// Broadcasts events for UI and other systems.
/// </summary>
public class EnemyHealth : MonoBehaviour
{
    /// <summary>
    /// Reduces enemy health by damage amount.
    /// Triggers death animation and events when health reaches 0.
    /// </summary>
    /// <param name="damage">Amount of damage to apply</param>
    public void TakeDamage(float damage)
    {
        // Implementation
    }
}
```

### 2. Required Systems (From Course)
- ✅ Events (Observer Pattern) - PlayerHealth, EnemyHealth use events
- ✅ NavMesh - EnemyAI.cs uses NavMeshAgent
- ✅ Animation Rigging - For advanced character animations (not yet implemented)
- ✅ Object Pooling - Required for optimization (not yet implemented)
- ✅ Interactive Objects - IInteractable + Interactor system
- ✅ Pickups - IPickupable interface

### 3. Design Patterns Used
- **Singleton Pattern:** HitmarkerDisplay.Instance
- **Observer Pattern:** Events (PlayerHealth.OnHealthChanged, etc.)
- **Strategy Pattern:** PickupData polymorphism (if implemented)
- **Factory Pattern:** Could be used for enemy spawning
- **Object Pool Pattern:** Required for bullets/particles (not yet implemented)

---

## Common Errors & Solutions

### Error: "InvalidOperationException: You are trying to read Input using UnityEngine.Input"
**Cause:** Using `Input.GetKey()`, `Input.GetMouseButton()`, etc.
**Solution:** Use InputReader events instead (see "Critical: Input System" section above)

### Error: "NullReferenceException: InputReader.Instance"
**Cause:** Trying to access InputReader.Instance but SYNTY's version doesn't use Singleton
**Solution:** Use `GetComponent<InputReader>()` instead
```csharp
// WRONG:
InputReader inputReader = InputReader.Instance;

// RIGHT:
InputReader inputReader = GetComponent<InputReader>();
```

### Error: "Type 'Controls.IPlayerActions' does not contain definition for 'OnShoot'"
**Cause:** Controls.cs auto-generated code is out of sync with .inputactions file
**Solution:**
1. Select `Controls.inputactions` asset in Project window
2. In Inspector, check "Generate C# Class"
3. Click Apply
4. Unity regenerates Controls.cs

### Error: Scripts won't compile / "All compiler errors have to be fixed"
**Cause:** Missing dependencies (referencing scripts that don't exist)
**Solution:**
1. Read Console error messages
2. Identify which script is missing
3. Create that script FIRST (follow Build Order)
4. Then come back to original script

### Error: NavMeshAgent not moving
**Cause:** NavMesh not baked or not on correct layer
**Solution:**
1. Window → AI → Navigation → Bake
2. Make sure ground is marked "Navigation Static"
3. Make sure NavMeshAgent is enabled

### Error: Player walks through enemy corpse
**This is INTENDED BEHAVIOR** (required for gameplay)
**Implementation:**
```csharp
private void Die()
{
    collider.enabled = false; // Disable collider so player can walk through
    // ... death animation
    Destroy(gameObject, 2f);
}
```

### Error: Event listeners causing multiple calls
**Cause:** Subscribing multiple times without unsubscribing
**Solution:**
```csharp
void OnEnable()
{
    // Subscribe
    PlayerHealth.OnHealthChanged += UpdateHealthBar;
}

void OnDisable()
{
    // CRITICAL: Unsubscribe
    PlayerHealth.OnHealthChanged -= UpdateHealthBar;
}
```

---

## Project Status (As of Current State)

### ✅ Completed Systems:
- Input System (extended SYNTY's InputReader with 5 new actions)
- InputTester.cs (debug script - KEEP THIS)
- IDamageable interface
- IPickupable interface
- IInteractable interface (from course)
- PlayerHealth system with events
- EnemyHealth system with events and death animation
- EnemyAI with NavMesh pathfinding
- PlayerHealthUI (Screen Space)
- EnemyHealthbar (World Space, billboard)
- HitmarkerDisplay (visual/audio feedback)
- Bullet.cs (simple projectile)
- AmmoType enum
- EnemyData ScriptableObject with BasicZombie asset
- Interactor.cs (from course - uses old Input, needs fixing)
- Multiple WeaponController versions (testing different approaches)

### ⏳ Partially Implemented:
- Weapon system (multiple versions exist, need to pick and complete one)
- Pickup system (IPickupable exists, PickupItem.cs exists but needs work)

### ❌ Not Yet Implemented:
- Inventory system
- Wave spawning system
- Object pooling (REQUIRED for assignment)
- Animation Rigging (REQUIRED for assignment)
- Game over screen
- Main menu
- Score system

---

## Key Files to Reference

### Build Order Documentation:
- `BUILD_ORDER_GUIDE.md` - Complete 7-day roadmap with all code
- `QUICK_START_TODAY.md` - Day 1 checklist (Phases 0-4)
- `DEPENDENCY_DIAGRAM.md` - Visual explanation of dependency order
- `CONVERSATION_SUMMARY.md` - Full project context and architecture decisions
- `CONVERSATION_SUMMARY_INPUT_SYSTEM.md` - Input system implementation details
- `HEALTH_DAMAGE_SUMMARY.md` - Complete health/damage system documentation

### Existing Script Documentation:
All scripts include XML comments explaining:
- Purpose
- Setup requirements
- Dependencies
- Public methods with parameter descriptions

---

## Development Workflow

### Before Creating Any Script:
1. Check BUILD_ORDER_GUIDE.md - is this the right time in the build order?
2. Check DEPENDENCY_DIAGRAM.md - do all dependencies exist?
3. Read similar existing scripts to match patterns
4. Follow the code conventions in this document

### After Creating a Script:
1. Add XML comments to every public method and class
2. Test immediately in Play Mode
3. Check Console for errors
4. Verify it works with other systems
5. Update build order/summary docs if significant changes

### Testing Philosophy:
- **Test each layer** before moving to next
- **Don't build 20 scripts** then try to fix all at once
- **Debug.Log everything** temporarily, remove later
- **Use InputTester.cs** to verify input works before building gameplay
- **Keep simple versions** of complex systems until basics work

---

## Assignment Deliverables

When project is complete, you'll need to prepare:
1. **Unity Project Files** - Entire project folder
2. **Build Files** - Windows/Stand-alone executable
3. **PowerPoint (10-15 slides):**
   - Game overview
   - Architecture explanation
   - Design patterns used
   - Key systems (4-5 detailed explanations)
   - Screenshots and gameplay footage
   - How assignment requirements were addressed
4. **Video Demonstration:**
   - Show all features working
   - 2-5 minutes long
5. **Manual with:**
   - Script table (all scripts with 4-5 line descriptions)
   - Detailed explanation of 4-5 most important scripts
   - Asset sources (SYNTY, SimpleZombies, etc.)
   - User guide (how to play)
   - Screenshots
   - How each requirement was met

---

## Important Notes for Agents

1. **ALWAYS read existing scripts** before modifying or creating new ones - patterns matter
2. **NEVER skip build order** - circular dependencies will break everything
3. **Use New Input System exclusively** - old Input class causes errors
4. **Comment everything** - assignment heavily weights code comments
5. **Test incrementally** - don't batch-create scripts
6. **Follow established patterns** - events, ScriptableObjects, interfaces
7. **Keep InputTester.cs** - invaluable for debugging
8. **Document changes** - update summary files if you make major changes
9. **Respect third-party code** - SYNTY assets are copyrighted, only extend their InputReader
10. **Assignment requirements are strict** - events, NavMesh, pooling, animation are required

---

## Contact Context

This project is for a Virtual Reality course assignment. The user is building a 3rd person zombie wave shooter in Unity. Previous attempts failed due to creating too many scripts without proper build order, causing circular dependencies and input system conflicts.

The current approach follows a bottom-up, layer-by-layer construction with heavy emphasis on:
- Event-driven architecture
- ScriptableObject data-driven design
- Interface-based polymorphism
- Proper dependency management
- Incremental testing

All architectural decisions and documentation are in the summary files listed above.
