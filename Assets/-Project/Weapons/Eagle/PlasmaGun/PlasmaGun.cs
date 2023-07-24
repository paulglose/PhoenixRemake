using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlasmaGunAbilites
{
    public bool PiercingBullets;

    public PlasmaGunAbilites(bool PiercingBullets)
    {
        this.PiercingBullets = PiercingBullets;
    }
}

public class PlasmaGun : Upgradeable
{
    #region Fields and Properties
    [Header("Channel")]
    [SerializeField] InputReader inputReader; // Handles user input
    [SerializeField] CameraShakeChannel cameraShakeChannel; // Controls camera shaking
    [SerializeField] MouseChannel mouseChannel; // Handles mouse input
    [SerializeField] ProgressChannel progressChannel; // Handles progress tracking
    [SerializeField] UIAbilityChannel UIAbilityChannel; // Handles progress tracking
    [SerializeField] SoundChannel SoundChannel; // Handles progress tracking

    [Header("Configurations")]
    [SerializeField] float aimSpeed; // Speed of aiming
    [SerializeField] float maxCanonAngle; // Maximum angle for the canon
    [SerializeField] float shakeStrength; // Strength of camera shake
    [SerializeField] float shakeDuration; // Duration of camera shake
    [SerializeField] float timeUntilDecreaseAfterMaxHeat; // Time until heat decreases after reaching max

    [Header("Stats")]
    public Stat ShootingCooldown; // Cooldown time between shots
    public Stat BulletDamage; // Damage dealt by a bullet
    public Stat BulletSpeed; // Speed of a bullet
    public Stat BulletSize; // Size of a bullet
    public Stat ShootingSlow; // Slow applied when shooting
    public Stat ShootingAccuracy; // Accuracy of shooting
    public Stat HeatPerShot; // Heat generated per shot
    public Stat MaxHeat; // Maximum heat before overheating
    public Stat CooldownSpeed; // Speed of cooling down
    public bool PiercingBullets; // Wether Bullets pierce Enemies or not
    public bool DoubleBullets; // If double bullets should be shot 

    [Header("Sounds")]
    public AudioClip shootingSound;

    [Header("Others")]
    [SerializeField] Transform[] canons; // Array of canons
    [SerializeField] Transform[] shootingPoints; // Points from where bullets are shot
    [SerializeField] Animator[] plasmaAnimator; // Animators for the plasma effect
    [SerializeField] MouseChannel mouse; // Mouse input
    [SerializeField] PlasmaBullet bulletPrefab; // Prefab for the bullet
    [SerializeField] ParticleSystem bulletParticle; // Particle system for the bullet
    [SerializeField] Material normalBarMaterial; // Material for the progress bar
    [SerializeField] Material OverheatedBarMaterial; // Material for the progress bar
    [SerializeField] Sprite AbilitySprite;
    [SerializeField] AbilityPackage UIPackage;

    public event Action<MonoBehaviour, Vector2, VelocityType> OnVelocityRegistered; // Event triggered when velocity is registered

    private bool isShootingRequested = false; // Whether shooting is currently requested
    private bool isOverheated = false; // Whether the gun is overheated
    private float currentHeat = 0; // Current heat of the gun
    private float timeUntilShoot = 0; // Time until the next shot can be fired
    private bool isLeftWeapon; // Whether the left weapon is currently active
    #endregion
    #region Unity Methods

    protected override void Start()
    {
        base.Start();
        UIAbilityChannel.RegisterAbility(UIPackage);
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        SubscribeToInputReaderEvents(); // Subscribe to input events
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        UnsubscribeFromInputReaderEvents(); // Unsubscribe from input events
    }

    public void Update()
    {
        AimCanon(); // Handle canon aiming
        HandleShooting(); // Handle shooting
        CoolDown(); // Handle cooling down
    }
    #endregion

    #region Private Methods
    private void SubscribeToInputReaderEvents()
    {
        inputReader.PrimaryPerformed += OnPrimaryPerformed; // Subscribe to primary performed event
        inputReader.PrimaryCanceled += OnPrimaryCanceled; // Subscribe to primary canceled event
    }

    private void UnsubscribeFromInputReaderEvents()
    {
        inputReader.PrimaryPerformed -= OnPrimaryPerformed; // Unsubscribe from primary performed event
        inputReader.PrimaryCanceled -= OnPrimaryCanceled; // Unsubscribe from primary canceled event
    }

    private void OnPrimaryPerformed()
    {
        if (isOverheated) return; // If the gun is overheated, do not perform primary action

        UIAbilityChannel.RegisterCasting();
        isShootingRequested = true; // Set shooting request to true
        inputReader.RegisterPrimary();
        mouseChannel.RaiseAccuracyRadius(ShootingAccuracy);
        ApplySlow(); // Apply slow effect
    }

    private void OnPrimaryCanceled()
    {
        inputReader.InterruptPrimary();
        UIAbilityChannel.RegisterAvailable();
        isShootingRequested = false; // Set shooting request to false
        mouseChannel.RaiseAccuracyRadius(0);
        ResetSlow(); // Reset the slow effect
    }

    private void AimCanon()
    {
        if (!inputReader.IsPlayerInput) return; // If there is no player input, do not aim

        // Aim each canon in the canons array
        foreach (Transform canon in canons)
        {
            Vector2 direction = mouse.Position - canon.position;
            float angle = Mathf.Clamp(Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg, -maxCanonAngle / 2, maxCanonAngle / 2);
            canon.localRotation = Quaternion.RotateTowards(canon.localRotation, Quaternion.Euler(0, 0, angle), aimSpeed * Time.fixedDeltaTime);
        }
    }

    private void HandleShooting()
    {
        mouseChannel.RaiseRotationSwitch(false);
        if (!isShootingRequested) return; // If shooting is not requested, do not handle shooting

        mouseChannel.RaiseRotationSwitch(true);

        // If it is time to shoot, shoot and set the time until the next shot
        if (Time.time > timeUntilShoot)
        {
            Shoot(isLeftWeapon ? 0 : 1);
            isLeftWeapon = !isLeftWeapon;
            timeUntilShoot = Time.time + ShootingCooldown;
        }
    }

    private void Shoot(int index)
    {
        SoundChannel.RequestSound(new SoundData(shootingSound, SoundType.Player, 1));
        IncreaseHeat(); // Increase the heat
        PlayShotAnimation(index); // Play the shot animation
        FireBullet(index); // Fire a bullet

        if (DoubleBullets) FireBullet(index);

        ShakeCamera(); // Shake the camera
    }

    private void IncreaseHeat()
    {
        currentHeat = Mathf.Clamp(currentHeat + HeatPerShot, 0, MaxHeat); // Increase the current heat

        // If the heat is at maximum, overheat the gun
        if (currentHeat == MaxHeat)
            Overheat();

        progressChannel.RegisterProgress(currentHeat, MaxHeat); // Register the progress
    }

    private void Overheat()
    {
        progressChannel.RaiseProgressMaterial(OverheatedBarMaterial);
        progressChannel.RaiseProgressHighlight(); // Visually show in the bar that the bar is full
        isOverheated = true; // Set the gun to overheated

        inputReader.InterruptPrimary();

        UIAbilityChannel.RegisterCooldown(MaxHeat / CooldownSpeed);
        isShootingRequested = false; // Set shooting request to false
        mouseChannel.RaiseAccuracyRadius(0);
        ResetSlow(); // Reset the slow effect
    }

    private void CoolDown()
    {
        // If shooting is not requested and the gun is not at 0 heat, cool down the gun
        if (!isShootingRequested && currentHeat > 0)
        {
            currentHeat = Mathf.Clamp(currentHeat - CooldownSpeed * Time.deltaTime, 0, MaxHeat);
            progressChannel.RegisterProgress(currentHeat, MaxHeat);

            // If the gun is at 0 heat, cancel the progress and set the gun to not overheated
            if (currentHeat == 0)
            {
                isOverheated = false;
                progressChannel.RaiseProgressCanceled();
                progressChannel.RaiseProgressMaterial(normalBarMaterial);
            }
        }
    }

    private void ApplySlow()
    {
        ISlowable iSlowable = GetComponentInParent<ISlowable>(); // Get the ISlowable component from the parent
        if (iSlowable == null)
        {
            Debug.Log(this + " did not Slow, because there is not ISlowable and with It not VelocityHandler");
            return;
        }

        iSlowable.ApplySlow(this, ShootingSlow); // Apply slow effect
    }

    private void ResetSlow()
    {
        ISlowable iSlowable = GetComponentInParent<ISlowable>(); // Get the ISlowable component from the parent

        if (iSlowable != null) iSlowable.ResetSlow(this); // Reset the slow effect
    }

    private void PlayShotAnimation(int index)
    {
        plasmaAnimator[index].CrossFade("Shot", .08f); // Play the shot animation
    }

    private void FireBullet(int index)
    {
        PlasmaGunAbilites Abilities = new PlasmaGunAbilites(PiercingBullets);

        // Instantiate a bullet and initialize it
        Instantiate(bulletPrefab, shootingPoints[index].transform.position, shootingPoints[index].transform.rotation).Init(mouseChannel.GetRandomAccuracyPoint(), BulletSpeed, BulletSize, BulletDamage, Abilities);
        // Instantiate a bullet particle
        Instantiate(bulletParticle, shootingPoints[index].transform.position, shootingPoints[index].transform.rotation);
    }

    private void ShakeCamera()
    {
        cameraShakeChannel.RaiseShakeImpulse(shakeStrength, shakeDuration); // Shake the camera
    }
    #endregion
}