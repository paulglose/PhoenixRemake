using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Channel/CameraShakeChannel")]
public class CameraShakeChannel : UnityEngine.ScriptableObject
{
    public event Action<float, float> AddShakeImpulse;
    public void RaiseShakeImpulse(float duration, float strength) => AddShakeImpulse?.Invoke(duration, strength);

    public event Action<MonoBehaviour, float> AddShakeConstant;
    public void RaiseShakeConstant(MonoBehaviour source, float strength) => AddShakeConstant?.Invoke(source, strength);

    public event Action<MonoBehaviour> ResetShakeConstant;
    public void RaiseShakeConstantReset(MonoBehaviour source) => ResetShakeConstant?.Invoke(source);
}
