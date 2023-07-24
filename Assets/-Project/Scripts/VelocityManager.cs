using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// USED BY OTHER CLASSES BUT BELONGS TO THIS CLASS
/// This interface is used by the VelocityHandler class to search for all classes that influence the own 
/// velocity of the Rigidbody. So, for Movement Dashes or Weapon Knockbacks. Everything that the GameObject applies to itself.
/// The interface is separated from IKnockbackable because the Velocity Handler searches for Events in its own GameObject and automatically subs to it,
/// while the Knockbackable is suited for external forces.
/// </summary>
public interface IVelocityInfluence
{
    public event Action<MonoBehaviour, Vector2, VelocityType> RegisterVelocity;
    public event Action<MonoBehaviour> ResetVelocity;
}

/// <summary>
/// An Interface to get for other MonoBehaviours (Mostly Bullets) that apply a Knockback to the Unit
/// </summary>
public interface IKnockbackable
{
    public void ApplyKnockback(MonoBehaviour source, Vector2 force, VelocityType type);
}

public interface ISlowable
{
    public void ApplySlow(MonoBehaviour source, float strength);
    public void ResetSlow(MonoBehaviour source);
}

[RequireComponent(typeof(Rigidbody2D))]
public class VelocityManager : MonoBehaviour, IKnockbackable, ISlowable
{
    /// <summary>
    /// The KnockbackResistance is a constant value, that determines how internal Knockback Bursts are lowered over time.
    /// It can be Changed to change the speed of the game. If Higher, the Knockback fades out faster. If lower the knockback
    /// fades out slower.
    /// </summary>
    [SerializeField] float KNOCKBACK_RESISTANCE = 10;

    Rigidbody2D rb;

    // Dictionaries to keep track of internal forces
    Dictionary<MonoBehaviour, Vector2> VelocityBursts = new Dictionary<MonoBehaviour, Vector2>();
    Dictionary<MonoBehaviour, Vector2> VelocityConstants = new Dictionary<MonoBehaviour, Vector2>();

    // Dictioanry to keep track of the current Slows
    Dictionary<MonoBehaviour, float> SlowSources = new Dictionary<MonoBehaviour, float>();

    // Dictionaries that keep track of slowing influences
    void ISlowable.ApplySlow(MonoBehaviour source, float strength) =>
        SlowSources[source] = strength;
    
    void ISlowable.ResetSlow(MonoBehaviour source) =>
        SlowSources.Remove(source);

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        // Subscribe to all VelocityParts in that GameObject
        foreach (IVelocityInfluence part in GetComponentsInChildren<IVelocityInfluence>())
        {
            part.ResetVelocity += ResetKnockback;
            part.RegisterVelocity += ApplyVelocity;
        }
    }

    private void FixedUpdate()
    {
        // Calculate and apply the total velocity
        rb.velocity = CalculateVelocity();

        // Decrease the magnitude of velocity bursts over time
        DecreaseInternalVelocityBursts();

        // Remove zero-magnitude entries from the dictionaries
        CleanAllDictionaries();
    }

    // Acts as a bridge to enable external forces
    void IKnockbackable.ApplyKnockback(MonoBehaviour source, Vector2 vector, VelocityType type)
    {
        ApplyVelocity(source, vector, type);
    }

    // Reset the external knockback (not implemented)
    public void ResetKnockback(MonoBehaviour source) 
    {
        VelocityConstants.Remove(source);
    }

    // Apply a velocity to this object
    void ApplyVelocity(MonoBehaviour part, Vector2 velocity, VelocityType type)
    {
        switch (type)
        {
            case VelocityType.Burst:
                VelocityBursts[part] = velocity;
                break;
            case VelocityType.Constant:
                VelocityConstants[part] = velocity;
                break;
        }
    }

    // Lowers the velocities of the bursts
    void DecreaseInternalVelocityBursts()
    {
        var keysCopy = new List<MonoBehaviour>(VelocityBursts.Keys);
        foreach (MonoBehaviour part in keysCopy)
        {
            float newVectorLength = VelocityBursts[part].magnitude - (KNOCKBACK_RESISTANCE * Time.fixedDeltaTime);
            VelocityBursts[part] = (newVectorLength > 0) ? VelocityBursts[part].normalized * newVectorLength : Vector2.zero;
        }
    }

    // Remove zero magnitude entries from the dictionaries
    void CleanAllDictionaries()
    {
        {
            List<KeyValuePair<MonoBehaviour, Vector2>> entriesToRemove = new List<KeyValuePair<MonoBehaviour, Vector2>>();
            foreach (KeyValuePair<MonoBehaviour, Vector2> pair in VelocityBursts) if (pair.Value.magnitude == 0) entriesToRemove.Add(pair);
            foreach (KeyValuePair<MonoBehaviour, Vector2> pair in entriesToRemove) VelocityBursts.Remove(pair.Key);
        }

        {
            List<KeyValuePair<MonoBehaviour, Vector2>> entriesToRemove = new List<KeyValuePair<MonoBehaviour, Vector2>>();
            foreach (KeyValuePair<MonoBehaviour, Vector2> pair in VelocityConstants) if (pair.Value.magnitude == 0) entriesToRemove.Add(pair);
            foreach (KeyValuePair<MonoBehaviour, Vector2> pair in entriesToRemove) VelocityConstants.Remove(pair.Key);
        }
    }

    // Calculate the summed velocity from all the bursts and constants
    Vector2 CalculateVelocity()
    {
        Vector2 summedVelocity = Vector2.zero;

        foreach (Vector2 vel in VelocityBursts.Values)  summedVelocity  += vel;
        foreach (Vector2 vel in VelocityConstants.Values) summedVelocity += vel *  (1 - GetHighestSlowSource()); // 1 - Methode because if the SlowSources Dictionary got a 20% slow (0.2f) the value has to be multipied by .8f to correctly apply the slow value

        return summedVelocity;
    }

    // returns the highest value out of the ShakeConstants value
    float GetHighestSlowSource()
    {
        // Stellt sicher, dass das Wörterbuch nicht leer ist, bevor es nach dem Maximalwert sucht.
        if (SlowSources != null && SlowSources.Count > 0)
        {
            return SlowSources.Max(x => x.Value);
        }

        return 0;
    }
}
