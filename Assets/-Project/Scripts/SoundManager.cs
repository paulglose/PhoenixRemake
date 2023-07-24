using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour
{
    [Header("Channel")]
    [SerializeField] SoundChannel SoundChannel;

    [Header("Others")]
    [SerializeField] AudioMixer AudioMixer;

    [SerializeField] AudioSource[] sources;

    [SerializeField] float PlayerVolumeScale = 1;
    [SerializeField] float GeneralVolumeScale = 1;
    [SerializeField] float InterfaceVolumeScale = 1;
    [SerializeField] float EnemyVolumeScale = 1;
    [SerializeField] float MusicVolumeScale = 1;

    void OnEnable() 
    {
        SoundChannel.OnSoundRequested += HandleSoundRequest;
        SoundChannel.OnVolumeChanged += HandleVolumeChange;
    }

    void OnDisable() 
    {
        SoundChannel.OnSoundRequested -= HandleSoundRequest;
        SoundChannel.OnVolumeChanged -= HandleVolumeChange;
    }

    void Awake() 
    {
        sources = GetComponentsInChildren<AudioSource>();
        LoadVolumes();
    }

    void Update() {}

    void HandleSoundRequest(SoundData soundData)
    {
        // Get and Activate an audioSource
        AudioSource source = ActivateAudioSource();

        // Get Master and Specific volume
        float targetVolumeScale = GetVolumeScaleByType(soundData.Type);

        // Calculate total volume
        float totalVolume = GeneralVolumeScale * targetVolumeScale * soundData.Volume;

        source.clip = soundData.AudioClip;
        source.volume = totalVolume;
        source.PlayOneShot(soundData.AudioClip, totalVolume);
        StartCoroutine(HandleAudioSourceRecovery(source));
    }

    AudioSource ActivateAudioSource()
    {
        foreach (AudioSource source in sources)
        {
            if (!source.enabled)
            {
                source.enabled = true;
                return source;
            }
        }

        return null; 
    }

    IEnumerator HandleAudioSourceRecovery(AudioSource source)
    {
        yield return new WaitUntil(() => !source.isPlaying);
        source.enabled = false;
    }

    private AudioMixerGroup GetAudioGroup(string groupName)
    {
        AudioMixerGroup[] audioGroups = AudioMixer.FindMatchingGroups(string.Empty);

        foreach (AudioMixerGroup group in audioGroups)
        {
            if (group.name == groupName)
            {
                return group;
            }
        }

        return null; // Group not found
    }

    public float GetVolumeScaleByType(SoundType type)
    {
        switch (type)
        {
            case SoundType.Interface: return InterfaceVolumeScale;
            case SoundType.Music: return MusicVolumeScale;
            case SoundType.Player: return PlayerVolumeScale;
            case SoundType.Enemy: return EnemyVolumeScale;
            case SoundType.General: return GeneralVolumeScale;
        }

        return 1;
    }

    void HandleVolumeChange(SoundType type, float newVolume)
    {
        switch (type)
        {
            case SoundType.Interface: InterfaceVolumeScale = newVolume; PlayerPrefs.SetFloat("InterfaceVolume", newVolume); break;
            case SoundType.Player: PlayerVolumeScale = newVolume; PlayerPrefs.SetFloat("PlayerVolume", newVolume); break;
            case SoundType.Enemy: EnemyVolumeScale = newVolume; PlayerPrefs.SetFloat("EnemyVolume", newVolume); break;
            case SoundType.Music: MusicVolumeScale = newVolume; PlayerPrefs.SetFloat("MusicVolume", newVolume); break;
            case SoundType.General: GeneralVolumeScale = newVolume; PlayerPrefs.SetFloat("GeneralVolume", newVolume); break;
        }
    }

    /// <summary>
    /// Loads the volumes out of the playerPrefs database
    /// </summary>
    void LoadVolumes()
    {
        GeneralVolumeScale = PlayerPrefs.GetFloat("GeneralVolume");
        MusicVolumeScale = PlayerPrefs.GetFloat("MusicVolume");
        PlayerVolumeScale = PlayerPrefs.GetFloat("PlayerVolume");
        InterfaceVolumeScale = PlayerPrefs.GetFloat("InterfaceVolume");
        EnemyVolumeScale = PlayerPrefs.GetFloat("EnemyVolume");
    }
}
