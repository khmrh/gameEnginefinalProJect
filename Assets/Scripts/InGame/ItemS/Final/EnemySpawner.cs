using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject[] enemyPrefabs;
    public float spawnRadius = 8f;
    public float initialSpawnInterval = 3f;
    public float spawnAccelerationRate = 0.95f; // 스폰 간격 감소 비율
    public float difficultyIncreaseInterval = 90f; // 90초마다 적 능력 증가
    public float healthMultiplierPerStage = 1.2f;
    public float speedMultiplierPerStage = 1.1f;

    private float spawnTimer = 0f;
    private float currentSpawnInterval;
    private float difficultyTimer = 0f;
    private int difficultyStage = 0;
    private Transform player;

    private void Start()
    {
        currentSpawnInterval = initialSpawnInterval;
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update()
    {
        if (player == null) return;

        spawnTimer += Time.deltaTime;
        difficultyTimer += Time.deltaTime;

        if (spawnTimer >= currentSpawnInterval)
        {
            SpawnEnemy();
            spawnTimer = 0f;
            currentSpawnInterval *= spawnAccelerationRate;
        }

        if (difficultyTimer >= difficultyIncreaseInterval)
        {
            difficultyStage++;
            difficultyTimer = 0f;
        }
    }

    void SpawnEnemy()
    {
        Vector2 spawnPos = (Vector2)player.position + Random.insideUnitCircle.normalized * spawnRadius;
        GameObject prefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];
        GameObject enemy = Instantiate(prefab, spawnPos, Quaternion.identity);

        Enemy enemyScript = enemy.GetComponent<Enemy>();
        if (enemyScript != null)
        {
            float healthBoost = Mathf.Pow(healthMultiplierPerStage, difficultyStage);
            float speedBoost = Mathf.Pow(speedMultiplierPerStage, difficultyStage);
            enemyScript.ApplyDifficultyScaling(healthBoost, speedBoost);
        }
    }
}
