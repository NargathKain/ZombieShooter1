# 🏗️ ZOMBIE SHOOTER - CORRECT BUILD ORDER
## Fresh Start Implementation Guide

---

## 📋 Overview

This guide builds your zombie shooter in **dependency order**, ensuring nothing breaks. Each phase is tested before moving to the next.

**Total estimated time:** 5-7 days (2-3 hours/day)

---

## 🎬 PHASE 0: Project Setup (30 minutes)

### Step 0.1: Create Folder Structure

Create these folders in `Assets/_Project/`:
```
Assets/
└── _Project/
    ├── Scripts/
    │   ├── Data/           (ScriptableObjects, Interfaces, Enums)
    │   ├── Player/         (Health, Inventory, Weapons)
    │   ├── Enemies/        (AI, Health)
    │   ├── Managers/       (Game, Wave, Pool)
    │   ├── UI/             (Health bars, Ammo display)
    │   └── Pickups/        (Collectible items)
    ├── ScriptableObjects/
    │   ├── Weapons/
    │   ├── Enemies/
    │   └── Pickups/
    ├── Prefabs/
    │   ├── Weapons/
    │   ├── Enemies/
    │   └── Pickups/
    ├── Scenes/
    └── Materials/
```

### Step 0.2: Scene Setup

1. Create new scene: `MainGame`
2. Add SYNTY environment assets (ground, buildings, props)
3. Add basic lighting (Directional Light)
4. Save scene

**✅ Test:** Scene opens without errors

---

## 🧱 PHASE 1: Foundation Layer (Day 1 - 1 hour)

**Goal:** Create data structures and interfaces that everything else depends on.

### Step 1.1: Create Enums

**Create:** `Assets/_Project/Scripts/Data/AmmoType.cs`

```csharp
/// <summary>
/// Types of ammunition in the game
/// </summary>
public enum AmmoType
{
    Pistol,
    Rifle,
    Shotgun
}
```

**Create:** `Assets/_Project/Scripts/Data/PickupType.cs`

```csharp
/// <summary>
/// Types of pickable items
/// </summary>
public enum PickupType
{
    Ammo,
    Health,
    Weapon
}
```

**✅ Test:** Scripts compile with 0 errors

---

### Step 1.2: Create Interfaces

**Create:** `Assets/_Project/Scripts/Data/IDamageable.cs`

```csharp
/// <summary>
/// Interface for any object that can take damage
/// Implemented by PlayerHealth and EnemyHealth
/// </summary>
public interface IDamageable
{
    void TakeDamage(float damage);
}
```

**Create:** `Assets/_Project/Scripts/Data/IInteractable.cs`

```csharp
/// <summary>
/// Interface for objects the player can interact with (doors, switches, etc.)
/// </summary>
public interface IInteractable
{
    void Interact();
    string GetInteractPrompt();
}
```

**✅ Test:** Scripts compile with 0 errors

---

### Step 1.3: Create ScriptableObject Templates

**Create:** `Assets/_Project/Scripts/Data/WeaponData.cs`

```csharp
using UnityEngine;

/// <summary>
/// ScriptableObject that defines a weapon's properties
/// Create via: Right-click → Create → Game/Weapon Data
/// </summary>
[CreateAssetMenu(fileName = "New Weapon", menuName = "Game/Weapon Data")]
public class WeaponData : ScriptableObject
{
    [Header("Basic Info")]
    public string weaponName = "Pistol";
    public GameObject weaponPrefab;
    
    [Header("Combat Stats")]
    public float damage = 10f;
    public float fireRate = 5f;        // Shots per second
    public float range = 100f;
    public int magazineSize = 30;
    public float reloadTime = 2f;
    public AmmoType ammoType = AmmoType.Pistol;
    
    [Header("Effects")]
    public AudioClip shootSound;
    public AudioClip reloadSound;
    public GameObject muzzleFlashPrefab;
    public GameObject bulletHolePrefab;
}
```

**Create:** `Assets/_Project/Scripts/Data/EnemyData.cs`

```csharp
using UnityEngine;

/// <summary>
/// ScriptableObject that defines an enemy's properties
/// Create via: Right-click → Create → Game/Enemy Data
/// </summary>
[CreateAssetMenu(fileName = "New Enemy", menuName = "Game/Enemy Data")]
public class EnemyData : ScriptableObject
{
    [Header("Basic Info")]
    public string enemyName = "Zombie";
    public GameObject enemyPrefab;
    
    [Header("Combat Stats")]
    public float maxHealth = 50f;
    public float attackDamage = 10f;
    public float attackCooldown = 1f;
    public float attackRange = 2f;
    
    [Header("Movement")]
    public float moveSpeed = 3f;
    public float detectionRange = 10f;
    
    [Header("Rewards")]
    public int pointsOnDeath = 100;
    public float dropChance = 0.3f;
    
    [Header("Effects")]
    public AudioClip attackSound;
    public AudioClip deathSound;
    public GameObject deathEffect;
}
```

**Create:** `Assets/_Project/Scripts/Data/PickupData.cs`

```csharp
using UnityEngine;

/// <summary>
/// ScriptableObject that defines a pickup item's properties
/// Create via: Right-click → Create → Game/Pickup Data
/// </summary>
[CreateAssetMenu(fileName = "New Pickup", menuName = "Game/Pickup Data")]
public class PickupData : ScriptableObject
{
    [Header("Basic Info")]
    public string pickupName = "Ammo Box";
    public PickupType pickupType;
    
    [Header("Ammo Pickup Settings")]
    public AmmoType ammoType = AmmoType.Pistol;
    public int ammoAmount = 20;
    
    [Header("Health Pickup Settings")]
    public float healthAmount = 25f;
    
    [Header("Weapon Pickup Settings")]
    public WeaponData weaponToGive;
    
    [Header("Effects")]
    public AudioClip pickupSound;
    public GameObject pickupEffect;
}
```

**✅ Test:** Scripts compile with 0 errors

---

## 🎮 PHASE 2: Input System (Day 1 - 1 hour)

**Goal:** Get input working so we can control things.

### Step 2.1: Create Input Actions

1. In Project window: Right-click → Create → Input Actions
2. Name it: `GameInputActions`
3. Double-click to open Input Actions window
4. Create Action Map: "Gameplay"
5. Add these actions:

| Action Name | Binding | Type |
|------------|---------|------|
| Fire | Left Mouse Button | Button |
| Reload | R key | Button |
| Interact | E key | Button |
| DebugDamage | K key | Button |
| WeaponSlot1 | 1 key | Button |
| WeaponSlot2 | 2 key | Button |
| WeaponSlot3 | 3 key | Button |
| WeaponSlot4 | 4 key | Button |
| WeaponSlot5 | 5 key | Button |
| WeaponSlot6 | 6 key | Button |
| WeaponSlot7 | 7 key | Button |
| WeaponScroll | Mouse Scroll Y | Value (Float) |

6. Click **"Save Asset"**
7. Click **"Generate C# Class"** (if available)

---

### Step 2.2: Create GameInputReader

**Create:** `Assets/_Project/Scripts/Player/GameInputReader.cs`

Copy the full script from your INPUT_SYSTEM_FIX.md file (lines 22-167).

**✅ Test:** Script compiles with 0 errors

---

### Step 2.3: Setup SYNTY Character

1. Drag SYNTY character prefab into scene
2. Position at (0, 0, 0)
3. Tag as **"Player"**
4. Verify it has SYNTY's movement scripts
5. Test movement works (WASD + mouse look)

**✅ Test:** Can walk around scene

---

### Step 2.4: Add GameInputReader to Player

1. Select Player GameObject
2. Add Component → **GameInputReader**
3. In Inspector, drag `GameInputActions` asset to "Input Actions Asset" field
4. Play mode → check Console for warnings about missing actions

**✅ Test:** 
- Enter Play mode
- Console shows no input errors
- All actions found (no warnings)

---

## 💚 PHASE 3: Player Health System (Day 2 - 1 hour)

**Goal:** Player can take damage and die.

### Step 3.1: Create PlayerHealth Script

**Create:** `Assets/_Project/Scripts/Player/PlayerHealth.cs`

```csharp
using UnityEngine;

/// <summary>
/// Manages player health, damage, healing, and death
/// </summary>
public class PlayerHealth : MonoBehaviour, IDamageable
{
    [Header("Settings")]
    [SerializeField] private float maxHealth = 100f;
    
    [Header("Debug")]
    [SerializeField] private float debugDamageAmount = 10f;
    
    // Current state
    private float currentHealth;
    public bool IsDead { get; private set; }
    
    // Events for UI to subscribe to
    public static event System.Action<float, float> OnHealthChanged; // current, max
    public static event System.Action OnPlayerDeath;
    public static event System.Action OnPlayerRespawn;
    
    void Start()
    {
        currentHealth = maxHealth;
        IsDead = false;
        OnHealthChanged?.Invoke(currentHealth, maxHealth);
    }
    
    void Update()
    {
        // Debug: Press K to damage yourself
        if (GameInputReader.Instance != null && GameInputReader.Instance.DebugDamagePressed)
        {
            TakeDamage(debugDamageAmount);
        }
    }
    
    /// <summary>
    /// Reduces player health by damage amount
    /// </summary>
    public void TakeDamage(float damage)
    {
        if (IsDead) return;
        
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);
        
        Debug.Log($"Player took {damage} damage. Health: {currentHealth}/{maxHealth}");
        
        // Notify listeners
        OnHealthChanged?.Invoke(currentHealth, maxHealth);
        
        // Check death
        if (currentHealth <= 0f)
        {
            Die();
        }
    }
    
    /// <summary>
    /// Restores player health
    /// </summary>
    public void Heal(float amount)
    {
        if (IsDead) return;
        
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);
        
        Debug.Log($"Player healed {amount}. Health: {currentHealth}/{maxHealth}");
        
        OnHealthChanged?.Invoke(currentHealth, maxHealth);
    }
    
    private void Die()
    {
        IsDead = true;
        Debug.Log("Player died!");
        OnPlayerDeath?.Invoke();
    }
    
    /// <summary>
    /// Resets health to full (for respawning)
    /// </summary>
    public void Respawn()
    {
        currentHealth = maxHealth;
        IsDead = false;
        OnPlayerRespawn?.Invoke();
        OnHealthChanged?.Invoke(currentHealth, maxHealth);
        Debug.Log("Player respawned!");
    }
}
```

---

### Step 3.2: Add to Player

1. Select Player GameObject
2. Add Component → **PlayerHealth**
3. In Inspector, set Max Health = 100

**✅ Test:**
- Enter Play mode
- Press K → see "Player took 10 damage" in Console
- Health decreases to 0 → see "Player died!" in Console

---

## 🎨 PHASE 4: Basic Health UI (Day 2 - 30 minutes)

**Goal:** See health on screen.

### Step 4.1: Create UI Canvas

1. Right-click in Hierarchy → UI → Canvas
2. Rename to "PlayerUI"
3. Set Canvas Scaler → UI Scale Mode → "Scale With Screen Size"
4. Reference Resolution: 1920x1080

---

### Step 4.2: Create Health Bar

1. Right-click PlayerUI → UI → Slider
2. Rename to "HealthBar"
3. Position in bottom-left corner
4. Anchor to bottom-left
5. Delete the "Handle Slide Area" child (we don't need a draggable slider)
6. Select Fill Area → Fill → set color to red
7. Select Background → set color to dark gray

---

### Step 4.3: Create PlayerHealthUI Script

**Create:** `Assets/_Project/Scripts/UI/PlayerHealthUI.cs`

```csharp
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Updates the player health bar UI
/// </summary>
public class PlayerHealthUI : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private Slider healthSlider;
    [SerializeField] private Image damageFlash; // Optional red flash effect
    
    [Header("Flash Settings")]
    [SerializeField] private float flashDuration = 0.2f;
    [SerializeField] private Color flashColor = new Color(1f, 0f, 0f, 0.3f);
    
    private float flashTimer;
    private float previousHealth;
    
    void OnEnable()
    {
        PlayerHealth.OnHealthChanged += UpdateHealthBar;
        PlayerHealth.OnPlayerDeath += OnPlayerDeath;
    }
    
    void OnDisable()
    {
        PlayerHealth.OnHealthChanged -= UpdateHealthBar;
        PlayerHealth.OnPlayerDeath -= OnPlayerDeath;
    }
    
    void Start()
    {
        if (damageFlash != null)
        {
            damageFlash.color = new Color(flashColor.r, flashColor.g, flashColor.b, 0f);
        }
    }
    
    void Update()
    {
        // Handle damage flash fade
        if (flashTimer > 0f)
        {
            flashTimer -= Time.deltaTime;
            float alpha = Mathf.Lerp(0f, flashColor.a, flashTimer / flashDuration);
            damageFlash.color = new Color(flashColor.r, flashColor.g, flashColor.b, alpha);
        }
    }
    
    private void UpdateHealthBar(float currentHealth, float maxHealth)
    {
        if (healthSlider != null)
        {
            healthSlider.maxValue = maxHealth;
            healthSlider.value = currentHealth;
        }
        
        // Trigger flash if took damage
        if (currentHealth < previousHealth && damageFlash != null)
        {
            flashTimer = flashDuration;
        }
        
        previousHealth = currentHealth;
    }
    
    private void OnPlayerDeath()
    {
        Debug.Log("PlayerHealthUI: Player died!");
        // Could show death screen here
    }
}
```

---

### Step 4.4: Connect UI

1. Select PlayerUI Canvas
2. Add Component → **PlayerHealthUI**
3. Drag HealthBar slider to "Health Slider" field
4. (Optional) Create a full-screen Image for damage flash, assign to "Damage Flash"

**✅ Test:**
- Enter Play mode
- Press K to damage
- Health bar decreases
- Red flash appears (if damage flash image assigned)

---

## 🔫 PHASE 5: Shooting System - Basic (Day 3 - 2 hours)

**Goal:** Click mouse → raycast fires, hits things.

### Step 5.1: Create Simplified WeaponController

**Create:** `Assets/_Project/Scripts/Player/WeaponController_Simple.cs`

```csharp
using UnityEngine;

/// <summary>
/// SIMPLIFIED shooting system for testing
/// No inventory, no ammo, just pure shooting
/// </summary>
public class WeaponController_Simple : MonoBehaviour
{
    [Header("Test Weapon")]
    [SerializeField] private WeaponData testWeapon;
    [SerializeField] private Transform shootPoint;
    
    [Header("Settings")]
    [SerializeField] private LayerMask hitLayers;
    [SerializeField] private GameObject bulletPrefab;
    
    private Camera playerCamera;
    private float lastFireTime;
    
    void Start()
    {
        playerCamera = Camera.main;
        
        if (testWeapon == null)
        {
            Debug.LogError("No test weapon assigned!");
            enabled = false;
        }
        
        if (shootPoint == null)
        {
            Debug.LogError("No shoot point assigned!");
            enabled = false;
        }
    }
    
    void Update()
    {
        if (GameInputReader.Instance == null) return;
        
        if (GameInputReader.Instance.FirePressed)
        {
            TryShoot();
        }
    }
    
    void TryShoot()
    {
        // Check fire rate
        float timeSinceLastShot = Time.time - lastFireTime;
        float fireInterval = 1f / testWeapon.fireRate;
        
        if (timeSinceLastShot < fireInterval)
        {
            return;
        }
        
        lastFireTime = Time.time;
        
        // Raycast from camera center
        Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        
        // Visual debug
        Debug.DrawRay(ray.origin, ray.direction * testWeapon.range, Color.yellow, 1f);
        
        if (Physics.Raycast(ray, out RaycastHit hit, testWeapon.range, hitLayers))
        {
            Debug.Log($"🎯 HIT: {hit.collider.name} at {hit.distance}m");
            
            // Try to damage
            IDamageable damageable = hit.collider.GetComponent<IDamageable>();
            if (damageable != null)
            {
                damageable.TakeDamage(testWeapon.damage);
            }
            
            // Spawn bullet hole
            if (testWeapon.bulletHolePrefab != null)
            {
                GameObject hole = Instantiate(testWeapon.bulletHolePrefab, hit.point, 
                                              Quaternion.LookRotation(hit.normal));
                Destroy(hole, 10f);
            }
        }
        else
        {
            Debug.Log("❌ MISS: Shot into void");
        }
        
        // Muzzle flash
        if (testWeapon.muzzleFlashPrefab != null)
        {
            GameObject flash = Instantiate(testWeapon.muzzleFlashPrefab, 
                                           shootPoint.position, 
                                           shootPoint.rotation);
            Destroy(flash, 0.1f);
        }
        
        // Sound
        if (testWeapon.shootSound != null)
        {
            AudioSource.PlayClipAtPoint(testWeapon.shootSound, shootPoint.position);
        }
    }
}
```

---

### Step 5.2: Create First Weapon Data Asset

1. In Project: Navigate to `_Project/ScriptableObjects/Weapons/`
2. Right-click → Create → Game/Weapon Data
3. Name it: **"TestPistol"**
4. Set values:
   - Weapon Name: "Test Pistol"
   - Damage: 10
   - Fire Rate: 5
   - Range: 100
   - Magazine Size: 30 (not used yet)
   - Reload Time: 2 (not used yet)
   - Ammo Type: Pistol

---

### Step 5.3: Setup Shoot Point

1. Select Player GameObject
2. Create empty child: Right-click Player → Create Empty
3. Rename to "ShootPoint"
4. Position it at camera/weapon position (where bullets come from)
5. Typical position: (0.5, 1.5, 0.5)

---

### Step 5.4: Add Weapon Controller

1. Select Player GameObject
2. Add Component → **WeaponController_Simple**
3. Drag TestPistol asset to "Test Weapon"
4. Drag ShootPoint to "Shoot Point"
5. Set Hit Layers to "Everything" or specific layers

---

### Step 5.5: Create Test Target

1. Create Cube in scene
2. Position 5 units in front of player
3. Scale to (1, 2, 1) - human-ish size
4. Tag as "Enemy"
5. Add Component → **BoxCollider** (if not present)

**✅ Test:**
- Enter Play mode
- Aim at cube
- Left-click
- See yellow debug ray in Scene view
- Console shows "🎯 HIT: Cube at Xm"

---

## 🧟 PHASE 6: Enemy Health System (Day 3 - 1 hour)

**Goal:** Enemies can take damage and die.

### Step 6.1: Create EnemyHealth Script

**Create:** `Assets/_Project/Scripts/Enemies/EnemyHealth.cs`

```csharp
using UnityEngine;

/// <summary>
/// Manages enemy health and death
/// </summary>
public class EnemyHealth : MonoBehaviour, IDamageable
{
    [Header("Data")]
    [SerializeField] private EnemyData enemyData;
    
    // Current state
    private float currentHealth;
    public bool IsDead { get; private set; }
    
    // Events
    public event System.Action<float, float> OnHealthChanged; // current, max
    public event System.Action OnDeath;
    
    void Start()
    {
        if (enemyData == null)
        {
            Debug.LogError($"EnemyHealth on {gameObject.name}: No EnemyData assigned!");
            return;
        }
        
        currentHealth = enemyData.maxHealth;
        IsDead = false;
        OnHealthChanged?.Invoke(currentHealth, enemyData.maxHealth);
    }
    
    public void TakeDamage(float damage)
    {
        if (IsDead) return;
        
        currentHealth -= damage;
        currentHealth = Mathf.Max(0f, currentHealth);
        
        Debug.Log($"{enemyData.enemyName} took {damage} damage. Health: {currentHealth}/{enemyData.maxHealth}");
        
        OnHealthChanged?.Invoke(currentHealth, enemyData.maxHealth);
        
        if (currentHealth <= 0f)
        {
            Die();
        }
    }
    
    private void Die()
    {
        IsDead = true;
        Debug.Log($"{enemyData.enemyName} died!");
        
        OnDeath?.Invoke();
        
        // Play death sound
        if (enemyData.deathSound != null)
        {
            AudioSource.PlayClipAtPoint(enemyData.deathSound, transform.position);
        }
        
        // Spawn death effect
        if (enemyData.deathEffect != null)
        {
            GameObject effect = Instantiate(enemyData.deathEffect, transform.position, Quaternion.identity);
            Destroy(effect, 3f);
        }
        
        // Destroy after short delay
        Destroy(gameObject, 0.5f);
    }
    
    public EnemyData GetEnemyData()
    {
        return enemyData;
    }
}
```

---

### Step 6.2: Create Enemy Data Asset

1. Navigate to `_Project/ScriptableObjects/Enemies/`
2. Right-click → Create → Game/Enemy Data
3. Name: **"BasicZombie"**
4. Set values:
   - Enemy Name: "Zombie"
   - Max Health: 50
   - Attack Damage: 10
   - Attack Cooldown: 1
   - Attack Range: 2
   - Move Speed: 3
   - Detection Range: 10
   - Points On Death: 100

---

### Step 6.3: Convert Test Cube to Enemy

1. Select the test Cube
2. Add Component → **EnemyHealth**
3. Drag BasicZombie asset to "Enemy Data"
4. Make sure it's tagged "Enemy"

**✅ Test:**
- Enter Play mode
- Shoot the cube
- Console shows damage logs
- After enough shots (5 hits × 10 damage = 50), cube destroys
- See "Zombie died!" in Console

---

## 🎯 PHASE 7: Basic UI - Crosshair (Day 4 - 30 minutes)

### Step 7.1: Create Crosshair

1. In PlayerUI Canvas, create: UI → Image
2. Rename to "Crosshair"
3. Anchor to center
4. Size: 32x32 pixels
5. Set sprite to a crosshair image (or simple white square for now)
6. Set color to white with transparency (255, 255, 255, 150)

---

### Step 7.2: Create ReticleController

**Create:** `Assets/_Project/Scripts/UI/ReticleController.cs`

```csharp
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Changes crosshair color based on what you're aiming at
/// </summary>
public class ReticleController : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private Image reticleImage;
    
    [Header("Settings")]
    [SerializeField] private float raycastDistance = 100f;
    [SerializeField] private LayerMask detectLayers;
    
    [Header("Colors")]
    [SerializeField] private Color defaultColor = Color.white;
    [SerializeField] private Color enemyColor = Color.red;
    [SerializeField] private Color interactableColor = Color.cyan;
    
    private Camera playerCamera;
    
    void Start()
    {
        playerCamera = Camera.main;
        
        if (reticleImage == null)
        {
            Debug.LogError("ReticleController: No reticle image assigned!");
            enabled = false;
        }
    }
    
    void Update()
    {
        UpdateReticleColor();
    }
    
    void UpdateReticleColor()
    {
        Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        
        if (Physics.Raycast(ray, out RaycastHit hit, raycastDistance, detectLayers))
        {
            // Check for enemy
            if (hit.collider.CompareTag("Enemy"))
            {
                reticleImage.color = enemyColor;
                return;
            }
            
            // Check for interactable
            IInteractable interactable = hit.collider.GetComponent<IInteractable>();
            if (interactable != null)
            {
                reticleImage.color = interactableColor;
                return;
            }
        }
        
        // Default color
        reticleImage.color = defaultColor;
    }
}
```

---

### Step 7.3: Setup Reticle

1. Select Crosshair image
2. Add Component → **ReticleController**
3. Drag Crosshair to "Reticle Image"
4. Set Detect Layers to "Everything"

**✅ Test:**
- Enter Play mode
- Look at nothing → crosshair white
- Aim at enemy cube → crosshair turns red

---

## 📦 CHECKPOINT: Working Prototype (End of Day 4)

At this point you have:
- ✅ Player movement (SYNTY)
- ✅ Player can take damage and die
- ✅ Health bar UI
- ✅ Shooting system works
- ✅ Enemies take damage and die
- ✅ Crosshair changes color

This is a **working game**! Everything from here is building on this foundation.

---

## 🏪 PHASE 8: Inventory & Ammo System (Day 5 - 2 hours)

Now we add the full inventory system with ammo management.

**[Instructions continue...]**

**Want me to continue with:**
- Phase 8: Full Inventory System
- Phase 9: Enemy AI with NavMesh
- Phase 10: Pickup System
- Phase 11: Wave Manager

Or do you want to start implementing Phases 0-7 first and test?
