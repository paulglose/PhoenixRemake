using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeEntryHoverHandler : MonoBehaviour
{
    public event Action OnUpgradeEnter;
    public event Action OnUpgradeExit;

    bool EventInvoked;
    public void OnMouseOver()
    {
        if (Selectable && !EventInvoked)
        {
            EventInvoked = true;
            OnUpgradeEnter?.Invoke();
        }

        if (!Selectable && EventInvoked)
        {
            EventInvoked = false; 
            OnUpgradeExit?.Invoke();
        }
    }

    public void OnMouseExit()
    {
        if (EventInvoked) {
            EventInvoked = false;
            OnUpgradeExit?.Invoke();
        }
    }

    bool Selectable = false;
    public void MakeSelectable() => Selectable = true;
    public void MakeUnselectable() => Selectable = false;
}
