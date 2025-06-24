using UnityEngine;

public class ProjectileBase : MonoBehaviour
{
    [Header("공통 설정")]
    public float speed = 10f;
    public float lifetime = 3f;

    [Header("데미지 설정")]
    public int baseDamage = 1;
    public bool isHealingShot = false;
    public float healChance = 0.2f;

    private Vector2 direction;
    private int level = 1;

    private void Start()
    {
        Destroy(gameObject, lifetime);
    }

    public void Initialize(Vector2 dir, int shotLevel, bool healing = false, float healProb = 0f)
    {
        direction = dir.normalized;
        level = shotLevel;
        isHealingShot = healing;
        healChance = healProb;
    }

    private void Update()
    {
        transform.Translate(direction * speed * Time.deltaTime, Space.World);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            Enemy enemy = other.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(baseDamage + level);

                if (isHealingShot && Random.value <= healChance)
                {
                    Players player = FindObjectOfType<Players>();
                    if (player != null)
                        player.Heal(1 + level); // 레벨 기반 회복량 증가 가능
                }
            }

            Destroy(gameObject);
        }
    }
}
