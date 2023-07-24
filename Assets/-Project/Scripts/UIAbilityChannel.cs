using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Channel/UIAbilityChannel")]
public class UIAbilityChannel : ScriptableObject
{
    public event Action<AbilityPackage> OnAbilityRegistered;
    public void RegisterAbility(AbilityPackage package) => OnAbilityRegistered?.Invoke(package);

    public event Action<float> OnCooldownRegistered;
    public void RegisterCooldown(float cooldown) => OnCooldownRegistered?.Invoke(cooldown);

    public event Action OnAvailableRegistered;
    public void RegisterAvailable() => OnAvailableRegistered?.Invoke();

    public event Action OnCastingRegistered;
    public void RegisterCasting() => OnCastingRegistered?.Invoke();

    public event Action OnCooldownPaused;
    public void PauseCooldown() => OnCooldownPaused?.Invoke();

    public event Action OnCooldownContinued;
    public void ContinueCooldown() => OnCooldownContinued?.Invoke();
}
