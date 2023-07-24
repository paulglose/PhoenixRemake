using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuantumDash : Upgradeable, IVelocityInfluence
{
    [Header("Stats")] 
    public Stat DashPower;
    public Stat MovementSpeed;
    public Stat DashCooldown;

    [Header("Channel")]
    public InputReader InputReader;
    public UIAbilityChannel AbilityChannel;
    [SerializeField] PlayerControllerChannel PlayerControllerChannel;

    public event Action<MonoBehaviour, Vector2, VelocityType> RegisterVelocity;
    public event Action<MonoBehaviour> ResetVelocity;

    [Header("Other")]
    [SerializeField] AbilityPackage UIPackage;

    Vector2 direction;

    protected override void OnEnable()
    {
        InputReader.MovementPerformed += MovementPerformed;
        InputReader.MobilityPerformed += ExecuteDash;
    }

    protected override void OnDisable()
    {
        InputReader.MovementPerformed -= MovementPerformed;
        InputReader.MobilityPerformed -= ExecuteDash;
    }

    protected override void Start()
    {
        AbilityChannel.RegisterAbility(UIPackage);
    }

    void MovementPerformed(Vector2 vector) => direction = vector;

    float aimedDashTime = 0;
    void ExecuteDash()
    {
        if (direction == Vector2.zero || Time.time < aimedDashTime) return;

        aimedDashTime = Time.time + DashCooldown;
        AbilityChannel.RegisterCooldown(DashCooldown);
        RegisterVelocity?.Invoke(this, direction * MovementSpeed * DashPower.Value, VelocityType.Burst);
    }

    void BoostMovement() => StartCoroutine(BoostingCoroutine());

    IEnumerator BoostingCoroutine()
    {
        PlayerControllerChannel.BoostMovement();
        yield return new WaitForSeconds(4f);
        PlayerControllerChannel.NormalizeMovement();
    }
}
