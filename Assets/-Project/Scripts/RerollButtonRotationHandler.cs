using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RerollButtonRotationHandler : MonoBehaviour
{
    [Header("Configuration")]
    [SerializeField] float RotateSpeed;

    [Header("Others")]
    [SerializeField] Transform Icon;

    private void OnEnable()
    {
        GetComponent<RerollButton>().OnButtonStateChanged += HandleButtonState;
    }

    private void OnDisable()
    {
        GetComponent<RerollButton>().OnButtonStateChanged -= HandleButtonState;
    }

    RerollButtonState currentButtonState;
    void HandleButtonState(RerollButtonState buttonState)
    {
        currentButtonState = buttonState;
    }

    private void Update()
    {
        if (currentButtonState == RerollButtonState.Selected)
        {
            Icon.Rotate(new Vector3(0, 0, -RotateSpeed * Time.deltaTime));
        }
    }
}
