using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseManager : MonoBehaviour
{
    [Header("Channel")]
    [SerializeField] MouseChannel MouseChannel;

    [Header("Configurations")]
    [SerializeField] float DefaultRadius;

    [Header("Others")]
    [SerializeField] AccuracyObject AccuracyObject;
    [SerializeField] Camera Camera;

    private void OnEnable()
    {
        MouseChannel.OnAccuracyRadiusRegistered += ApplyRadius;
        MouseChannel.OnRotatationSwitched += (value) => AccuracyObject.RotationEnabled = value;
    }

    private void OnDisable()
    {
        MouseChannel.OnAccuracyRadiusRegistered -= ApplyRadius;
        MouseChannel.OnRotatationSwitched -= (value) => AccuracyObject.RotationEnabled = value;
    }

    private void Awake()
    {
        AccuracyObject = Instantiate(AccuracyObject);
    }

    private void Start()
    {
        MouseChannel.Collider = AccuracyObject.AccuracyCollider;
    }

    private void Update()
    {
        Vector3 mousePosition = Camera.ScreenToWorldPoint(Input.mousePosition);
        mousePosition = new Vector3(mousePosition.x, mousePosition.y, 0);

        MouseChannel.Position = mousePosition;
        AccuracyObject.transform.position = mousePosition;
    }

    void ApplyRadius(float radius)
    {
        if (radius == 0)
        {
            AccuracyObject.SetAccuracy(DefaultRadius);
            AccuracyObject.RotationEnabled = false;
        }
        else AccuracyObject.SetAccuracy(radius);
    }
}
