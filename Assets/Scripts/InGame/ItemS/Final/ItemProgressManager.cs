using UnityEngine;

public static class ItemProgressManager
{
    public static void SaveItemLevels(int zone, int rotator, int healing)
    {
        PlayerPrefs.SetInt("ZoneLevel", zone);
        PlayerPrefs.SetInt("RotatorLevel", rotator);
        PlayerPrefs.SetInt("HealingShotLevel", healing);
        PlayerPrefs.Save();
    }

    public static (int zone, int rotator, int healing) LoadItemLevels()
    {
        int zone = PlayerPrefs.GetInt("ZoneLevel", 0);
        int rotator = PlayerPrefs.GetInt("RotatorLevel", 0);
        int healing = PlayerPrefs.GetInt("HealingShotLevel", 0);
        return (zone, rotator, healing);
    }

    public static void ResetItemLevels()
    {
        PlayerPrefs.DeleteKey("ZoneLevel");
        PlayerPrefs.DeleteKey("RotatorLevel");
        PlayerPrefs.DeleteKey("HealingShotLevel");
    }
}




