using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WaveManager : MonoBehaviour, IGameObjectConstant
{
    [Header("Channel")]
    [SerializeField] WaveChannel waveChannel; // Channel for handling wave events
    [SerializeField] StageDisplayChannel StageDisplayChannel; // Event to signal the start of a new wave partition
    [SerializeField] VoidEvent onEnemySpawn; // Event raised when an enemy is spawned
    [SerializeField] VoidEvent onEnemyKilled; // Event raised when an enemy is killed

    [Header("Others")]
    [SerializeField] List<Enemy> allEnemies; // List of all possible enemies
    [SerializeField] BoxCollider2D enemyArea; // Area in which enemies can spawn

    [Header("Configurations")]
    [SerializeField] float eliteDifficultyIncrease; // Increase in difficulty for elite waves
    [SerializeField] float enemySpawnXOffset; // X offset for enemy spawn position
    [SerializeField] int totalEnemyValueStart; // Starting total enemy value
    [SerializeField] int totalEnemyValueGrowthPerCyclus; // Growth of total enemy value per cycle
    [SerializeField] int bossWaveEvery; // Frequency of boss waves

    [SerializeField] public static int CurrentWaveCyclus = 0; // Current wave cycle

    private int enemiesAlive; // Number of enemies currently alive

    private void OnEnable()
    {
        // Subscribe to relevant events on enable
        waveChannel.OnUpgradeSelected += OnUpgradeSelected;
        onEnemySpawn.OnEventRaised += OnEnemySpawn;
        onEnemyKilled.OnEventRaised += OnEnemyKilled;
    }

    private void OnDisable()
    {
        // Unsubscribe from relevant events on disable
        waveChannel.OnUpgradeSelected -= OnUpgradeSelected;
        onEnemySpawn.OnEventRaised -= OnEnemySpawn;
        onEnemyKilled.OnEventRaised -= OnEnemyKilled;
    }

    private void Start() => Invoke("WaveStarter", 1f); // Start the wave after a delay

    void OnUpgradeSelected() => StartCoroutine(WaveCyclus());

    void OnEnemySpawn() => enemiesAlive++;

    void OnEnemyKilled() => enemiesAlive--;

    public void Initialize()
    {
        CurrentWaveCyclus = 0;
    }

    private void WaveStarter() => StartCoroutine(WaveCyclus()); // Start the wave cycle

    int currentPartition = 0;
    private IEnumerator WaveCyclus()
    {
        if (currentPartition == 9) currentPartition = 0;
        // Increment the wave cycle count
        CurrentWaveCyclus += 1;

        // Calculate the total enemy value for this cycle
        var waveCyclusEnemyValue = CalculateEnemyValue();

        // Spawn and handle each wave partition
        StageDisplayChannel.RaiseStage(++currentPartition);
        SpawnWavePartition(false, waveCyclusEnemyValue);
        yield return new WaitForSeconds(1f);
        yield return new WaitUntil(() => enemiesAlive == 0);

        StageDisplayChannel.RaiseStage(++currentPartition);
        SpawnWavePartition(false, waveCyclusEnemyValue);
        yield return new WaitForSeconds(1f);
        yield return new WaitUntil(() => enemiesAlive == 0);

        StageDisplayChannel.RaiseStage(++currentPartition);
        SpawnWavePartition(CurrentWaveCyclus % 3 == 0, (int)(waveCyclusEnemyValue * eliteDifficultyIncrease));
        yield return new WaitForSeconds(1f);
        yield return new WaitUntil(() => enemiesAlive == 0);

        waveChannel.RaiseWaveCompleted(); // Raise the wave completed event
    }

    private void SpawnWavePartition(bool bossPartition, int totalEnemyValue)
    {
        // Create a list of enemies that can be spawned this cycle
        var spawnableEnemies = new List<Enemy>(allEnemies.Where(enemy => enemy.RequiredWaveCyclus <= CurrentWaveCyclus));

        // Handle boss spawn logic
        if (bossPartition)
        {
            // Get a list of bosses that can be spawned
            var bossList = spawnableEnemies.Where(enemy => enemy.isBoss && enemy.Value <= totalEnemyValue);
            if (bossList.Any())
            {
                // Find the boss with the highest value that can be spawned
                var strongestBossAvailable = bossList.OrderByDescending(enemy => enemy.Value).First();

                // Spawn the boss
                SpawnEnemy(strongestBossAvailable);
                totalEnemyValue -= strongestBossAvailable.Value;
                spawnableEnemies.Remove(strongestBossAvailable);
            }
        }

        // Spawn regular enemies until the total enemy value is exhausted
        while (totalEnemyValue > 0 && spawnableEnemies.Count > 0)
        {
            // Randomly select an enemy to spawn
            var selectedEnemy = spawnableEnemies[Random.Range(0, spawnableEnemies.Count)];
            if (selectedEnemy.Value <= totalEnemyValue)
            {
                // If we can afford to spawn the selected enemy, do so
                SpawnEnemy(selectedEnemy);
                totalEnemyValue -= selectedEnemy.Value;
            }
            else
            {
                // If we can't afford the selected enemy, remove it from the list
                spawnableEnemies.Remove(selectedEnemy);
            }
        }
    }

    // Calculate the total enemy value for this wave cycle
    private int CalculateEnemyValue() => totalEnemyValueStart + totalEnemyValueGrowthPerCyclus * ((int)CurrentWaveCyclus - 1);

    private void SpawnEnemy(Enemy enemyToSpawn)
        => Instantiate(enemyToSpawn).AsWaveUnit();
}