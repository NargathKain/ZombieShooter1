# PROMPT: Create Complete Weapon Controller System

## Current Project State

You are working on a Unity 3rd person zombie shooter project. The project uses:

- **Unity 6000.2.8f1** with URP
- **New Input System** (via extended SYNTY InputReader)
- **Event-driven architecture** (Observer pattern)
- **ScriptableObject data system** (WeaponData, EnemyData)
- **IDamageable interface** for damage handling

### What Already Exists:

**Input System** (already working):
- `Assets/Synty/AnimationBaseLocomotion/Samples/Scripts/InputSystem/InputReader.cs`
- Extended with these actions:
  - `onShootPerformed` - Left Mouse Button
  - `onReloadPerformed` - R key
  - `onInteractPerformed` - E key
  - `onPausePerformed` - Escape key
  - `onWeaponScrollPerformed` - Mouse Scroll Y
  - `onAimActivated` - Right Mouse Button (started)
  - `onAimDeactivated` - Right Mouse Button (canceled)

**Health Systems** (already working):
- `IDamageable.cs` interface in `Scripts/Data/IDamageable.cs`
- `EnemyHealth.cs` in `Scripts/Damage/EnemyHealth.cs` - manages enemy HP, death, events
- `PlayerHealth.cs` in `Scripts/Damage/PlayerHealth.cs` - manages player HP
- `EnemyHealthbar.cs` in `Scripts/Damage/EnemyHealthbar.cs` - UI above enemies

**Weapon Data** (already defined):
- `WeaponData.cs` should exist as ScriptableObject with fields like:
  - weaponName
  - damage
  - fireRate
  - magazineSize
  - reloadTime
  - range
  - ammoType
  - bulletPrefab
  - shootSound
  - reloadSound
  - muzzleFlashPrefab

**Enemy Data** (already working):
- `EnemyData.cs` ScriptableObject exists with BasicZombie asset
- Has fields: maxHealth, attackDamage, moveSpeed, etc.

**Weapon Prefabs** (already exist):
- Located in `_Project/Prefabs/Weapons/` folder
- Multiple weapon prefabs: Pistol, Rifle, Shotgun, Minigun, RPG, etc.
- All weapons already attached to player with animation rigging

**UI** (partially working):
- Player UI canvas exists (`PlayerUI`)
- Health bar exists
- Hitmarker display exists

### What Needs to Be Created:

1. **Inventory System** - Track weapons player owns and ammo counts
2. **Weapon Controller** - Handle shooting, reloading, aiming, weapon switching
3. **Ammo UI** - Display ammo count (backpack/magazine format)

---

## Your Task: Create Complete Weapon Controller System

### IMPORTANT: Create SETUP.md Along with Code Changes

**You must create TWO deliverables:**

1. **Code Changes** - All of C# scripts and Unity setup described below
2. **SETUP.md** - A comprehensive step-by-step implementation guide

**SETUP.md Must Include:**

#### 1. Overview Section
- What system does (weapon controller with inventory integration)
- Dependencies required (InputReader, Inventory, EnemyHealth, etc.)
- Expected outcome (can shoot, reload, aim, see ammo)

#### 2. Files Created/Modified
- List every new script created
- List every existing script modified
- Brief description of what each does

#### 3. Unity Editor Setup Steps (Manual, Click-by-Click)

**Create ScriptableObject Assets:**
- How to create PistolData.asset
- What values to set (damage: 10, fireRate: 5, magazineSize: 12, reloadTime: 2, etc.)
- How to assign bulletPrefab, sounds, muzzleFlash

**Create Bullet Prefab:**
- Step-by-step: Create GameObject → Add Sphere → Scale → Add Rigidbody → Add Collider → Add Bullet script
- Exact values for each component (isKinematic: false, useGravity: false, isTrigger: true, etc.)

**Setup Player GameObject:**
- Which components to add (Inventory, WeaponController)
- How to assign references in Inspector (drag WeaponData, drag Bullet prefab, etc.)

**Setup UI:**
- How to create AmmoText in PlayerUI canvas
- How to assign to AmmoUI script
- Position, font size, color settings

#### 4. Inspector Configuration
- Show what each component should look like when properly configured
- List all fields that need to be filled in
- Provide example values

#### 5. Hierarchy Structure
- Show expected GameObject hierarchy
- Example:
  ```
  Player
  ├── Inventory (script)
  ├── WeaponController (script)
  └── PlayerUI
      ├── HealthBar
      └── AmmoText
  ```

#### 6. Troubleshooting Section
- Common issues and fixes:
  - "Weapon won't fire" → Check aiming, check InputReader, check bulletPrefab
  - "No damage to enemies" → Check EnemyHealth IDamageable implementation, check bullet trigger
  - "Ammo not displaying" → Check AmmoUI subscription, check Inventory events
  - "Reload not working" → Check inventory ammo, check reload time
  - "Camera not zooming" → Check onAimActivated subscription, check camera FOV changes

#### 7. Testing Checklist
Complete checklist that user can work through:
- [ ] Scripts compile with 0 errors
- [ ] Inventory component on Player
- [ ] WeaponController component on Player
- [ ] InputReader component on Player
- [ ] PistolData.asset created with correct values
- [ ] Bullet prefab created with correct components
- [ ] AmmoText UI created and assigned
- [ ] Enter Play Mode
- [ ] Console shows "equipped weapon: Pistol"
- [ ] Ammo display shows correct values (e.g., "60/12")
- [ ] Hold Right Mouse → Camera zooms (FOV decreases)
- [ ] Release Right Mouse → Camera returns to normal
- [ ] Left Mouse (not aiming) → Nothing happens
- [ ] Hold Right Mouse + Left Mouse → Fire!
- [ ] Console shows "firing Pistol"
- [ ] Muzzle flash appears
- [ ] Sound plays
- [ ] Bullet spawns and moves forward
- [ ] Ammo decreases
- [ ] Bullet hits zombie → Console shows "damage X applied to Zombie"
- [ ] Zombie health bar decreases
- [ ] After 5-6 shots → Zombie dies
- [ ] Console shows "Zombie died!"
- [ ] Zombie falls over animation
- [ ] Can walk through zombie corpse
- [ ] Press R → Reload starts
- [ ] Console shows "reloading Pistol"
- [ ] Reload sound plays
- [ ] Cannot shoot while reloading
- [ ] After 2 seconds → Magazine refilled
- [ ] Ammo display shows full magazine

#### 8. How System Works Together
- Explain data flow: InputReader → WeaponController → Bullet → EnemyHealth
- Show event subscriptions: Inventory → AmmoUI
- Show weapon equipping: WeaponController → Inventory → UI updates

**SETUP.md Format Guidelines:**
- Use clear, numbered steps (1., 2., 3.)
- Use bullet points for sub-steps
- Include code blocks where helpful for configuration
- Use checkbox format `[ ]` for testing checklist
- Make it complete enough that someone else could follow it from scratch without asking questions
- Assume reader knows basic Unity but needs specific setup guidance

**Example SETUP.md Structure:**
```markdown
# Weapon Controller System - Setup Guide

## Overview
This guide explains how to implement complete weapon controller system with inventory, ammo management, aiming, and bullet-based shooting...

## Files Created
- **Inventory.cs** - Tracks player weapons and ammo counts
- **WeaponController.cs** - Handles shooting, reloading, aiming
- **Bullet.cs** - Physical projectile with damage
- **AmmoUI.cs** - Displays ammo count on screen
- **EnemyHealth.cs** (modified) - Fixed damage handling and event broadcasting

## Step 1: Create ScriptableObject Assets

### Create PistolData Asset
1. In Project window, navigate to `_Project/ScriptableObjects/Weapons/`
2. Right-click → Create → Game → Weapon Data
3. Name it: `PistolData`
4. In Inspector, set values:
   - Weapon Name: `Pistol`
   - Damage: `10`
   - Fire Rate: `5` (shots per second)
   - Magazine Size: `12`
   - Reload Time: `2`
   - Range: `100`
   - Ammo Type: `Pistol`
   - (Assign bulletPrefab, shootSound, reloadSound, muzzleFlashPrefab later)
5. Save asset (Ctrl+S)

## Step 2: Create Bullet Prefab

### Create Bullet GameObject
1. Right-click in Hierarchy → 3D Object → Sphere
2. Name it: `Bullet`
3. Scale: (0.05, 0.05, 0.1)
4. Drag to Prefabs folder to create prefab

### Add Components to Bullet
1. Select Bullet prefab
2. Add Component → Rigidbody
   - Is Kinematic: `false`
   - Use Gravity: `false`
3. Add Component → Box Collider (or Sphere Collider)
   - Is Trigger: `true`
4. Add Component → Bullet script
   - Speed: `100`
   - Lifetime: `3`

## Step 3: Setup Player GameObject

[Detailed steps for adding components and assigning references...]

## Testing Checklist
[ ] All scripts compile
[ ] Pistol equipped on start
[ ] Aiming works with right mouse
[ ] Shooting works while aiming
...
```

---

## Create Complete Weapon Controller System

### 1. Inventory System (Create if doesn't exist)

**Requirements:**
- Track which weapons player has in inventory
- Track ammo count for each ammo type
- Player starts with pistol only (default weapon)
- Other weapons can be added later (pickups)
- Methods to:
  - `HasWeapon(WeaponData weapon)` - Check if player owns weapon
  - `AddWeapon(WeaponData weapon)` - Add weapon to inventory
  - `RemoveWeapon(WeaponData weapon)` - Remove weapon from inventory
  - `GetAmmo(AmmoType type)` - Get ammo count for type
  - `AddAmmo(AmmoType type, int amount)` - Add ammo
  - `RemoveAmmo(AmmoType type, int amount)` - Remove ammo
  - Events for inventory changes (for UI to subscribe)

**Example Interface:**
```csharp
public class Inventory : MonoBehaviour
{
    public static event Action<WeaponData> OnWeaponEquipped;
    public static event Action<AmmoType, int> OnAmmoChanged;

    public bool HasWeapon(WeaponData weapon);
    public int GetAmmo(AmmoType type);
    public void EquipWeapon(WeaponData weapon);
    public void ReloadCurrentWeapon();

    public WeaponData EquippedWeapon { get; }
    public int CurrentMagazineAmmo { get; }
    public int TotalAmmoForCurrentWeapon { get; }
}
```

---

### 2. Weapon Controller (Create complete version)

**Location:** `_Project/Scripts/Player/WeaponController.cs` (overwrite or create new)

**Core Requirements:**

#### 2.1. Inventory Integration
- Player has ALL weapons attached to character (animation rigging), but can ONLY use weapons that are in inventory
- Check inventory before using any weapon:
  - Cannot equip weapon if not in inventory
  - Cannot shoot if weapon not equipped
- Start with pistol equipped (check inventory, equip pistol)

#### 2.2. Aiming System
- **Right Mouse Button** (hold down): Camera zooms (aiming mode)
  - Use `inputReader.onAimActivated` when right mouse pressed
  - Use `inputReader.onAimDeactivated` when right mouse released
- Can ONLY shoot when aiming (right mouse button held)
  - If not aiming → ignore left mouse clicks
- Camera zoom can be implemented by:
  - Changing camera FOV (Field of View)
  - Or moving camera closer to weapon
  - Your choice

#### 2.3. Shooting System
- **Left Mouse Button**: Fire weapon (ONLY if aiming)
  - Use `inputReader.onShootPerformed`
  - Check: IsAiming → if true, then fire
  - If not aiming → do nothing
- **Bullet Prefab System**:
  - Spawn actual bullet prefab (not raycast instant hit)
  - Bullet should be physical projectile (moves forward, has collider)
  - Use `WeaponData.bulletPrefab`
  - Instantiate at weapon's shoot point/barrel
  - Bullet must have damage value
- **Fire Rate Control**:
  - Use `WeaponData.fireRate` (shots per second)
  - Prevent rapid-fire beyond fire rate
- **Muzzle Flash**:
  - Show `WeaponData.muzzleFlashPrefab` when shooting
  - Destroy after 0.1 seconds
- **Sound Effects**:
  - Play `WeaponData.shootSound` when firing
  - Use `AudioSource.PlayClipAtPoint()`

#### 2.4. Damage System
When bullet hits enemy:
- Get `IDamageable` component from hit collider
- Call `target.TakeDamage(damage)` with `WeaponData.damage`
- Show debug log: `"damage X applied to [enemy name]"`
- Call `HitmarkerDisplay.Instance?.ShowHitmarker()` for feedback

**Note:** If you find issues in `EnemyHealth.cs` that prevent proper damage communication, you MUST fix them.

#### 2.5. Reload System
- **R Key**: Reload weapon
  - Use `inputReader.onReloadPerformed`
  - Check: IsAiming → if aiming, cannot reload (or auto-stop aiming)
- **Reload Conditions**:
  - Must have enough ammo in inventory for current weapon
  - Magazine is not already full
- **Reload Logic**:
  - Check inventory: `inventory.GetAmmo(weaponData.ammoType)`
  - Calculate needed ammo: `weaponData.magazineSize - currentMagazineAmmo`
  - If has enough ammo:
    - Remove from inventory: `inventory.RemoveAmmo(...)`
    - Refill magazine: `currentMagazineAmmo = weaponData.magazineSize`
    - Play reload sound: `WeaponData.reloadSound`
    - Play reload animation (if available)
  - If not enough ammo:
    - Show "Not enough ammo" message or sound
- **Reload Time**:
  - Use `WeaponData.reloadTime`
  - Cannot shoot while reloading
  - Set `isReloading = true`, wait `reloadTime` seconds, then `isReloading = false`

#### 2.6. Ammo UI Display
- Display ammo count on screen (Text element in PlayerUI canvas)
- Format: `backpack/magazine` (e.g., "50/8" for shotgun)
  - First number: Total ammo in inventory for this weapon's ammo type
  - Second number: Ammo currently in magazine
- Update display whenever:
  - Ammo changes (shoot)
  - Reload completes
  - Weapon switched (if implementing weapon switching)
- Subscribe to inventory ammo change events

#### 2.7. Weapon Equipping
- **Weapon Data** from ScriptableObject defines weapon properties
- Player starts with pistol equipped
- When equipping weapon:
  - Hide/disable all other weapon models on player
  - Show/enable equipped weapon model
  - Update ammo display for that weapon
  - Console log: `"equipped weapon: [weapon name]"`
- Weapon switching can be:
  - Number keys (1, 2, 3...) - use `onWeaponScrollPerformed` or add number key actions
  - OR mouse wheel - `onWeaponScrollPerformed` passes scroll delta
- For this task, focus on PISTOL only (keep switching simple or skip for now)

#### 2.8. Console Logging (for debugging)
Add these console logs:
1. When weapon fires: `"firing [weapon name]"`
2. When weapon hits enemy: `"damage [damageAmount] applied to [enemyName]"`
3. When enemy dies: `EnemyHealth.cs` already logs `"[enemy name] died!"` - keep this
4. When equipping weapon: `"equipped weapon: [weaponName]"`
5. When reloading: `"reloading [weapon name]"`
6. When ammo pickup collected: (if implementing pickups) `"collected [amount] [ammoType] ammo"`

#### 2.9. Component Lifecycle
- Use `RequireComponent` attributes to ensure required components exist
- Subscribe to InputReader events in `Start()` or `OnEnable()`
- **CRITICAL**: Unsubscribe from events in `OnDisable()` or `OnDestroy()`
- Use `[SerializeField]` for configurable fields in Inspector
- Use XML comments on all public methods

---

### 3. Enemy Health System (Check and Fix if Needed)

**File:** `_ProjectFiles/Scripts/Damage/EnemyHealth.cs` (or similar location)

**Check for these issues and fix them:**

#### Issue 1: IDamageable Implementation
- Must implement `IDamageable` interface correctly
- `TakeDamage(float damage)` method must be public
- Should broadcast `OnHealthChanged` event when damaged
- Should fire `OnDeath` event when health reaches 0

#### Issue 2: Death Animation
- When enemy dies, should:
  - Set `IsDead = true`
  - Disable `EnemyAI` and `NavMeshAgent` (stop chasing)
  - Disable collider (so player can walk through)
  - Start fall-over animation (rotate -90° on X axis)
  - Play death sound
  - Spawn death particles/effect
  - Destroy after delay (2 seconds)

#### Issue 3: Event Broadcasting
- Should have events for other systems to subscribe:
```csharp
public static event Action<EnemyHealth> OnEnemyDeath; // When enemy dies
public event Action<float, float> OnHealthChanged;     // When health changes
```

#### Issue 4: Log Messages
- When taking damage: Log `"[enemyName] took [damage] damage. Health: [current]/[max]"`
- When dying: Log `"[enemyName] died!"`

#### Issue 5: Null Checks
- Must check if `enemyData` is assigned in Start()
- Must handle null `OnHealthChanged` events gracefully
- Must check if audio/particle effects exist before instantiating

**If EnemyHealth has any of these issues, fix them.**

---

### 4. Bullet Prefab (Ensure exists or create)

**Location:** Create bullet prefab in `_Project/Prefabs/Weapons/Bullet.prefab`

**Requirements:**
- Small sphere or capsule GameObject
- Scale: (0.05, 0.05, 0.1) - small size
- Rigidbody component:
  - `isKinematic = false` (affected by physics)
  - `useGravity = false` (don't fall)
- Collider component:
  - `isTrigger = true` (detect hits without physics collision)
- Script: `Bullet.cs` or similar
  - Move forward at constant speed
  - Destroy after lifetime (3 seconds)
  - Deal damage on collision with enemy
- Optional: TrailRenderer or particle effect for visual

**Bullet Script Example:**
```csharp
public class Bullet : MonoBehaviour
{
    [SerializeField] private float speed = 100f;
    [SerializeField] private float lifetime = 3f;
    [SerializeField] private float damage = 10f;

    void Start()
    {
        Destroy(gameObject, lifetime);
    }

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
            Debug.Log($"damage {damage} applied to {other.name}");
        }
        Destroy(gameObject);
    }
}
```

**Note:** Bullet damage can be set from WeaponController when spawning:
```csharp
GameObject bullet = Instantiate(bulletPrefab, shootPoint.position, shootPoint.rotation);
Bullet bulletScript = bullet.GetComponent<Bullet>();
bulletScript.SetDamage(weaponData.damage); // Need to add this method to Bullet.cs
```

---

### 5. UI Elements (Create if missing)

**Ammo Text UI:**
- Location: `PlayerUI` Canvas (Screen Space Overlay)
- Add Text component: "AmmoText"
- Position: Bottom-right or near health bar
- Format: "50/8" (Total/Magazine)
- Font size: Large enough to be readable
- Color: White or yellow for visibility

**Ammo UI Script:**
```csharp
public class AmmoUI : MonoBehaviour
{
    [SerializeField] private Text ammoText;
    [SerializeField] private Inventory inventory;

    void Start()
    {
        inventory = GetComponent<Inventory>();
        Inventory.OnAmmoChanged += UpdateAmmoDisplay;
        UpdateAmmoDisplay();
    }

    void OnDisable()
    {
        Inventory.OnAmmoChanged -= UpdateAmmoDisplay;
    }

    void UpdateAmmoDisplay()
    {
        int totalAmmo = inventory.GetAmmo(inventory.EquippedWeapon.ammoType);
        int magazineAmmo = inventory.CurrentMagazineAmmo;
        ammoText.text = $"{totalAmmo}/{magazineAmmo}";
    }
}
```

---

### 6. Input Reader Integration

**Namespace:** `using Synty.AnimationBaseLocomotion.Samples.InputSystem;`

**Subscriptions needed in WeaponController.cs:**
```csharp
void Start()
{
    inputReader = GetComponent<InputReader>();

    inputReader.onShootPerformed += TryShoot;
    inputReader.onReloadPerformed += TryReload;
    inputReader.onAimActivated += StartAiming;
    inputReader.onAimDeactivated += StopAiming;
    // If implementing weapon switching:
    inputReader.onWeaponScrollPerformed += ScrollWeapons;
}

void OnDestroy()
{
    if (inputReader != null)
    {
        inputReader.onShootPerformed -= TryShoot;
        inputReader.onReloadPerformed -= TryReload;
        inputReader.onAimDeactivated -= StopAiming;
        // Unsubscribe from weapon scroll too
    }
}
```

---

## Expected Behavior Testing

### Test Sequence:

1. **Start Game:**
   - Player spawns with pistol equipped
   - Console shows: `"equipped weapon: Pistol"`
   - Ammo display shows starting ammo (e.g., "60/12")

2. **Aiming:**
   - Hold Right Mouse → Camera zooms (FOV decreases)
   - Release Right Mouse → Camera returns to normal

3. **Shooting (without aiming):**
   - Press Left Mouse → Nothing happens (not aiming)

4. **Shooting (while aiming):**
   - Hold Right Mouse (aiming)
   - Press Left Mouse → Fire!
     - Console: `"firing Pistol"`
     - Muzzle flash plays
     - Sound plays
     - Bullet prefab spawns from gun barrel
     - Ammo decreases: "59/12"
   - Repeat: Fire rate limits rapid shooting

5. **Hitting Enemy:**
   - Bullet hits zombie
     - Console: `"damage 10 applied to Zombie"`
     - Zombie health bar decreases
     - Zombie takes damage

6. **Killing Enemy:**
   - After enough shots (5-6 for pistol):
     - Console: `"Zombie took 10 damage. Health: 10/50"`
     - Console: `"Zombie died!"`
     - Zombie falls over animation
     - Zombie collider disables (can walk through)
     - Zombie destroys after 2 seconds

7. **Reloading (with ammo):**
   - Press R → Reload starts
     - Console: `"reloading Pistol"`
     - Reload sound plays
     - Reload animation plays
     - Cannot shoot while reloading
   - After 2 seconds → Reload complete
     - Ammo display: "60/12" (full magazine)

8. **Reloading (no ammo):**
   - Press R → Check inventory
   - If 0 ammo in backpack → Show "Not enough ammo" or sound

9. **Empty Magazine:**
   - Fire 12 shots → Magazine empty: "48/0"
   - Try to shoot → Cannot fire (click sound or nothing)
   - Reload to get more ammo from backpack

---

## Technical Requirements Summary

### Must Create/Modify:

1. **Inventory.cs** (if doesn't exist)
   - Track weapons and ammo
   - Events for UI subscription
   - Methods for equip, reload, ammo management

2. **WeaponController.cs** (create complete version)
   - Input Reader integration (aim, shoot, reload)
   - Bullet prefab spawning
   - Damage system with IDamageable
   - Reload system with ammo check
   - Aiming with camera zoom
   - Fire rate control
   - Magazine tracking
   - Console logging

3. **EnemyHealth.cs** (check and fix if needed)
   - Proper IDamageable implementation
   - Death animation and collider disable
   - Event broadcasting
   - Console logging

4. **Bullet.cs** (ensure exists)
   - Move forward at speed
   - Deal damage on trigger
   - Destroy after lifetime

5. **AmmoUI.cs** (create)
   - Subscribe to inventory ammo events
   - Display backpack/magazine format

6. **UI Setup** (in Unity Editor)
   - Add AmmoText to PlayerUI canvas
   - Create bullet prefab
   - Create/review WeaponData ScriptableObjects for each weapon

---

## File Locations Reference

```
_Project/
├── Scripts/
│   ├── Player/
│   │   ├── WeaponController.cs          (CREATE/UPDATE)
│   │   ├── Bullet.cs                   (ENSURE EXISTS)
│   │   └── Inventory.cs                (CREATE IF MISSING)
│   ├── Damage/
│   │   ├── EnemyHealth.cs              (CHECK AND FIX IF NEEDED)
│   │   └── PlayerHealth.cs            (exists - no changes needed)
│   └── UI/
│       └── AmmoUI.cs                   (CREATE)
├── Prefabs/
│   └── Weapons/
│       ├── Bullet.prefab               (ENSURE EXISTS)
│       ├── Pistol.prefab               (exists)
│       └── [other weapons]           (exist)
└── ScriptableObjects/
    └── Weapons/
        └── PistolData.asset            (ENSURE EXISTS)
```

---

## Important Constraints & Notes

1. **Use New Input System ONLY** - Never use `Input.GetKey()` or `Input.GetMouseButton()`
2. **Event-Driven Architecture** - Subscribe/unsubscribe properly, use events for UI
3. **ScriptableObjects for Data** - Create PistolData.asset with all weapon properties
4. **IDamageable Interface** - Use polymorphism for damage (don't check for "EnemyHealth" directly)
5. **XML Comments** - Comment every public method and class (assignment requirement)
6. **Incremental Testing** - Test each feature before adding next
7. **Build Order** - Inventory should exist before WeaponController
8. **Component Lifecycle** - Use `[RequireComponent]`, null checks, proper unsubscribe
9. **Ammo Format** - Display as "total/magazine" (e.g., "50/8")
10. **Console Logs** - All required debug messages for debugging

---

## Additional Context

**Assignment Requirements:**
- Well-commented code (critical for grading)
- Proper architecture (ScriptableObjects, Events, Interfaces)
- Event-driven communication between systems
- Object pooling (not required for this weapon system yet, but plan for it)

**Existing Systems:**
- Input system fully functional with all needed actions
- Health systems working (Player and Enemy)
- Enemy AI with NavMesh pathworking
- UI foundation exists (PlayerUI canvas)

**Your Goal:**
Create a complete, functional weapon controller that:
- Spawns physical bullets
- Deals damage to enemies via IDamageable interface
- Requires aiming to shoot (right mouse hold)
- Reloads with ammo check (R key)
- Displays ammo on screen
- Logs to console for debugging
- Integrates with inventory (weapon ownership, ammo tracking)

---

## Start Here

1. Read existing scripts first (EnemyHealth.cs, Inventory.cs if exists, current WeaponController.cs)
2. Check if Inventory exists - if not, create it first (foundation)
3. Check EnemyHealth.cs for issues - fix any problems with damage handling
4. Ensure bullet prefab exists or create it
5. Create complete WeaponController.cs
6. Create AmmoUI.cs
7. Test in Unity:
   - Start game, check console for "equipped weapon: Pistol"
   - Aim (right mouse), shoot (left mouse)
   - Check console for "firing Pistol"
   - Shoot zombie, check console for "damage X applied to Zombie"
   - Verify zombie health bar decreases and enemy dies
   - Test reload with R key
   - Verify ammo display updates correctly

**DELIVERABLES:**
1. All code changes (C# scripts, Unity setup)
2. **SETUP.md** - Step-by-step implementation guide (MANDATORY)
   - File changes overview
   - Unity Editor setup instructions
   - Component references and Inspector values
   - Hierarchy structure
   - Testing checklist
   - Troubleshooting section

**Remember to update AGENTS.md if you make significant architectural changes!**
