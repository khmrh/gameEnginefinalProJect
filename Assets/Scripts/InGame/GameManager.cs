using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("UI ���")]
    public TextMeshProUGUI timerText;   // ���� �ð� �ؽ�Ʈ
    public TextMeshProUGUI healthText;  // ü�� ǥ�� �ؽ�Ʈ
    public GameObject gameOverPanel;    // ���� ���� �г�

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

        // ���� �ð� ����
        elapsedTime += Time.deltaTime;

        int minutes = Mathf.FloorToInt(elapsedTime / 60f);
        int seconds = Mathf.FloorToInt(elapsedTime % 60f);
        timerText.text = $"�����ð� / {minutes:D2}:{seconds:D2}";
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
            Debug.LogWarning("healthText�� ����Ǿ� ���� �ʽ��ϴ�!");
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
            Debug.LogWarning("GameOverPanel�� ������� �ʾҽ��ϴ�!");
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
