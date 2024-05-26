using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveLoadManager : MonoBehaviour
{
    [SerializeField] private PlayerManager playerOBJ =new PlayerManager();
    [SerializeField] private List<string> playerKeys =new List<string>();
    
    
    // Start is called before the first frame update
    void Start()
    {
        LoadKeysData();
    }

    void SaveKeysData()
    {
        string jsonKeysData = JsonUtility.ToJson(playerKeys);
        PlayerPrefs.SetString("key", jsonKeysData);
        PlayerPrefs.Save();
    }

    bool LoadKeysData()
    {
        if (PlayerPrefs.HasKey("keys"))
        {
            string jsonData = PlayerPrefs.GetString("keys");
            playerOBJ = JsonUtility.FromJson<PlayerManager>(jsonData);
            return true;
        }
        else
        {
            playerKeys =new List<string>();
            SaveKeysData();
            return false;
        }
    }
    
    void SavePlayerData(string key)
    {
        string jsonData = JsonUtility.ToJson(playerOBJ);
        PlayerPrefs.SetString(key, jsonData);
        PlayerPrefs.Save();
    }
    
    bool LoadPlayerData(string key)
    {
        if (PlayerPrefs.HasKey(key))
        {
            string jsonData = PlayerPrefs.GetString(key);
            playerOBJ = JsonUtility.FromJson<PlayerManager>(jsonData);
            return true;
        }
        else
        {
            playerOBJ = new PlayerManager();
            return false;
        }
    }
}