using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleIndicator : MonoBehaviour
{
    [SerializeField] RingRenderer CurrentCircle;
    [SerializeField] RingRenderer MaxCircle;
    [SerializeField] Transform CircleBackground;

    float MaxRadius;
    float TimeUntilFull;
    float TimeUntilDestruction;
    public CircleIndicator Init(float MaxRadius, float TimeUntilFull, float TimeUntilDestruction)
    {
        this.MaxRadius = MaxRadius;
        this.TimeUntilFull = TimeUntilFull;
        this.TimeUntilDestruction = TimeUntilDestruction;

        MaxCircle.currentRadius = MaxRadius;
        CurrentCircle.currentRadius = 0;

        return this;
    }

    void OnEnable() 
    { }

    void OnDisable() 
    { }

    void Awake() 
    { }

    void Start() 
    { }

    bool destroyTriggered = false;
    void Update() 
    {
        if (CurrentCircle.currentRadius < MaxRadius)
        {
            CurrentCircle.currentRadius += MaxRadius / TimeUntilFull * Time.deltaTime;
            CurrentCircle.currentRadius = Mathf.Clamp(CurrentCircle.currentRadius, 0, MaxRadius); // To prevent overshoot
            CircleBackground.localScale = new Vector3(CurrentCircle.currentRadius * 2, CurrentCircle.currentRadius * 2);
        }

        else if (!destroyTriggered)
        {
            destroyTriggered = true;
            Destroy(gameObject, TimeUntilDestruction);
        }
    }
}
