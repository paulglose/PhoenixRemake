using System;
using UnityEngine;

public class DroneShooting : EnemyComponent, IVelocityInfluence
{
    [Header("Configuration")]
    [SerializeField] Vector2 shootingCooldown;
    [SerializeField] float BulletDamage;
    [SerializeField] float BulletSpeed;
    [SerializeField] float KnockbackStrength;

    [Space]
    [SerializeField] DefaultEnemyBullet bulletPrefab;

    float aimedShootingTime;

    public event Action<MonoBehaviour, Vector2, VelocityType> RegisterVelocity;
    public event Action<MonoBehaviour> ResetVelocity;

    protected override void Update()
    {
        base.Update();

        // Handle Shooting
        if (Configuration.CanAttack && Time.time > aimedShootingTime) Shoot();
    }

    void Shoot()
    {
        // Instantiate The Bullet
        Instantiate(bulletPrefab, transform.position, transform.rotation).Init(BulletDamage * Configuration.DamageMultiplier, BulletSpeed);

        // Reset the shootingCooldown
        aimedShootingTime = Time.time + UnityEngine.Random.Range(shootingCooldown.x, shootingCooldown.y);

        // Register the Knockback Velocity
        RegisterVelocity(this, Vector2.right * KnockbackStrength, VelocityType.Burst);
    }
} 
