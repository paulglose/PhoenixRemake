using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Channel/PlayerControllerChannel")]
public class PlayerControllerChannel : ScriptableObject
{
    public void DisableMovement() => OnMovementDisabled?.Invoke();
    public event Action OnMovementDisabled;

    public void EnableMovement() => OnMovementEnabled?.Invoke();
    public event Action OnMovementEnabled;

    public void BoostMovement() => OnMovementBoosted?.Invoke();
    public event Action OnMovementBoosted;

    public void NormalizeMovement() => OnMovementNormalized?.Invoke();
    public event Action OnMovementNormalized;
}
