using UnityEngine;
using UnityEngine.SceneManagement;

public class GameTimer : MonoBehaviour
{
    public float maxGameTime = 600f; // 10분 제한
    private float elapsedTime = 0f;
    private bool isGameOver = false;

    public Players player;
    public GameObject gameOverPanel;
    public TMPro.TextMeshProUGUI survivalTimeText;

    void Update()
    {
        if (isGameOver) return;

        elapsedTime += Time.deltaTime;

        if (elapsedTime >= maxGameTime)
        {
            EndGame();
        }
    }

    public void EndGame()
    {
        isGameOver = true;

        Time.timeScale = 0f;

        RecordBonusManager.SaveSurvivalTime(elapsedTime);
        survivalTimeText.text = $"생존 시간: {Mathf.FloorToInt(elapsedTime)}초";

        if (/* 조건 예: 최대 레벨 도달 */ true)
        {
            RecordBonusManager.SetClearedMax(true);
        }

        gameOverPanel.SetActive(true);
    }

    public float GetElapsedTime() => elapsedTime;
}
