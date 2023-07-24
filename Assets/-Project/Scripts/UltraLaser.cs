using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UltraLaser : Upgradeable, IVelocityInfluence
{
    [Header("Channel")]
    [SerializeField] InputReader InputReader; // Handles user input
    [SerializeField] CameraShakeChannel CameraShakeChannel; // Controls camera shaking
    [SerializeField] MouseChannel MouseChannel; // Handles mouse input
    [SerializeField] ProgressChannel ProgressChannel; // Handles progress tracking
    [SerializeField] UIAbilityChannel UIAbilityChannel; // Handles progress tracking
    [SerializeField] SoundChannel SoundChannel; // Handles progress tracking

    [Header("Stats")]
    public Stat LaserLoadingTime;
    public Stat LaserDamagePerTick;
    public Stat LaserShootingDuration;
    public Stat LaserCooldown;
    public Stat LaserSlow;
    public Stat LaserKnockbackStrength;
    public bool IsPiercing = false;

    [Header("Configuration")]
    public float MaxLoadingShakeStrength;
    public float ShootingShakeStrength;
    public float InitialLaserShakeStrength;

    [Header("Others")]
    [SerializeField] ParticleSystem LoadingParts;
    [SerializeField] SpriteRenderer LoadingCircle;
    [SerializeField] Laser Laser;
    [SerializeField] GameObject AimPoints;
    [SerializeField] AbilityPackage UIPackage; 

    const float LOADING_POINT_MAX_SCALE = .4f;
    const float LOADING_PARTS_EMMISION = 30f;

    public event Action<MonoBehaviour, Vector2, VelocityType> RegisterVelocity;
    public event Action<MonoBehaviour> ResetVelocity;

    protected override void OnEnable() 
    {
        base.OnEnable();
        InputReader.UltimatePerformed += OnUltimatePerformed;
        InputReader.UltimateCanceled += OnUltimateCanceled;
    }

    protected override void OnDisable() 
    {
        base.OnDisable();
    }

    protected override void Awake() 
    {
        
    }

    protected override void Start() 
    {
        base.Start();
        UIAbilityChannel.RegisterAbility(UIPackage);
    }

    void Update() 
    {
        if (LoadingRequested)
        {
            HandleCameraShake();
            GrowLoadingCircle();
            HandleProgressBar();
        }
    }

    bool LoadingRequested;
    float aimedLoadedTime;
    private void OnUltimatePerformed()
    {
        if (Time.time < aimedCooldownTime) return;

        UIAbilityChannel.RegisterCasting();
        aimedLoadedTime = Time.time + LaserLoadingTime;
        InputReader.RegisterUltimate();
        SetParticleEmmisionRate(LOADING_PARTS_EMMISION);
        ApplySlow();
        AimPoints.SetActive(true);

        // Needed values for the camera shake
        currentShakeValue = 0;
        elapsedShakeTime = 0;

        // Needed for the progress bar
        progressTime = 0;

        LoadingRequested = true;
    }

    float aimedCooldownTime = 0;
    private void OnUltimateCanceled()
    {
        // If its loaded enough
        if (Time.time >= aimedLoadedTime)
        {
            // SHOOT THE LASER
            Laser.Init(LaserDamagePerTick, LaserShootingDuration, IsPiercing);
            CameraShakeChannel.RaiseShakeConstant(this, ShootingShakeStrength);
            CameraShakeChannel.RaiseShakeImpulse(InitialLaserShakeStrength, .15f);
            Invoke("FinishShooting", LaserShootingDuration);
            aimedCooldownTime = Time.time + LaserCooldown;
            UIAbilityChannel.RegisterCooldown(LaserCooldown);
            RegisterVelocity?.Invoke(this, Vector2.left * LaserKnockbackStrength, VelocityType.Burst);
            RegisterVelocity?.Invoke(this, Vector2.left * LaserKnockbackStrength / 5, VelocityType.Constant);
        } 
        
        // If it is interrupted earlier
        else
        {
            FinishShooting();
            UIAbilityChannel.RegisterAvailable();
        }

        ResetLoadingCircle();
        SetParticleEmmisionRate(0);
        ProgressChannel.RaiseProgressCanceled();
        AimPoints.SetActive(false);
        LoadingRequested = false;
    }

    void FinishShooting()
    {
        InputReader.InterruptUltimate();
        ResetSlow();
        CameraShakeChannel.RaiseShakeConstantReset(this);
        ResetVelocity?.Invoke(this);
    }

    private float elapsedTime = 0f;
    void GrowLoadingCircle()
    {
        if (LoadingCircle.transform.localScale.x >= LOADING_POINT_MAX_SCALE) return;
        
        // Check if it need to be initialized
        if (!LoadingCircle.enabled)
        {
            elapsedTime = 0;
            LoadingCircle.enabled = true;
        }

        elapsedTime += Time.deltaTime;
        float t = Mathf.Clamp01(elapsedTime / LaserLoadingTime);
        Vector3 newScale = Vector3.Lerp(new Vector3(0, 0), new Vector3(LOADING_POINT_MAX_SCALE, LOADING_POINT_MAX_SCALE), t);
        LoadingCircle.transform.localScale = newScale;

        if (t >= 1f)
        {
            LoadingCircle.transform.localScale = new Vector3(LOADING_POINT_MAX_SCALE, LOADING_POINT_MAX_SCALE);
        }
    }

    void ResetLoadingCircle()
    {
        LoadingCircle.enabled = false;
        LoadingCircle.transform.localScale = new Vector3(0, 0);
    }

    void SetParticleEmmisionRate(float rate)
    {
        var emmision = LoadingParts.emission;
        emmision.rateOverTime = new ParticleSystem.MinMaxCurve(rate);
    }

    private void ApplySlow()
    {
        ISlowable iSlowable = GetComponentInParent<ISlowable>(); // Get the ISlowable component from the parent
        if (iSlowable == null)
        {
            Debug.Log(this + " did not Slow, because there is not ISlowable and with It not VelocityHandler");
            return;
        }

        iSlowable.ApplySlow(this, LaserSlow); // Apply slow effect
    }

    private void ResetSlow()
    {
        ISlowable iSlowable = GetComponentInParent<ISlowable>(); // Get the ISlowable component from the parent

        if (iSlowable != null) iSlowable.ResetSlow(this); // Reset the slow effect
    }

    float progressTime;
    void HandleProgressBar()
    {
        progressTime += Time.deltaTime;
        progressTime = Mathf.Clamp(progressTime, 0, LaserLoadingTime);

        ProgressChannel.RegisterProgress(progressTime, LaserLoadingTime);

        if (progressTime >= LaserLoadingTime) ProgressChannel.RaiseProgressHighlight();
    }

    float currentShakeValue = 0;
    float elapsedShakeTime = 0;

    void HandleCameraShake()
    {
        elapsedShakeTime += Time.deltaTime;
        float t = Mathf.Clamp01(elapsedShakeTime / LaserLoadingTime);
        currentShakeValue = Mathf.Lerp(0f, MaxLoadingShakeStrength, t);

        CameraShakeChannel.RaiseShakeConstant(this, currentShakeValue);
    }
}
