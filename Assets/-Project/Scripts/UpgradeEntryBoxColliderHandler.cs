using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D), typeof(RectTransform))]
public class UpgradeEntryBoxColliderHandler : MonoBehaviour
{
    BoxCollider2D col;
    [SerializeField] RectTransform aimedRectTransform;

    private void Awake()
    {
        col = GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        col.size = new Vector2(col.size.x, aimedRectTransform.sizeDelta.y);
        col.offset = new Vector2(0, -aimedRectTransform.sizeDelta.y / 2);
    }
}
