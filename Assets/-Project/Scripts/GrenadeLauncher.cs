using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeLauncherAbilities
{
    public bool InversedKnockback;

    public GrenadeLauncherAbilities(bool InversedExplosionKnockback)
    {
        this.InversedKnockback = InversedExplosionKnockback;
    }
}

public class GrenadeLauncher : Upgradeable
{
    [Header("Channel")]
    [SerializeField] InputReader InputReader;
    [SerializeField] CameraShakeChannel CameraShakeChannel;
    [SerializeField] MouseChannel MouseChannel;
    [SerializeField] ProgressChannel ProgressChannel;
    [SerializeField] UIAbilityChannel UIAbilityChannel;
    [SerializeField] SoundChannel SoundChannel;

    [Header("Stats")]
    public Stat Damage;
    public Stat ExplosionRadius;
    public Stat Cooldown;
    public Stat TimeToLoaded;
    public Stat Slow;
    public Stat TimeUntilExplosion;
    public bool InversedKnockback;

    [Header("Configuration")]
    [SerializeField] float ShootingSpeed;
    [SerializeField] float ShootingShakeStrength;

    [Header("Other")]
    [SerializeField] Transform[] ShootingPoints;
    [SerializeField] GrenadeProjectile Grenade;
    [SerializeField] AbilityPackage UIPackage;

    protected override void OnEnable()
    {
        base.OnEnable();
        InputReader.SpecialPerformed += SpecialPerformed;
        InputReader.SpecialCanceled += SpecialCanceled;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        InputReader.SpecialPerformed -= SpecialPerformed;
        InputReader.SpecialCanceled -= SpecialCanceled;
    }

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Start() 
    {
        base.Start();
        UIAbilityChannel.RegisterAbility(UIPackage);
    }

    int GrenadeCounter;
    float AimedShootingTime;
    float CurrentLoadedTime;
    private void Update()
    {
        if (ShootingRequested)
        {
            if (CurrentLoadedTime < TimeToLoaded)
            {
                CurrentLoadedTime += Time.deltaTime;
                if (CurrentLoadedTime > TimeToLoaded)
                {
                    ProgressChannel.RaiseProgressHighlight();
                    CurrentLoadedTime = TimeToLoaded;
                }

                ProgressChannel.RegisterProgress(CurrentLoadedTime, TimeToLoaded);
            } 
        }
    }

    float aimedCooldownTime;
    bool ShootingRequested;
    private void SpecialPerformed()
    {
        if (Time.time < aimedCooldownTime) return;

        UIAbilityChannel.RegisterCasting();
        MouseChannel.RaiseAccuracyRadius(ExplosionRadius);
        InputReader.RegisterSpecial();
        ApplySlow();
        GrenadeCounter = 0;
        CurrentLoadedTime = 0;
        AimedShootingTime = 0;
        ShootingRequested = true;
    }

    private void SpecialCanceled()
    {
        if (CurrentLoadedTime >= TimeToLoaded)
        {
            ShootGrenade();
            UIAbilityChannel.RegisterCooldown(Cooldown);
            aimedCooldownTime = Time.time + Cooldown;
        } else UIAbilityChannel.RegisterAvailable();

        MouseChannel.RaiseAccuracyRadius(0);
        CameraShakeChannel.RaiseShakeConstantReset(this);
        ResetSlow();
        InputReader.InterruptSpecial();
        ProgressChannel.RaiseProgressCanceled();
        ShootingRequested = false;
    }

    bool isLeft;
    void ShootGrenade()
    {
        CameraShakeChannel.RaiseShakeImpulse(.15f, ShootingShakeStrength);
        GrenadeLauncherAbilities Abilities = new GrenadeLauncherAbilities(InversedKnockback);

        if (isLeft)
        {
            Instantiate(Grenade, ShootingPoints[0].transform.position, transform.rotation).Init(MouseChannel.Position, Damage, ExplosionRadius, ShootingSpeed, TimeUntilExplosion, Abilities);
        } 
        else
        {
            Instantiate(Grenade, ShootingPoints[1].transform.position, transform.rotation).Init(MouseChannel.Position, Damage, ExplosionRadius, ShootingSpeed, TimeUntilExplosion, Abilities);
        }

        isLeft = !isLeft;
    }

    private void ApplySlow()
    {
        ISlowable iSlowable = GetComponentInParent<ISlowable>(); // Get the ISlowable component from the parent
        if (iSlowable == null)
        {
            Debug.Log(this + " did not Slow, because there is not ISlowable and with It not VelocityHandler");
            return;
        }

        iSlowable.ApplySlow(this, Slow); // Apply slow effect
    }

    private void ResetSlow()
    {
        ISlowable iSlowable = GetComponentInParent<ISlowable>(); // Get the ISlowable component from the parent

        if (iSlowable != null) iSlowable.ResetSlow(this); // Reset the slow effect
    }
}
