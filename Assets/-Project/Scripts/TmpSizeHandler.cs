using UnityEngine;
using TMPro;

public class TMP_SizeManager : MonoBehaviour
{
    RectTransform rectTransform;
    TMP_Text textMeshPro;
    float initialWidth;

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        textMeshPro = GetComponent<TextMeshProUGUI>();


    }

    private void Update()
    {
        rectTransform.pivot = new Vector2(0.5f, 1); // Set the pivot to the top center of the object
        rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, textMeshPro.preferredHeight);
    }
}