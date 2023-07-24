using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    [Header("Channel")]
    [SerializeField] InputReader InputReader;
    [SerializeField] TimeScaleChannel TimeScaleChannel;
    [SerializeField] VoidEvent OnBattleSceneLoaded;

    [Header("Configuration")]
    [SerializeField] float transitionTime;

    Animator anim;

    void OnEnable() 
    {
        InputReader.PausePerformed += TriggerPause;
    }

    void OnDisable() 
    { 
        InputReader.PausePerformed -= TriggerPause;
    }

    void Awake() {}

    void Start() 
    {
        anim = GetComponent<Animator>();
        OnBattleSceneLoaded.OnEventRaised += Initialize;
    }

    void Update() {}

    bool paused = false;
    public void TriggerPause()
    {
        if (!paused)
        {
            InputReader.SwitchToUI();
            paused = true;
            TimeScaleChannel.RequestTimeScale(this, 0f);
            anim.CrossFade("Show", 0.1f);
        } else
        {
            InputReader.SwitchToPlayer();
            paused = false;
            TimeScaleChannel.ResetTimeScale(this);
            anim.CrossFade("Hide", 0.1f);
        }
    }

    public void Initialize()
    {
        InputReader.SwitchToPlayer();
        paused = false;
        TimeScaleChannel.ResetTimeScale(this);
        anim.CrossFade("Hide", 0f);
    }

    public void SwitchToAnimationState(string stateName) =>
        anim.CrossFade(stateName, transitionTime);
}
