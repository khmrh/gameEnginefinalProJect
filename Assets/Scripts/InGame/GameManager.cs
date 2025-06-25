using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("UI 요소")]
    public TextMeshProUGUI timerText;
    public Slider healthSlider;
    public GameObject gameOverPanel; // 게임 오버 패널

    private float elapsedTime;
    public float ElapsedTime => elapsedTime;

    private bool isGameOver = false;

    public int enemyKillCount = 0;
    public int killThreshold = 10;
    public int damageIncreasePerThreshold = 1;

    public int GetBonusDamage()
    {
        return (enemyKillCount / killThreshold) * damageIncreasePerThreshold;
    }

    public void RegisterKill()
    {
        enemyKillCount++;
    }

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
            healthSlider.maxValue = max;
            healthSlider.value = current;
        }
    }

    public void EndGame()
    {
        if (isGameOver) return;

        isGameOver = true;

        GameDataManager.Instance.SaveSurvivalTime(elapsedTime);

        Time.timeScale = 0f; // 시간 정지
        gameOverPanel.SetActive(true); // 게임오버 UI 활성화
    }

    // 버튼 함수
    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ReturnToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu"); // 메인 메뉴 씬 이름에 맞게 수정
    }
}
