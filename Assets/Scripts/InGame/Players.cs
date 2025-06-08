using System.Collections;
using TMPro;
using UnityEngine;

public class Players : MonoBehaviour
{
    public float moveSpeed = 10f;

    [Header("체력 설정")]
    public int maxHealth = 5;
    private int currentHealth;

    [SerializeField] Sprite spriteup;
    [SerializeField] Sprite spriteDown;
    [SerializeField] Sprite spriteLeft;
    [SerializeField] Sprite spriteRight;

    Rigidbody2D rb;
    SpriteRenderer sR;

    Vector2 input;
    Vector2 velocity;

    [SerializeField] TextMeshProUGUI Scoretext;

    public float score;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sR = GetComponent<SpriteRenderer>();
        rb.bodyType = RigidbodyType2D.Kinematic;

        currentHealth = maxHealth;
        GameManager.Instance?.UpdateHealthUI(currentHealth, maxHealth);
    }

    private void Update()
    {
        input.x = Input.GetAxisRaw("Horizontal");
        input.y = Input.GetAxisRaw("Vertical");

        velocity = input.normalized * moveSpeed;

        if (input.sqrMagnitude > .01f)
        {
            if (Mathf.Abs(input.x) > Mathf.Abs(input.y))
            {
                sR.sprite = input.x > 0 ? spriteRight : spriteLeft;
            }
            else
            {
                sR.sprite = input.y > 0 ? spriteup : spriteDown;
            }
        }
    }

    private void FixedUpdate()
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
        // 사망 처리 로직 추가 가능 (재시작, 게임 오버 등)
    }
}
