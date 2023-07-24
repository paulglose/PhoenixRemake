using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WaveCounter : MonoBehaviour, IGameObjectConstant
{
    [Header("Channel")]
    [SerializeField] WaveChannel WaveChannel;
    [SerializeField] VoidEvent OnGameOver;

    [Header("Others")]
    [SerializeField] TextMeshProUGUI text;

    public void Initialize()
    {
        text.text = "1";
    }

    int localCounter = 0;
    private void OnEnable()
    {
        WaveChannel.OnUpgradeSelected += HandleStageDisplay;
        OnGameOver.OnEventRaised += HandleStageHighscoreSaving;
    }

    private void OnDisable()
    {
        WaveChannel.OnUpgradeSelected -= HandleStageDisplay;
    }

    void HandleStageDisplay()
    {
        localCounter++;

        if (localCounter == 3)
        {
            localCounter = 0;
            text.text = (int.Parse(text.text) + 1).ToString();
        }
    }

    private void HandleStageHighscoreSaving()
    {
        int highestStage = PlayerPrefs.GetInt("HighestStage");
        int currentStage = int.Parse(text.text);

        if (currentStage > highestStage) PlayerPrefs.SetInt("HighestStage", currentStage);
    }
}
