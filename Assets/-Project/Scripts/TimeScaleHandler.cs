using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TimeScaleHandler : MonoBehaviour
{
    [Header("Channels")]
    [SerializeField] TimeScaleChannel TimeScaleChannel;

    Dictionary<MonoBehaviour, float> timeScaleDict = new Dictionary<MonoBehaviour, float>();

    private void OnEnable()
    {
        TimeScaleChannel.OnTimeScaleRequested += OnTimeScaleRequested;
        TimeScaleChannel.OnTimeScaleReseted += OnTimeScaleReseted;
    }

    private void OnDisable()
    {
        TimeScaleChannel.OnTimeScaleRequested -= OnTimeScaleRequested;
        TimeScaleChannel.OnTimeScaleReseted -= OnTimeScaleReseted;
    }

    private void Awake()
    {
        this.fixedDeltaTime = Time.fixedDeltaTime;
    }

    void Update()
    {
        CleanTimeScaleDictionary();
        HandleCurrentTimeScale();
    }

    void OnTimeScaleRequested(MonoBehaviour source, float slowFactor) => timeScaleDict[source] = slowFactor;

    void OnTimeScaleReseted(MonoBehaviour source) => timeScaleDict.Remove(source);

    float fixedDeltaTime;
    public void HandleCurrentTimeScale()
    {
        if (timeScaleDict.Count == 0 && Time.timeScale != 1)
        {
            Time.timeScale = 1;
            Time.fixedDeltaTime = 0.02f;
        }

        else if (timeScaleDict.Count != 0)
        {
            Time.timeScale = timeScaleDict.Values.Max();
            Time.fixedDeltaTime = this.fixedDeltaTime * Time.timeScale;
        }
    }

    void CleanTimeScaleDictionary() =>
        timeScaleDict = timeScaleDict.Where(pair => pair.Key != null).ToDictionary(pair => pair.Key, pair => pair.Value);
}
