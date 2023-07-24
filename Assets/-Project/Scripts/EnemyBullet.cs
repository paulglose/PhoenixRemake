using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : Bullet
{
    [Header("EnemyBullet")]
    [SerializeField] WaveChannel WaveChannel;

    protected virtual void OnEnable()
    {
        WaveChannel.OnWaveCompleted += DestroyBullet;
    }

    protected virtual void OnDisable()
    {
        WaveChannel.OnWaveCompleted -= DestroyBullet;
    }

    protected override void OnAimedCollision(Collider2D col) { }
}
