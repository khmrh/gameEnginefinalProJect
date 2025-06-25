using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("�� ���� ����")]
    public GameObject[] enemyPrefabs;
    public float spawnRadius = 8f;

    [Header("���� �ӵ� ����")]
    public float initialSpawnInterval = 3f;
    public float spawnAccelerationRate = 0.95f;

    [Header("���̵� ���� ����")]
    public float difficultyIncreaseInterval = 90f;
    public float healthMultiplierPerStage = 1.2f;
    public float speedMultiplierPerStage = 1.1f;

    [Header("���� �ӵ� �ּ� �Ѱ�ġ")]
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

        // �� ���� ����
        if (spawnTimer >= currentSpawnInterval)
        {
            SpawnEnemy();
            spawnTimer = 0f;

            // ���� �ӵ� ����
            currentSpawnInterval *= spawnAccelerationRate;

            // �ּ� ���� �ӵ� ��ȣ
            currentSpawnInterval = Mathf.Max(currentSpawnInterval, minSpawnInterval);
        }

        // ���̵� ���� ����
        if (difficultyTimer >= difficultyIncreaseInterval)
        {
            difficultyStage++;
            difficultyTimer = 0f;

            Debug.Log($"[EnemySpawner] ���̵� ����! ���� ��������: {difficultyStage}");
        }
    }

    void SpawnEnemy()
    {
        Vector2 spawnPos = (Vector2)player.position + Random.insideUnitCircle.normalized * spawnRadius;

        if (enemyPrefabs == null || enemyPrefabs.Length == 0)
        {
            Debug.LogWarning("[EnemySpawner] enemyPrefabs�� ��� �ֽ��ϴ�.");
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
