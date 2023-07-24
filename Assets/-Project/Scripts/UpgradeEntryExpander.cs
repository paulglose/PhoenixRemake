using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeEntryExpander : MonoBehaviour
{
    [Header("Configurations")]
    [SerializeField] float additionalGap = 35f;
    [SerializeField] float expandSpeed = 10f;

    [Header("Others")]
    [SerializeField] RectTransform descriptionRectTransform;
    [SerializeField] RectTransform AimedRectTransform;

    private void Update()
    {
        // Expand
        float neededHeight = -descriptionRectTransform.localPosition.y + descriptionRectTransform.sizeDelta.y + additionalGap;
        AimedRectTransform.sizeDelta = new Vector2(AimedRectTransform.sizeDelta.x, Mathf.Lerp(AimedRectTransform.sizeDelta.y, neededHeight, expandSpeed * Time.deltaTime));
    }
}