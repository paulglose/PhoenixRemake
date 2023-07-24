using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class LineRendererAnimator : MonoBehaviour
{
    Animator anim;
    LineRenderer ln;

    public float LineRendererWidth;

    void OnEnable() {}

    void OnDisable() {}

    void Awake() 
    {
        anim = GetComponent<Animator>();
        ln = GetComponent<LineRenderer>();
    }

    void Start() {}

    void Update() 
    {
        ln.startWidth = LineRendererWidth;
    }
}
