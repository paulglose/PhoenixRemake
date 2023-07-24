using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyConfiguration
{
    /// <summary>
    /// The Tag for what the EnemyController searches for, to move inside
    /// </summary>
    public string AreaTag;

    /// <summary>
    /// The Damage dealt by that Unit
    /// </summary>
    public float DamageMultiplier = 1f;

    /// <summary>
    /// Determines if the Enemy is allowed to attack in any way
    /// </summary>
    public bool CanAttack;
    
    /// <summary>
    /// The Health Multiplier, that determines the amount of health
    /// </summary>
    public float HealthMultiplier = 1f;

    /// <summary>
    /// How fast the Enemy can Move, 0 if it cant at all
    /// </summary>
    public float MoveSpeedMultiplier = 1f;

    /// <summary>
    /// If the Healthbar Should be displayed or not
    /// </summary>
    public bool HealthBarEnabled = true;

    /// <summary>
    /// Determines the ControllerType the Enemycontroller will make. 
    /// Default Type, keeps the Controller Unchanged, and will take the ControllerType that it has currently Equiped
    /// </summary>
    public EnemyControllerType ControllerType;

    public EnemyConfiguration(string AreaTag, bool CanAttack, float DamageMultiplier, float HealthMultiplier, float MoveSpeedMultiplier, EnemyControllerType ControllerType, bool HealthbarEnabled)
    {
        this.AreaTag = AreaTag;
        this.DamageMultiplier = DamageMultiplier;
        this.CanAttack = CanAttack;
        this.HealthMultiplier = HealthMultiplier;
        this.MoveSpeedMultiplier = MoveSpeedMultiplier;
        this.ControllerType = ControllerType;
        this.HealthBarEnabled = HealthbarEnabled;
    }
}

/// <summary>
/// Used by the WaveHandler, to spawn Enemies with specific properties. Like in the 3rd wave should be an elite enemy
/// </summary>
[RequireComponent(typeof(UnitSpawner))]
public class Enemy : MonoBehaviour
{
    [Header("Configurations")]
    public int RequiredWaveCyclus;
    public int Value;
    public bool isBoss;

    [HideInInspector] public EnemyConfiguration Configuration;
    [HideInInspector] public BoxCollider2D Area;

    #region Constructors

    public virtual void AsWaveUnit()
    {
        Configuration = new EnemyConfiguration("EnemyArea", true, 1f, 1f, 1f, EnemyControllerType.Default, true);
        InitializeArea();
        InitializeUnitSpawner();
    }

    public virtual void AsDummy()
    {
        Configuration = new EnemyConfiguration("EnemyArea", false, 0, 10000, 0, EnemyControllerType.None, false);
        InitializeArea();
        InitializeUnitSpawner();
    }

    public virtual void AsTutorialUnit() { }
    #endregion 

    void InitializeUnitSpawner()
    {
        GetComponent<UnitSpawner>().enabled = true;
        foreach (MonoBehaviour behaviour in GetComponent<UnitSpawner>().componentsToEnable) behaviour.enabled = false;
    }

    void InitializeArea() =>
         Area = GameObject.FindWithTag(GetComponent<Enemy>().Configuration.AreaTag).GetComponent<BoxCollider2D>();
}