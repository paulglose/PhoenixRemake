using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressManager : MonoBehaviour
{
    [Header("Configuration")]
    [SerializeField] float BarSpeed;

    [Header("Channel")]
    [SerializeField] ProgressChannel ProgressChannel;

    [Header("Others")]
    [SerializeField] Transform PivotParent;
    [SerializeField] Transform[] VisualTransforms;
    [SerializeField] SpriteRenderer bar;

    float maxSliderValue;
    float currentSliderValue;
    Animator anim;

    void Start()
    {
        VisuallyDisable();
        anim = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        ProgressChannel.OnProgressCanceled += OnProgressCanceled;
        ProgressChannel.OnProgressRegistered += OnProgressRegistered;
        ProgressChannel.OnProgressHighlighted += OnProgressHighlighted;
        ProgressChannel.OnProgressMaterialRegistered += OnProgressMaterialRegistered;
    }

    private void OnDisable()
    {
        ProgressChannel.OnProgressCanceled -= OnProgressCanceled;
        ProgressChannel.OnProgressRegistered -= OnProgressRegistered;
        ProgressChannel.OnProgressHighlighted -= OnProgressHighlighted;
        ProgressChannel.OnProgressMaterialRegistered -= OnProgressMaterialRegistered;
    }

    void OnProgressHighlighted() => anim.CrossFade("Highlight", 0f);

    void OnProgressMaterialRegistered(Material mat)
        => bar.material = mat;

    private void Update()
    {
        if (IsVisuallyEnabled)
        {
            // Handle the Scale
            float currentBarScale = Mathf.Lerp(PivotParent.transform.localScale.y, Mathf.Clamp(currentSliderValue / maxSliderValue, 0, 1), BarSpeed * Time.deltaTime);

            PivotParent.transform.localScale = new Vector3(PivotParent.transform.localScale.x, currentBarScale, PivotParent.transform.localScale.y);
        }
    }

    public void VisuallyEnable()
    {
        IsVisuallyEnabled = true;
        PivotParent.transform.localScale = new Vector3(PivotParent.transform.localScale.x, Mathf.Lerp(Mathf.Clamp(currentSliderValue / maxSliderValue, 0, 1), 1, Time.deltaTime * 10), PivotParent.transform.localScale.z);
        foreach (Transform trans in VisualTransforms) trans.gameObject.SetActive(true);
    }

    bool IsVisuallyEnabled;
    void VisuallyDisable()
    {
        IsVisuallyEnabled = false;
        foreach (Transform trans in VisualTransforms) trans.gameObject.SetActive(false);
    }

    void OnProgressCanceled()
    {
        VisuallyDisable();
    }

    void OnProgressRegistered(float currentValue, float maxValue)
    {
        currentSliderValue = currentValue;
        maxSliderValue = maxValue;
        VisuallyEnable();
    }
}
