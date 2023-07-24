using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Channel/WaveChannel")]
public class WaveChannel : UnityEngine.ScriptableObject
{
    public event Action OnUpgradeSelected;
    public void RaiseCardSelected() => OnUpgradeSelected?.Invoke();

    public event Action OnWaveCompleted;
    public void RaiseWaveCompleted() => OnWaveCompleted?.Invoke();
}

