using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D), typeof(Enemy))]
public class EnemyHealth : EnemyComponent, IDamageable
{
    [Header("Channel")] 
    public VoidEvent OnEnemySpawned;
    public VoidEvent OnEnemyKilled;

    [Header("Configuration")]
    [SerializeField] float MaxHealth;
    [SerializeField] float CurrentHealth;

    DamageNumber DamageNumber;
    HealthBar HealthBar; 

    // Used for OnHitEffect
    public event Action OnEnemyDamaged;

    protected override void Start()
    {
        base.Start();

        HealthBar = GetComponentInChildren<HealthBar>();
        if (HealthBar != null)
        {
            HealthBar.MaxHealth = MaxHealth;
            HealthBar.CurrentHealth = MaxHealth;
            if (!Configuration.HealthBarEnabled) HealthBar.gameObject.SetActive(false);
        } else Debug.LogError("HealhBar is not Attached to Enemy");

        DamageNumber = Resources.Load<DamageNumber>("DamageNumber");
        MaxHealth *= Configuration.HealthMultiplier;
        CurrentHealth = MaxHealth;
        OnEnemySpawned.RaiseEvent();
    }

    void IDamageable.TakeDamage(float amount)
    {
        Instantiate(DamageNumber, transform.position, Quaternion.identity).Init(amount, Color.white);

        if (CurrentHealth - amount <= 0)
        {
            CurrentHealth = 0;
            Die();
        } else
        {
            CurrentHealth -= amount;
            HealthBar.CurrentHealth = CurrentHealth;
            OnEnemyDamaged?.Invoke();
        }
    }

    void Die()
    {
        OnEnemyKilled.RaiseEvent();
        Destroy(gameObject);
    }
}
