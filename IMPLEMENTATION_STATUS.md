# Implementation Status & Architecture Study Guide
## 3rd Person Zombie Shooter -- Full Project Reference

---

## 1. Complete Script Inventory

All scripts live under `Assets/_ProjectFiles/Scripts/` unless marked (Synty).

### Data Layer (9 files)

| File | Path | Purpose |
|------|------|---------|
| `AmmoType.cs` | `Data/` | Enum: Rifle, Pistol, Shotgun, Minigun, RPG, Grenade |
| `IPickupable.cs` | `Data/` | Interface for collectible items |
| `PickupItem.cs` | `Data/` | Modular pickup handler -- rotation, bob, detection zone, collection queue |
| `WeaponData.cs` | `Data/Weapons/` | ScriptableObject: damage, fireRate, magazineSize, reloadTime, ammoType, bulletPrefab, sounds |
| `PickupData_Modular.cs` | `Data/Pickup/` | Abstract base MonoBehaviour for all pickup types |
| `AmmoPickupData.cs` | `Data/Pickup/Ammo/` | Ammo pickup -- calls `Inventory.AddAmmo()` |
| `HealthPickupData.cs` | `Data/Pickup/Health/` | Health pickup -- calls `PlayerHealth.Heal()` |
| `WeaponPickupData.cs` | `Data/Pickup/Weapon/` | Weapon pickup -- calls `Inventory.AddWeapon()` + `AddAmmo()` |

### Damage Layer (6 files)

| File | Path | Purpose |
|------|------|---------|
| `IDamageable.cs` | `Damage/` | Interface: `TakeDamage(float)` |
| `PlayerHealth.cs` | `Damage/` | Events: OnHealthChanged, OnPlayerDeath, OnPlayerRespawn. Methods: Heal(), TakeDamage(), Respawn() |
| `EnemyHealth.cs` | `Damage/` | Events: OnEnemyDeath (static), OnHealthChanged (instance). Death animation (fall-over rotation) |
| `PlayerHealthUI.cs` | `Damage/` | Health slider, color gradient (green-yellow-red), damage flash |
| `EnemyHealthbar.cs` | `Damage/` | World-space billboard healthbar, color states |
| `HitmarkerDisplay.cs` | `Damage/` (also `UI/`) | Singleton. `ShowHitmarker()`, fade animation |

### Player Layer (3 files)

| File | Path | Purpose |
|------|------|---------|
| `Inventory.cs` | `Player/` | 7-slot weapons, dual ammo (magazine + backpack), reload coroutine, **POINTS SYSTEM** (AddPoints, SpendPoints, OnPointsChanged, OnPointsGained, EnemyDeath listener) |
| `WeaponController.cs` | `Player/` | Aiming (FOV zoom), shooting (fire-rate, continuous fire), reloading, weapon switching (scroll wheel) |
| `Bullet.cs` | `Player/` | Rigidbody projectile, shooter exclusion, IDamageable damage, hitmarker integration |

### Enemies Layer (3 files)

| File | Path | Purpose |
|------|------|---------|
| `EnemyData.cs` | `Enemies/` | ScriptableObject: maxHealth, attackDamage, moveSpeed, pointsOnDeath, dropChance |
| `EnemyAI.cs` | `Enemies/` | NavMesh chase, proximity attack, idle sounds |
| `RandomDrop.cs` | `Enemies/` | Drops health/ammo on enemy death, matches ammo type to player's owned weapons |

### NPC Layer (4 files)

| File | Path | Purpose |
|------|------|---------|
| `NPCBase.cs` | `NPC/` | Abstract IInteractable base. Indicator toggle, interactor caching, message helpers |
| `TutorialNPC.cs` | `NPC/` | Cycles through gameplay tips on each interaction |
| `ShopkeeperNPC.cs` | `NPC/` | Sells weapons for points. Cycles through unowned offerings, grants bonus ammo on purchase |
| `MedicNPC.cs` | `NPC/` | Heals player for points. Subscribes to PlayerHealth.OnHealthChanged to track if healing is needed |

### UI Layer (5 files)

| File | Path | Purpose |
|------|------|---------|
| `AmmoUI.cs` | `UI/` | "backpack / magazine" display, color-coded (white/yellow/red/gray) |
| `PointsUI.cs` | `UI/` | Points display with yellow flash on gain. Subscribes to Inventory.OnPointsChanged/OnPointsGained |
| `ReloadPromptUI.cs` | `UI/` | "Press R to reload" prompt when magazine empty + countdown timer during reload |
| `GameOverScreen.cs` | `UI/` | Game over panel, Time.timeScale=0, restart button, scene reload |
| `HitmarkerDisplay.cs` | `UI/` | (Same singleton as in Damage/) |

### Managers Layer (2 files)

| File | Path | Purpose |
|------|------|---------|
| `PauseManager.cs` | `Managers/` | Subscribes to InputReader.onPausePerformed, toggles pause panel, Time.timeScale, cursor lock |
| `WaveManager.cs` | `Managers/` | **NOT YET IMPLEMENTED** -- planned: spawns enemies in configurable waves, tracks alive enemies, auto-starts next wave |

### Interaction Layer (3 files)

| File | Path | Purpose |
|------|------|---------|
| `IInteractable.cs` | `Interaction/` | Interface: OnInteract, OnEndInteract, OnReadyInteract, OnAbortInteract |
| `Interactor.cs` | `Interaction/` | Raycasts for "Interactable" tag, uses New Input System |
| `TextSign.cs` | `Interaction/` | Simple text-based interactable (signs, notes) |

### Synty (external -- not in _ProjectFiles)

| File | Path | Purpose |
|------|------|---------|
| `InputReader.cs` | `Synty/.../InputSystem/` | All input events: shoot, aim, reload, interact, pause, scroll, etc. |
| `Controls.cs` | `Synty/.../InputSystem/` | Auto-generated input action bindings |
| `SamplePlayerAnimationController.cs` | `Synty/.../` | State machine, gait blending, aim speed |

### Script Count Summary

| Category | Count |
|----------|-------|
| Data | 9 |
| Damage | 6 |
| Player | 3 |
| Enemies | 3 |
| NPC | 4 |
| UI | 5 |
| Managers | 1 implemented, 1 planned |
| Interaction | 3 |
| Synty (external) | 3 |
| **Total** | **37 files (36 implemented)** |

---

## 2. Complete Event Map

Every event in the project, who fires it, and who listens.

### Static Events

| Event | Signature | Fired By | Subscribers |
|-------|-----------|----------|-------------|
| `Inventory.OnWeaponEquipped` | `Action<WeaponData>` | `Inventory.EquipWeapon()` | WeaponController, AmmoUI, ReloadPromptUI |
| `Inventory.OnAmmoChanged` | `Action<AmmoType, int, int>` | `Inventory.BroadcastAmmoChanged()` | AmmoUI, ReloadPromptUI |
| `Inventory.OnReloadStarted` | `Action` | `Inventory.ReloadCoroutine()` | AmmoUI, ReloadPromptUI |
| `Inventory.OnReloadCompleted` | `Action` | `Inventory.ReloadCoroutine()` | AmmoUI, ReloadPromptUI |
| `Inventory.OnPointsChanged` | `Action<int>` | `Inventory.AddPoints()`, `SpendPoints()` | PointsUI |
| `Inventory.OnPointsGained` | `Action<int, int>` | `Inventory.AddPoints()` | PointsUI |
| `PlayerHealth.OnHealthChanged` | `Action<float, float>` | `PlayerHealth.TakeDamage()`, `Heal()`, `Respawn()` | PlayerHealthUI, MedicNPC |
| `PlayerHealth.OnPlayerDeath` | `Action` | `PlayerHealth.Die()` | GameOverScreen |
| `PlayerHealth.OnPlayerRespawn` | `Action` | `PlayerHealth.Respawn()` | GameOverScreen |
| `EnemyHealth.OnEnemyDeath` | `Action<EnemyHealth>` | `EnemyHealth.Die()` | Inventory (points), RandomDrop (loot) |

### Instance Events

| Event | Signature | Fired By | Subscribers |
|-------|-----------|----------|-------------|
| `EnemyHealth.OnHealthChanged` | `Action<float, float>` | `EnemyHealth.TakeDamage()` | EnemyHealthbar (on same GameObject) |

### InputReader Events (Synty)

| Event | Triggered By | Consumed By |
|-------|-------------|-------------|
| `onAimActivated` | Right Mouse press | WeaponController |
| `onAimDeactivated` | Right Mouse release | WeaponController |
| `onShootPerformed` | Left Mouse click | WeaponController |
| `onShootStarted` | Left Mouse press | WeaponController (continuous fire) |
| `onShootCanceled` | Left Mouse release | WeaponController (continuous fire) |
| `onReloadPerformed` | R key | WeaponController |
| `onWeaponScrollPerformed` | Scroll wheel | WeaponController |
| `onInteractPerformed` | E key | Interactor |
| `onPausePerformed` | Escape key | PauseManager |

---

## 3. Event Flow Diagrams

### Enemy Kill -- Full Chain

```
Bullet.OnTriggerEnter(enemy collider)
    |
    v
IDamageable target = GetComponent / GetComponentInParent
    |
    v
EnemyHealth.TakeDamage(damage)
    |-- currentHealth -= damage
    |-- OnHealthChanged?.Invoke(current, max)
    |       |
    |       v
    |   EnemyHealthbar updates slider + color
    |
    |-- if (health <= 0) --> Die()
            |
            |-- IsDead = true
            |-- EnemyHealth.OnEnemyDeath?.Invoke(this)  <-- STATIC EVENT
            |       |
            |       +-------------------------------+
            |       |                               |
            |       v                               v
            |   Inventory.HandleEnemyDeath()    RandomDrop.HandleEnemyDeath()
            |       |                               |
            |       v                               v
            |   AddPoints(enemyData.pointsOnDeath)  Roll against dropChance
            |       |                               |
            |       +-- OnPointsGained?.Invoke()    +-- 50/50: health or ammo
            |       |       |                       |-- SpawnHealthDrop() or
            |       |       v                       |-- SpawnAmmoDrop()
            |       |   PointsUI flash yellow       |   (matches player's owned weapons)
            |       |
            |       +-- OnPointsChanged?.Invoke()
            |               |
            |               v
            |           PointsUI updates display
            |
            |-- Disable EnemyAI + NavMeshAgent
            |-- Disable Collider (optional)
            |-- Fall-over animation (-90 deg X rotation)
            |-- Play death sound + particle effect
            |-- Destroy(gameObject, corpseLifetime)
```

### Shooting Flow

```
Player holds Right Mouse
    |
    v
InputReader.onAimActivated
    |
    v
WeaponController.isAiming = true
    |
    v
Camera FOV lerps 60 --> 40 (smooth zoom)
    |
    v
Player presses Left Mouse
    |
    v
InputReader.onShootPerformed
    |
    v
WeaponController.TryFire()
    |-- Check: isAiming?           yes
    |-- Check: weapon equipped?    yes
    |-- Check: not reloading?      yes
    |-- Check: fire-rate cooldown? yes
    |-- inventory.ConsumeAmmo()    --> magazineAmmo[weapon] -= 1
    |       |
    |       v
    |   Inventory.BroadcastAmmoChanged()
    |       |
    |       v
    |   OnAmmoChanged?.Invoke(type, backpack, magazine)
    |       |
    |       +-- AmmoUI updates "59 / 11"
    |       +-- ReloadPromptUI checks if magazine == 0
    |
    v
ExecuteFire()
    |-- GetAimDirection() --> raycast from camera viewport center
    |-- SpawnBullet() --> Instantiate bulletPrefab at ShootPoint
    |       |
    |       v
    |   bullet.Initialize(damage, direction)
    |   bullet.SetShooter(player)
    |       |
    |       v
    |   Bullet: Rigidbody.velocity = direction * speed
    |       |
    |       v
    |   OnTriggerEnter(collider)
    |       |-- skip triggers, skip shooter
    |       |-- IDamageable search (self then parent)
    |       |-- target.TakeDamage(damage)
    |       |-- HitmarkerDisplay.Instance?.ShowHitmarker()
    |       |-- Destroy bullet
    |
    |-- PlayShootSound()
    |-- SpawnMuzzleFlash() --> destroy after 0.1s
```

### Reload Flow

```
Player presses R
    |
    v
InputReader.onReloadPerformed
    |
    v
WeaponController.HandleReloadPerformed()
    |-- Validates: not reloading, mag not full, has backpack ammo
    |-- Releases aim if aiming
    |-- Plays reloadSound
    |-- inventory.StartReload()
            |
            v
        Inventory.ReloadCoroutine()
            |-- isReloading = true
            |-- OnReloadStarted?.Invoke()
            |       |
            |       +-- AmmoUI: shows "RELOADING..." (gray)
            |       +-- ReloadPromptUI: starts countdown "2.0s" "1.9s" ...
            |
            |-- yield WaitForSeconds(reloadTime)
            |
            |-- Transfer: min(needed, backpack) --> magazine
            |-- isReloading = false
            |-- OnReloadCompleted?.Invoke()
            |       |
            |       +-- AmmoUI: restores normal display
            |       +-- ReloadPromptUI: hides text
            |
            |-- BroadcastAmmoChanged()
                    |
                    v
                AmmoUI shows "48 / 12" (white)
```

### Weapon Switch Flow

```
Player scrolls mouse wheel
    |
    v
InputReader.onWeaponScrollPerformed(delta)
    |
    v
WeaponController.HandleWeaponScroll(delta)
    |-- Cancel active reload
    |-- Reset fire cooldown
    |-- delta > 0: inventory.EquipNextWeapon()
    |-- delta < 0: inventory.EquipPreviousWeapon()
            |
            v
        FindNextOccupiedSlot() --> skip empty slots, wrap around
            |
            v
        Inventory.EquipWeapon(newSlotIndex)
            |-- CancelReload()
            |-- equippedWeapon = weaponSlots[index]
            |-- OnWeaponEquipped?.Invoke(weaponData)
            |       |
            |       +-- WeaponController: cache WeaponData, find ShootPoint
            |       +-- AmmoUI: cache magazine size, refresh display
            |       +-- ReloadPromptUI: cache reload time, reset state
            |
            |-- BroadcastAmmoChanged()
                    |
                    v
                AmmoUI shows new weapon's ammo counts
```

### Points Economy Flow

```
EARNING POINTS:
    EnemyHealth.Die()
        |
        v
    EnemyHealth.OnEnemyDeath?.Invoke(this)
        |
        v
    Inventory.HandleEnemyDeath(enemyHealth)
        |-- Get EnemyData.pointsOnDeath (default: 100)
        |-- AddPoints(amount)
                |-- currentPoints += amount
                |-- OnPointsGained?.Invoke(amount, total) --> PointsUI flash
                |-- OnPointsChanged?.Invoke(total) ---------> PointsUI update

SPENDING POINTS:
    ShopkeeperNPC.AttemptPurchase(offering)
        |-- inventory.SpendPoints(cost) --> returns false if insufficient
        |       |
        |       v (on success)
        |   currentPoints -= cost
        |   OnPointsChanged?.Invoke(total) --> PointsUI update
        |
        |-- inventory.AddWeapon(weapon)
        |-- inventory.AddAmmo(ammoType, bonusAmmo)

    MedicNPC.PerformInteraction()
        |-- inventory.SpendPoints(healCost) --> returns false if insufficient
        |-- playerHealth.Heal(healAmount)
```

### NPC Interaction Flow

```
Player looks at NPC (tagged "Interactable")
    |
    v
Interactor raycast hits NPC
    |-- IInteractable.OnReadyInteract() --> indicator activates
    |
    v
Player presses E
    |
    v
InputReader.onInteractPerformed
    |
    v
Interactor calls IInteractable.OnInteract(this)
    |
    v
NPCBase.OnInteract(interactor)
    |-- Hide indicator
    |-- Cache interactor reference
    |-- GetInteractionMessage() --> subclass returns dialog text
    |-- interactor.ReceiveInteract(message) --> display on HUD
    |-- PerformInteraction(interactor) --> subclass performs action
    |
    +-- TutorialNPC: cycles tips array
    +-- ShopkeeperNPC: finds unowned weapon, attempts purchase
    +-- MedicNPC: checks health < max, charges points, heals
```

### Game Over / Pause Flow

```
PlayerHealth.currentHealth <= 0
    |
    v
PlayerHealth.Die()
    |-- IsDead = true
    |-- OnPlayerDeath?.Invoke()
            |
            v
        GameOverScreen.HandlePlayerDeath()
            |-- isGameOver = true
            |-- IsGameOver = true (static)
            |-- Time.timeScale = 0
            |-- Show gameOverPanel
            |-- Unlock cursor

        [Restart Button clicked]
            |
            v
        GameOverScreen.HandleRestart()
            |-- Time.timeScale = 1
            |-- SceneManager.LoadScene(current)

---

Player presses Escape
    |
    v
InputReader.onPausePerformed
    |
    v
PauseManager.HandlePause()
    |-- if GameOverScreen.IsGameOver --> ignore
    |-- Toggle isPaused
    |
    +-- Pause():  timeScale=0, show panel, unlock cursor
    +-- Resume(): timeScale=1, hide panel, lock cursor
```

---

## 4. Key System Deep-Dives

### 4.1 Inventory -- Dual Ammo System

```
MAGAZINE AMMO (per weapon)             BACKPACK AMMO (per AmmoType)
+----------------------------+         +----------------------------+
| Dictionary<WeaponData,int> |         | Dictionary<AmmoType, int>  |
|                            |         |                            |
| Pistol.asset  --> 12       |         | Pistol   --> 60            |
| Rifle.asset   --> 30       |         | Rifle    --> 90            |
| Shotgun.asset --> 6        |         | Shotgun  --> 24            |
+----------------------------+         +----------------------------+
       |                                       |
       |  ConsumeAmmo() subtracts 1            |  AddAmmo() adds to pool
       |  from magazine                        |  (clamped to maxBackpackAmmo)
       |                                       |
       +----------- RELOAD ------------------>+
         Transfer min(needed, backpack) from backpack to magazine
         after reloadTime seconds (coroutine)
```

**Display on HUD:** `backpack / magazine` (e.g., "60 / 12")

### 4.2 Inventory -- Points System

The points system is built directly into `Inventory.cs`:

| Method | Behavior |
|--------|----------|
| `AddPoints(int)` | Increases balance. Fires OnPointsGained and OnPointsChanged |
| `SpendPoints(int)` | Returns false if insufficient. Deducts and fires OnPointsChanged |
| `CurrentPoints` | Read-only property for current balance |

Auto-earning: Inventory subscribes to `EnemyHealth.OnEnemyDeath` in OnEnable. Each kill grants `EnemyData.pointsOnDeath` (fallback: 100 points).

### 4.3 Weapon Slots

```
Slot Index:   [0]      [1]      [2]     [3]     [4]     [5]     [6]
Key Mapping:   1        2        3       4       5       6       7
Example:     Pistol   Rifle   Shotgun  null    null    null    null
              ^
              |
         equippedSlotIndex = 0
```

- Scroll wheel skips null slots and wraps around
- Adding a weapon fills the first empty (null) slot
- Magazine starts full when a weapon is added

### 4.4 RandomDrop -- Loot System

```
Enemy dies --> EnemyHealth.OnEnemyDeath
                    |
                    v
              RandomDrop.HandleEnemyDeath()
                    |
                    v
              Roll: Random.value > enemyData.dropChance? --> no drop
                    |
                    v (drop passes)
              Roll: Random.value < healthDropChanceWeight (0.5)?
                   /                    \
                  v                      v
            Health Drop             Ammo Drop
            (spawn prefab)          |
                                    v
                              SelectAmmoConfig()
                                |-- Check player inventory
                                |-- Filter to owned weapon types
                                |-- Random pick from valid configs
                                |-- Fallback: any config with prefab
                                |-- Fallback: health drop instead
```

---

## 5. AmmoUI Color States

| State | Color | Condition |
|-------|-------|-----------|
| Normal | White | Magazine > 25% capacity |
| Low Ammo | Yellow | Magazine <= 25% and > 0 |
| Empty | Red | Magazine = 0 rounds |
| Reloading | Gray | During reload, shows "RELOADING..." |

---

## 6. Controls Quick Reference

| Action | Input | Condition |
|--------|-------|-----------|
| Move | WASD | Always |
| Look | Mouse | Always |
| Jump | Space | Always |
| Sprint | Shift (hold) | Always |
| Crouch | Ctrl | Always |
| Aim | Right Mouse (hold) | Always |
| Shoot | Left Mouse | Must be aiming |
| Reload | R | Not aiming, magazine not full, has backpack ammo |
| Switch Weapon | Scroll Wheel | Has multiple weapons owned |
| Interact | E | Looking at tagged "Interactable" object |
| Pause | Escape | Game not over |

---

## 7. Unity Editor Setup Checklist

### Player GameObject

- [ ] Has `InputReader` component (Synty -- should already exist)
- [ ] Has `SamplePlayerAnimationController` (Synty -- should already exist)
- [ ] Has `PlayerHealth` component
- [ ] Has `Inventory` component
- [ ] Has `WeaponController` component
- [ ] Has `Interactor` component (or on a child)
- [ ] Configure Inventory: Starting Weapon = Pistol.asset
- [ ] Configure Inventory: Weapon Slots[0] = Pistol.asset
- [ ] Configure Inventory: Initial Ammo Types = [Pistol], Amounts = [60]
- [ ] Configure Inventory: Max Ammo per type as needed

### Bullet Prefab

- [ ] Create Sphere, scale (0.05, 0.05, 0.1)
- [ ] Rigidbody: Use Gravity = false, Collision Detection = Continuous Dynamic
- [ ] SphereCollider: Is Trigger = true
- [ ] Add Bullet.cs: Speed = 100, Lifetime = 3
- [ ] Save as prefab, assign to each WeaponData.bulletPrefab

### ShootPoint Children

- [ ] On each weapon model attached to the player character
- [ ] Create empty child GameObject named to match WeaponData.shootPointName
- [ ] Position at barrel tip

### WeaponData ScriptableObjects

- [ ] Create via Assets > Create > ScriptableObject > WeaponData
- [ ] Configure: weaponName, damage, fireRate, magazineSize, reloadTime, range
- [ ] Set ammoType enum value
- [ ] Assign bulletPrefab, shootSound, reloadSound
- [ ] Set shootPointName to match the child transform name

### NPC GameObjects

- [ ] Tag each NPC as "Interactable"
- [ ] Add indicator child (e.g., UI icon or floating marker)
- [ ] Assign indicator reference in NPCBase Inspector
- [ ] TutorialNPC: populate tips array
- [ ] ShopkeeperNPC: populate weaponOfferings (weapon, cost, description)
- [ ] MedicNPC: set healCost and healAmount

### HUD Canvas Elements

- [ ] **AmmoUI:** TextMeshPro text, anchor bottom-right, add AmmoUI.cs, assign TMP reference
- [ ] **PointsUI:** TextMeshPro text, add PointsUI.cs, assign TMP reference
- [ ] **ReloadPromptUI:** TextMeshPro text, add ReloadPromptUI.cs, assign TMP reference
- [ ] **GameOverScreen:** Full-screen panel (disabled by default), add GameOverScreen.cs, assign panel + restart button
- [ ] **HitmarkerDisplay:** UI Image (disabled or alpha 0), add HitmarkerDisplay.cs
- [ ] **Pause Panel:** UI Panel (disabled by default)

### Managers

- [ ] **PauseManager:** attach to player or manager object, assign pause panel, wire Resume button
- [ ] **RandomDrop:** attach to manager object, assign health prefab, configure ammo drop configs (AmmoType + weapon + prefab per entry)
- [ ] **WaveManager:** (not yet implemented -- future task: create with spawn points and wave configs)

### Enemy Prefabs

- [ ] EnemyData ScriptableObject assigned
- [ ] EnemyHealth component (assign EnemyData)
- [ ] EnemyAI component
- [ ] NavMeshAgent component
- [ ] Collider (NOT a trigger -- bullets need solid collider)
- [ ] EnemyHealthbar on a child Canvas (world-space, billboard)

---

## 8. Console Log Reference

All debug messages use prefixed tags for filtering:

| Prefix | Source | Example Messages |
|--------|--------|-----------------|
| `[Inventory]` | Inventory.cs | "Starting ammo: Pistol = 60", "Equipped weapon: Pistol", "Reloading Pistol (2s)", "Reload completed for Pistol. Magazine: 12/12", "+100 points (Total: 300)", "Spent 500 pts. Remaining: 200" |
| `[WeaponController]` | WeaponController.cs | "Initialized. Normal FOV=60, Aim FOV=40", "Aim started.", "Firing Pistol", "Equipped weapon: Uzi", "Auto-reloading Pistol (magazine empty)." |
| `[Bullet]` | Bullet.cs | "Hit SimpleZombie1, dealt 10 damage" |
| `[RandomDrop]` | RandomDrop.cs | "Spawned health drop at (x,y,z)", "Spawned Pistol ammo drop at (x,y,z)" |
| `[PauseManager]` | PauseManager.cs | "Game paused.", "Game resumed." |
| `[GameOverScreen]` | GameOverScreen.cs | "Game Over.", "Restarting scene." |
| `[NPC]` | NPCBase.cs | "Shopkeeper interaction started." |
| `[ShopkeeperNPC]` | ShopkeeperNPC.cs | "Sold Rifle for 500 pts. Bonus ammo: +60 Rifle." |
| `[MedicNPC]` | MedicNPC.cs | "Healed player for 50 HP. Charged 50 points." |
| `[PointsUI]` | PointsUI.cs | "Inventory not found in scene at Start." |
| `[ReloadPromptUI]` | ReloadPromptUI.cs | "Weapon equipped: reload time 2s" |
| (no prefix) | PlayerHealth.cs | "Player took 10 damage. Health: 90/100", "Player died!" |
| (no prefix) | EnemyHealth.cs | "SimpleZombie1 took 10 damage. Health: 40/50", "SimpleZombie1 died!" |

---

## 9. Troubleshooting

### "Weapon won't fire"
1. Are you holding Right Mouse (must aim before shooting)?
2. Is `InputReader` on the Player? WeaponController needs it.
3. Is `Inventory` on the Player? WeaponController requires it.
4. Does Inventory have a weapon in slot 0?
5. Does magazine have ammo? Check `[Inventory]` logs.

### "No damage to enemies"
1. Does enemy have `EnemyHealth` component?
2. Is `Bullet.cs` on the bullet prefab?
3. Is bullet's collider set to Is Trigger = true?
4. Is enemy's collider NOT a trigger? (Bullet's OnTriggerEnter needs a solid collider on the target.)
5. Check Layer Collision Matrix in Physics settings.

### "Bullets not spawning"
1. Is `bulletPrefab` assigned in the WeaponData asset?
2. Does weapon model have a child matching `shootPointName`?
3. Check Console for "ShootPoint NOT found" warning.

### "Points not showing / NPC says not enough"
1. Is `PointsUI` component on a TMP text object?
2. Is the TMP reference assigned in Inspector?
3. Is `Inventory` on the Player? Points live in Inventory.
4. Are enemies set up with `EnemyData` that has `pointsOnDeath > 0`?

### "NPC not interactable"
1. Is the NPC tagged "Interactable"?
2. Does the player have an `Interactor` component?
3. Is the indicator child assigned in the NPC's Inspector?
4. Is the player within raycast range?

### "Pause not working"
1. Does `PauseManager` have a reference to `InputReader`? It searches parent then scene.
2. Is the pause panel assigned and disabled by default?
3. Is `GameOverScreen.IsGameOver` false? Pause is blocked during game over.

### "Game over screen not appearing"
1. Is `GameOverScreen` subscribed to `PlayerHealth.OnPlayerDeath`?
2. Is the game over panel assigned and disabled by default?
3. Is `PlayerHealth` on the Player?

---

## 10. Architecture Layer Diagram

```
+------------------------------------------------------------------+
|                        INPUT LAYER (Synty)                       |
|  InputReader.cs -- routes all input to C# events                 |
|  Controls.cs -- auto-generated bindings                          |
+------------------------------------------------------------------+
        |               |              |            |          |
        v               v              v            v          v
   onAimActivated  onShootPerformed  onReload  onInteract  onPause
   onAimDeactivated onShootStarted             onScroll
                    onShootCanceled
        |               |              |            |          |
        v               v              v            v          v
+------------------------------------------------------------------+
|                      CONTROLLER LAYER                            |
|  WeaponController.cs -- aim, shoot, reload, switch               |
|  Interactor.cs -- raycast + interact dispatch                    |
|  PauseManager.cs -- pause toggle                                 |
+------------------------------------------------------------------+
        |                              |            |
        v                              v            v
+------------------------------------------------------------------+
|                       CORE SYSTEMS                               |
|  Inventory.cs -- weapons, ammo, points, reload coroutine         |
|  PlayerHealth.cs -- health, damage, death, heal                  |
|  EnemyHealth.cs -- health, damage, death events                  |
|  EnemyAI.cs -- NavMesh chase + attack                            |
|  Bullet.cs -- projectile + IDamageable dispatch                  |
+------------------------------------------------------------------+
        |               |              |
        v               v              v
+------------------------------------------------------------------+
|                        DATA LAYER                                |
|  WeaponData (SO)    EnemyData (SO)    AmmoType (enum)            |
|  IDamageable        IInteractable     IPickupable                |
|  PickupData_Modular + Ammo/Health/Weapon variants                |
+------------------------------------------------------------------+
        |
        v
+------------------------------------------------------------------+
|                      GAME SYSTEMS                                |
|  NPCBase + TutorialNPC / ShopkeeperNPC / MedicNPC               |
|  PickupItem.cs -- modular pickup handler                         |
|  RandomDrop.cs -- loot drops on enemy death                      |
|  GameOverScreen.cs -- death screen + restart                     |
|  WaveManager.cs -- (NOT YET IMPLEMENTED)                         |
+------------------------------------------------------------------+
        |
        v
+------------------------------------------------------------------+
|                         UI LAYER                                 |
|  AmmoUI -- backpack/magazine text, color states                  |
|  PointsUI -- points display with flash                           |
|  ReloadPromptUI -- "Press R" + countdown timer                   |
|  PlayerHealthUI -- slider + gradient + damage flash              |
|  EnemyHealthbar -- world-space billboard                         |
|  HitmarkerDisplay -- singleton fade                              |
|  GameOverScreen -- death panel + restart                         |
+------------------------------------------------------------------+
```

---

## 11. What Remains to Be Built

| System | Status | Notes |
|--------|--------|-------|
| WaveManager.cs | NOT IMPLEMENTED | Planned: configurable waves, spawn points, alive enemy tracking, auto-start next wave |
| All other systems | COMPLETE | 36 scripts fully implemented and wired |

---

*This document reflects the project state as of February 2026. For Stavros to study.*
