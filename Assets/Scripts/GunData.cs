using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// DUNNO WHERE TO PUT THIS FOR NOW
// WHILE GUN IS SHOOTING ADD TO COOLDOWN TIMER (TIME.DELTATIME / 10)
// IF IT REACHES 1 THENDON'T ALLOW SHOOTING WHILE COOLING DOWN
// COOLDOWN TIMER GOES DOWN WHILE NOT SHOOTING (SAME RATE AS IT GOES UP?)
// TIMER SYNCS WITH HUD OBJECT BEING FILLED


[CreateAssetMenu(fileName = "New Gun", menuName = "Weapons/New Gun")]
public class GunData : ScriptableObject
{
    // info
    [SerializeField] private string weaponName;

    // shooting
    [SerializeField] private float damage;
    [SerializeField] private float maxDistance;

    // data
    [SerializeField] private float fireRate;
    [SerializeField] private float timeUntilCooldown;
    [SerializeField] private float cooldownLength;

    public string WeaponName
    {
        get { return weaponName; }
    }

    public float Damage
    {
        get { return damage; }
    }

    public float FireRate
    {
        get { return fireRate; }
    }

    public float TimeUntilCooldown
    {
        get { return timeUntilCooldown; }
    }

    public float MaxDistance
    {
        get { return maxDistance; }
    }

    public float CooldownLength
    {
        get { return cooldownLength; }
    }
}
