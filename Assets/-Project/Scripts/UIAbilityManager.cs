using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Is the Pacakge that is needed from the UI to configure the Ability
/// </summary>
[System.Serializable]
public class AbilityPackage
{
    public AbilityPackage(Sprite AbilitySprite)
    {
        this.AbilitySprite = AbilitySprite;
    }

    public Sprite AbilitySprite;
}

public class UIAbilityManager : MonoBehaviour, IGameObjectConstant
{
    [Header("Channel")]
    [SerializeField] UIAbilityChannel AbilityChannel;
    [SerializeField] WaveChannel WaveChannel;

    [Header("Configurations")]
    [SerializeField] float ScaleSpeed;

    [Header("Others")]
    public UIAbilityState CurrentUIState;
    [SerializeField] Image Border;
    [SerializeField] Image Icon;
    [SerializeField] Color CooldownBorderColor;
    [SerializeField] Color AvailableBorderColor;

    [SerializeField, Space] float AimedCooldownScale;
    [SerializeField] float AimedCastingScale;
    [SerializeField] float AimedAvailableScale;
    [SerializeField] float AimedCastingOffset;

    bool CooldownPaused;
    AbilityPackage holdingPackage;
    Animator anim;

    public void Initialize()
    {
        anim.CrossFade("Idle", 0f);
    }

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        AbilityChannel.OnAvailableRegistered += OnAvailableRegistered;
        AbilityChannel.OnCastingRegistered += OnCastingRegistered;
        AbilityChannel.OnCooldownRegistered += OnCooldownRegistered;
        AbilityChannel.OnAbilityRegistered += ApplyAbilityLook;

        AbilityChannel.OnCooldownPaused += OnCooldownPaused;
        AbilityChannel.OnCooldownContinued += OnCooldownContinued;
    }

    private void OnDisable()
    {
        AbilityChannel.OnAvailableRegistered -= OnAvailableRegistered;
        AbilityChannel.OnCastingRegistered -= OnCastingRegistered;
        AbilityChannel.OnCooldownRegistered -= OnCooldownRegistered;
        AbilityChannel.OnAbilityRegistered -= ApplyAbilityLook;

        WaveChannel.OnWaveCompleted -= OnCooldownPaused;
        WaveChannel.OnUpgradeSelected -= OnCooldownContinued;
    }

    private void Update()
    {
        TickCurrentUIState();
    }

    void OnAvailableRegistered() => SwitchUIState(UIAbilityState.Available, 0);

    void OnCastingRegistered() => SwitchUIState(UIAbilityState.Casting, 0);

    void OnCooldownRegistered(float aimedCooldown) => SwitchUIState(UIAbilityState.Cooldown, aimedCooldown);

    void OnCooldownPaused() => CooldownPaused = true;

    void OnCooldownContinued() => CooldownPaused = false;

    void SwitchUIState(UIAbilityState UIState, float Cooldown)
    {
        switch (UIState)
        {
            case UIAbilityState.Casting: StartCastingState(); break;

            case UIAbilityState.Available: StartAvailableState(); break;

            case UIAbilityState.Cooldown: StartCooldownState(Cooldown); break;
        }

        CurrentUIState = UIState;
    }

    /// <summary>
    /// Ticks the current Animation State
    /// </summary>
    void TickCurrentUIState()
    {
        switch (CurrentUIState)
        {
            case UIAbilityState.Casting:
                TickCastingState();
                break;

            case UIAbilityState.Available:
                TickAvailableState();
                break;

            case UIAbilityState.Cooldown:
                TickCooldownState();
                break;
        }
    }

    void StartCastingState() 
    {
        Border.color = Color.white;
        Border.fillAmount = 1;
    }

    void TickCastingState()
    {
        transform.localScale = Vector3.Lerp(transform.localScale, new Vector3(AimedCastingScale, AimedCastingScale, 0), ScaleSpeed * Time.deltaTime);
    }

    void StartAvailableState() 
    {
        Border.color = Color.white;
        Border.fillAmount = 1;
    }

    void TickAvailableState() 
    {
        transform.localScale = Vector3.Lerp(transform.localScale, new Vector3(AimedAvailableScale, AimedAvailableScale, 0), ScaleSpeed * Time.deltaTime);
    }

    float aimedTime;
    float currentTime;
    void StartCooldownState(float aimedCooldown) 
    {
        aimedTime = aimedCooldown;
        currentTime = 0;
        Border.color = CooldownBorderColor;
        Border.fillAmount = 0;
    }

    void TickCooldownState() 
    {
        if (!CooldownPaused)
            currentTime += Time.deltaTime;

        if (currentTime > aimedTime) SwitchUIState(UIAbilityState.Available, 0);

        transform.localScale = Vector3.Lerp(transform.localScale, new Vector3(AimedCooldownScale, AimedCooldownScale, 0), ScaleSpeed * Time.deltaTime);
        Border.fillAmount = currentTime / aimedTime;
    }

    void ApplyAbilityLook(AbilityPackage package)
    {
        holdingPackage = package;
        Icon.sprite = package.AbilitySprite;
        if (!anim.GetCurrentAnimatorStateInfo(0).IsName("FadeIn")) anim.CrossFade("FadeIn", 0f);
    }
}
