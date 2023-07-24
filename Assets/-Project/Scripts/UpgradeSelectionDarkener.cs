using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeSelectionDarkener : MonoBehaviour
{
    Image image;
    [Header("Channel")]
    [SerializeField] WaveChannel WaveChannel;

    [Header("Configuration")]
    [SerializeField] float aimedAlphaValue;
    [SerializeField] float fadeSpeed;

    private void OnEnable()
    {
        WaveChannel.OnWaveCompleted += OnWaveCompleted;
        WaveChannel.OnUpgradeSelected += OnUpgradeSelected;
    }

    private void OnDisable()
    {
        WaveChannel.OnWaveCompleted -= OnWaveCompleted;
        WaveChannel.OnUpgradeSelected -= OnUpgradeSelected;
    }

    void Awake()
    {
        image = GetComponent<Image>();
    }

    void OnWaveCompleted() => IsCovering = true;

    void OnUpgradeSelected() => IsCovering = false;

    bool IsCovering;
    void Update()
    {
        Color color = image.color;
        color.a = IsCovering ? Mathf.Lerp(color.a, aimedAlphaValue, fadeSpeed * Time.deltaTime) : Mathf.Lerp(color.a, 0, fadeSpeed * Time.deltaTime);
        image.color = color;
    }
}
