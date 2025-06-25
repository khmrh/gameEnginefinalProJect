using UnityEngine;
using UnityEngine.SceneManagement;

public class InGameEnd : MonoBehaviour
{
    [Header("ESC Ű�� ������ ��� �г�")]
    public GameObject pausePanel;

    private bool isPaused = false;

    void Update()
    {
        if (!isPaused && Input.GetKeyDown(KeyCode.Escape))
        {
            PauseGame();
        }
    }

    void PauseGame()
    {
        isPaused = true;
        Time.timeScale = 0f;
        pausePanel.SetActive(true);
    }

    public void ReStrart()
    {
        isPaused = false;
        Time.timeScale = 1f;
        pausePanel.SetActive(false);
    }

    public void BackMian()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
