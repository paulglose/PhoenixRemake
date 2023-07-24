using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Events/Int Event")]
public class IntEvent : UnityEngine.ScriptableObject
{
    public event Action<int> OnEventRaised;
    public void RaiseEvent(int number) => OnEventRaised?.Invoke(number);
}
