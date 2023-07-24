using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Events/Void Event")]
public class VoidEvent: UnityEngine.ScriptableObject {
    public event Action OnEventRaised;
    public void RaiseEvent() => OnEventRaised?.Invoke();
}
