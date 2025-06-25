using UnityEngine;
using TMPro;

public class KillCounter : MonoBehaviour
{
    public static int killCount = 0;
    public TextMeshProUGUI killText;

    private void Start()
    {
        UpdateUI();
    }

    public static void AddKill()
    {
        killCount++;
        if (Instance != null)
            Instance.UpdateUI();
    }

    private void UpdateUI()
    {
        if (killText != null)
            killText.text = $"처치 수: {killCount}";
    }

    // Singleton 등록
    public static KillCounter Instance;

    private void Awake()
    {
        Instance = this;
    }
}
