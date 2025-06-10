using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("UI 요소")]
    public TextMeshProUGUI timerText;   // 생존 시간 텍스트
    public TextMeshProUGUI healthText;  // 체력 표시 텍스트
    public GameObject gameOverPanel;    // 게임 오버 패널

    private float elapsedTime;
    private bool isGameOver = false;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    void Update()
    {
        if (isGameOver) return;

        // 생존 시간 갱신
        elapsedTime += Time.deltaTime;

        int minutes = Mathf.FloorToInt(elapsedTime / 60f);
        int seconds = Mathf.FloorToInt(elapsedTime % 60f);
        timerText.text = $"생존시간 / {minutes:D2}:{seconds:D2}";
    }

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

    public void GameOver()
    {
        if (isGameOver) return;

        isGameOver = true;
        Time.timeScale = 0f;

        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
        }
        else
        {
            Debug.LogWarning("GameOverPanel이 연결되지 않았습니다!");
        }
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void BackToMain()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
