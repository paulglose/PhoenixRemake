using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Is the Channel to communikate between Upgradeables and CardEntries. CardEntries RaiseUpgrades. 
/// </summary>
[CreateAssetMenu(menuName ="Channel/UpgradeableChannel")]
public class UpgradeChannel : ScriptableObject
{
    public event Action<Upgrade> OnUpgradeChoosen; // Used by all Upgradeables to check if he has the upgrade
    public void RaiseUpgrade(Upgrade upgrade) => OnUpgradeChoosen?.Invoke(upgrade); // Used by the Cards to Raise an Upgrade Event

    public void RegisterUpgrade(Upgrade upgrade) => OnUpgradeRegistered?.Invoke(upgrade);
    public event Action<Upgrade> OnUpgradeRegistered;

    public event Action OnUpgradesRerolled;
    public void RaiseUpgradeReroll() => OnUpgradesRerolled?.Invoke();

    public event Action<UpgradePackage> OnUpgradePackageReceived;
    public void RaiseUpgradePackage(UpgradePackage package) => OnUpgradePackageReceived?.Invoke(package);
}
