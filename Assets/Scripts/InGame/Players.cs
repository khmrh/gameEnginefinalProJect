using TMPro;
using UnityEngine;

public class Players : MonoBehaviour
{

    public float baseMoveSpeed = 5f; // �⺻ �̵��ӵ�
    public float moveSpeed = 5f;
    public float baseAttackInterval = 1.0f;
    public float currentAttackInterval = 1.0f;

    public PlayerItemEffects itemManager;

    [Header("ü�� ����")]
    public int maxHealth = 5;
    private int currentHealth;

    [SerializeField] Sprite spriteup;
    [SerializeField] Sprite spriteDown;
    [SerializeField] Sprite spriteLeft;
    [SerializeField] Sprite spriteRight;

    private Rigidbody2D rb;
    private SpriteRenderer sR;
    private Vector2 input, velocity;

    private Collider2D playerCollider; // �� �߰�: �浹 ���ÿ�
    private bool isInvincible = false; // �� ���� ���� ����

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sR = GetComponent<SpriteRenderer>();
        playerCollider = GetComponent<Collider2D>(); // �� Collider2D ��������
        rb.bodyType = RigidbodyType2D.Kinematic;
        if (itemManager == null)
            itemManager = GetComponent<PlayerItemEffects>();
    }

    void Start()
    {
        currentHealth = maxHealth;
        GameManager.Instance?.UpdateHealthUI(currentHealth, maxHealth);

        float bestTime = GameDataManager.Instance.playerData.bestSurvivalTime;
        if (bestTime >= 60f)
        {
            ApplySurvivorBuff();
        }
    }

    void ApplySurvivorBuff()
    {
        moveSpeed *= 1.5f;
        Debug.Log("���� ���� �����!");
    }

    public void SetMoveSpeedMultiplier(float multiplier)
    {
        moveSpeed = baseMoveSpeed * multiplier;
    }

    void Update()
    {
        input.x = Input.GetAxisRaw("Horizontal");
        input.y = Input.GetAxisRaw("Vertical");
        velocity = input.normalized * moveSpeed;

        if (input.sqrMagnitude > .01f)
        {
            sR.sprite = Mathf.Abs(input.x) > Mathf.Abs(input.y)
                ? (input.x > 0 ? spriteRight : spriteLeft)
                : (input.y > 0 ? spriteup : spriteDown);
        }

        if (itemManager != null && itemManager.useZone)
        {
            int zoneLevel = itemManager.GetZoneLevel();
            moveSpeed = baseMoveSpeed + zoneLevel * 0.5f;
            currentAttackInterval = baseAttackInterval / (1f + zoneLevel * 0.2f);
        }
        else
        {
            moveSpeed = baseMoveSpeed;
            currentAttackInterval = baseAttackInterval;
        }
    }

    void FixedUpdate()
    {
        rb.MovePosition(rb.position + velocity * Time.fixedDeltaTime);
    }

    public void Heal(int amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        GameManager.Instance?.UpdateHealthUI(currentHealth, maxHealth);
    }

    public void IncreaseBaseMoveSpeed(float amount)
    {
        baseMoveSpeed += amount;
        moveSpeed = baseMoveSpeed;
    }
    public void TakeDamage(int amount)
    {
        if (isInvincible) return;

        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        GameManager.Instance?.UpdateHealthUI(currentHealth, maxHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
        else
        {
            StartCoroutine(InvincibilityCoroutine()); // ���� ����
        }
    }

    System.Collections.IEnumerator InvincibilityCoroutine()
    {
        isInvincible = true;
        if (playerCollider != null)
            playerCollider.enabled = false;

        yield return new WaitForSeconds(1f); // 1�� ����

        if (playerCollider != null)
            playerCollider.enabled = true;
        isInvincible = false;
    }


    public void IncreaseMaxHealth(int amount)
    {
        maxHealth += amount;
        currentHealth = maxHealth;
    }

    public void StartWithAllItemsLevel1()
    {
        itemManager.ActivateItem(ItemType.Zone);
        itemManager.ActivateItem(ItemType.Rotator);
        itemManager.ActivateItem(ItemType.HealingShot);
    }

    public void ApplyEnemyDifficultyMultiplier(float factor)
    {
        EnemySpawner.SetGlobalDifficultyMultiplier(factor); // �ʿ� �� ����
    }

    void Die()
    {
        Debug.Log("�÷��̾� ���");
        GameManager.Instance?.EndGame(); // ���� �ð� ����
    }
}
