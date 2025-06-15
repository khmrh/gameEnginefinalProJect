using TMPro;
using UnityEngine;

public class MainMenuUIManager : MonoBehaviour
{
    public TextMeshProUGUI bestSurvivalTimeText;

    void Start()
    {
        PlayerData data = GameDataManager.Instance.LoadData();
        string formatted = FormatTime(data.bestSurvivalTime);
        bestSurvivalTimeText.text = $"�ְ� ���� �ð�: {formatted}";
    }

    string FormatTime(float time)
    {
        int minutes = Mathf.FloorToInt(time / 60f);
        int seconds = Mathf.FloorToInt(time % 60f);
        return $"{minutes:D2}:{seconds:D2}";
    }
}
