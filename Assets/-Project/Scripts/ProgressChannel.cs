using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Channel/ProgressChannel")]
public class ProgressChannel : ScriptableObject
{
    public event Action<float, float> OnProgressRegistered;
    public void RegisterProgress(float currentValue, float maxValue) => OnProgressRegistered?.Invoke(currentValue, maxValue);

    public event Action<Material> OnProgressMaterialRegistered;
    public void RaiseProgressMaterial(Material mat) => OnProgressMaterialRegistered?.Invoke(mat);

    public event Action OnProgressCanceled;
    public void RaiseProgressCanceled() => OnProgressCanceled?.Invoke();

    public event Action OnProgressHighlighted;
    public void RaiseProgressHighlight() => OnProgressHighlighted?.Invoke();
}
