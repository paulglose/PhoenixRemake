using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName =("Channel/StageDisplayChannel"))]
public class StageDisplayChannel : ScriptableObject
{
    public event Action<int> OnStageEntered;
    public void RaiseStage(int stageCount) => OnStageEntered?.Invoke(stageCount);
}
