using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    Animator anim;

    [SerializeField] List<IDamageable> damageablesInRange = new List<IDamageable>();

    const float FADE_OUT_SPEED = 0.15f;
    const float DAMAGE_TICK_RATE = 0.15f;
    
    float Damage;
    float LifeTime;
    bool IsPiercing;
    public void Init(float Damage, float LifeTime, bool IsPiercing)
    {
        this.Damage = Damage;
        this.LifeTime = LifeTime;
        this.IsPiercing = IsPiercing;
        anim.CrossFade("Shooting", 0f);

        Invoke("FadeOut", LifeTime);
    }

    void FadeOut()
    {
        anim.CrossFade("Hidden", FADE_OUT_SPEED);
    }

    void OnEnable() {}

    void OnDisable() {}

    void Awake() 
    {
        anim = GetComponent<Animator>();
    }

    void Start() {}

    float nextDamageTime = 0;
    void Update() 
    {
        damageablesInRange.RemoveAll(item => item == null);
        if (damageablesInRange.Count > 0 && Time.time > nextDamageTime)
        {
            for (int i = 0; i < damageablesInRange.Count; i++) damageablesInRange[i].TakeDamage(Damage);
            nextDamageTime = Time.time + DAMAGE_TICK_RATE;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        IDamageable iDamageable = other.GetComponent<IDamageable>();
        if (iDamageable != null)
        {
            damageablesInRange.Add(iDamageable);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        IDamageable iDamageable = collision.GetComponent<IDamageable>();
        if (iDamageable != null)
        {
            damageablesInRange.Remove(iDamageable);
        }
    }
}
