using UnityEngine;

public class DamageDealer : MonoBehaviour
{
    public int baseDamage = 1;
    public float damageInterval = 0.5f;
    public bool useKnockback = true;
    public float knockbackForce = 5f;

    private float damageTimer = 0f;

    private void Update()
    {
        damageTimer += Time.deltaTime;
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (damageTimer < damageInterval) return;

        if (other.CompareTag("Enemy"))
        {
            Enemy enemy = other.GetComponent<Enemy>();
            if (enemy != null)
            {
                int bonus = GameManager.Instance != null ? GameManager.Instance.GetBonusDamage() : 0;
                enemy.TakeDamage(baseDamage + bonus);
                damageTimer = 0f;

                if (useKnockback)
                {
                    Rigidbody2D rb = other.GetComponent<Rigidbody2D>();
                    if (rb != null)
                    {
                        Vector2 dir = (other.transform.position - transform.position).normalized;
                        rb.AddForce(dir * knockbackForce, ForceMode2D.Impulse);
                    }
                }
            }
        }
    }
}
