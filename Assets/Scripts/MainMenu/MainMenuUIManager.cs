using TMPro;
using UnityEngine;

public class MainMenuUIManager : MonoBehaviour
{
    public TextMeshProUGUI bestSurvivalTimeText;
    public TextMeshProUGUI collectedItemsText;
    public TextMeshProUGUI killCountText;

    void Start()
    {
        PlayerData data = GameDataManager.Instance.LoadData();
        bestSurvivalTimeText.text = $"최고 생존 시간: {FormatTime(data.bestSurvivalTime)}";
        killCountText.text = $"총 처치 수: {data.totalKills}";
        collectedItemsText.text = $"획득 아이템: {string.Join(", ", data.collectedItems)}";
    }

    string FormatTime(float time)
    {
        int minutes = Mathf.FloorToInt(time / 60f);
        int seconds = Mathf.FloorToInt(time % 60f);
        return $"{minutes:D2}:{seconds:D2}";
    }
}
