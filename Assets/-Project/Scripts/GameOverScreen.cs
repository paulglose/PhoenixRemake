using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverScreen : MonoBehaviour
{
    [Header("Channel")]
    [SerializeField] InputReader InputReader;
    [SerializeField] VoidEvent GameOver;

    [Header("Configuration")]
    [SerializeField] float CrossFadeTime;

    Animator anim;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        GameOver.OnEventRaised += GameOverRoutine;
    }

    private void OnDisable()
    {
        GameOver.OnEventRaised -= GameOverRoutine;
    }

    void GameOverRoutine() => Invoke("FadeIn", 1f);

    void FadeIn()
    {
        InputReader.SwitchToUI();
        anim.CrossFade("FadeIn", CrossFadeTime);
    }

    public void Reset()
    {
        Invoke("Idle", 0.5f);
    }

    void Idle()
    {
        anim.CrossFade("Idle", 0f);
    }
}
