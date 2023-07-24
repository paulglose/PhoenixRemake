using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Channel/VignetteChannel")]
public class VignetteChannel : ScriptableObject
{
    public event Action<int> OnHealthChanged;
    public void RaiseHealthChanged(int currentHealth) => OnHealthChanged?.Invoke(currentHealth);
}
