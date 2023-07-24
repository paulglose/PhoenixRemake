using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Channel/TimeScaleChannel")]
public class TimeScaleChannel : ScriptableObject
{
    public event Action<MonoBehaviour, float> OnTimeScaleRequested;
    public void RequestTimeScale(MonoBehaviour source, float strength) => OnTimeScaleRequested?.Invoke(source, strength);

    public event Action<MonoBehaviour> OnTimeScaleReseted;
    public void ResetTimeScale(MonoBehaviour source) => OnTimeScaleReseted?.Invoke(source);
}
