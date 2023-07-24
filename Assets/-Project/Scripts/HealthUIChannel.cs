using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Channel/HealthUIChannel")]
public class HealthUIChannel : ScriptableObject
{
    public event Action<int, int> OnHealthRegistered;
    public void RaiseHealth(int current, int max) => OnHealthRegistered?.Invoke(current, max);
}
