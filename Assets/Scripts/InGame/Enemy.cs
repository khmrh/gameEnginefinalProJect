using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int baseHealth = 3;
    private int currentHealth;

    public float moveSpeed = 2f;
    private Transform player;

    public int damage = 1; // ���ݷ�

    public GameObject[] dropItems; // 3���� ������ ������
    public float dropChance = 0.3f; // 30% Ȯ��

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
            Debug.LogWarning("Player�� ã�� �� �����ϴ�. 'Player' �±װ� �����Ǿ� �ִ��� Ȯ���ϼ���.");
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
