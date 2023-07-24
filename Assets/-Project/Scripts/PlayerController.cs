using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(CircleCollider2D))]
public class PlayerController : MonoBehaviour, IVelocityInfluence
{
    [Header("Channel")]
    [SerializeField] InputReader InputReader;
    [SerializeField] PlayerControllerChannel PlayerControllerChannel;

    [Header("Stats")]
    [SerializeField] float AccelerationFactor;
    public Stat MovementSpeed;

    // The Vector, that the InputReader Invokes
    private Vector2 MovementInput;          
    
    // The Vector that is Invoking the Velocity Event for the VelocityHandler, it is handled and faded towards "MovementInput" 
    private Vector2 MovementVector = Vector2.zero;                      

    // Interfaces Event, that can be called tto register a velocity at the velocityHandler
    public event Action<MonoBehaviour, Vector2, VelocityType> RegisterVelocity;
    public event Action<MonoBehaviour> ResetVelocity;

    private void OnEnable()
    {
        InputReader.MovementPerformed += vector => MovementInput = vector;
        PlayerControllerChannel.OnMovementBoosted += OnMovementBoosted;
        PlayerControllerChannel.OnMovementDisabled += OnMovementDisabled;
        PlayerControllerChannel.OnMovementEnabled += OnMovementEnabled;
        PlayerControllerChannel.OnMovementNormalized += OnMovementNormalized;
    }

    private void OnDisable()
    {
        InputReader.MovementPerformed -= vector => MovementInput = vector;
    }

    void Start()
    {
        InputReader.SwitchToPlayer();
    }

    bool isMovementEnabled = true;
    bool isMovementBoosted = false;
    private void FixedUpdate()
    {
        if (!isMovementEnabled) MovementInput = Vector3.zero;

        // Handle the Movement of the Player
        if (!isMovementBoosted)
        {
            MovementVector = Vector3.MoveTowards(MovementVector, MovementInput * MovementSpeed, MovementSpeed * Time.fixedDeltaTime * AccelerationFactor);
            RegisterVelocity?.Invoke(this, MovementVector, VelocityType.Constant);
        } else
        {
            RegisterVelocity?.Invoke(this, MovementInput * MovementSpeed * 1.5f, VelocityType.Constant);
        }
    }

    private void OnMovementNormalized() => isMovementBoosted = false;
    private void OnMovementEnabled() => isMovementEnabled = true;
    private void OnMovementDisabled() => isMovementEnabled = false;
    private void OnMovementBoosted() => isMovementBoosted = true;

    void MovementCanceled() => MovementInput = Vector2.zero;
}
