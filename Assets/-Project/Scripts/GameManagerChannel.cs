using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Channel/GameManagerChannel")]
public class GameManagerChannel : ScriptableObject
{
    public event Action OnConstantInitializeRequested;
    public void RequestConstantInitialization() => OnConstantInitializeRequested?.Invoke();

}
