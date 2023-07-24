using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : Bullet
{
    float carriedDamage;
    [SerializeField] GameObject destroyParts;
    [SerializeField] TrailRenderer tr;
    [SerializeField] SpriteRenderer sr;

    void OnEnable() 
    { }

    void OnDisable() 
    { }

    void Awake() 
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<CircleCollider2D>();
    }

    void Start() 
    { }

    void Update() 
    { }

    protected override void OnAimedCollision(Collider2D col)
    {
        IDamageable damageable = col.GetComponent<IDamageable>();

        if (damageable != null)
        {
            damageable.TakeDamage(carriedDamage);
        }
    }

    protected override void DestroyBullet()
    {
        base.DestroyBullet();
        if (destroyParts) Instantiate(destroyParts, transform.position, transform.rotation);

        sr.enabled = false;
        tr.emitting = false;
        Destroy(gameObject, tr.time);
    }

    public void Init(Vector2 velocity, float carriedDamage)
    {
        rb.velocity = velocity;
        this.carriedDamage = carriedDamage;
    }
}
