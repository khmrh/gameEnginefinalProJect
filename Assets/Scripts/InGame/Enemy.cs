using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int baseHealth = 3;
    private int currentHealth;

    public float moveSpeed = 2f;
    private Transform player;

    public int damage = 1; // 공격력

    public GameObject[] dropItems; // 3가지 아이템 프리팹
    public float dropChance = 0.3f; // 30% 확률

    public void TryDropItem()
    {
        if (Random.value <= dropChance)
        {
            int index = Random.Range(0, dropItems.Length);
            Instantiate(dropItems[index], transform.position, Quaternion.identity);
        }
    }

    public void SetHealthMultiplier(float multiplier)
    {
        currentHealth = Mathf.RoundToInt(baseHealth * multiplier);
    }

    void Start()
    {
        if (currentHealth == 0)
            currentHealth = baseHealth;

        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        if (player == null)
        {
            Debug.LogWarning("Player를 찾을 수 없습니다. 'Player' 태그가 설정되어 있는지 확인하세요.");
        }
    }

    void Update()
    {
        if (player != null)
        {
            Vector3 direction = (player.position - transform.position).normalized;
            transform.position += direction * moveSpeed * Time.deltaTime;
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Players playersScript = collision.GetComponent<Players>();
            if (playersScript != null)
            {
                playersScript.TakeDamage(damage);
            }
        }
    }

    public void ApplyDifficultyScaling(float healthMultiplier, float speedMultiplier)
    {
        baseHealth = Mathf.RoundToInt(baseHealth * healthMultiplier);
        currentHealth = baseHealth;
        moveSpeed *= speedMultiplier;
    }



    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        GetComponent<ItemDrop>()?.TryDropItem();
        KillCounter.AddKill();
        Destroy(gameObject);
    }

    void OnDestroy()
    {
        if (GameManager.Instance != null)
            GameManager.Instance.RegisterKill();
    }
}
