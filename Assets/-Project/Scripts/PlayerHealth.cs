using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable
{
    public void TakeDamage(float amount);
}

[RequireComponent(typeof(CircleCollider2D))]
public class PlayerHealth : Upgradeable, IDamageable
{
    [Header("Stats")]
    public Stat MaxHealth;

    [Header("Abilites")]
    public bool ShieldBurst;

    [Header("Channel")]
    [SerializeField] CameraShakeChannel CameraShakeChannel;
    [SerializeField] VoidEvent GameOver;
    [SerializeField] VoidEvent OnPlayerDamage;
    [SerializeField] HealthUIChannel HealthUIChannel;
    [SerializeField] WaveChannel WaveChannel;
    [SerializeField] VignetteChannel VignetteChannel;

    [Header("Configurations")]
    [SerializeField] float ShakeDuration;
    [SerializeField] float ShakeStrength;

    float CurrentHealth;

    protected override void OnEnable()
    {
        WaveChannel.OnUpgradeSelected += OnUpgradeSelected;
    }

    protected override void OnDisable()
    {
        WaveChannel.OnUpgradeSelected -= OnUpgradeSelected;
    }

    protected override void Start()
    {
        CurrentHealth = MaxHealth;
        HealthUIChannel.RaiseHealth((int)CurrentHealth, (int)MaxHealth);
        VignetteChannel.RaiseHealthChanged((int)CurrentHealth);
    }

    void OnUpgradeSelected() => HealthUIChannel.RaiseHealth((int)CurrentHealth, (int)MaxHealth);

    void IDamageable.TakeDamage(float notNeeded)
    {
        CurrentHealth -= 1;
        
        if (CurrentHealth == 0)
        {
            CameraShakeChannel.RaiseShakeImpulse(0.8f, 5);
            GameOver.RaiseEvent();
            CurrentHealth = 0;
            Destroy(gameObject);
        }

        VignetteChannel.RaiseHealthChanged((int)CurrentHealth);
        HealthUIChannel.RaiseHealth((int)CurrentHealth, (int)MaxHealth);
        CameraShakeChannel.RaiseShakeImpulse(ShakeStrength, ShakeDuration);
        OnPlayerDamage.RaiseEvent();
    }
}
