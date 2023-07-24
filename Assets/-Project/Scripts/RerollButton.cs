using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RerollButton : MonoBehaviour
{
    [Header("Channel")]
    [SerializeField] InputReader InputReader;
    [SerializeField] WaveChannel WaveChannel;
    [SerializeField] UpgradeChannel UpgradeChannel;

    [Header("Configuration")]
    [SerializeField] float SelectedDuration;

    [Header("Others")]
    [SerializeField] TextMeshProUGUI RerollsText;
    [SerializeField] Image Icon;

    public int RerollsPerRound;
    int RemainingRerolls;
    Animator anim;

    public event Action<RerollButtonState> OnButtonStateChanged;

    [SerializeField] RerollButtonState currentButtonState;
    void SwitchButtonState(RerollButtonState aimedState)
    {
        currentButtonState = aimedState;
        OnButtonStateChanged?.Invoke(aimedState);
    }

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    private void OnMouseOver()
    {
        if (currentButtonState == RerollButtonState.Selected || currentButtonState == RerollButtonState.Disabled) return;

        if (currentButtonState == RerollButtonState.Enabled) SwitchButtonState(RerollButtonState.Hovered);
    }

    void OnMouseExit()
    {
        if (currentButtonState == RerollButtonState.Selected || currentButtonState == RerollButtonState.Disabled) return;

        SwitchButtonState(RerollButtonState.Enabled);
    }

    private void OnEnable()
    {
        InputReader.ClickPerformed += HandleClickPerformed;
        InputReader.ClickCanceled += HandleClickCanceled;
        WaveChannel.OnWaveCompleted += OnWaveCompleted;
        WaveChannel.OnWaveCompleted += InitializeButton;
        WaveChannel.OnUpgradeSelected += OnUpgradeSelected;
    }

    private void OnDisable()
    {
        InputReader.ClickPerformed -= HandleClickPerformed;
        InputReader.ClickCanceled -= HandleClickCanceled;
        WaveChannel.OnWaveCompleted -= OnWaveCompleted;
        WaveChannel.OnWaveCompleted -= InitializeButton;
        WaveChannel.OnUpgradeSelected -= OnUpgradeSelected;
    }

    private void Start()
    {
        InitializeButton();
    }

    void OnWaveCompleted() => anim.CrossFade("Shown", .5f);

    void OnUpgradeSelected() => anim.CrossFade("Hidden", .5f);

    void InitializeButton()
    {
        RemainingRerolls = RerollsPerRound;
        RerollsText.text = RemainingRerolls.ToString();

        SwitchButtonState(RerollButtonState.Enabled);
    }

    void HandleClickPerformed()
    {
        if (currentButtonState == RerollButtonState.Hovered) SwitchButtonState(RerollButtonState.Preselected);
    }

    void HandleClickCanceled()
    {
        if (currentButtonState == RerollButtonState.Preselected)
        {
            SwitchButtonState(RerollButtonState.Selected);
            StartCoroutine(SelectedRoutine());
        }
    }

    IEnumerator SelectedRoutine()
    {
        RemainingRerolls -= 1;
        RerollsText.text = RemainingRerolls.ToString();
        UpgradeChannel.RaiseUpgradeReroll();

        yield return new WaitForSeconds(SelectedDuration);
        if (RemainingRerolls > 0) SwitchButtonState(RerollButtonState.Enabled);
        else SwitchButtonState(RerollButtonState.Disabled);
    }
}
