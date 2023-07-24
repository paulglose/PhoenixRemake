using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultEnemyBullet : EnemyBullet
{
    float carriedDamage;

    public DefaultEnemyBullet Init(float Damage, float force)
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<CircleCollider2D>();

        this.carriedDamage = Damage;

        rb.velocity = new Vector2(-force, 0);

        return this;
    }

    protected override void OnAimedCollision(Collider2D col)
    {
        base.OnAimedCollision(col);
        col.GetComponent<IDamageable>().TakeDamage(carriedDamage);
    }

    protected override void DestroyBullet()
    {
        base.DestroyBullet();
        Destroy(gameObject);
    }
}
