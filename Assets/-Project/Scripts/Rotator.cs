using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour
{
    [SerializeField] UpdateIntervall UpdateIntervall;
    public float RotationSpeed;

    void Update() 
    {
        if (UpdateIntervall != UpdateIntervall.Framerate) return;

        transform.Rotate(transform.forward, RotationSpeed * Time.deltaTime);
    }

    void FixedUpdate()
    {
        if (UpdateIntervall != UpdateIntervall.Fixed) return;

        transform.Rotate(transform.forward, RotationSpeed * Time.fixedDeltaTime);
    }

    void LateUpdate()
    {
        if (UpdateIntervall != UpdateIntervall.Late) return;

        transform.Rotate(transform.forward, RotationSpeed * Time.deltaTime);
    }
}
