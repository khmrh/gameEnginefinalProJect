using UnityEngine;

public class EnemyZoneDamage : MonoBehaviour
{
    public int damage = 1;
    public float damageInterval = 1f;
    private float nextDamageTime = 0f;

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Enemy") && Time.time >= nextDamageTime)
        {
            Enemy enemy = other.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
                nextDamageTime = Time.time + damageInterval;
            }
        }
    }
}
