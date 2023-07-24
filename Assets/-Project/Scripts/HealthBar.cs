using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    [Header("Configuration")]
    [SerializeField] float Width;
    [SerializeField] float Height = 0.06f;
    [SerializeField] float DividerThickness;

    [Header("Others")]
    [SerializeField] LineRenderer RedBar;
    [SerializeField] LineRenderer BackgroundBar;
    [SerializeField] SpriteRenderer DividerPref;
    [SerializeField] List<SpriteRenderer> Dividers = new List<SpriteRenderer>();

    [HideInInspector] public float MaxHealth;
    [HideInInspector] public float CurrentHealth;

    const float DIVIER_EVERY = 10f;

    private void OnValidate()
    {
        VisuallyCalculateHealthBar();
    }

    private void Start()
    {
        CalculateHealthBar();
    }

    private void Update()
    {
        VisuallyCalculateHealthBar();
    }

    void CalculateHealthBar()
    {
        VisuallyCalculateHealthBar();
        HandleDividerCount();
        HandleDividerPositioning();
        HandleDividerScale();
    }

    void VisuallyCalculateHealthBar()
    {
        RedBar.transform.localPosition = new Vector3(-Width / 2, 0, 0);

        // Set CurrentHealth Length
        RedBar.SetPosition(1, new Vector3((Width / MaxHealth) * CurrentHealth, 0));
        RedBar.startWidth = Height;
        BackgroundBar.startWidth = Height;
        BackgroundBar.SetPosition(0, new Vector3(Width / 2, 0, 0));
        BackgroundBar.SetPosition(1, new Vector3(-Width / 2, 0, 0));
    }

    void HandleDividerCount()
    {
        int AimedCount = (int)MaxHealth / (int)DIVIER_EVERY;
        if (Dividers.Count == --AimedCount) return;

        for (int i = Dividers.Count - 1; i >= 0; i--)
        {
            Destroy(Dividers[i].gameObject);
        }

        Dividers = new List<SpriteRenderer>();
        for (int i = 0; i < AimedCount; i++)
        {
            SpriteRenderer div = Instantiate(DividerPref, transform);
            Dividers.Add(div);
            div.transform.localPosition = new Vector3(0, 0, 0);
            div.gameObject.SetActive(true);
        }
    }

    void HandleDividerPositioning()
    {
        foreach (SpriteRenderer div in Dividers) div.transform.localPosition = new Vector2(-Width / 2, 0);

        for (int i = 0; i < Dividers.Count; i++)
        {
            float xPos = (Width / MaxHealth) * DIVIER_EVERY * (i + 1);
            Dividers[i].transform.localPosition += new Vector3(xPos, 0, 0);
        }
    }

    void HandleDividerScale()
    {
        foreach (SpriteRenderer div in Dividers)
            div.transform.localScale = new Vector3(DividerThickness, Height);
    }
}