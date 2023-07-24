using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageDisplayManager : MonoBehaviour, IGameObjectConstant
{
    [Header("Channels")]
    [SerializeField] StageDisplayChannel StageDisplayChannel;

    [Header("Others")]
    [SerializeField] Image[] ImageGlows;
    [SerializeField] RectTransform PlayerIndicator;

    [Header("Configuration")]
    [SerializeField] float PlayerIndicatorSpeed;

    public void Initialize()
    {
        currentStage = 1;
        HandleStageEntering(currentStage);
    }

    private void OnEnable()
    {
        StageDisplayChannel.OnStageEntered += HandleStageEntering;
    }

    private void OnDisable()
    {
        StageDisplayChannel.OnStageEntered -= HandleStageEntering;
    }

    private void Update()
    {
        HandlePlayerIndexPosition();    
    }

    void HandlePlayerIndexPosition()
    {
        PlayerIndicator.localPosition = Vector3.Lerp(PlayerIndicator.localPosition, new Vector3(ImageGlows[currentStage-1].rectTransform.parent.localPosition.x, PlayerIndicator.localPosition.y), PlayerIndicatorSpeed * Time.deltaTime);
    }

    int currentStage = 1;
    void HandleStageEntering(int stage)
    {
        currentStage = stage;

        if (currentStage == 1)
            foreach (Image image in ImageGlows) image.enabled = true;

        for(int i = 0; i < currentStage - 1; i++)
        {
            ImageGlows[i].enabled = false;
        }
    }
}
