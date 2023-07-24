using System;
using UnityEngine;
using System;
using UnityEngine;

[RequireComponent(typeof(VelocityManager))]
public class EnemyController : EnemyComponent, IVelocityInfluence
{
    const float PICK_NEXT_LOCATION_DISTANCE = 1f; // Distance within which a new location will be picked

    [SerializeField] float MovementSpeed = 10; // Movement speed of the enemy
    [SerializeField] float Weight = 6; // Weight of the enemy, and with it, the force of which the enemy changes direction

    Vector3 nextLocation; // The next location the enemy will move to

    public event Action<MonoBehaviour, Vector2, VelocityType> RegisterVelocity; // Event to register velocity
    public event Action<MonoBehaviour> ResetVelocity;

    public EnemyControllerType ControllType;

    protected override void Start()
    {
        base.Start();

        if (ControllType == EnemyControllerType.Default) Debug.LogError("The Controll Type must not be Default, by Default");

        ControllType = Configuration.ControllerType == EnemyControllerType.Default ? ControllType : Configuration.ControllerType;

        PickNextLocation();
    }

    protected override void Update()
    {
        base.Update();

        if (ControllType == EnemyControllerType.None)
        {
            nextLocation = transform.position;
            RegisterVelocity(this, Vector2.zero, VelocityType.Constant);
        }
        else
        {
            // Check if it's time to pick a new location and move towards the next location
            HandleNextLocation();
            MoveTowardsNextLocation();
        }


    }

    /// <summary>
    /// Handles if and when the next location is picked
    /// </summary>
    void HandleNextLocation()
    {
        // If the enemy is close enough to the next location, pick a new one
        if (Vector3.Distance(transform.position, nextLocation) < PICK_NEXT_LOCATION_DISTANCE)
            PickNextLocation();
    }

    /// <summary>
    /// Picks the Next location
    /// </summary>
    void PickNextLocation()
    {
        // Get the size and center of the enemy area
        Vector2 size = Enemy.Area.size;
        Vector2 center = (Vector2)Enemy.Area.transform.position + Enemy.Area.offset;

        // Define the range for picking the waypoint based on ControllType
        float minX = center.x - size.x / 2;
        float maxX = center.x + size.x / 2;

        // Generate a random point within the specified range
        float randomX;
        if (ControllType == EnemyControllerType.Front)
        {
            randomX = UnityEngine.Random.Range(minX, center.x);
        }
        else if (ControllType == EnemyControllerType.Back)
        {
            randomX = UnityEngine.Random.Range(center.x, maxX);
        }
        else // ControllType is Mixed
        {
            randomX = UnityEngine.Random.Range(minX, maxX);
        }

        float randomY = UnityEngine.Random.Range(center.y - size.y / 2, center.y + size.y / 2);
        Vector2 randomPointInBox = new Vector2(randomX, randomY);

        // Convert local point to world point
        randomPointInBox = Enemy.Area.transform.TransformPoint(randomPointInBox);

        // Set the next location to the random point
        nextLocation = randomPointInBox;
    }

    Vector3 movementVector;
    void MoveTowardsNextLocation()
    {
        // Calculate the vector towards the next location
        Vector3 vectorTowardsLocation = (nextLocation - transform.position).normalized / Weight;
        movementVector += vectorTowardsLocation;

        // Clamp the magnitude of the movement vector to the enemy's speed
        movementVector = movementVector.normalized * Mathf.Clamp(movementVector.magnitude, 0, MovementSpeed);

        // Register the new velocity
        RegisterVelocity(this, movementVector * MovementSpeed * Time.fixedDeltaTime * Configuration.MoveSpeedMultiplier, VelocityType.Constant);
    }
}
