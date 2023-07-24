using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SoundSlider : MonoBehaviour
{
    [Header("Channel")]
    [SerializeField] SoundChannel SoundChannel;
    [SerializeField] VoidEvent DefaultSound;

    [Header("Configuration")]
    [SerializeField] SoundType SoundType;

    [Header("Others")]
    [SerializeField] TextMeshProUGUI percantageSlider;
    [SerializeField] Slider slider;


    void OnEnable() 
    {
        DefaultSound.OnEventRaised += DefaultSoundSlider;
    }

    void OnDisable() {}

    void Awake() 
    {
        LoadVolumes();
    }

    void Start() 
    {
        
    }

    float prevSliderValue = 10;
    void Update() 
    {
        if (prevSliderValue != slider.value)
        {
            // Apply the value
            prevSliderValue = slider.value;
            percantageSlider.text = ((int)(slider.value * 100)).ToString() + "%";
            SoundChannel.ChangeVolume(SoundType, slider.value);
        }
    }

    void LoadVolumes()
    {
        switch (SoundType)
        {
            case SoundType.Interface: slider.value = PlayerPrefs.GetFloat("InterfaceVolume"); break;
            case SoundType.Music: slider.value = PlayerPrefs.GetFloat("MusicVolume"); break;
            case SoundType.Player: slider.value = PlayerPrefs.GetFloat("PlayerVolume"); break;
            case SoundType.Enemy: slider.value = PlayerPrefs.GetFloat("EnemyVolume"); break;
            case SoundType.General: slider.value = PlayerPrefs.GetFloat("GeneralVolume"); break;
        }
    }

    void DefaultSoundSlider()
    {
        prevSliderValue = 1;
        slider.value = 1;
        percantageSlider.text = "100%";
        SoundChannel.ChangeVolume(SoundType, 1);
    }
}
