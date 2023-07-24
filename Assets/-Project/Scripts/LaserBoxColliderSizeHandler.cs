using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D), typeof(LineRenderer))]
public class LaserBoxColliderSizeHandler : MonoBehaviour
{
    LineRenderer ln;
    BoxCollider2D col;

    void OnEnable() {}

    void OnDisable() {}

    void Awake() 
    {
        ln = GetComponent<LineRenderer>();
        col = GetComponent<BoxCollider2D>();
    }

    void Start() {}

    void Update() 
    {
        float colliderHeight = ln.GetPosition(1).y - ln.GetPosition(0).y;
        col.size = new Vector2(col.size.x, colliderHeight);
        col.offset = new Vector3(col.offset.x, colliderHeight / 2);
    }
}
