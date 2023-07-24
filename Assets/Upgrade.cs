using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public abstract class Upgrade : ScriptableObject
{
    [Header("Upgrade")]
    public bool IsUnique;
    public Ability AbilitySource;
    public UpgradeRarity UpgradeRarity;
    public VideoClip video;
    public string[] AccordingUpgradeables;
    [TextArea(5, 20)] public string Description;

    public abstract void ApplyUpgrade(Upgradeable weapon);
}
