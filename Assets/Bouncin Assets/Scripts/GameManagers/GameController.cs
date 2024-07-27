using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;




public class GameController : MonoBehaviour
{
    
    public bool isPlayerOnGame;
    public Languages generalLanguage;
    
    [SerializeField] public SuperStrike playerPrefab;
    [SerializeField] private List<RewardScriptableObject> AllBallsData =new List<RewardScriptableObject>();

    public bool isPlaying = false;

    
    public static GameController Instance { get; private set; }
    private void Awake() 
    { 
        // If there is an instance, and it's not me, delete myself.
    
        if (Instance != null && Instance != this) 
        { 
            Destroy(this.gameObject); 
        } 
        else 
        { 
            DontDestroyOnLoad(this.gameObject);
            Instance = this; 
        } 
    }
    
    public void Update()
    {
        if (isPlaying && Input.GetKeyUp(KeyCode.Escape) && !UiController.Instance.pauseCanvas.enabled)
        {
            Pause();
        }
    }

    public void Pause()
    {
        UiController.Instance.PauseUI();
    }
    public void UnPause()
    {
        UiController.Instance.UnPauseUI();
    }

    public void PlayLevel()
    {
        //SaveLoadManager.SetPlayer(
    }

    public RewardScriptableObject GetBallData()
    {
        int ballRandomIndex = Random.Range(0, AllBallsData.Count);
        if(!isPlayerOnGame || SaveLoadManager.Data.GetCurrentPlayer().currentBall == null) SaveLoadManager.Data.GetCurrentPlayer().currentBall = AllBallsData[ballRandomIndex];
        SaveLoadManager.Data.SaveAllPlayerData();
        return SaveLoadManager.Data.GetCurrentPlayer().currentBall;
    }
}
