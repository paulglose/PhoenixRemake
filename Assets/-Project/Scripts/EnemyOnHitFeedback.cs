using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EnemyHealth))]
public class EnemyOnHitFeedback : EnemyComponent
{
    [SerializeField] float scaleAim; 
    [SerializeField] float scaleSpeed = 20f;
    Vector3 initialScale;

    protected override void OnEnable()
    {
        base.OnEnable();

        GetComponent<EnemyHealth>().OnEnemyDamaged += OnDamaged;
    }

    protected override void OnDisable()
    {
        base.OnDisable();

        GetComponent<EnemyHealth>().OnEnemyDamaged -= OnDamaged;
    }

    protected override void Start()
    {
        base.Start();

        if (transform.localScale.x != transform.localScale.y) Debug.LogError("The ParentObject of the GameObject '" + gameObject + "' has a wrong Scale");

        initialScale = transform.localScale;
    }

    void OnDamaged()
    {
        StopAllCoroutines();
        StartCoroutine(DamageAnimation());
    }

    IEnumerator DamageAnimation()
    {
        while(transform.localScale.x > scaleAim)
        {
            transform.localScale = Vector3.MoveTowards(transform.localScale, new Vector3(scaleAim, scaleAim, 0), scaleSpeed * Time.deltaTime);
            yield return null;
        }   

        while(transform.localScale.x < initialScale.x)
        {
            transform.localScale = Vector3.MoveTowards(transform.localScale, initialScale, scaleSpeed * Time.deltaTime);
            yield return null;
        }
    }
}
