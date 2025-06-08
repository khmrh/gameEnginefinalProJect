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

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Players playersScript = collision.gameObject.GetComponent<Players>();
            if (playersScript != null)
            {
                playersScript.TakeDamage(damage);
            }
        }
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
        Destroy(gameObject);
    }
}
