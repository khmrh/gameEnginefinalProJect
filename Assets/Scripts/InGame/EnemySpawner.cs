using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public Transform player;
    public float initialSpawnInterval = 3f;
    public float minSpawnInterval = 0.5f;
    public float spawnRadius = 10f;
    public float minDistanceFromPlayer = 5f;

    public float intervalDecreaseAmount = 0.5f; // 1�� 30�ʸ��� ������ ��
    private float currentSpawnInterval;
    private float timer;
    private float difficultyTimer;
    private float strongEnemyTimer;

    public float spawnHealthMultiplier = 1f;

    void Start()
    {
        currentSpawnInterval = initialSpawnInterval;
    }

    void Update()
    {
        float elapsedTime = Time.time;

        timer += Time.deltaTime;
        difficultyTimer += Time.deltaTime;
        strongEnemyTimer += Time.deltaTime;

        // ���� �ֱ⸶�� ���� ���� ���� (1�� 30�� = 90��)
        if (difficultyTimer >= 90f)
        {
            currentSpawnInterval = Mathf.Max(minSpawnInterval, currentSpawnInterval - intervalDecreaseAmount);
            difficultyTimer = 0f;
        }

        // 3�и��� ���� �� ����
        if (strongEnemyTimer >= 180f)
        {
            spawnHealthMultiplier += 0.5f; // 1.0 �� 1.5 �� 2.0 ��
            strongEnemyTimer = 0f;
        }

        if (timer >= currentSpawnInterval)
        {
            SpawnEnemy();
            timer = 0f;
        }
    }

    void SpawnEnemy()
    {
        Vector2 spawnPosition = GetValidSpawnPosition();
        GameObject newEnemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);

        // �� ü�� ���� ����
        Enemy enemyScript = newEnemy.GetComponent<Enemy>();
        if (enemyScript != null)
        {
            enemyScript.SetHealthMultiplier(spawnHealthMultiplier);
        }
    }

    Vector2 GetValidSpawnPosition()
    {
        Vector2 spawnPos;
        int attempt = 0;

        do
        {
            Vector2 randomDirection = Random.insideUnitCircle.normalized;
            float distance = Random.Range(minDistanceFromPlayer, spawnRadius);
            spawnPos = (Vector2)player.position + randomDirection * distance;

            attempt++;
        } while (Vector2.Distance(spawnPos, player.position) < minDistanceFromPlayer && attempt < 10);

        return spawnPos;
    }
}


