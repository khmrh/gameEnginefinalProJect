using System.Collections;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("UI ���")]
    public TextMeshProUGUI timerText;   // ���� �ð� �ؽ�Ʈ
    public TextMeshProUGUI healthText;  // ü�� ǥ�� �ؽ�Ʈ

    private float elapsedTime;

    private void Awake()
    {
        // �̱��� �ν��Ͻ� ����
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    void Update()
    {
        // ���� �ð� ����
        elapsedTime += Time.deltaTime;

        int minutes = Mathf.FloorToInt(elapsedTime / 60f);
        int seconds = Mathf.FloorToInt(elapsedTime % 60f);
        timerText.text = $"�����ð� / {minutes:D2}:{seconds:D2}";
    }

    // �÷��̾� ü�� UI ����
    public void UpdateHealthUI(int current, int max)
    {
        if (healthText != null)
        {
            int percent = Mathf.CeilToInt((float)current / max * 100f);
            healthText.text = $"HP {current} / {max} ({percent}%)";
        }
        else
        {
            Debug.LogWarning("healthText�� ����Ǿ� ���� �ʽ��ϴ�!");
        }
    }
}
