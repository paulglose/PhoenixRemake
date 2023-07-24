using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Upgradeable : MonoBehaviour
{
    [Header("Upgradeable")]
    [SerializeField] UpgradeChannel UpgradeableChannel;
    [SerializeField] protected List<Upgrade> Upgrades;

    protected virtual void OnEnable()
    {
        UpgradeableChannel.OnUpgradeChoosen += CheckUpgrade;
    }

    protected virtual void OnDisable()
    {
        UpgradeableChannel.OnUpgradeChoosen -= CheckUpgrade;
    }

    protected virtual void Awake()
    {
        // Get Own Upgrades
        var tempList = new List<Upgrade>(Resources.LoadAll<Upgrade>("Upgrades"));
        foreach(Upgrade upgrade in tempList)
        {
            foreach(string className in upgrade.AccordingUpgradeables)
                if (className == GetType().Name) Upgrades.Add(upgrade);
        }
    }

    protected virtual void Start()
    {
        foreach (Upgrade upgrade in Upgrades)
        {
            UpgradeableChannel.RegisterUpgrade(upgrade);
        }
    }

    void CheckUpgrade(Upgrade upgrade)
    {
        if (Upgrades.Contains(upgrade))
        {
            upgrade.ApplyUpgrade(this);
        }
    }
}
