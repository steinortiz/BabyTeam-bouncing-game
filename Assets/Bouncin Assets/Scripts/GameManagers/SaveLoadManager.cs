using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


[Serializable]
public class PlayerData
{
    public SceneAsset currentLevel;
    public SuperStrike currentBall;
    public int completedLevels;
    public int completedSecretLevels;
    
}


public class SaveLoadManager : MonoBehaviour
{
    [SerializeField] private PlayerData playerSavedData =new PlayerData();
    [SerializeField] private List<string> playerKeys =new List<string>();
    [SerializeField] private List<RewardScriptableObject> AllBallsData =new List<RewardScriptableObject>();
    public RewardScriptableObject currentBallData;
    public SceneAsset currentLevel;
    
    public static SaveLoadManager Instance { get; private set; }
    private void Awake() 
    { 
        // If there is an instance, and it's not me, delete myself.
    
        if (Instance != null && Instance != this) 
        { 
            Destroy(this.gameObject); 
        } 
        else 
        { 
            
            Instance = this; 
        } 
    }
    
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
            //playerOBJ = JsonUtility.FromJson<PlayerData>(jsonData);
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
        string jsonData = ""; //JsonUtility.ToJson(playerOBJ);
        PlayerPrefs.SetString(key, jsonData);
        PlayerPrefs.Save();
    }
    
    bool LoadPlayerData(string key)
    {
        if (PlayerPrefs.HasKey(key))
        {
            string jsonData = PlayerPrefs.GetString(key);
            //playerOBJ = JsonUtility.FromJson<PlayerData>(jsonData);
            return true;
        }
        else
        {
            //playerOBJ = new PlayerData();
            return false;
        }
    }
}
