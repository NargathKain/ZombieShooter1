# NPC System - Architecture & Setup Guide

## Overview

This system adds three interactive NPCs and a kill-based points economy to the
zombie shooter. The NPCs are:

- **TutorialNPC** -- Cycles through gameplay tips on each interaction.
- **ShopkeeperNPC** -- Sells weapons in exchange for points.
- **MedicNPC** -- Heals the player in exchange for points.

Points are earned by killing zombies and are stored directly in the `Inventory`
class (not in a separate manager). This follows Stavros's preference: foundation
scripts first, then managers with Observer-pattern subscribers.

The NPC system plugs into the existing `IInteractable` / `Interactor` interaction
framework that was already in the project. No modifications to `IInteractable` or
`Interactor` were needed.

---

## Architecture

### Dependency Order (Top-Down)

Build and study the scripts in this order. Each layer only depends on layers
above it, never below.

```
Layer 0 (Foundation -- no dependencies):
    IInteractable           interface, defines 4 methods
    EnemyData               ScriptableObject, holds pointsOnDeath

Layer 1 (Uses Layer 0):
    Interactor              raycasts for IInteractable, manages interaction state
    EnemyHealth             fires OnEnemyDeath static event on death

Layer 2 (Uses Layers 0-1):
    NPCBase                 abstract class, implements IInteractable
    Inventory               weapon/ammo management + Points region
                            (subscribes to EnemyHealth.OnEnemyDeath)

Layer 3 (Uses Layers 0-2):
    TutorialNPC             extends NPCBase, no extra dependencies
    ShopkeeperNPC           extends NPCBase, uses Inventory for purchases
    MedicNPC                extends NPCBase, uses Inventory + PlayerHealth

Layer 4 (UI -- subscribes to events only):
    PointsUI                subscribes to Inventory.OnPointsChanged
                            and Inventory.OnPointsGained
```

### ASCII Dependency Diagram

```
+-------------------+      +-------------------+
|   IInteractable   |      |    EnemyData      |
|   (interface)     |      | (ScriptableObject)|
+--------+----------+      +--------+----------+
         |                           |
         v                           v
+--------+----------+      +---------+---------+
|    Interactor      |      |   EnemyHealth     |
| (raycasts for     |      | fires             |
|  IInteractable)   |      | OnEnemyDeath      |
+--------+----------+      +---------+---------+
         |                            |
         v                            v
+--------+----------+      +----------+--------+
|     NPCBase       |      |    Inventory      |
| (abstract, impls  |      | Points region     |
|  IInteractable)   |      | subscribes to     |
+--+-----+-----+---+      | OnEnemyDeath      |
   |     |     |           +---+----------+----+
   v     v     v               |          |
+--+--+ ++-+ +-+--+           |          |
|Tutor| |Shop| |Medic|        |          |
|ialNPC| |keeper| |NPC |      |          |
+-----+ |NPC | +--+---+      |          |
         +--+-+    |          |          |
            |      |          |          |
            +------+----------+          |
            uses Inventory               |
            (SpendPoints, AddWeapon,     |
             AddAmmo, HasWeapon)         |
                                         |
                              +----------+----------+
                              |      PointsUI       |
                              | subscribes to       |
                              | OnPointsChanged     |
                              | OnPointsGained      |
                              +---------------------+
```

---

## Points System (in Inventory)

Points live inside `Inventory` (file: `Inventory.cs`, region: `Points Management`,
lines 462-508). There is no separate PointsManager class. This keeps the player's
economic state (weapons, ammo, points) in one authoritative source.

### How Points Are Earned

1. A zombie dies.
2. `EnemyHealth` fires its static event: `EnemyHealth.OnEnemyDeath`.
3. `Inventory.OnEnable()` (line 161) subscribes: `EnemyHealth.OnEnemyDeath += HandleEnemyDeath`.
4. `HandleEnemyDeath(EnemyHealth enemyHealth)` (line 498) runs:
   - Reads `EnemyData` from the dead enemy via `enemyHealth.GetEnemyData()`.
   - Extracts `data.pointsOnDeath` (defaults to 100 if EnemyData is null).
   - Calls `AddPoints(points)`.
5. `AddPoints(int amount)` (line 468):
   - Increments `currentPoints`.
   - Fires `OnPointsGained(amount, currentPoints)`.
   - Fires `OnPointsChanged(currentPoints)`.
6. `PointsUI` receives both events and updates the HUD.

### Private Field

```csharp
private int currentPoints;    // line 105
```

### Public Property

```csharp
public int CurrentPoints => currentPoints;    // line 141
```

### Events

| Event | Signature | When Fired |
|-------|-----------|------------|
| `OnPointsChanged` | `Action<int>` (currentTotal) | After AddPoints or SpendPoints succeeds |
| `OnPointsGained` | `Action<int, int>` (amountGained, newTotal) | After AddPoints only |

### Methods

| Method | Signature | Purpose |
|--------|-----------|---------|
| `AddPoints` | `public void AddPoints(int amount)` | Add points from any source. Fires both events. |
| `SpendPoints` | `public bool SpendPoints(int amount)` | Deduct points. Returns false if insufficient. Fires OnPointsChanged on success. |
| `HandleEnemyDeath` | `private void HandleEnemyDeath(EnemyHealth)` | Event handler that reads EnemyData and calls AddPoints. |

### ASCII Flow: Zombie Kill to UI Update

```
+-------------+     OnEnemyDeath      +--------------------------+
| EnemyHealth +---------------------->| Inventory                |
| (zombie     |     (static event)    | .HandleEnemyDeath()      |
|  dies)      |                       |   |                      |
+-------------+                       |   v                      |
                                      | enemyHealth.GetEnemyData()|
                                      |   |                      |
                                      |   v                      |
                                      | data.pointsOnDeath       |
                                      | (or default 100)         |
                                      |   |                      |
                                      |   v                      |
                                      | AddPoints(amount)        |
                                      |   |                      |
                                      |   +---> currentPoints += |
                                      |   |                      |
                                      |   +---> OnPointsGained   |
                                      |   |     (amount, total)  |
                                      |   |                      |
                                      |   +---> OnPointsChanged  |
                                      |         (total)          |
                                      +-----------+--------------+
                                                  |
                              +-------------------+-------------------+
                              |                                       |
                              v                                       v
                    +---------+----------+               +------------+--------+
                    | PointsUI           |               | Any other subscriber |
                    | HandlePointsChanged|               | (future systems)     |
                    |   -> UpdateDisplay |               +---------------------+
                    | HandlePointsGained |
                    |   -> StartFlash    |
                    +--------------------+
```

---

## NPCBase

**File:** `NPCBase.cs` (82 lines)

NPCBase is an abstract MonoBehaviour that implements `IInteractable`. It follows
the same pattern as the existing `TextSign` class that was already in the project:
hide indicator on interact, cache the interactor, send a message, clean up on end.

### IInteractable Implementation

| Method | What it does |
|--------|-------------|
| `OnInteract(Interactor)` | Hides indicator, caches interactor, calls `GetInteractionMessage()` to get the text, sends it via `interactor.ReceiveInteract(message)`, then calls `PerformInteraction(interactor)`. |
| `OnEndInteract()` | Logs interaction end, clears cached interactor. |
| `OnReadyInteract()` | Shows the indicator (player is looking at NPC). |
| `OnAbortInteract()` | Hides the indicator (player looked away). |

### Serialized Fields

| Field | Type | Purpose |
|-------|------|---------|
| `indicator` | `GameObject` | Visual indicator shown when player is in range and looking at NPC. |
| `npcName` | `string` | Display name used in debug logs. |

### Abstract Methods (subclasses MUST implement)

```csharp
protected abstract string GetInteractionMessage();
// Return the text to display to the player.

protected abstract void PerformInteraction(Interactor interactor);
// Execute the NPC's logic (advance tip, attempt purchase, attempt heal, etc).
```

### Helper Methods

```csharp
protected void SendMessage(string message)
// Sends a follow-up message to the cached interactor (e.g., purchase result).
// Uses cachedInteractor.ReceiveInteract(message).

protected void EndInteraction()
// Programmatically ends the interaction from the NPC side.
// Calls cachedInteractor.EndInteract(this).
```

### Key Design Detail

`OnInteract` calls `GetInteractionMessage()` BEFORE `PerformInteraction()`.
This matters for ShopkeeperNPC and MedicNPC, which override `OnInteract` to
cache the Inventory reference first, then call `base.OnInteract()`. Without this,
`GetInteractionMessage()` would not have access to the player's inventory data.

---

## Tutorial NPC

**File:** `TutorialNPC.cs` (43 lines)

The simplest NPC. It cycles through an array of tips, showing the next one on
each interaction. After the last tip, it wraps back to the first.

### Serialized Fields

| Field | Type | Purpose |
|-------|------|---------|
| `tips` | `string[]` | Array of tip strings. Uses `[TextArea(2, 5)]` for multi-line editing in Inspector. |

### Private State

| Field | Type | Purpose |
|-------|------|---------|
| `currentTipIndex` | `int` | Tracks which tip to show next. Starts at 0. |

### Logic

- `GetInteractionMessage()` -- Returns `tips[currentTipIndex]`. Returns `"..."` if no tips configured.
- `PerformInteraction()` -- Logs the current tip, then advances: `currentTipIndex = (currentTipIndex + 1) % tips.Length`.

### No External Dependencies

TutorialNPC does not use Inventory, PlayerHealth, or any other system. It only
depends on NPCBase.

---

## Shopkeeper NPC

**File:** `ShopkeeperNPC.cs` (189 lines)

Sells weapons to the player. Browses through unowned weapons on each interaction.
If the player can afford the current offering, it is purchased immediately.

### WeaponOffering Inner Class (lines 21-27)

```csharp
[System.Serializable]
public class WeaponOffering
{
    public WeaponData weapon;      // The weapon ScriptableObject
    public int cost;               // Price in points
    public string description;     // Shown to player (TextArea)
}
```

### Serialized Fields

| Field | Type | Default | Purpose |
|-------|------|---------|---------|
| `weaponOfferings` | `WeaponOffering[]` | -- | All weapons available for sale. |
| `bonusAmmoOnPurchase` | `int` | 60 | Ammo granted with each weapon purchase. |

### Private State

| Field | Type | Purpose |
|-------|------|---------|
| `browseIndex` | `int` | Current position in the offerings array. |
| `cachedInventory` | `Inventory` | Reference to player's Inventory, cached on interact. |

### Interaction Flow

```
Player presses E on Shopkeeper
         |
         v
OnInteract(interactor) [OVERRIDE, line 173]
  |
  +--> cachedInventory = interactor.GetComponentInParent<Inventory>()
  |
  +--> base.OnInteract(interactor)
         |
         +--> GetInteractionMessage()
         |      |
         |      +--> ValidateSetup() -- are weaponOfferings configured?
         |      |
         |      +--> FindNextUnownedOffering()
         |      |      |
         |      |      +--> Loop from browseIndex, wrap around
         |      |      |    Skip offerings where cachedInventory.HasWeapon() == true
         |      |      |    Return first unowned offering (or null if all owned)
         |      |      |
         |      +--> If null: "You already own everything!"
         |      +--> If can afford: show weapon + "Purchasing..."
         |      +--> If cannot afford: show weapon + "Not enough points!"
         |
         +--> PerformInteraction(interactor)
                |
                +--> cachedInventory = GetComponentInParent<Inventory>()
                |
                +--> FindNextUnownedOffering()
                |
                +--> If null: log, advance index, return
                |
                +--> AttemptPurchase(offering)
                |      |
                |      +--> cachedInventory.SpendPoints(offering.cost)
                |      |    Returns false? -> log failure, return
                |      |
                |      +--> cachedInventory.AddWeapon(offering.weapon)
                |      |
                |      +--> cachedInventory.AddAmmo(offering.weapon.ammoType,
                |      |                            bonusAmmoOnPurchase)
                |      |
                |      +--> SendMessage("Sold! ...")
                |
                +--> AdvanceBrowseIndex()
```

### ASCII Flow: Purchase Sequence

```
+-----------+    E key    +---------------+   GetComponentInParent  +-------------+
|  Player   +------------>| ShopkeeperNPC +------------------------>|  Inventory  |
|           |             | OnInteract()  |   <Inventory>           |             |
+-----------+             +-------+-------+                         +------+------+
                                  |                                        |
                                  v                                        |
                       FindNextUnownedOffering()                           |
                        (skips owned weapons)                              |
                                  |                                        |
                                  v                                        |
                          offering found?                                  |
                           /          \                                    |
                         yes           no                                  |
                          |             \                                  |
                          v              v                                 |
                   AttemptPurchase    "You own                             |
                          |           everything!"                        |
                          v                                                |
                   SpendPoints(cost) -------------------------------->  SpendPoints()
                          |                                             returns bool
                          v                                                |
                   success?                                                |
                    /      \                                               |
                  yes       no                                             |
                   |         \                                             |
                   v          v                                            |
            AddWeapon() ---> "Not enough     <-----------------------------+
            AddAmmo()        points!"
                   |
                   v
            SendMessage("Sold! ...")
```

---

## Medic NPC

**File:** `MedicNPC.cs` (95 lines)

Heals the player in exchange for points. Tracks the player's health passively
via the `PlayerHealth.OnHealthChanged` event so it knows whether healing is needed
before the player even interacts.

### Serialized Fields

| Field | Type | Default | Purpose |
|-------|------|---------|---------|
| `healCost` | `int` | 50 | Points charged per heal. |
| `healAmount` | `float` | 50f | HP restored per heal. |

### Private State

| Field | Type | Purpose |
|-------|------|---------|
| `cachedInventory` | `Inventory` | Player's inventory, cached during interaction. |
| `lastKnownHealth` | `float` | Continuously updated via OnHealthChanged event. |
| `lastKnownMaxHealth` | `float` | Continuously updated via OnHealthChanged event. |

### Event Subscription (Passive Health Tracking)

```csharp
// OnEnable (line 21):
PlayerHealth.OnHealthChanged += HandleHealthChanged;

// OnDisable (line 26):
PlayerHealth.OnHealthChanged -= HandleHealthChanged;

// Handler (line 29):
private void HandleHealthChanged(float current, float max)
{
    lastKnownHealth = current;
    lastKnownMaxHealth = max;
}
```

This means MedicNPC always knows the player's health, even before an interaction
starts. This allows `GetInteractionMessage()` to show context-appropriate messages.

### Interaction Flow

```
Player presses E on Medic
         |
         v
OnInteract(interactor) [NPCBase default -- NOT overridden]
  |
  +--> GetInteractionMessage()
  |      |
  |      +--> If lastKnownHealth >= lastKnownMaxHealth: "You look healthy!"
  |      +--> If cachedInventory == null: "My supplies aren't available."
  |      +--> If CurrentPoints < healCost: "Not enough points!"
  |      +--> Else: "I can patch you up for {healCost} points."
  |
  +--> PerformInteraction(interactor)
         |
         +--> Get Inventory from interactor hierarchy
         +--> Get PlayerHealth from interactor hierarchy
         |
         +--> Guard checks:
         |      - Inventory null? -> warn and return
         |      - PlayerHealth null? -> warn and return
         |      - Player dead? -> return
         |      - Already full health? -> return
         |
         +--> cachedInventory.SpendPoints(healCost)
         |      Returns false? -> log, return (not enough points)
         |
         +--> playerHealth.Heal(healAmount)
         |
         +--> SendMessage("Healed you for {healAmount} HP!")
```

### ASCII Flow: Heal Transaction

```
+----------+   E key   +-----------+                +-----------+   +--------------+
|  Player  +---------->| MedicNPC  |                | Inventory |   | PlayerHealth |
+----------+           +-----+-----+                +-----+-----+   +------+-------+
                             |                             |                |
                             | check lastKnownHealth       |                |
                             | (from OnHealthChanged)      |                |
                             |                             |                |
                             | full health? --> "You       |                |
                             |   look healthy!" (return)   |                |
                             |                             |                |
                             | SpendPoints(healCost)------>|                |
                             |                             |                |
                             |<------- true/false ---------|                |
                             |                             |                |
                             | false? --> return            |                |
                             |                             |                |
                             | Heal(healAmount) --------------------------->|
                             |                             |                |
                             | SendMessage("Healed!")      |                |
                             |                             |                |
+----------+<---message------+                             |                |
|  Player  |  (via Interactor.ReceiveInteract)             |                |
+----------+                                               |                |
```

---

## PointsUI

**File:** `PointsUI.cs` (239 lines)

A HUD element that displays the player's current point total. Fully decoupled
from game logic -- it only reads events, never calls methods on Inventory.

### Serialized Fields

| Field | Type | Default | Purpose |
|-------|------|---------|---------|
| `pointsText` | `TextMeshProUGUI` | -- | The TMP component to write the score into. |
| `format` | `string` | `"Points: {0}"` | Format string. `{0}` is replaced with the point total. |
| `normalColor` | `Color` | White | Default text color. |
| `gainColor` | `Color` | Yellow | Flash color when points are gained. |
| `flashDuration` | `float` | 0.5 | Seconds for the flash to lerp back to normalColor. |

### Event Subscriptions

Subscribed in `OnEnable()` (line 86), unsubscribed in `OnDisable()` (line 91):

```csharp
Inventory.OnPointsChanged += HandlePointsChanged;
Inventory.OnPointsGained  += HandlePointsGained;
```

### Handlers

| Handler | Triggered by | Action |
|---------|-------------|--------|
| `HandlePointsChanged(int currentTotal)` | `Inventory.OnPointsChanged` | Calls `UpdateDisplay(currentTotal)` which sets `pointsText.text = string.Format(format, points)`. |
| `HandlePointsGained(int gained, int newTotal)` | `Inventory.OnPointsGained` | Calls `StartFlash()` which runs `FlashRoutine()` coroutine. |

### Flash Effect

1. `StartFlash()` cancels any running flash, then starts `FlashRoutine()`.
2. `FlashRoutine()` sets text color to `gainColor` (yellow by default).
3. Over `flashDuration` seconds, `Color.Lerp` transitions from `gainColor` back to `normalColor`.
4. When done, resets color and nulls the coroutine reference.

### Initialization

In `Start()` (line 69), PointsUI finds the Inventory via `FindFirstObjectByType<Inventory>()`
and calls `UpdateDisplay(inventory.CurrentPoints)` to show the initial value. This
handles the case where points exist before PointsUI subscribes.

### Validation

In `Awake()`, if `pointsText` is not assigned, it tries `GetComponent<TextMeshProUGUI>()`.
If still null, it logs an error and disables itself.

The `[RequireComponent(typeof(RectTransform))]` attribute ensures the script can
only be added to UI GameObjects.

---

## Event Flow Summary

This diagram shows every event in the NPC system, who fires it, and who listens.

```
+=======================================================================+
|                    COMPLETE EVENT MAP                                  |
+=======================================================================+

  PUBLISHER                EVENT                      SUBSCRIBER(S)
  ---------                -----                      -------------

  EnemyHealth        OnEnemyDeath                 --> Inventory
                     Action<EnemyHealth>               .HandleEnemyDeath()
                                                       (adds points)

  Inventory          OnPointsChanged              --> PointsUI
                     Action<int>                       .HandlePointsChanged()
                     (currentTotal)                    (updates display text)

  Inventory          OnPointsGained               --> PointsUI
                     Action<int, int>                  .HandlePointsGained()
                     (amountGained, newTotal)          (triggers flash effect)

  PlayerHealth       OnHealthChanged              --> MedicNPC
                     Action<float, float>              .HandleHealthChanged()
                     (currentHP, maxHP)                (tracks health passively)

  Inventory          OnWeaponEquipped             --> (existing weapon UI)
                     Action<WeaponData>

  Inventory          OnAmmoChanged                --> (existing ammo UI)
                     Action<AmmoType, int, int>

+=======================================================================+
|                    DIRECT METHOD CALLS                                 |
+=======================================================================+

  CALLER             METHOD                        TARGET
  ------             ------                        ------

  ShopkeeperNPC      SpendPoints(cost)          -> Inventory
  ShopkeeperNPC      AddWeapon(weapon)           -> Inventory
  ShopkeeperNPC      AddAmmo(type, amount)       -> Inventory
  ShopkeeperNPC      HasWeapon(weapon)           -> Inventory
  ShopkeeperNPC      CurrentPoints (property)    -> Inventory

  MedicNPC           SpendPoints(healCost)       -> Inventory
  MedicNPC           CurrentPoints (property)    -> Inventory
  MedicNPC           Heal(healAmount)            -> PlayerHealth

  NPCBase            ReceiveInteract(message)    -> Interactor
  NPCBase            EndInteract(this)           -> Interactor
```

---

## Unity Setup Checklist

### 1. Tutorial NPC

```
[ ] Create an empty GameObject in the scene, name it "TutorialNPC"
[ ] Set its Tag to "Interactable"
[ ] Add a Collider component (e.g., BoxCollider or SphereCollider)
    - The Interactor raycasts for tagged colliders, so the collider
      must be on the SAME GameObject that has the tag and script
[ ] Add the TutorialNPC component (Component > Scripts > TutorialNPC)
[ ] In the Inspector, fill in:
    - NPC Name: e.g., "Tutorial Guide"
    - Tips array: add your gameplay tip strings
      Example tips:
        "Use RIGHT MOUSE to aim, LEFT MOUSE to shoot."
        "Press R to reload."
        "Use SCROLL WHEEL to switch weapons."
        "Kill zombies to earn points. Visit the Shopkeeper!"
        "The Medic can heal you for points."
[ ] Create a child GameObject for the indicator (e.g., a floating arrow sprite)
    - Assign it to the Indicator field in TutorialNPC
    - Make sure it starts disabled (the script enables/disables it)
```

### 2. Shopkeeper NPC

```
[ ] Create an empty GameObject, name it "ShopkeeperNPC"
[ ] Set its Tag to "Interactable"
[ ] Add a Collider component
[ ] Add the ShopkeeperNPC component
[ ] In the Inspector, fill in:
    - NPC Name: e.g., "Arms Dealer"
    - Bonus Ammo On Purchase: 60 (or your preferred amount)
    - Weapon Offerings array: for each weapon you want to sell:
        - Weapon: drag in a WeaponData ScriptableObject
        - Cost: point price (e.g., 500, 1000, 2000)
        - Description: text shown to player (e.g., "Fully automatic rifle")
[ ] Create a child indicator GameObject, assign to Indicator field
[ ] IMPORTANT: The player GameObject must have an Inventory component
    somewhere in its hierarchy (the Shopkeeper finds it via
    GetComponentInParent<Inventory>() from the Interactor)
```

### 3. Medic NPC

```
[ ] Create an empty GameObject, name it "MedicNPC"
[ ] Set its Tag to "Interactable"
[ ] Add a Collider component
[ ] Add the MedicNPC component
[ ] In the Inspector, fill in:
    - NPC Name: e.g., "Field Medic"
    - Heal Cost: 50 (points per heal)
    - Heal Amount: 50 (HP restored per heal)
[ ] Create a child indicator GameObject, assign to Indicator field
[ ] IMPORTANT: The player must have both Inventory AND PlayerHealth
    components in its hierarchy. PlayerHealth must fire the static
    OnHealthChanged event for the Medic to track health passively.
```

### 4. PointsUI

```
[ ] On your HUD Canvas, create a TextMeshPro - Text (UI) element
    (GameObject > UI > Text - TextMeshPro)
[ ] Add the PointsUI component to the same GameObject
[ ] Assign the TMP_Text component to the Points Text field
    (if on the same GameObject, it auto-detects in Awake)
[ ] Optionally configure:
    - Format: "Points: {0}" (or "SCORE: {0}", "${0}", etc.)
    - Normal Color: white (default)
    - Gain Color: yellow (default, the flash color)
    - Flash Duration: 0.5 seconds (default)
```

### 5. Points in Inventory (No Extra Setup)

```
The points system is built into Inventory. It auto-subscribes to
EnemyHealth.OnEnemyDeath in OnEnable(). As long as:

[ ] Inventory component exists on the player
[ ] Enemies have EnemyHealth components that fire OnEnemyDeath
[ ] Enemies have EnemyData ScriptableObjects with pointsOnDeath set

...points will be awarded automatically on each kill.
No additional wiring or configuration is needed.
```

### Common Mistakes to Avoid

- **Missing "Interactable" tag:** The Interactor raycasts and checks
  `hit.collider.CompareTag("Interactable")`. If the tag is missing, the NPC
  will be invisible to the interaction system.
- **Collider on wrong GameObject:** The collider must be on the same GameObject
  that has the NPC script and the "Interactable" tag. Not on a child.
- **Indicator not assigned:** If the `indicator` field in NPCBase is null,
  you will get a NullReferenceException when the player looks at the NPC.
- **WeaponOfferings empty:** ShopkeeperNPC will say "not set up for business"
  if the array is empty or null.
- **PlayerHealth missing OnHealthChanged:** MedicNPC tracks health passively.
  If PlayerHealth does not fire this event, `lastKnownMaxHealth` stays at 0
  and the Medic will never say "you look healthy."

---

## File Locations

| File | Path | Type |
|------|------|------|
| IInteractable | `Assets/_ProjectFiles/Scripts/Interaction/IInteractable.cs` | Existing (unchanged) |
| Interactor | `Assets/_ProjectFiles/Scripts/Interaction/Interactor.cs` | Existing (unchanged) |
| Inventory | `Assets/_ProjectFiles/Scripts/Player/Inventory.cs` | Modified (added Points Management region, lines 462-508) |
| NPCBase | `Assets/_ProjectFiles/Scripts/NPC/NPCBase.cs` | New file |
| TutorialNPC | `Assets/_ProjectFiles/Scripts/NPC/TutorialNPC.cs` | New file |
| ShopkeeperNPC | `Assets/_ProjectFiles/Scripts/NPC/ShopkeeperNPC.cs` | New file |
| MedicNPC | `Assets/_ProjectFiles/Scripts/NPC/MedicNPC.cs` | New file |
| PointsUI | `Assets/_ProjectFiles/Scripts/UI/PointsUI.cs` | New file |
