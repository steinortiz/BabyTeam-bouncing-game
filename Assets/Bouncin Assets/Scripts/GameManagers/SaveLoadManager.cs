using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;


[Serializable]
public class PlayerData
{
    public string nameSlot;
    
    // THIS IS THE GAME DATA TO SAVE...
    public RewardScriptableObject currentBall;
    public string currentLevel ="";
    public int currentCoins=1;
    public List<string> completedLevels =new List<string>();
    public List<string> completedSecretLevels =new List<string>();
    public List<RewardScriptableObject> rewards =new List<RewardScriptableObject>();

    /// CONSTRUCTOR:
    public PlayerData(int index) 
    {
        nameSlot = "Slot " + (index + 1).ToString();
        currentLevel ="";
        currentCoins = 1;
    }

    public void CleanLevels()
    {
        completedLevels = new List<string>();
        completedSecretLevels = new List<string>(); 
    }
}

[Serializable]
public class PlayerSlots
{
    public List<PlayerData> allPlayersData= new List<PlayerData>();

    public PlayerSlots()
    {
        allPlayersData = new List<PlayerData>();
        PlayerData initialPlayer = new PlayerData(allPlayersData.Count);
        allPlayersData.Add(initialPlayer);
    }
}

public class SaveLoadManager : MonoBehaviour
{
    public int currentPlayerIndex;
    [SerializeField] public PlayerSlots playerSlotsData = new PlayerSlots();
    [SerializeField] private string keyPlayers="";
    public static SaveLoadManager Data { get; private set; }
    private void Awake() 
    {
        if (Data != null && Data != this) 
        { 
            Destroy(this.gameObject); 
        } 
        else 
        { 
            
            Data = this; 
        } 
    }
  
    void Start()
    {
        LoadPlayerData();
    }

    public void SaveAllPlayerData( )
    {
        Debug.Log("Saving Data");
        string jsonData = JsonUtility.ToJson(playerSlotsData);
        PlayerPrefs.SetString(keyPlayers, jsonData);
        PlayerPrefs.Save();
    }
    
    public bool LoadPlayerData( )
    {
        if (PlayerPrefs.HasKey(keyPlayers))
        {
            Debug.Log("Loading Saved Data");
            string jsonData = PlayerPrefs.GetString(keyPlayers);
            playerSlotsData = JsonUtility.FromJson<PlayerSlots>(jsonData);
            return true;
        }
        else
        {
            Debug.Log("Creating New Data");
            CreateNewData();
            return false;
        }
    }

    public void SetCurrentPlayer(int index)
    {
        currentPlayerIndex = index;
    }
    public void SetCurrentPlayer(string slotName)
    {
        currentPlayerIndex = playerSlotsData.allPlayersData.FindIndex(x => x.nameSlot == slotName);
    }

    public PlayerData GetCurrentPlayer()
    {
        return playerSlotsData.allPlayersData[currentPlayerIndex];
    }

    public void CleanAllData()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
        CreateNewData();
        
    }

    public void CreateNewData()
    {
        currentPlayerIndex =0;
        playerSlotsData = new PlayerSlots();
        SaveAllPlayerData();
    }
    
    
    
}
