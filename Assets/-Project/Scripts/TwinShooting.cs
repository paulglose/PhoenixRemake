using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TwinShooting : EnemyComponent
{
    [Header("Configuration")]
    [SerializeField] Vector2 ShootingCooldown;
    [SerializeField] float BulletSpeed;
    [SerializeField] int BulletWaves;
    [SerializeField] float TimeBetweenBullets;

    [Header("Others")]
    [SerializeField] Animator Canons;
    [SerializeField] Transform[] ShootingPoint;
    [SerializeField] DefaultEnemyBullet BulletPrefab;

    float aimedShootingTime;
    protected override void Update() 
    {
        if (Configuration.CanAttack && Time.time > aimedShootingTime)
        {
            StartCoroutine(ShootBulletWaves());
        }
    }

    IEnumerator ShootBulletWaves()
    {
        aimedShootingTime = Mathf.Infinity;

        for(int i = 0; i < BulletWaves; i++)
        {
            ShootBulletWave();
            yield return new WaitForSeconds(TimeBetweenBullets);
        }

        aimedShootingTime = Time.time + Random.Range(ShootingCooldown.x, ShootingCooldown.y);
    }

    void ShootBulletWave()
    {
        Canons.CrossFade("Shoot", .1f);

        Instantiate(BulletPrefab, ShootingPoint[0].transform.position, ShootingPoint[0].transform.rotation).Init(1, BulletSpeed);
        Instantiate(BulletPrefab, ShootingPoint[1].transform.position, ShootingPoint[1].transform.rotation).Init(1, BulletSpeed);
    }
}
