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
    public float range = 100f;         // Max shooting distance (replaces maxShootDistance)
    public int magazineSize = 30;
    public float reloadTime = 2f;
    public AmmoType ammoType ;

    [Header("Shooting")]
    [Tooltip("Name of the child GameObject that marks the barrel tip (default: 'ShootPoint')")]
    public string shootPointName = "ShootPoint";

    [Tooltip("Optional: Bullet trail prefab spawned when shooting")]
    public GameObject bulletPrefab;

    [Header("Effects")]
    [Tooltip("Sound for single-shot weapons or first shot of automatic weapons")]
    public AudioClip shootSound;

    [Tooltip("Looping sound for automatic weapons (minigun, uzi, rifle). Leave empty for single-shot weapons.")]
    public AudioClip loopingFireSound;

    public AudioClip reloadSound;
    public GameObject muzzleFlashPrefab;
    public GameObject bulletHolePrefab;
}