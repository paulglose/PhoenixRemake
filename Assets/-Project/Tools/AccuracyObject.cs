using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AccuracyObject : MonoBehaviour
{
    [HideInInspector] public CircleCollider2D AccuracyCollider;

    [Header("Configuration")]
    [SerializeField] float ScaleSpeed;
    [SerializeField] float ProgressHandlerOffset;
    [SerializeField] float RotationSpeed;

    [Header("Others")]
    [SerializeField] Transform ProgressHandler;
    [SerializeField] Transform[] IndicatorPoints;
    [SerializeField] Transform IndicatorPointsParent;
    [SerializeField] Transform[] VisualProgressHandlerComponents;

    public bool RotationEnabled;

    private void Awake()
    {
        AccuracyCollider = GetComponent<CircleCollider2D>();
        DisableIndex();
    }

    float aimedRadius;
    public void SetAccuracy(float radius)
    {
        EnableIndex();

        AccuracyCollider.radius = radius;
        aimedRadius = radius;
    }

    void DisableIndex()
    {
        foreach (Transform point in IndicatorPoints) point.gameObject.SetActive(false);
    }
    
    void EnableIndex()
    {
        foreach (Transform point in IndicatorPoints) point.gameObject.SetActive(true);
    }

    private void Update()
    {
        // Handle the area display
        foreach (Transform point in IndicatorPoints) point.localPosition = Vector3.Lerp(point.localPosition, new Vector3(0, aimedRadius, 0), ScaleSpeed * Time.deltaTime);
        ProgressHandler.transform.localPosition = new Vector3(ProgressHandler.transform.localPosition.x, -aimedRadius -ProgressHandlerOffset);

        // Handle Rotation of the Points
        if (RotationEnabled) IndicatorPointsParent.transform.Rotate(new Vector3(0, 0, RotationSpeed * Time.deltaTime));

        // Is enough because all are the same
        if (IndicatorPoints[0].transform.localPosition.y < 0.1) DisableIndex();
        else EnableIndex();
    }
}
