# ⚡ QUICK START CHECKLIST
## What to Do RIGHT NOW (Today)

---

## ✅ Phase 0: Setup (30 min)

### Folder Structure
- [ ] Create `Assets/_Project/Scripts/Data/` folder
- [ ] Create `Assets/_Project/Scripts/Player/` folder
- [ ] Create `Assets/_Project/Scripts/Enemies/` folder
- [ ] Create `Assets/_Project/Scripts/UI/` folder
- [ ] Create `Assets/_Project/ScriptableObjects/Weapons/` folder
- [ ] Create `Assets/_Project/ScriptableObjects/Enemies/` folder
- [ ] Create `Assets/_Project/Prefabs/` folder
- [ ] Create `Assets/_Project/Scenes/` folder

### Scene
- [ ] Create new scene: `MainGame`
- [ ] Add SYNTY ground/terrain
- [ ] Add Directional Light
- [ ] Save scene

---

## ✅ Phase 1: Foundation (1 hour)

### Create These Scripts (Copy from BUILD_ORDER_GUIDE.md):
- [ ] `AmmoType.cs` in Data folder
- [ ] `PickupType.cs` in Data folder  
- [ ] `IDamageable.cs` in Data folder
- [ ] `IInteractable.cs` in Data folder
- [ ] `WeaponData.cs` in Data folder
- [ ] `EnemyData.cs` in Data folder
- [ ] `PickupData.cs` in Data folder

### Verify:
- [ ] All 7 scripts created
- [ ] Unity Console shows 0 errors
- [ ] Scripts compile successfully

---

## ✅ Phase 2: Input System (1 hour)

### Create Input Actions:
- [ ] Right-click Project → Create → Input Actions
- [ ] Name it `GameInputActions`
- [ ] Open Input Actions window
- [ ] Create "Gameplay" action map
- [ ] Add Fire action (Left Mouse Button)
- [ ] Add Reload action (R key)
- [ ] Add Interact action (E key)
- [ ] Add DebugDamage action (K key)
- [ ] Add WeaponSlot1-7 actions (1-7 keys)
- [ ] Add WeaponScroll action (Mouse Scroll Y)
- [ ] Click "Save Asset"
- [ ] Click "Generate C# Class"

### Create GameInputReader:
- [ ] Copy `GameInputReader.cs` from INPUT_SYSTEM_FIX.md
- [ ] Place in `Scripts/Player/` folder
- [ ] Verify it compiles

### Setup Player:
- [ ] Drag SYNTY character into scene
- [ ] Position at (0, 0, 0)
- [ ] Tag as "Player"
- [ ] Test movement works (WASD + mouse)
- [ ] Add GameInputReader component to Player
- [ ] Drag GameInputActions asset to "Input Actions Asset" field
- [ ] Play mode → verify no input errors

---

## ✅ Phase 3: Player Health (1 hour)

### Create Script:
- [ ] Copy `PlayerHealth.cs` from BUILD_ORDER_GUIDE.md
- [ ] Place in `Scripts/Player/` folder
- [ ] Verify it compiles

### Setup:
- [ ] Select Player GameObject
- [ ] Add PlayerHealth component
- [ ] Set Max Health = 100

### Test:
- [ ] Enter Play mode
- [ ] Press K key
- [ ] See "Player took 10 damage" in Console
- [ ] Keep pressing K until health = 0
- [ ] See "Player died!" in Console
- [ ] ✅ If this works → move to Phase 4

---

## ✅ Phase 4: Health UI (30 min)

### Create UI:
- [ ] Hierarchy → Right-click → UI → Canvas
- [ ] Rename to "PlayerUI"
- [ ] Set Canvas Scaler → Scale With Screen Size
- [ ] Reference Resolution: 1920x1080
- [ ] Right-click PlayerUI → UI → Slider
- [ ] Rename to "HealthBar"
- [ ] Position bottom-left corner
- [ ] Delete "Handle Slide Area" child
- [ ] Set Fill color to red
- [ ] Set Background color to dark gray

### Create Script:
- [ ] Copy `PlayerHealthUI.cs` from BUILD_ORDER_GUIDE.md
- [ ] Place in `Scripts/UI/` folder

### Setup:
- [ ] Select PlayerUI Canvas
- [ ] Add PlayerHealthUI component
- [ ] Drag HealthBar to "Health Slider" field

### Test:
- [ ] Enter Play mode
- [ ] Press K to damage
- [ ] Health bar decreases
- [ ] ✅ If working → move to Phase 5

---

## 🎯 END OF DAY 1 GOAL

You should have:
- ✅ Player that moves (SYNTY controller)
- ✅ Player that takes damage (K key)
- ✅ Health bar that updates
- ✅ Input system working (no errors)
- ✅ 10 scripts created and working

**Total time:** ~3-4 hours

---

## 📅 TOMORROW (Day 2): Shooting

Next session you'll add:
- Simplified weapon controller
- Raycast shooting
- Test weapon (pistol)
- Enemy health
- Ability to shoot and kill enemies

---

## ❌ TROUBLESHOOTING

### "InvalidOperationException: Input System error"
**Fix:** 
- Edit → Project Settings → Player → Other Settings
- Set "Active Input Handling" to "Input System Package (New)"
- Unity will restart

### "NullReferenceException: GameInputReader.Instance is null"
**Fix:**
- Make sure GameInputReader component is on Player GameObject
- Make sure GameInputActions asset is assigned in Inspector
- Try Play mode again

### "Scripts won't compile"
**Fix:**
- Check for typos in script names
- Make sure all `using UnityEngine;` statements are present
- Make sure curly braces `{}` are balanced
- Check Console for specific error messages

### "Health bar doesn't update"
**Fix:**
- Make sure PlayerHealthUI is on the Canvas GameObject (not on HealthBar)
- Make sure HealthBar slider is assigned in Inspector
- Check Console for "OnHealthChanged" subscription errors

---

## 💡 PRO TIPS

1. **Save often** - Ctrl+S after every change
2. **Test each phase** - Don't skip the test steps
3. **Read Console** - It tells you exactly what's wrong
4. **One error at a time** - Fix the first error, often others disappear
5. **Use Debug.Log** - Add more logs if confused about what's happening

---

## 📝 PROGRESS TRACKER

Mark completed phases:
- [ ] Phase 0: Setup ✓
- [ ] Phase 1: Foundation ✓
- [ ] Phase 2: Input System ✓
- [ ] Phase 3: Player Health ✓
- [ ] Phase 4: Health UI ✓
- [ ] Phase 5: Shooting (Tomorrow)
- [ ] Phase 6: Enemy Health (Tomorrow)
- [ ] Phase 7: Crosshair (Tomorrow)

---

## 🆘 IF STUCK

Share with me:
1. Which phase you're on
2. The Console error message (copy exact text)
3. What you were trying to do when it broke

I'll help you fix it!
