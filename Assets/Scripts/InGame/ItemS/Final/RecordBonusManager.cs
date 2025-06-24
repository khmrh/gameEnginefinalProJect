using UnityEngine;

public static class RecordBonusManager
{
    private const string HighTimeKey = "BestSurvivalTime";
    private const string ClearedMaxKey = "ClearedMax";

    public static void SaveSurvivalTime(float seconds)
    {
        if (seconds > GetBestTime())
        {
            PlayerPrefs.SetFloat(HighTimeKey, seconds);
            PlayerPrefs.Save();
        }
    }

    public static float GetBestTime()
    {
        return PlayerPrefs.GetFloat(HighTimeKey, 0f);
    }

    public static void SetClearedMax(bool value)
    {
        PlayerPrefs.SetInt(ClearedMaxKey, value ? 1 : 0);
        PlayerPrefs.Save();
    }

    public static bool HasClearedMax()
    {
        return PlayerPrefs.GetInt(ClearedMaxKey, 0) == 1;
    }

    public static void ApplyRecordBonus(Players player)
    {
        float best = GetBestTime();

        if (best >= 300f) // 예: 5분 이상 생존 시
        {
            player.IncreaseBaseMoveSpeed(0.5f);
            player.IncreaseMaxHealth(20);
        }

        if (HasClearedMax())
        {
            player.StartWithAllItemsLevel1();
            player.ApplyEnemyDifficultyMultiplier(2f + Random.Range(0f, 3f)); // 2~5배 강화
        }
    }
}
