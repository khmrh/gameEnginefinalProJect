using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI; // ← 추가

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("UI 요소")]
    public TextMeshProUGUI timerText;
    public GameObject gameOverPanel;
    public Slider healthSlider;  // ← 추가

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

        elapsedTime += Time.deltaTime;

        int minutes = Mathf.FloorToInt(elapsedTime / 60f);
        int seconds = Mathf.FloorToInt(elapsedTime % 60f);
        timerText.text = $"생존시간 / {minutes:D2}:{seconds:D2}";
    }

    public void UpdateHealthUI(int current, int max)
    {
        if (healthSlider != null)
        {
            healthSlider.value = (float)current / max;
        }
        else
        {
            Debug.LogWarning("Health Slider가 연결되어 있지 않습니다!");
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
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
}
