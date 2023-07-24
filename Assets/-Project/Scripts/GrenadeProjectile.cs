using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeProjectile : MonoBehaviour
{
    [SerializeField] GrenadeExplosion Explosion;
    [SerializeField] CircleIndicator CircleIndicator;

    Rigidbody2D rb;
    Animator anim;

    Vector3 targetPosition;
    float damage;
    float explosionRadius;
    float projectileSpeed;
    float TimeUntilExplosion;
    GrenadeLauncherAbilities Abilities;

    public void Init(Vector3 targetPosition, float damage, float explosionRadius, float projectileSpeed, float TimeUntilExplosion, GrenadeLauncherAbilities Abilities)
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        this.targetPosition = targetPosition;
        this.damage = damage;
        this.explosionRadius = explosionRadius;
        this.projectileSpeed = projectileSpeed;
        this.TimeUntilExplosion = TimeUntilExplosion;
        this.Abilities = Abilities;

        Instantiate(CircleIndicator, targetPosition, Quaternion.identity).Init(explosionRadius, TimeUntilExplosion, 0);
        Invoke("Detonate", TimeUntilExplosion);
    }

    void OnEnable() 
    { }

    void OnDisable() 
    { }

    void Awake() 
    { }

    void Start() 
    { }

    bool reachedLocation;
    void Update() 
    {
        if (!reachedLocation)

        rb.MovePosition(Vector3.Lerp(rb.position, targetPosition, projectileSpeed * Time.deltaTime));

        if (Vector3.Distance(rb.position, targetPosition) < .05f){
            reachedLocation = true;
        }
    }

    void Detonate()
    {
        GrenadeExplosion explosion = Instantiate(Explosion, targetPosition, Quaternion.identity).Init(damage, Abilities.InversedKnockback);
        explosion.transform.localScale = new Vector3(explosionRadius * 2, explosionRadius * 2);
        Destroy(gameObject);
    }
}
