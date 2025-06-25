using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PlayerData
{
    public List<string> collectedItems = new List<string>();
    public int stage = 1;
    public float bestSurvivalTime = 0f;
    public int totalKills = 0;
}

public class GameDataManager : MonoBehaviour
{
    public static GameDataManager Instance;
    public PlayerData playerData;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        playerData = LoadData(); // �ڵ� �ε�
    }

    public void SaveData(PlayerData playerData)
    {
        string filePath = Application.persistentDataPath + "/Player_data.json";
        string json = JsonUtility.ToJson(playerData, true);
        System.IO.File.WriteAllText(filePath, json);
        Debug.Log("���� ������ �����: " + json);
    }

    public PlayerData LoadData()
    {
        string filePath = Application.persistentDataPath + "/Player_data.json";
        if (System.IO.File.Exists(filePath))
        {
            string json = System.IO.File.ReadAllText(filePath);
            return JsonUtility.FromJson<PlayerData>(json);
        }
        return new PlayerData();
    }

    public void SaveSurvivalTime(float time)
    {
        if (time > playerData.bestSurvivalTime)
        {
            playerData.bestSurvivalTime = time;
            SaveData(playerData);
            Debug.Log("�ְ� ���� �ð� ���ŵ�: " + time);
        }
    }
}
