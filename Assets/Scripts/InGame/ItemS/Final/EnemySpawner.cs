using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("적 스폰 설정")]
    public GameObject[] enemyPrefabs;
    public float spawnRadius = 8f;

    [Header("스폰 속도 설정")]
    public float initialSpawnInterval = 3f;
    public float spawnAccelerationRate = 0.95f;

    [Header("난이도 증가 설정")]
    public float difficultyIncreaseInterval = 90f;
    public float healthMultiplierPerStage = 1.2f;
    public float speedMultiplierPerStage = 1.1f;

    [Header("스폰 속도 최소 한계치")]
    public float minSpawnInterval = 0.5f;

    public static float globalDifficultyMultiplier = 1f;

    private float spawnTimer = 0f;
    private float currentSpawnInterval;
    private float difficultyTimer = 0f;
    private int difficultyStage = 0;

    private Transform player;

    private void Start()
    {
        currentSpawnInterval = initialSpawnInterval;
        player = GameObject.FindGameObjectWithTag("Player")?.transform;

        if (player == null)
        {
            Debug.LogWarning("[EnemySpawner] Player not found! Enemy spawns will be skipped.");
        }
    }

    private void Update()
    {
        if (player == null) return;

        spawnTimer += Time.deltaTime;
        difficultyTimer += Time.deltaTime;

        // 적 스폰 로직
        if (spawnTimer >= currentSpawnInterval)
        {
            SpawnEnemy();
            spawnTimer = 0f;

            // 스폰 속도 감소
            currentSpawnInterval *= spawnAccelerationRate;

            // 최소 스폰 속도 보호
            currentSpawnInterval = Mathf.Max(currentSpawnInterval, minSpawnInterval);
        }

        // 난이도 증가 로직
        if (difficultyTimer >= difficultyIncreaseInterval)
        {
            difficultyStage++;
            difficultyTimer = 0f;

            Debug.Log($"[EnemySpawner] 난이도 증가! 현재 스테이지: {difficultyStage}");
        }
    }

    void SpawnEnemy()
    {
        Vector2 spawnPos = (Vector2)player.position + Random.insideUnitCircle.normalized * spawnRadius;

        if (enemyPrefabs == null || enemyPrefabs.Length == 0)
        {
            Debug.LogWarning("[EnemySpawner] enemyPrefabs이 비어 있습니다.");
            return;
        }

        GameObject prefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];
        GameObject enemy = Instantiate(prefab, spawnPos, Quaternion.identity);

        Enemy enemyScript = enemy.GetComponent<Enemy>();
        if (enemyScript != null)
        {
            float healthBoost = Mathf.Pow(healthMultiplierPerStage, difficultyStage) * globalDifficultyMultiplier;
            float speedBoost = Mathf.Pow(speedMultiplierPerStage, difficultyStage) * globalDifficultyMultiplier;

            enemyScript.ApplyDifficultyScaling(healthBoost, speedBoost);
        }
    }

    public static void SetGlobalDifficultyMultiplier(float multiplier)
    {
        globalDifficultyMultiplier = multiplier;
    }
}
