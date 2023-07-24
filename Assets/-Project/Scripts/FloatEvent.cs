using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Events/Float Event")]
public class FloatEvent : UnityEngine.ScriptableObject
{
    public event Action<float> OnEventRaised;
    public void RaiseEvent(float number) => OnEventRaised?.Invoke(number);
}
