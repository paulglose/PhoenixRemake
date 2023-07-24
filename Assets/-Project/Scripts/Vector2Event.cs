using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Events/Vector2 Event")]
public class Vector2Event : UnityEngine.ScriptableObject
{
    public event Action<Vector2> OnEventRaised;
    public void RaiseEvent(Vector2 vector) => OnEventRaised?.Invoke(vector);
}
