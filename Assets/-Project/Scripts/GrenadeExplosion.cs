using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GrenadeExplosion : MonoBehaviour
{
    [Header("Channel")]
    [SerializeField] CameraShakeChannel CameraShakeChannel;

    [Header("Configuration")]
    [SerializeField] float DisableColliderAfter;
    [SerializeField] float DestroyAfterSeconds;
    [SerializeField] float ShakeStrength;
    [SerializeField] float KnockbackStrength;

    CircleCollider2D col;

    float Damage;
    bool InversedKnockback;

    public GrenadeExplosion Init(float Damage, bool InversedKnockback)
    {
        this.InversedKnockback = InversedKnockback;
        this.Damage = Damage;
        return this;
    }

    void Awake() 
    {
        col = GetComponent<CircleCollider2D>();
    }

    void Start() 
    {
        Invoke("DisableCollider", DisableColliderAfter);
        Destroy(gameObject, DestroyAfterSeconds);
        StartCoroutine(DamageEnemiesInsideRadius());
        CameraShakeChannel.RaiseShakeImpulse(0.25f, ShakeStrength);
    }

    void DisableCollider()
    {
        col.enabled = false;
    }

    IEnumerator DamageEnemiesInsideRadius()
    {
        while (true)
        {
            yield return null;

            foreach (EnemyHealth enemy in EnemiesInRange.Except(EnemiesDamaged).ToList())
            {
                enemy.GetComponent<IDamageable>().TakeDamage(Damage);

                Vector2 knockBackdirection = InversedKnockback? (transform.position - enemy.transform.position).normalized * 0.8f: (enemy.transform.position - transform.position).normalized;
                enemy.GetComponent<IKnockbackable>().ApplyKnockback(this, knockBackdirection * KnockbackStrength, VelocityType.Burst);
                EnemiesDamaged.Add(enemy);
            }

            if (!col.enabled) break;
        }
    }

    List<EnemyHealth> EnemiesInRange = new List<EnemyHealth>();
    List<EnemyHealth> EnemiesDamaged = new List<EnemyHealth>();
    private void OnTriggerEnter2D(Collider2D collision)
    {
        EnemyHealth enemy = collision.GetComponent<EnemyHealth>();
        if (enemy == null) return;

        EnemiesInRange.Add(enemy);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        EnemyHealth enemy = collision.GetComponent<EnemyHealth>();
        if (enemy == null) return;

        EnemiesInRange.Remove(enemy);
    }
}
