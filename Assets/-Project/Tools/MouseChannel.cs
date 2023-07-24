using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Channel/MouseChannel")]
public class MouseChannel : ScriptableObject
{
    public Vector3 Position;
    public CircleCollider2D Collider;

    public Vector3 GetRandomAccuracyPoint()
    {
        Vector3 origin = Collider.transform.position;
        Vector3 additional = new Vector3(0, UnityEngine.Random.Range(0, Collider.radius), 0);
        additional = Quaternion.AngleAxis(UnityEngine.Random.Range(0, 360), Vector3.forward) * additional;

        return origin + additional;
    }

    public event Action<float> OnAccuracyRadiusRegistered;
    public void RaiseAccuracyRadius(float radius) => OnAccuracyRadiusRegistered?.Invoke(radius);
    public void RaiseAccuracyReset() => OnAccuracyRadiusRegistered?.Invoke(0); // 0 So the Accuraxy Object knows that it should get back to default

    public event Action<bool> OnRotatationSwitched;
    public void RaiseRotationSwitch(bool to) => OnRotatationSwitched?.Invoke(to);
}
