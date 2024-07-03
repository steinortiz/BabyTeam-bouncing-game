using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
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
    
    [SerializeField] private bool adminMode;
    
    
    private SuperStrike playerInstance=null;
    private bool isPlayerOnGame;
    private List<GameObject> objetiveList = new List<GameObject>();
    private bool isObjetiveCOmplete=false;
    [SerializeField] private GameObject spawnPoint;
    [SerializeField] private ExitController ExitPoint;
    [SerializeField] private string nexSceneName;
    
    void Start()
    {
        SpawnPlayer();
    }
    private void Update()
    {
        if (!isPlayerOnGame && adminMode && Input.GetButtonDown("Jump"))
        {
            SpawnPlayer();
        }
    }

    public void SpawnPlayer()
    {
        if (GameController.Instance != null && spawnPoint!=null)
        {
            playerInstance = Instantiate(GameController.Instance?.playerPrefab, spawnPoint.transform.position,new Quaternion(0,0,0,0));
            isPlayerOnGame = true;
        }
    }

    public void SetObjetive(GameObject objetive)
    {
        objetiveList.Add(objetive);
    }
    public void OnDestroyPlayer()
    {
        isPlayerOnGame = false;
    }

    public void CompleteObjetive(GameObject obj)
    {
        if (objetiveList.Contains(obj))
        {
            objetiveList.Remove(obj);
        }

        if (objetiveList.Count == 0)
        {
           WintheGame(); 
        }
    }

    private void WintheGame()
    {
        Debug.Log("Ganaste el level");
        ActivateExit();
    }

    private void ActivateExit()
    {
        isObjetiveCOmplete = true;
        ExitPoint.gameObject?.SetActive(true);

    }

    public void LoadNextLevelScene()
    {
        Debug.Log("Exit");
        if (nexSceneName != "")
        {
            Destroy(playerInstance.gameObject);
            SceneLoader.Instance.LoadAdditiveLevel(nexSceneName); 
        } 
    }

    

    
}
