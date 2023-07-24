using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DamageNumber : MonoBehaviour
{
    [Header("Configuration")]
    [SerializeField] float offset;

    [SerializeField] TextMeshPro text;

    public void Init(float damage, Color color)
    {
        text.transform.localPosition += new Vector3(Random.Range(-offset, offset), Random.Range(-offset, offset));
        text.text = damage.ToString();
        text.color = color;
    }

    public void Destroy() => Destroy(gameObject);
}
