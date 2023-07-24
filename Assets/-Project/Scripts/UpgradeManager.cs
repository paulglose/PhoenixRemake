using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradePackage
{
    public UpgradePackage(Upgrade upgrade1, Upgrade upgrade2, Upgrade upgrade3)
    {
        Upgrades[0] = upgrade1;
        Upgrades[1] = upgrade2;
        Upgrades[2] = upgrade3;
    }

    public Upgrade[] Upgrades = new Upgrade[3];
}

public class UpgradeManager : MonoBehaviour
{
    [Header("Configuration")]
    public float RareChance;
    public float LegendaryChance;

    [Header("Channel")]
    [SerializeField] UpgradeChannel UpgradeChannel;
    [SerializeField] WaveChannel WaveChannel;

    [Header("Others")]
    [SerializeField] HashSet<Upgrade> AvailableUpgrades = new HashSet<Upgrade>();
    [SerializeField] List<Upgrade> RareUpgrades;
    [SerializeField] List<Upgrade> LegendaryUpgrades;
    [SerializeField] List<Upgrade> CommonUpgrades;

    private void OnEnable()
    {
        UpgradeChannel.OnUpgradeRegistered += RegisterUpgrade;
        UpgradeChannel.OnUpgradesRerolled += RaiseUpgradePackage;
        WaveChannel.OnWaveCompleted += HandleLegendaryDraw;
        WaveChannel.OnWaveCompleted += RaiseUpgradePackage;
        UpgradeChannel.OnUpgradeChoosen += CheckUpgradeRemoval;
    }

    private void OnDisable()
    {
        UpgradeChannel.OnUpgradeRegistered -= RegisterUpgrade;
        UpgradeChannel.OnUpgradesRerolled -= RaiseUpgradePackage;
        WaveChannel.OnWaveCompleted -= HandleLegendaryDraw;
        WaveChannel.OnWaveCompleted -= RaiseUpgradePackage;
        UpgradeChannel.OnUpgradeChoosen += CheckUpgradeRemoval;
    }

    // Every 3rd Time you draw a card (so after every bossfight) all should be legendary
    int localCounter = 0;
    void RaiseUpgradePackage()
    {
        SortUpgradeLists();

        Upgrade upgrade1 = DrawUpgrade();
        Upgrade upgrade2 = DrawUpgrade();
        Upgrade upgrade3 = DrawUpgrade();

        UpgradeChannel.RaiseUpgradePackage(new UpgradePackage(upgrade1, upgrade2, upgrade3));
    }

    void SortUpgradeLists()
    {
        CommonUpgrades = new List<Upgrade>();
        RareUpgrades = new List<Upgrade>();
        LegendaryUpgrades = new List<Upgrade>();

        foreach(Upgrade upgrade in AvailableUpgrades)
        {
            if (upgrade.UpgradeRarity == UpgradeRarity.Rare) RareUpgrades.Add(upgrade);
            else if (upgrade.UpgradeRarity == UpgradeRarity.Legendary) LegendaryUpgrades.Add(upgrade);
            else CommonUpgrades.Add(upgrade);
        }
    }

    // Draws a upgrade from all the upgrade while applying the rarity
    Upgrade DrawUpgrade()
    {
        float number = Random.Range(0f, 10f);
        // If Legendary is rolled
        if (localCounter == 3 || LegendaryUpgrades.Count != 0 && number <= LegendaryChance * 10)
        {
            Upgrade choosenUpgrade = LegendaryUpgrades[Random.Range(0, LegendaryUpgrades.Count)];
            LegendaryUpgrades.Remove(choosenUpgrade);
            return choosenUpgrade;
        }

        // If Rare is rolled
        else if (RareUpgrades.Count != 0 && number <= LegendaryChance * 10 + RareChance * 10)
        {
            Upgrade choosenUpgrade = RareUpgrades[Random.Range(0, RareUpgrades.Count)];
            RareUpgrades.Remove(choosenUpgrade);
            return choosenUpgrade;
        }

        // Else Return Common
        else
        {
            Upgrade choosenUpgrade = CommonUpgrades[Random.Range(0, CommonUpgrades.Count)];
            CommonUpgrades.Remove(choosenUpgrade);
            return choosenUpgrade;
        }
    }

    // Registers an Upgrade coming from Upgradeable Classes. And adds them to the list
    void RegisterUpgrade(Upgrade ugprade)
    {
        AvailableUpgrades.Add(ugprade);
    }

    // Called when an upgrade is selected
    void CheckUpgradeRemoval(Upgrade upgrade)
    {
        if (upgrade.IsUnique) AvailableUpgrades.Remove(upgrade);
    }

    void HandleLegendaryDraw()
    {
        localCounter++;
        if (localCounter == 4) localCounter = 1;
    }
}
