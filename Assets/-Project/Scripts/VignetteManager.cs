using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

/// <summary>
/// Handles the vVignette effect
/// </summary>
[RequireComponent(typeof(Volume))]
public class VignetteManager : MonoBehaviour
{
    [Header("Configurations")]
    [SerializeField] float MaxIntensity;
    [SerializeField] float IntensityDecay;
    [SerializeField] float IntensityOnLowHp;
    [SerializeField] float IntensityIncreaseUponDamage;
    [SerializeField] int LowLifeAtHealth;

    [Header("Channel")]
    [SerializeField] VignetteChannel VignetteChannel;

    Volume volume;
    Vignette vignette;
    float currentPlayerHealth = 2;

    private void OnEnable()
    {
        VignetteChannel.OnHealthChanged += OnHealthChanged;
    }

    private void OnDisable()
    {
        VignetteChannel.OnHealthChanged -= OnHealthChanged;
    }

    private void Awake()
    {
        volume = GetComponent<Volume>();
    }

    private void Update()
    {
        // Handle the Intensity
        if (volume.profile.TryGet(out vignette))
        {
            float clampValue = (currentPlayerHealth > LowLifeAtHealth)? 0: IntensityOnLowHp;
            vignette.intensity.value = Mathf.Clamp(vignette.intensity.value - Time.deltaTime * IntensityDecay, clampValue, MaxIntensity);
        }
    }

    void OnHealthChanged(int currentHealth)
    {
        if (currentHealth < currentPlayerHealth)
        {
            // Increase Vignette
            if (volume.profile.TryGet(out vignette))
            {
                vignette.intensity.value = Mathf.Clamp(vignette.intensity.value + IntensityIncreaseUponDamage, 0, MaxIntensity);
            }
        }

        currentPlayerHealth = currentHealth;
    }
}
