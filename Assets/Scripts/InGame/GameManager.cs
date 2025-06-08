using System.Collections;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("UI 요소")]
    public TextMeshProUGUI timerText;   // 생존 시간 텍스트
    public TextMeshProUGUI healthText;  // 체력 표시 텍스트

    private float elapsedTime;

    private void Awake()
    {
        // 싱글톤 인스턴스 설정
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    void Update()
    {
        // 생존 시간 갱신
        elapsedTime += Time.deltaTime;

        int minutes = Mathf.FloorToInt(elapsedTime / 60f);
        int seconds = Mathf.FloorToInt(elapsedTime % 60f);
        timerText.text = $"생존시간 / {minutes:D2}:{seconds:D2}";
    }

    // 플레이어 체력 UI 갱신
    public void UpdateHealthUI(int current, int max)
    {
        if (healthText != null)
        {
            int percent = Mathf.CeilToInt((float)current / max * 100f);
            healthText.text = $"HP {current} / {max} ({percent}%)";
        }
        else
        {
            Debug.LogWarning("healthText가 연결되어 있지 않습니다!");
        }
    }
}
