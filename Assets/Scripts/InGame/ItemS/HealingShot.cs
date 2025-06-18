using UnityEngine;

public class HealingShot : MonoBehaviour
{
    public float speed = 8f;
    public int damage = 1;
    private Vector2 direction;
    private float healChance;
    private float lifeTime = 3f;

    public void Initialize(Vector2 dir, float chance)
    {
        direction = dir;
        healChance = chance;
        Destroy(gameObject, lifeTime);
    }

    void Update()
    {
        transform.Translate(direction * speed * Time.deltaTime, Space.World);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            Enemy enemy = other.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);

                if (Random.value <= healChance)
                {
                    Players player = FindObjectOfType<Players>();
                    if (player != null) player.Heal(1); // Heal 함수 필요
                }
            }
            Destroy(gameObject);
        }
    }

    public void Initialize(Vector2 dir, float chance, int level)
    {
        direction = dir;
        healChance = chance;
        damage = 1 + level;
        Destroy(gameObject, lifeTime);
    }

}
