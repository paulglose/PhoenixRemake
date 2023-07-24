using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CameraShakeManager : MonoBehaviour
{
    [Header("Channel")]
    [SerializeField] CameraShakeChannel CameraShakeChannel;

    [Header("Others")]
    [SerializeField] AnimationCurve shakeCurve;

    // Dictionary to store all the continuous shake sources
    Dictionary<MonoBehaviour, float> ShakeConstants = new Dictionary<MonoBehaviour, float>();
    Vector3 originalPosition;

    private void OnEnable()
    {
        CameraShakeChannel.ResetShakeConstant += ResetShakeConstant;
        CameraShakeChannel.AddShakeConstant += AddShakeConstant;
        CameraShakeChannel.AddShakeImpulse += AddShakeImpulse;
    }

    private void OnDisable()
    {
        CameraShakeChannel.ResetShakeConstant -= ResetShakeConstant;
        CameraShakeChannel.AddShakeConstant -= AddShakeConstant;
        CameraShakeChannel.AddShakeImpulse -= AddShakeImpulse;
    }

    private void Awake()
    {
        originalPosition = transform.position;
    }

    private void Update()
    {
        // Handle the continuous shake
        if (ShakeConstants.Count > 0)
        {
            float x = UnityEngine.Random.Range(-1f, 1f) * GetHighestShakeConstant();
            float y = UnityEngine.Random.Range(-1f, 1f) * GetHighestShakeConstant();

            transform.localPosition = new Vector3(x, y, -100);
        }
    }

    void ResetShakeConstant(MonoBehaviour source)
    {
        ShakeConstants.Remove(source);
        transform.localPosition = originalPosition;
    }

    private IEnumerator Shake(float strength, float duration)
    {
        AnimationCurve shakeCurveCopy = shakeCurve;

        float elapsed = 0.0f;
        Vector3 lastOffset = Vector3.zero; // Store the last offset applied

        while (elapsed < duration)
        {
            float x = UnityEngine.Random.Range(-1f, 1f) * strength * shakeCurveCopy.Evaluate(elapsed / duration);
            float y = UnityEngine.Random.Range(-1f, 1f) * strength * shakeCurveCopy.Evaluate(elapsed / duration);

            Vector3 newOffset = new Vector3(x, y, 0);

            // Subtract the last offset, then add the new one
            transform.localPosition += newOffset - lastOffset;

            // Store the new offset as the lastOffset for the next iteration
            lastOffset = newOffset;

            elapsed += Time.deltaTime;

            yield return null;
        }

        // Remove the last offset when the shake is done
        transform.localPosition -= lastOffset;
    }

    // returns the highest value out of the ShakeConstants value
    public float GetHighestShakeConstant()
    {
        // Stellt sicher, dass das Wörterbuch nicht leer ist, bevor es nach dem Maximalwert sucht.
        if (ShakeConstants != null && ShakeConstants.Count > 0)
        {
            return ShakeConstants.Max(x => x.Value);
        }
        return 0;
    }

    void AddShakeConstant(MonoBehaviour source, float strength)
    {
        ShakeConstants[source] = strength;
    }

    private void AddShakeImpulse(float duration, float strength)
    {
        StartCoroutine(Shake(strength, duration));
    }
}
