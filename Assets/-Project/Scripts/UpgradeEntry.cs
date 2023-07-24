using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class UpgradeEntry : MonoBehaviour, IGameObjectConstant
{
    [Header("Channel")]
    [SerializeField] UpgradeEntryHoverHandler HoverManager;
    [SerializeField] WaveChannel WaveChannel;
    [SerializeField] InputReader InputReader;
    [SerializeField] UpgradeChannel UpgradeChannel;

    [Header("Configuration")]
    [SerializeField] int EntryIndex;
    [SerializeField] float delayBetweenEntries;

    [Header("Others")]
    [SerializeField] Vector2 DescriptionHeights;
    [SerializeField] Upgrade HoldingUpgrade;
    [SerializeField] TextMeshProUGUI UpgradeSource;
    [SerializeField] TextMeshProUGUI UpgradeDescription;
    [SerializeField] TextMeshProUGUI UpgradeType;
    [SerializeField] GameObject[] RareTransforms;
    [SerializeField] GameObject[] CommonTransforms;
    [SerializeField] GameObject[] LegendaryTransforms;
    [SerializeField] Image[] Borders;
    [SerializeField] Material commonMat;
    [SerializeField] Material rareMat;
    [SerializeField] Material legendaryMat;
    [SerializeField] VideoPlayer videoPlayer;
    [SerializeField] Material GreenMat;
    [SerializeField] Material WhiteMat;
    [SerializeField] Material BlueMat;
    [SerializeField] Material RedMat;
    [SerializeField] Material PurpleMat;

    public ButtonPhase CurrentCardPhase;
    public event Action<ButtonPhase> OnCardPhaseChanged; // Childs or Siblings can Sub to that event to determines their 

    Animator anim;

    public void Initialize()
    {
        anim.CrossFade("Hidden", 0f);
    }

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    public void OnEnable()
    {
        UpgradeChannel.OnUpgradePackageReceived += OnUpgradePackageReceived;
        UpgradeChannel.OnUpgradesRerolled += OnUpgradesRerolled;

        WaveChannel.OnUpgradeSelected += HandleUpgradeFadeOut;
        WaveChannel.OnWaveCompleted += OnWaveCompleted;

        HoverManager.OnUpgradeEnter += HandleUpgradeEnter;
        HoverManager.OnUpgradeExit += HandleUpgradeMouseExit;

        InputReader.ClickPerformed += HandleClickPerformed;
    }

    private void OnDisable()
    {
        UpgradeChannel.OnUpgradePackageReceived -= OnUpgradePackageReceived;
        UpgradeChannel.OnUpgradesRerolled -= OnUpgradesRerolled;

        WaveChannel.OnUpgradeSelected -= HandleUpgradeFadeOut;
        WaveChannel.OnWaveCompleted -= OnWaveCompleted;

        HoverManager.OnUpgradeEnter -= HandleUpgradeEnter;
        HoverManager.OnUpgradeExit -= HandleUpgradeMouseExit;

        InputReader.ClickPerformed -= HandleClickPerformed;
    }

    void OnUpgradesRerolled() => Invoke("RerollUpgrade", (EntryIndex + 1) * delayBetweenEntries);

    void OnWaveCompleted() => StartCoroutine(OnUpgradeFadeIn());

    void OnUpgradePackageReceived(UpgradePackage package) => HoldingUpgrade = package.Upgrades[EntryIndex];

    // Called by Animation Event
    public void VisuallyApplyUpgrade()
    {
        // Apply Borders
        if (HoldingUpgrade.UpgradeRarity == UpgradeRarity.Common)
        {
            foreach (Image border in Borders) border.material = commonMat;
            foreach (GameObject obj in CommonTransforms) obj.SetActive(true);
            foreach (GameObject obj in RareTransforms) obj.SetActive(false);
            foreach (GameObject obj in LegendaryTransforms) obj.SetActive(false);
        }

        if (HoldingUpgrade.UpgradeRarity == UpgradeRarity.Rare)
        {
            foreach (Image border in Borders) border.material = rareMat;
            foreach (GameObject obj in CommonTransforms) obj.SetActive(false);
            foreach (GameObject obj in RareTransforms) obj.SetActive(true);
            foreach (GameObject obj in LegendaryTransforms) obj.SetActive(false);
        }

        if (HoldingUpgrade.UpgradeRarity == UpgradeRarity.Legendary)
        {
            foreach (Image border in Borders) border.material = legendaryMat;
            foreach (GameObject obj in CommonTransforms) obj.SetActive(false);
            foreach (GameObject obj in RareTransforms) obj.SetActive(false);
            foreach (GameObject obj in LegendaryTransforms) obj.SetActive(true);
        }

        // Apply Video and Description Text Height
        if (HoldingUpgrade.video != null)
        {
            videoPlayer.GetComponent<RawImage>().enabled = true;
            videoPlayer.clip = HoldingUpgrade.video;
            UpgradeDescription.rectTransform.anchoredPosition = new Vector2(UpgradeDescription.rectTransform.anchoredPosition.x, DescriptionHeights.y);
        }
        else
        {
            videoPlayer.GetComponent<RawImage>().enabled = false;
            UpgradeDescription.rectTransform.anchoredPosition = new Vector2(UpgradeDescription.rectTransform.anchoredPosition.x, DescriptionHeights.x);
        }
        StopAndResetVideo();

        UpgradeType.text = HoldingUpgrade.AbilitySource.ToString();

        UpgradeDescription.text = HoldingUpgrade.Description;
    }

    void SwitchToCardPhase(ButtonPhase aimPhase)
    {
        CurrentCardPhase = aimPhase;
        OnCardPhaseChanged?.Invoke(aimPhase);
    }

    /// <summary>
    /// Called when the Upgrade fades in
    /// </summary>
    /// <returns></returns>
    IEnumerator OnUpgradeFadeIn()
    {
        yield return new WaitUntil(() => HoldingUpgrade != null);
        SwitchToCardPhase(ButtonPhase.Idle);
        yield return new WaitForSeconds((EntryIndex + 1) * delayBetweenEntries);

        anim.CrossFade("FadeIn", 0f);
    }

    void RerollUpgrade() => anim.CrossFade("Reroll", 0f);

    /// <summary>
    /// Called when the upgrade fades out
    /// </summary>
    /// <returns></returns>
    IEnumerator OnUpgradeRejectedFadeOut()
    {
        anim.CrossFade("FadeOut", 0f);
        HoldingUpgrade = null;
        yield return null;
    }

    IEnumerator OnUpgradeSelectedFadeOut()
    {
        yield return new WaitForSeconds(1f);
        HoldingUpgrade = null;
        anim.CrossFade("FadeOut", 0f);
    }

    void HandleUpgradeFadeOut()
    {
        if (CurrentCardPhase == ButtonPhase.Selected)
            StartCoroutine(OnUpgradeSelectedFadeOut());
        else StartCoroutine(OnUpgradeRejectedFadeOut());
    }

    void HandleUpgradeMouseExit()
    {
        if (CurrentCardPhase != ButtonPhase.Selected)
        {
            StopAndResetVideo();
            SwitchToCardPhase(ButtonPhase.Idle);
            anim.CrossFade("Shown", 0f);
        }
    }

    void HandleUpgradeEnter()
    {
        if (CurrentCardPhase != ButtonPhase.Selected)
        {
            videoPlayer.Stop();
            videoPlayer.Play();
            SwitchToCardPhase(ButtonPhase.Hovered);
        }
    }

    void HandleClickPerformed()
    {
        if (CurrentCardPhase == ButtonPhase.Hovered)
        {
            SwitchToCardPhase(ButtonPhase.Selected);
            ActicateUpgrade();
        }
    }

    public void StopAndResetVideo()
    {
        videoPlayer.Play();
    }

    void ActicateUpgrade()
    {
        UpgradeChannel.RaiseUpgrade(HoldingUpgrade);
        WaveChannel.RaiseCardSelected();
    }
}
