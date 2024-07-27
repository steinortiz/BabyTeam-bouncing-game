using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;


public class LevelController : MonoBehaviour
{
    
    public static LevelController Instance { get; private set; }
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
    
    public bool isSecretLevel;
    private SuperStrike playerInstance=null;
    private List<GameObject> objetiveList = new List<GameObject>();
    private bool isObjetiveComplete=false;
    private bool playerIsDead= false;
    [SerializeField] public SpawnController spawnPoint;
    [HideInInspector]public bool mustReload;
    
    //On Gravity change
    [HideInInspector] public Vector3 gravityDir= Vector3.down;
    [HideInInspector] public Vector3 horzDir= Vector3.right;
    [HideInInspector] public Vector3 vertDir = Vector3.forward;
    public UnityEvent OnLevelComplete;
    
    
    
    private void Start()
    {
        bool mustSpawn = false;
        if (GameController.Instance != null)
        {
            mustSpawn= GameController.Instance.isPlayerOnGame;
            SaveLoadManager.Data.SaveAllPlayerData();
            MachineUIController.Instance.UpdateLevelImages();
        }
        SpawnProcess(mustSpawn);
    }

    public void SpawnProcess(bool spawn = false)
    {
        spawnPoint.gameObject.SetActive(true);
        if (spawn)
        {
            spawnPoint.ActivateAndSpawn();
        }
        else
        {
            spawnPoint.Activate();
        }
    }
    
    public void SetObjetive(GameObject objetive)
    {
        objetiveList.Add(objetive);
    }
    public void OnDestroyPlayer()
    {
        GameController.Instance.isPlayerOnGame = false;
        mustReload = true;
        MachineInteractionsReciever.Instance.SpawnCoin();
    }

    public void CompleteObjetive(GameObject obj)
    {
        if (objetiveList.Contains(obj))
        {
            objetiveList.Remove(obj);
        }

        if (objetiveList.Count == 0 && !isObjetiveComplete)
        {
            CompleteLevel(); 
        }
    }

    private void CompleteLevel()
    {
        OnLevelComplete.Invoke();
        isObjetiveComplete = true;
        if (isSecretLevel)
        {
            SaveLoadManager.Data.GetCurrentPlayer().completedSecretLevels.Add(SceneLoader.Instance.currentLevel);
        }
        else
        {
            SaveLoadManager.Data.GetCurrentPlayer().completedLevels.Add(SceneLoader.Instance.currentLevel); 
        }
        MachineUIController.Instance.UpdateLevelImages();
    }
    
    public void ReLoadScene()
    {
        SceneLoader.Instance.LoadAdditiveLevel(SceneLoader.Instance.currentLevel);
    }

}
