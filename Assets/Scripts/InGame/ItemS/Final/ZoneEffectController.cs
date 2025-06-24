using UnityEngine;

public class ZoneEffectController : MonoBehaviour
{
    [Header("장판 설정")]
    public float healInterval = 2f;
    public int baseHealAmount = 1;
    public float moveSpeedBonus = 1f;
    public float duration = -1f; // -1 = 무제한

    private float timer = 0f;
    private int level = 1;
    private Transform player;
    private Players playerScript;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        playerScript = player.GetComponent<Players>();
        ApplyMoveSpeedBonus();
    }

    private void Update()
    {
        if (player == null || playerScript == null) return;

        transform.position = player.position;
        timer += Time.deltaTime;

        if (healInterval > 0 && timer >= healInterval)
        {
            playerScript.Heal(baseHealAmount + (level - 1));
            timer = 0f;
        }

        if (duration > 0)
        {
            duration -= Time.deltaTime;
            if (duration <= 0)
                Destroy(gameObject);
        }
    }

    public void SetLevel(int newLevel)
    {
        level = Mathf.Clamp(newLevel, 1, 5);
        baseHealAmount = 1 + (level - 1);
        healInterval = Mathf.Max(0.5f, 2f - (level * 0.2f));
        moveSpeedBonus = 1f + (level * 0.2f);
        ApplyMoveSpeedBonus();
    }

    void ApplyMoveSpeedBonus()
    {
        if (playerScript != null)
        {
            playerScript.SetMoveSpeedMultiplier(moveSpeedBonus);
        }
    }

    private void OnDestroy()
    {
        if (playerScript != null)
        {
            playerScript.SetMoveSpeedMultiplier(1f); // 리셋
        }
    }
}
