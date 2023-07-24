using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerOnHitFeedback : MonoBehaviour
{
    Material InitialMaterial;

    [Header("Stats")]
    [SerializeField] float MaterialSwitchDuration = 0.1f;
    
    [Header("Others")]
    [SerializeField] Material RedMaterial;
    [SerializeField] VoidEvent OnPlayerDamage;

    SpriteRenderer sr;

    private void OnEnable()
    {
        OnPlayerDamage.OnEventRaised += OnEventRaised;
    }

    private void OnDisable()
    {
        OnPlayerDamage.OnEventRaised -= OnEventRaised;
    }

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        InitialMaterial = sr.material;
    }

    void OnEventRaised() => StartCoroutine(OnDamaged());

    IEnumerator OnDamaged()
    {
        sr.material = RedMaterial;
        yield return new WaitForSeconds(MaterialSwitchDuration);
        sr.material = InitialMaterial;
    }
}
