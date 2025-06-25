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
        bestSurvivalTimeText.text = $"�ְ� ���� �ð�: {FormatTime(data.bestSurvivalTime)}";
        killCountText.text = $"�� óġ ��: {data.totalKills}";
        collectedItemsText.text = $"ȹ�� ������: {string.Join(", ", data.collectedItems)}";
    }

    string FormatTime(float time)
    {
        int minutes = Mathf.FloorToInt(time / 60f);
        int seconds = Mathf.FloorToInt(time % 60f);
        return $"{minutes:D2}:{seconds:D2}";
    }
}
