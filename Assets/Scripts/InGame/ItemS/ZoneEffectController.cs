using UnityEngine;

public class ZoneEffectController : MonoBehaviour
{
    [Header("���� ����")]
    public int damage = 1;
    public float tickInterval = 1f;
    public float baseScale = 1f;

    private float tickTimer;
    private Transform player;

    public int level = 1;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        UpdateZoneStats(); // �ʱ�ȭ
    }

    private void Update()
    {
        if (player != null)
        {
            transform.position = player.position;
        }

        tickTimer += Time.deltaTime;
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Enemy") && tickTimer >= tickInterval)
        {
            Enemy e = other.GetComponent<Enemy>();
            if (e != null)
            {
                e.TakeDamage(damage);
                tickTimer = 0f;
            }
        }
    }

    public void SetLevel(int newLevel)
    {
        level = newLevel;
        UpdateZoneStats();
    }

    void UpdateZoneStats()
    {
        // ������ �������� ���� Ŀ����, ������ ��������, ���� �پ��
        damage = 1 + level;
        tickInterval = Mathf.Max(0.2f, 1f - level * 0.1f);
        float scale = baseScale + (level - 1) * 0.2f;
        transform.localScale = new Vector3(scale, scale, 1f);
    }
}
