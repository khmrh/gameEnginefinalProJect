using TMPro;
using UnityEngine;

public class Players : MonoBehaviour
{
    public float moveSpeed = 2f;

    [Header("체력 설정")]
    public int maxHealth = 5;
    private int currentHealth;

    [SerializeField] Sprite spriteup;
    [SerializeField] Sprite spriteDown;
    [SerializeField] Sprite spriteLeft;
    [SerializeField] Sprite spriteRight;

    Rigidbody2D rb;
    SpriteRenderer sR;
    Vector2 input, velocity;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sR = GetComponent<SpriteRenderer>();
        rb.bodyType = RigidbodyType2D.Kinematic;
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
        Debug.Log("생존 버프 적용됨!");
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
    }

    void FixedUpdate()
    {
        rb.MovePosition(rb.position + velocity * Time.fixedDeltaTime);
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        GameManager.Instance?.UpdateHealthUI(currentHealth, maxHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("플레이어 사망");
        GameManager.Instance?.EndGame(); // 생존 시간 저장
    }
}
