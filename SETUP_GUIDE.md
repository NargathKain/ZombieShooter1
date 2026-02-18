# Unity Editor Setup Guide -- Zombie Shooter

**Project:** Unity 6000.2.8f1 with URP
**Scripts:** `Assets/_ProjectFiles/Scripts/`
**Third-party:** Synty AnimationBaseLocomotion (provides player controller + InputReader)

All code is complete. This guide covers everything you need to do in the Unity Editor to make the game playable.

---

## Table of Contents

1. [Quick Start (Minimum Viable Game)](#1-quick-start-minimum-viable-game)
2. [Scene Setup](#2-scene-setup)
3. [Player Setup](#3-player-setup)
4. [Weapon Data Assets](#4-weapon-data-assets)
5. [Bullet Prefab](#5-bullet-prefab)
6. [Enemy Setup](#6-enemy-setup)
7. [Pickup Prefabs](#7-pickup-prefabs)
8. [Manager GameObjects](#8-manager-gameobjects)
9. [UI Canvas Setup](#9-ui-canvas-setup)
10. [NPC Setup (Optional)](#10-npc-setup-optional)
11. [NavMesh Baking](#11-navmesh-baking)
12. [Testing Checklist](#12-testing-checklist)

---

## 1. Quick Start (Minimum Viable Game)

Fastest path to a playable prototype. Do these steps in order, skip nothing.

1. Open or create a scene with a ground plane and the Synty player prefab
2. Tag the player GameObject **"Player"**
3. Tag the camera **"MainCamera"** (usually already done)
4. Add components to the player: **PlayerHealth**, **Inventory**, **WeaponController**, **Interactor**
5. Create at least one **WeaponData** asset (or use `Assets/_ProjectFiles/Scripts/Data/Weapons/Pistol.asset`)
6. Create a **Bullet prefab** (small sphere, Rigidbody with no gravity, trigger Collider, Bullet script)
7. Assign the bullet prefab to the WeaponData's **bulletPrefab** field
8. On the player's **Inventory**: set **startingWeapon** to the Pistol asset, put it in **weaponSlots** Element 0, set up **initialAmmoTypes** and **initialAmmoAmounts** (e.g., Pistol / 100)
9. Create an **EnemyData** asset (right-click > Create > Game/Enemy Data), set the **enemyPrefab**
10. Create an enemy prefab with: model, **NavMeshAgent**, **EnemyHealth** (assign EnemyData), **EnemyAI** (assign same EnemyData), and a Collider
11. Bake the **NavMesh** (Window > AI > Navigation > Bake)
12. Create an empty GameObject named "WaveManager", add the **WaveManager** script, add spawn points, configure at least one wave with the EnemyData + count
13. Create a basic UI Canvas with at least **AmmoUI** and **PlayerHealthUI**
14. Hit Play

Everything below expands on these steps with full detail.

---

## 2. Scene Setup

### 2.1 Ground and Environment

1. Create a ground plane or terrain. Scale it large enough for gameplay (e.g., Plane scaled 10x10)
2. Add any environment props you want (walls, buildings, etc.)
3. All walkable surfaces must be marked **Navigation Static** in the Inspector (checkbox at top-right of Inspector, or via the Static dropdown)

### 2.2 Camera

1. Your main camera must be tagged **"MainCamera"**
   - Select the camera > Inspector > Tag dropdown > **MainCamera**
   - WeaponController uses `Camera.main` -- if this tag is missing, shooting will not work
2. If using Synty's third-person camera rig, the camera is typically already tagged correctly

### 2.3 Lighting

1. Standard URP setup. Add a Directional Light if one does not exist
2. No custom lighting scripts are needed

---

## 3. Player Setup

Start with the Synty AnimationBaseLocomotion player prefab already in the scene. It should already have an **InputReader** component.

### 3.1 Tag the Player

1. Select the player root GameObject
2. In the Inspector, set Tag to **"Player"**
   - If the "Player" tag does not exist, go to the Tag dropdown > Add Tag > create "Player"
   - Enemies use `FindGameObjectWithTag("Player")` to locate the player. If this tag is missing, AI will not work

### 3.2 Add PlayerHealth

1. Select the player root GameObject
2. Add Component > search **PlayerHealth**
3. Inspector fields:
   - **maxHealth**: `100` (default is fine)
   - **debugDamageAmount**: `10` (used for testing with debug key)

### 3.3 Add Inventory

1. Select the player root GameObject
2. Add Component > search **Inventory**
3. Inspector fields:

   **Starting Configuration:**
   - **startingWeapon**: Drag a WeaponData asset here (e.g., `Pistol.asset`)
   - **weaponSlots**: Array of 7 slots. Put the Pistol in Element 0. Leave the rest empty (or fill in as desired). The array is automatically resized to 7 in the editor.

   **Starting Ammo (IMPORTANT: arrays must match length):**
   - **initialAmmoTypes**: Set the size to match how many ammo types you want to give at start. Example for 2 entries:
     - Element 0: `Pistol`
     - Element 1: `Rifle`
   - **initialAmmoAmounts**: Same size as above.
     - Element 0: `100`
     - Element 1: `60`

   **Ammo Limits (IMPORTANT: arrays must match length):**
   - **maxAmmoTypes**: Set the size. Example:
     - Element 0: `Pistol`
     - Element 1: `Rifle`
   - **maxAmmoAmounts**: Same size as above.
     - Element 0: `300`
     - Element 1: `200`
   - **defaultMaxAmmo**: `999` (fallback for any ammo type not listed above)

   > **Warning:** If `initialAmmoTypes` and `initialAmmoAmounts` have different lengths, only the minimum length is used and a warning is logged. Same for the max arrays. Always keep them matched.

### 3.4 Add WeaponController

1. Select the player root GameObject
2. Add Component > search **WeaponController**
   - This will auto-add Inventory if not already present (via `[RequireComponent]`)
3. Inspector fields:
   - **aimFOV**: `40` (camera FOV when holding right-click to aim)
   - **zoomSpeed**: `10` (how fast the FOV lerps)
   - **hitLayers**: Set to **Everything** or configure to include `Default`, `Enemy`, and any custom layers. Exclude `Player` and `Ignore Raycast` to prevent self-hits
   - **shootVolume**: `1` (0 to 1 range)
   - **reloadVolume**: `0.8` (0 to 1 range)
4. Requirements:
   - **Inventory** must be on the same GameObject (auto-added)
   - **InputReader** must be on the same GameObject (from Synty)
   - **Camera.main** must exist (camera tagged "MainCamera")

### 3.5 Add Interactor

1. Select the player root GameObject (or the camera child if you want raycasts from the camera)
2. Add Component > search **Interactor**
3. Inspector fields:
   - **targetTag**: `Interactable` (default)
   - **rayMaxDistance**: `20` (how far the interact raycast reaches)
   - **layerMask**: Set to **Everything** minus Player and Ignore Raycast
   - **interactorUI**: Drag the InteractorUI component from the UI Canvas here (set up in Section 9)
   - **hint**: Drag the "Press E" hint UI GameObject here (a UI element that gets toggled on/off)
   - **cancelInteractionKeys**: Add KeyCodes that cancel an active interaction (e.g., `W`, `A`, `S`, `D`, `Escape`)

### 3.6 Verify InputReader

The Synty **InputReader** component should already be on the player. Verify these input actions are mapped:
- Move, Look, Sprint, Crouch, Jump
- **Aim** (right mouse hold)
- **Shoot** (left mouse hold)
- **Reload** (R key)
- **WeaponScroll** (scroll wheel)
- **Interact** (E key) -- used by `onInteractPerformed`
- **Pause** (Escape key) -- used by `onPausePerformed`

If any are missing, configure them in the Synty Input Actions asset.

---

## 4. Weapon Data Assets

Seven WeaponData assets already exist at:
```
Assets/_ProjectFiles/Scripts/Data/Weapons/
```
- `Pistol.asset`
- `Uzi.asset`
- `AssaultRifle.asset`
- `Shotgun.asset`
- `Mingun.asset` (Minigun)
- `RPG.asset`
- `GrenadeLauncher.asset`

### 4.1 Creating a New WeaponData

1. Right-click in Project window > **Create > Game/Weapon Data**
2. Name it (e.g., "Shotgun")
3. Fill in the fields:

| Field | Description | Example |
|-------|-------------|---------|
| **weaponName** | Display name | "Shotgun" |
| **weaponPrefab** | 3D model prefab for the weapon | (drag weapon model) |
| **damage** | Damage per hit | 25 |
| **fireRate** | Shots per second | 2 |
| **range** | Max shooting distance | 50 |
| **magazineSize** | Rounds per magazine | 8 |
| **reloadTime** | Seconds to reload | 2.5 |
| **ammoType** | Enum: Pistol, Rifle, Shotgun, Minigun, RPG, Grenade | Shotgun |
| **shootPointName** | Name of the child transform on the weapon model that marks the barrel tip | "ShootPoint" |
| **bulletPrefab** | Bullet prefab (see Section 5) | (drag bullet prefab) |
| **shootSound** | AudioClip for firing | (drag audio) |
| **reloadSound** | AudioClip for reloading | (drag audio) |
| **muzzleFlashPrefab** | Particle effect at barrel | (drag prefab, optional) |
| **bulletHolePrefab** | Decal for bullet holes | (drag prefab, optional) |

### 4.2 Weapon Prefab Shoot Point

Each weapon model prefab needs a child empty GameObject named to match the WeaponData's **shootPointName** (default: `"ShootPoint"`).

1. Open the weapon prefab
2. Create an empty child GameObject, name it `ShootPoint`
3. Position it at the tip of the barrel
4. Point its local Z-axis (blue arrow) forward out of the barrel

The WeaponController searches the player's entire hierarchy for this name (case-insensitive) when a weapon is equipped.

---

## 5. Bullet Prefab

Create one bullet prefab (or one per weapon type if you want different visuals).

### 5.1 Create the Prefab

1. Create an empty GameObject in the scene, name it `Bullet`
2. Add a child **Sphere** or **Capsule** mesh. Scale it small: `(0.05, 0.05, 0.05)` to `(0.1, 0.1, 0.1)`
3. On the root `Bullet` GameObject:
   - Add Component > **Rigidbody**
     - **Use Gravity**: `false` (unchecked)
     - **Is Kinematic**: `false` (unchecked)
     - Collision detection is set to ContinuousDynamic automatically by the script
   - Add Component > **Sphere Collider** (or Capsule Collider)
     - **Is Trigger**: `true` (checked)
   - Add Component > search **Bullet** (the script)
4. Optional: Add a **Trail Renderer** for a visual tail effect

### 5.2 Bullet Inspector Fields

- **speed**: `100` (units per second, default)
- **lifetime**: `3` (seconds before self-destruct, default)
- **impactEffectPrefab**: Optional particle effect spawned on hit (drag a prefab or leave empty)
- **impactEffectLifetime**: `2` (how long the impact effect lives)

### 5.3 Save as Prefab

1. Drag the `Bullet` GameObject from the Hierarchy into the Project window to create a prefab
2. Delete the instance from the scene
3. Assign this prefab to WeaponData's **bulletPrefab** field on each weapon that should use it

---

## 6. Enemy Setup

### 6.1 Create EnemyData Asset

An existing asset is at `Assets/_ProjectFiles/Scripts/Enemies/BasicZombie.asset`.

To create a new one:
1. Right-click in Project > **Create > Game/Enemy Data**
2. Name it (e.g., "FastZombie")
3. Fill in the fields:

| Field | Description | Suggested Value |
|-------|-------------|-----------------|
| **enemyName** | Display name | "Basic Zombie" |
| **enemyPrefab** | The enemy prefab (set after creating it below) | (drag prefab) |
| **maxHealth** | Starting health | 50-150 |
| **attackDamage** | Damage per hit to player | 10-25 |
| **attackCooldown** | Seconds between attacks | 1.5 |
| **attackRange** | Distance to attack player | 2 |
| **moveSpeed** | NavMeshAgent speed | 3-6 |
| **detectionRange** | Distance to start chasing | 15-25 |
| **stoppingDistance** | How close before stopping | 1.5 |
| **pointsOnDeath** | Points awarded on kill | 100 |
| **dropChance** | Chance to drop a pickup (0-1) | 0.3 |
| **attackSound** | AudioClip | (optional) |
| **deathSound** | AudioClip | (optional) |
| **deathEffect** | Particle effect on death | (optional) |
| **idleSounds** | Array of idle AudioClips | (optional) |
| **idleSoundInterval** | Seconds between idle sounds | 5 |

### 6.2 Create Enemy Prefab

1. Create a new GameObject (or use a zombie model) in the scene
2. Add a **Collider** (CapsuleCollider works well for humanoids)
   - **Is Trigger**: `false` (solid collider -- bullets detect enemies via trigger enter on the bullet's trigger, hitting the enemy's solid collider)
3. Add Component > **NavMeshAgent**
   - Speed is overridden at runtime from EnemyData, so the Inspector value does not matter
4. Add Component > search **EnemyHealth**
   - **enemyData**: Drag the EnemyData asset here
   - **deathRotationSpeed**: `360` (degrees/sec for fall-over animation)
   - **corpseLifetime**: `2` (seconds before corpse is destroyed)
   - **disableColliderOnDeath**: `true`
   - **sinkIntoGround**: `false` (set `true` if you want corpses to sink)
   - **sinkSpeed**: `0.5`
   - **sinkDelay**: `1`
5. Add Component > search **EnemyAI** (this auto-requires NavMeshAgent and EnemyHealth)
   - **enemyData**: Drag the same EnemyData asset here
   - **showDebugGizmos**: `true` (shows detection/attack range spheres in Scene view)

### 6.3 Enemy Health Bar (Optional)

1. As a child of the enemy prefab root, create: **UI > Canvas**
   - Set Render Mode to **World Space**
   - Set Canvas scale to `(0.01, 0.01, 0.01)`
2. Inside the Canvas, create a **UI > Slider**
   - Set **Min Value**: `0`, **Max Value**: `100`
   - Set **Interactable**: `false`
   - Set **Transition**: `None`
   - Remove the Handle Slide Area child if you do not want a handle
   - Configure the **Fill** Image color to white or green
3. Add Component to the Canvas (or the Slider): search **EnemyHealthBar**
   - **healthSlider**: Drag the Slider here
   - **offset**: `(0, 2.5, 0)` (height above enemy)
   - **hideWhenFull**: `true`
   - **hideWhenDead**: `true`
   - **changeColor**: `true`
   - Colors: green / yellow / red

### 6.4 Save Enemy Prefab

1. Drag the enemy from Hierarchy into the Project to create a prefab
2. Delete the scene instance
3. Go back to the EnemyData asset and assign this prefab to the **enemyPrefab** field

---

## 7. Pickup Prefabs

Pickups auto-collect when the player walks into them. They use the PickupData_Modular system (MonoBehaviour components, not ScriptableObjects attached to the same prefab).

### 7.1 Health Pickup Prefab

1. Create a small GameObject (e.g., red cube or sphere, scale ~0.3)
2. Add a **Collider** (BoxCollider or SphereCollider)
   - **Is Trigger**: `true`
3. Add Component > search **PickupItem**
   - **rotatePickup**: `true`
   - **rotationSpeed**: `50`
   - **bobUpDown**: `true`
   - **bobHeight**: `0.3`
   - **bobSpeed**: `2`
   - Leave **pickupData** empty (auto-detects the component below)
4. Add Component > search **HealthPickupData**
   - **pickupName**: "Health Pack"
   - **healthAmount**: `25`
   - **pickupSound**: (optional AudioClip)
   - **pickupEffect**: (optional particle prefab)
5. Save as a prefab

### 7.2 Ammo Pickup Prefab

Create one prefab per ammo type (Pistol, Rifle, Shotgun, etc.):

1. Create a small GameObject (e.g., yellow cube, scale ~0.3)
2. Add a **Collider** with **Is Trigger**: `true`
3. Add Component > **PickupItem** (same settings as health)
4. Add Component > search **AmmoPickupData**
   - **pickupName**: "Pistol Ammo"
   - **ammoType**: `Pistol` (from the AmmoType enum)
   - **ammoAmount**: `20`
   - **pickupSound**: (optional)
   - **pickupEffect**: (optional)
5. Save as a prefab (e.g., `PistolAmmoPickup`)

Repeat for each ammo type you want as a drop.

### 7.3 Weapon Pickup Prefab (Optional)

For placing weapons in the world that the player can walk over to collect:

1. Create a GameObject with the weapon model
2. Add a **Collider** with **Is Trigger**: `true`
3. Add Component > **PickupItem**
4. Add Component > search **WeaponPickupData**
   - **pickupName**: "Shotgun"
   - **weaponToGive**: Drag the WeaponData asset (e.g., `Shotgun.asset`)
   - **bonusAmmo**: `30`
   - **pickupSound**: (optional)
   - **pickupEffect**: (optional)
5. Save as prefab

> **Reminder:** The player must be tagged `"Player"` for pickups to work. The PickupItem checks `other.CompareTag("Player")` on trigger enter.

---

## 8. Manager GameObjects

Create empty GameObjects in the scene for each manager. They do not need to be children of anything.

### 8.1 WaveManager

1. Create an empty GameObject, name it `WaveManager`
2. Add Component > search **WaveManager**
3. Inspector fields:

   **Spawn Points:**
   - Create several empty GameObjects in the scene where you want enemies to appear. Place them on the NavMesh surface. Name them `SpawnPoint1`, `SpawnPoint2`, etc.
   - Drag all spawn point transforms into the **spawnPoints** array

   **Wave Configuration:**
   - **waves**: Set the array size (e.g., 5 for five waves)
   - For each wave element:
     - **waveName**: "Wave 1", "Wave 2", etc.
     - **delayBeforeWave**: `5` (seconds of breather before wave starts)
     - **enemies**: Array of enemy types per wave
       - Each entry has:
         - **enemyData**: Drag an EnemyData asset
         - **count**: Number to spawn (e.g., 3, 5, 10)

   **Auto Start:**
   - **autoStartFirstWave**: `true` (wave 1 starts automatically after the initial delay)
   - **initialDelay**: `3` (seconds after scene loads before wave 1)
   - **spawnInterval**: `0.5` (seconds between individual enemy spawns within a wave)

   **Example wave progression:**
   | Wave | Enemy Data | Count | Delay |
   |------|-----------|-------|-------|
   | Wave 1 | BasicZombie | 3 | 5s |
   | Wave 2 | BasicZombie | 5 | 5s |
   | Wave 3 | BasicZombie | 8 | 5s |

### 8.2 RandomDrop (Drop Manager)

1. Create an empty GameObject, name it `DropManager`
2. Add Component > search **RandomDrop**
3. Inspector fields:
   - **healthDropPrefab**: Drag the health pickup prefab (from Section 7.1)
   - **ammoDropConfigs**: Set the array size to match however many ammo types you support. For each entry:
     - **ammoType**: The AmmoType enum value (Pistol, Rifle, etc.)
     - **weapon**: The WeaponData asset that uses this ammo (used for ownership check -- only drops ammo for weapons the player owns)
     - **dropPrefab**: The ammo pickup prefab for this type (from Section 7.2)
   - **dropHeightOffset**: `1` (spawns pickup above the death position)
   - **healthDropChanceWeight**: `0.5` (50% health vs 50% ammo when a drop occurs)
   - **minAmmoDrop**: `10`
   - **maxAmmoDrop**: `30`

### 8.3 PauseManager

1. Create an empty GameObject, name it `PauseManager` (or attach to the player)
2. Add Component > search **PauseManager**
3. Inspector fields:
   - **pausePanel**: Drag the Pause menu UI panel here (set up in Section 9). This panel must be **disabled by default** in the scene
   - **lockCursorOnResume**: `true`

---

## 9. UI Canvas Setup

### 9.1 Create the HUD Canvas

1. Right-click in Hierarchy > **UI > Canvas**
2. Name it `HUDCanvas`
3. Canvas settings:
   - **Render Mode**: Screen Space - Overlay
   - **UI Scale Mode**: Scale With Screen Size
   - **Reference Resolution**: 1920 x 1080

### 9.2 Ammo Counter

1. Inside HUDCanvas, create: **UI > Text - TextMeshPro**
2. Name it `AmmoText`
3. Position it (e.g., bottom-right corner)
4. Add Component > search **AmmoUI**
5. Inspector fields:
   - **ammoText**: Drag the TextMeshProUGUI component on this same GameObject (auto-detects if on same object)
   - **normalColor**: White
   - **lowAmmoColor**: Yellow
   - **emptyColor**: Red
   - **reloadingColor**: Gray

### 9.3 Points Display

1. Inside HUDCanvas, create: **UI > Text - TextMeshPro**
2. Name it `PointsText`
3. Position it (e.g., top-right corner)
4. Add Component > search **PointsUI**
5. Inspector fields:
   - **pointsText**: Drag the TextMeshProUGUI component here (auto-detects if on same object)
   - **format**: `"Points: {0}"`
   - **normalColor**: White
   - **gainColor**: Yellow
   - **flashDuration**: `0.5`

### 9.4 Health Bar

1. Inside HUDCanvas, create: **UI > Slider**
2. Name it `HealthBar`
3. Position it (e.g., top-left corner)
4. Configure the Slider:
   - Remove or hide the Handle
   - Set **Min Value**: `0`, **Max Value**: `100`
   - Set **Interactable**: `false`
   - The **Fill** Image should be green initially
5. Add Component to the Canvas or the Slider parent: search **PlayerHealthUI**
6. Inspector fields:
   - **healthSlider**: Drag the Slider here
   - **damageFlash**: (Optional) Create a full-screen semi-transparent red Image, drag it here. Starts transparent.
   - **useColorGradient**: `true`
   - **fullHealthColor**: Green
   - **halfHealthColor**: Yellow
   - **lowHealthColor**: Red

### 9.5 Reload Prompt

1. Inside HUDCanvas, create: **UI > Text - TextMeshPro**
2. Name it `ReloadPrompt`
3. Position it center-bottom or near the crosshair
4. Add Component > search **ReloadPromptUI**
5. Inspector fields:
   - **reloadText**: Drag the TextMeshProUGUI component here (auto-detects if on same object)
   - **promptColor**: White
   - **countdownColor**: Yellow

### 9.6 Hitmarker

1. Inside HUDCanvas, create: **UI > Image**
2. Name it `Hitmarker`
3. Set the sprite to a crosshair/X image (white color)
4. Position it at the exact center of the screen (anchor to center, position 0,0)
5. Set the Image color alpha to `0` (starts invisible)
6. Add Component > search **HitmarkerDisplay** (the one in `Scripts/Damage/`)
7. Inspector fields:
   - **hitmarkerImage**: Drag the Image component here
   - **displayDuration**: `0.2`
   - **hitColor**: White
   - **hitSound**: (Optional) Drag a short hit-confirmation AudioClip
   - **hitSoundVolume**: `0.5`

> **Note:** There are two HitmarkerDisplay scripts in the codebase (`Damage/HitmarkerDisplay.cs` and `UI/Hitmarkerdisplay.cs`). They are functionally identical. Use the one in `Damage/` folder (class name `HitmarkerDisplay` with capital D) as that is what the Bullet script references via `HitmarkerDisplay.Instance?.ShowHitmarker()`.

### 9.7 Interaction Text

1. Inside HUDCanvas, create: **UI > Text - TextMeshPro**
2. Name it `InteractionText`
3. Position it center-screen or lower-center
4. Start it **disabled** (uncheck the GameObject active checkbox)
5. Add Component > search **InteractorUI** (either on this object or a parent)
6. Inspector fields:
   - **messageText**: Drag the TextMeshProUGUI component here

### 9.8 Interact Hint ("Press E")

1. Inside HUDCanvas, create: **UI > Text - TextMeshPro** (or an Image)
2. Name it `InteractHint`
3. Set text to "Press E to interact"
4. Position it near center-bottom
5. Start it **disabled**
6. Go back to the player's **Interactor** component and drag this GameObject into the **hint** field

Also go back to the player's **Interactor** component and drag the **InteractorUI** component (from step 9.7) into the **interactorUI** field.

### 9.9 Game Over Panel

1. Inside HUDCanvas, create: **UI > Panel**
2. Name it `GameOverPanel`
3. Make it cover the full screen (stretch anchors)
4. Add a child "GAME OVER" text (TextMeshPro)
5. Add a child **UI > Button** named `RestartButton` with text "Restart"
6. **Disable** the GameOverPanel (uncheck active in Inspector)
7. Add Component to the Canvas or any persistent object: search **GameOverScreen**
8. Inspector fields:
   - **gameOverPanel**: Drag the `GameOverPanel` GameObject here
   - **restartButton**: Drag the `RestartButton` Button component here
   - **showCursor**: `true`

### 9.10 Pause Panel

1. Inside HUDCanvas, create: **UI > Panel**
2. Name it `PausePanel`
3. Add text "PAUSED" and a Resume button
4. **Disable** the PausePanel (uncheck active in Inspector)
5. Go back to the **PauseManager** component and assign this panel to **pausePanel**
6. Wire the Resume button's **OnClick** event to PauseManager > **Resume()** method

---

## 10. NPC Setup (Optional)

NPCs use the interaction system. All NPC types follow the same base setup.

### 10.1 Base NPC Setup (All Types)

1. Place a character model in the scene
2. Tag it **"Interactable"**
   - If the "Interactable" tag does not exist: Tag dropdown > Add Tag > create "Interactable"
3. Add a **Collider** (CapsuleCollider or BoxCollider) -- not trigger, the Interactor raycast hits solid colliders
4. Create a child "indicator" object (e.g., a floating exclamation mark or UI element above the NPC's head). Start it **disabled**.
5. The NPC script's **indicator** field should reference this child object
6. Set **npcName** to a display name

### 10.2 TutorialNPC

1. Add Component > search **TutorialNPC**
2. Inspector fields:
   - **indicator**: Drag the indicator child
   - **npcName**: "Tutorial Guide"
   - **tips**: Array of strings. Example:
     - "Use RIGHT MOUSE to aim, LEFT MOUSE to shoot. You can only fire while aiming!"
     - "Press R to reload. Your weapon auto-reloads when the magazine empties."
     - "Use SCROLL WHEEL to switch weapons."
     - "Kill zombies to earn points. Visit the Shopkeeper to buy new weapons."

### 10.3 ShopkeeperNPC

1. Add Component > search **ShopkeeperNPC**
2. Inspector fields:
   - **indicator**: Drag the indicator child
   - **npcName**: "Shopkeeper"
   - **weaponOfferings**: Array of offerings, each with:
     - **weapon**: Drag a WeaponData asset (e.g., `Uzi.asset`)
     - **cost**: Points cost (e.g., 500)
     - **description**: "Fast-firing SMG, great for crowds"
   - **bonusAmmoOnPurchase**: `60` (ammo given with each weapon purchase)

### 10.4 MedicNPC

1. Add Component > search **MedicNPC**
2. Inspector fields:
   - **indicator**: Drag the indicator child
   - **npcName**: "Medic"
   - **healCost**: `50` (points to spend)
   - **healAmount**: `50` (HP restored)

---

## 11. NavMesh Baking

Enemies use NavMeshAgent and will not move without a baked NavMesh.

1. Select all ground/floor/walkable objects in the scene
2. In the Inspector, check **Navigation Static** (or open the Static dropdown and check Navigation Static)
3. Open **Window > AI > Navigation**
4. Go to the **Bake** tab
5. Configure Agent Radius and Agent Height to match your enemy character size
6. Click **Bake**
7. You should see a blue overlay on walkable surfaces in the Scene view
8. If enemies need to navigate around obstacles, mark those obstacles as Navigation Static too

> **Tip:** Place your spawn points on the NavMesh surface. If a spawn point is off the NavMesh, enemies may spawn and immediately fail to pathfind.

---

## 12. Testing Checklist

Run through each item. If something fails, check the Console for error messages -- every script logs detailed errors with `[ClassName]` prefixes.

### Player

- [ ] Player is tagged "Player"
- [ ] Camera is tagged "MainCamera"
- [ ] InputReader component is present on player
- [ ] PlayerHealth, Inventory, WeaponController, Interactor are all on the player root
- [ ] Inventory has a **startingWeapon** assigned and it is in **weaponSlots**
- [ ] **initialAmmoTypes** and **initialAmmoAmounts** have matching lengths
- [ ] **maxAmmoTypes** and **maxAmmoAmounts** have matching lengths

### Weapons

- [ ] At least one WeaponData asset exists with **bulletPrefab** assigned
- [ ] Bullet prefab has: Rigidbody (no gravity), trigger Collider, Bullet script
- [ ] Weapon model prefabs have a child named "ShootPoint" (or matching `shootPointName`)
- [ ] Right-click to aim, left-click to shoot while aiming -- bullets spawn from ShootPoint
- [ ] R key reloads (check AmmoUI shows "RELOADING...")
- [ ] Scroll wheel switches weapons
- [ ] Auto-reload triggers when magazine empties and backpack has ammo

### Enemies

- [ ] NavMesh is baked (blue overlay visible in Scene view)
- [ ] Enemy prefab has: NavMeshAgent, EnemyHealth, EnemyAI, Collider
- [ ] Both EnemyHealth and EnemyAI reference the same EnemyData asset
- [ ] EnemyData has **enemyPrefab** assigned back to the enemy prefab
- [ ] Enemies chase the player when within detection range
- [ ] Enemies deal damage at attack range (player health decreases)
- [ ] Enemies die when shot enough (fall over, corpse disappears after corpseLifetime)
- [ ] Points are awarded on kill (check PointsUI)

### Waves

- [ ] WaveManager exists in the scene with waves configured
- [ ] Spawn points are on the NavMesh surface
- [ ] First wave starts automatically after initialDelay
- [ ] Next wave starts after all enemies in current wave are killed
- [ ] Console logs wave progression: "Wave X spawning!", "Wave X completed!"

### Drops

- [ ] RandomDrop manager exists with healthDropPrefab assigned
- [ ] ammoDropConfigs have valid prefabs and weapon references
- [ ] Killing enemies sometimes drops health or ammo pickups
- [ ] Walking into pickups collects them (health heals, ammo adds to backpack)

### UI

- [ ] Ammo counter updates when shooting and reloading
- [ ] Ammo counter changes color: white (normal) > yellow (low) > red (empty) > gray (reloading)
- [ ] Points display updates on enemy kills with yellow flash
- [ ] Health bar updates when taking damage, color grades green > yellow > red
- [ ] "Press R to reload" appears when magazine is empty
- [ ] Hitmarker flashes when hitting enemies
- [ ] Game Over panel appears when player health reaches 0
- [ ] Restart button reloads the scene
- [ ] Escape pauses the game, shows pause panel, unlocks cursor
- [ ] Resume button (or Escape again) unpauses

### Interaction (if NPCs are set up)

- [ ] NPCs are tagged "Interactable"
- [ ] Looking at NPC shows the "Press E" hint
- [ ] Pressing E triggers interaction, shows message text
- [ ] TutorialNPC cycles through tips
- [ ] ShopkeeperNPC shows weapon offers, purchases work with enough points
- [ ] MedicNPC heals player for points when not at full health
- [ ] Moving away or pressing cancel keys ends interaction

### Common Problems

| Problem | Solution |
|---------|----------|
| Enemies don't move | Bake the NavMesh. Check console for "No NavMesh" errors. |
| Enemies don't chase | Player not tagged "Player". Check console for tag error. |
| Bullets don't spawn | WeaponData missing **bulletPrefab** reference. |
| Bullets hit the player | Add the player's layer to WeaponController **hitLayers** exclude list. The Bullet script ignores the shooter via `SetShooter()`. |
| Shooting does nothing | Camera not tagged "MainCamera". WeaponController disables itself. |
| No ammo on start | **initialAmmoTypes** / **initialAmmoAmounts** arrays empty or mismatched. |
| Pickups don't work | Player not tagged "Player", or Collider on pickup is not set to **Is Trigger**. |
| Game Over doesn't trigger | GameOverScreen not in scene, or **gameOverPanel** not assigned. |
| Pause doesn't work | PauseManager not in scene, or no InputReader found. |
| NPC interaction fails | NPC not tagged "Interactable", or no Collider on NPC. |
| UI text not updating | TMP_Text component not assigned in the UI script's Inspector field. |
