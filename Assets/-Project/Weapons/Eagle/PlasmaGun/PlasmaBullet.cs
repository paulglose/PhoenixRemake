using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlasmaBullet : Bullet
{
    float size;
    float carriedDamage;

    [Header("Others")]
    [SerializeField] Transform Sprite;
    [SerializeField] TrailRenderer TrailRenderer;
    [SerializeField] GameObject destroyParts;
    [SerializeField] SpriteRenderer look;

    [SerializeField] float KnockbackStrength;

    PlasmaGunAbilites Abilities;

    public PlasmaBullet Init(Vector2 referencePosition, float bulletSpeed, float size, float carriedDamage, PlasmaGunAbilites Abilities)
    {
        col = GetComponent<CircleCollider2D>();
        rb = GetComponent<Rigidbody2D>();

        // Apply the velocity
        rb.velocity = (referencePosition - (Vector2)transform.position).normalized * bulletSpeed;

        // Apply the size
        this.size = size;
        TrailRenderer.startWidth = size;
        col.radius = size / 2;
        Sprite.transform.localScale = new Vector3(size, size, 0);
        this.Abilities = Abilities;

        if (Abilities.PiercingBullets) destroyTags.Remove("Enemy");

        this.carriedDamage = carriedDamage;
        return this;
    }

    protected override void OnAimedCollision(Collider2D col)
    {
        col.GetComponent<IDamageable>().TakeDamage(carriedDamage);
        col.GetComponent<IKnockbackable>().ApplyKnockback(this, transform.up * KnockbackStrength, VelocityType.Burst);

        GameObject parts = Instantiate(destroyParts, transform.position, transform.rotation);
        parts.transform.localScale = new Vector3(size / 2, size / 2, 0);
    }

    protected override void DestroyBullet()
    {
        base.DestroyBullet();

        look.enabled = false;
        TrailRenderer.emitting = false;

        Destroy(gameObject, TrailRenderer.time);
    }
}
