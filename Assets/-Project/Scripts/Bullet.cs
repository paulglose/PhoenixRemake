using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Require that every Bullet has a Rigidbody2D and CircleCollider2D.
[RequireComponent(typeof(Rigidbody2D), typeof(CircleCollider2D))]
public abstract class Bullet : MonoBehaviour
{
    // Lists of tags that the Bullet pays attention to.
    [Header("Bullet")]
    [SerializeField] protected List<string> aimedTags;
    [SerializeField] protected List<string> destroyTags;

    // References to the required components.
    protected Rigidbody2D rb;
    protected CircleCollider2D col;

    // Called when the Bullet collides with another object.
    void OnTriggerEnter2D(Collider2D col)
    {
        // If the collided object has an aimed tag, call the aimed collision method.
        if (aimedTags.Contains(col.tag))
        {
            OnAimedCollision(col);
        }

        // If the collided object has a destroy tag, call the destroy collision method and destroy the Bullet.
        if (destroyTags.Contains(col.tag))
        {
            DestroyBullet();
        }
    }

    // Abstract methods to be implemented in subclasses to define what happens when the Bullet collides with an aimed object and when it should be destroyed.
    protected abstract void OnAimedCollision(Collider2D col);
    protected virtual void DestroyBullet() { }
}