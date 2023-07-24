using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class RingRenderer : MonoBehaviour
{
    [SerializeField] int pointsCount = 100;
    public float currentRadius;
    public float radiusThickness;
    LineRenderer ln;

    public RingRenderer init(float currentRadius, float radiusThickness)
    {
        this.currentRadius = currentRadius;
        this.radiusThickness = radiusThickness;
        return this;
    }

    [ExecuteAlways]
    void OnValidate()
    {
        DrawCircle(currentRadius);
    }

    private void Update()
    {
        DrawCircle(currentRadius);
    }

    void DrawCircle(float radius)
    {
        if (!ln) ln = GetComponent<LineRenderer>();
        ln.positionCount = pointsCount + 1;

        ln.startWidth = radiusThickness;
        float angleBetweenPoints = 360f / pointsCount;

        for (int i = 0; i <= pointsCount; i++)
        {
            float angle = i * angleBetweenPoints * Mathf.Deg2Rad;
            Vector3 direction = new Vector3(Mathf.Sin(angle), Mathf.Cos(angle), 0f);
            Vector3 position = direction * radius;

            ln.SetPosition(i, position);
        }
    }
}
