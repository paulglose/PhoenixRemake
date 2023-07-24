using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Upgrade/Eagle", fileName ="EGUP_")]
public class EagleUpgrade : Upgrade
{
    [Header("PlasmaGun")]
    [SerializeField] float ShootingCooldown;
    [SerializeField] float BulletDamage;
    [SerializeField] float BulletSpeed;
    [SerializeField] float BulletSize;
    [SerializeField] float ShootingSlow;
    [SerializeField] float ShootingAccuracy;
    [SerializeField] float HeatPerShot;
    [SerializeField] float MaxHeat;
    [SerializeField] float CooldownSpeed;
    [SerializeField] bool PiercingBullets;

    [Header("RocketLauncher")]
    public float Damage;
    public float ExplosionRadius;
    public float Cooldown;
    public float TimeToLoaded;
    public float Slow;
    public float TimeUntilExplosion;
    public bool InversedKnockback;
    public bool DoubleBullets;

    [Header("QuantumDash")]
    [SerializeField] float DashPower;
    [SerializeField] float MovementSpeed;
    [SerializeField] float DashCooldown;

    [Header("UltraLaser")]
    [SerializeField] float LaserLoadingTime;
    [SerializeField] float LaserDamagePerTick;
    [SerializeField] float LaserShootingDuration;
    [SerializeField] float LaserCooldown;
    [SerializeField] float LaserSlow;
    [SerializeField] float LaserKnockbackStrength;

    public override void ApplyUpgrade(Upgradeable weapon)
    {
        if (weapon is PlasmaGun plasmaGun)
        {
            plasmaGun.ShootingCooldown.ImproveStat(ShootingCooldown);
            plasmaGun.BulletDamage.ImproveStat(BulletDamage);
            plasmaGun.BulletSpeed.ImproveStat(BulletSpeed);
            plasmaGun.BulletSize.ImproveStat(BulletSize);
            plasmaGun.ShootingSlow.ImproveStat(ShootingSlow);
            plasmaGun.ShootingAccuracy.ImproveStat(ShootingAccuracy);
            plasmaGun.HeatPerShot.ImproveStat(HeatPerShot);
            plasmaGun.MaxHeat.ImproveStat(MaxHeat);
            plasmaGun.CooldownSpeed.ImproveStat(CooldownSpeed);

            if (PiercingBullets) plasmaGun.PiercingBullets = true;
        }

        if (weapon is GrenadeLauncher grenadeLauncher)
        {
            grenadeLauncher.Damage.ImproveStat(Damage);
            grenadeLauncher.ExplosionRadius.ImproveStat(ExplosionRadius);
            grenadeLauncher.Cooldown.ImproveStat(Cooldown);
            grenadeLauncher.TimeToLoaded.ImproveStat(TimeToLoaded);
            grenadeLauncher.Slow.ImproveStat(Slow);
            grenadeLauncher.TimeUntilExplosion.ImproveStat(TimeUntilExplosion);

            if (grenadeLauncher.InversedKnockback) InversedKnockback = true;
        }

        if (weapon is QuantumDash quantumDash)
        {
            quantumDash.DashPower.ImproveStat(DashPower);
            quantumDash.MovementSpeed.ImproveStat(MovementSpeed);
            quantumDash.DashCooldown.ImproveStat(DashCooldown);
        }

        if (weapon is UltraLaser ultraLaser)
        {
            ultraLaser.LaserLoadingTime.ImproveStat(LaserLoadingTime);
            ultraLaser.LaserDamagePerTick.ImproveStat(LaserDamagePerTick);
            ultraLaser.LaserShootingDuration.ImproveStat(LaserShootingDuration);
            ultraLaser.LaserCooldown.ImproveStat(LaserCooldown);
            ultraLaser.LaserSlow.ImproveStat(LaserSlow);
            ultraLaser.LaserKnockbackStrength.ImproveStat(LaserKnockbackStrength);
        }
    }
}
