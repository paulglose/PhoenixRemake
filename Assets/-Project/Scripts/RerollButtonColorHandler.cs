using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RerollButtonColorHandler : MonoBehaviour
{
    [Header("Configuration")]
    [SerializeField] Color DisabledIconColor;
    [SerializeField] Color EnabledIconColor;

    [SerializeField] Color DisabledTextColor;
    [SerializeField] Color EnabledTextColor;

    [Header("Other")]
    [SerializeField] Image Icon;
    [SerializeField] TextMeshProUGUI RemainingRerollsText;

    private void OnEnable()
    {
        GetComponent<RerollButton>().OnButtonStateChanged += HandleButtonState;
    }

    private void OnDisable()
    {
        GetComponent<RerollButton>().OnButtonStateChanged -= HandleButtonState;
    }

    void HandleButtonState(RerollButtonState currentState)
    {
        if (currentState == RerollButtonState.Enabled)
        {
            Icon.color = EnabledIconColor;
            RemainingRerollsText.color = EnabledTextColor;
        }

        if (currentState == RerollButtonState.Disabled)
        {
            Icon.color = DisabledIconColor;
            RemainingRerollsText.color = DisabledTextColor;
        }
    }
}
