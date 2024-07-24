using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


[Serializable]
public class PlayerData
{
    public string currentLevel;
    public RewardScriptableObject currentBall;
    public List<string> completedLevels;
    public List<string> completedSecretLevels;
    public List<RewardScriptableObject> rewards =new List<RewardScriptableObject>();
}


public class SaveLoadManager : MonoBehaviour
{
    [SerializeField] public PlayerData playerSavedData =new PlayerData();
    [SerializeField] private List<string> playerKeys =new List<string>();
    public SceneAsset currentLevel;
    public static SaveLoadManager Data { get; private set; }
    private void Awake() 
    { 
        // If there is an instance, and it's not me, delete myself.
    
        if (Data != null && Data != this) 
        { 
            Destroy(this.gameObject); 
        } 
        else 
        { 
            
            Data = this; 
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
