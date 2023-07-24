using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// NAMES MUST NOT BE CHANGED BECAUSE THEIR NAME IS ASSERTED 
public enum SoundType { Interface, Music, Player, Enemy, General }

public class SoundData
{
    public AudioClip AudioClip;
    public SoundType Type;
    public float Volume;

    public SoundData(AudioClip AudioClip, SoundType Type, float Volume)
    {
        this.AudioClip = AudioClip;
        this.Type = Type;
        this.Volume = Volume;
    }
}

[CreateAssetMenu(menuName ="Channel/SoundChannel")]
public class SoundChannel : ScriptableObject
{
    public void RequestSound(SoundData data) => OnSoundRequested?.Invoke(data);
    public event Action<SoundData> OnSoundRequested;

    public void ChangeVolume(SoundType type, float newValue) => OnVolumeChanged?.Invoke(type, newValue);
    public event Action<SoundType, float> OnVolumeChanged;
}
