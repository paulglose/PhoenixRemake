using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RerollButtonSizeManager : MonoBehaviour
{
    [Header("Configuration")]
    [SerializeField] float HoveredScale;
    [SerializeField] float DisabledScale;
    [SerializeField] float SelectedScale;
    [SerializeField] float PreselectedScale;
    [SerializeField] float EnabledScale;
    [SerializeField] float ScaleSpeed;

    [SerializeField] Transform scaleTransform;

    float currentAimedScale;


    private void OnEnable()
    {
        GetComponent<RerollButton>().OnButtonStateChanged += HandleButtonState;
    }

    private void OnDisable()
    {
        GetComponent<RerollButton>().OnButtonStateChanged -= HandleButtonState;
    }

    void HandleButtonState(RerollButtonState buttonState)
    {
        if (buttonState == RerollButtonState.Hovered) currentAimedScale = HoveredScale;
        else if (buttonState == RerollButtonState.Selected) currentAimedScale = SelectedScale;
        else if (buttonState == RerollButtonState.Enabled) currentAimedScale = EnabledScale;
        else if (buttonState == RerollButtonState.Disabled) currentAimedScale = DisabledScale;
        else if (buttonState == RerollButtonState.Preselected) currentAimedScale = PreselectedScale;
    }

    private void Update()
    {
        scaleTransform.localScale = Vector3.Lerp(scaleTransform.localScale, new Vector3(currentAimedScale, currentAimedScale, 0), ScaleSpeed * Time.deltaTime);
    }
}
