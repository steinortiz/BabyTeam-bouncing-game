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
    private SuperStrike playerInstance=null;
    private bool isPlayerOnGame;
    private List<GameObject> objetiveList = new List<GameObject>();
    private bool isObjetiveCOmplete=false;
    [SerializeField] private GameObject spawnPoint;
    [SerializeField] private LeanTweenType spawnAnimType;
    [SerializeField] private float spawnAnimTime;
    [SerializeField] private float spawnAnimForce;
    [SerializeField] private ExitController ExitPoint;
    [SerializeField] private string nextSceneName;
    [HideInInspector] public Vector3 gravityDir= Vector3.down;
    [HideInInspector] public Vector3 horzDir= Vector3.right;
    [HideInInspector] public Vector3 vertDir = Vector3.forward;
    public UnityEvent OnLevelComplete;
    [HideInInspector]public bool mustReload;
    [HideInInspector]public bool canSpawn;
    
    private void Update()
    {
        if (!isPlayerOnGame && Input.GetButtonDown("Jump"))
        {
            if(GameController.Instance.adminMode) SpawnPlayer();
        }
    }

    private void Start()
    {
        bool mustSpawn = false;
        if (GameController.Instance != null)
        {
            mustSpawn= GameController.Instance.isPlayerOnGame;
            Debug.Log(mustSpawn);
        }
        SpawnProcess(mustSpawn);
    }

    public void SpawnProcess(bool spawn = false)
    {
        LeanTween.moveLocal(spawnPoint, spawnPoint.transform.position + spawnPoint.transform.forward, spawnAnimTime)
            .setEase(spawnAnimType).setOnComplete(() =>
            {
                canSpawn = true;
                if (spawn)
                {
                    SpawnPlayer();
                    Debug.Log("spawn");
                }
            });
    }

    public void SpawnPlayer()
    {
        
        if (GameController.Instance != null && spawnPoint!=null)
        {
            MakeSpawnRigid(false);
            playerInstance = Instantiate(GameController.Instance.playerPrefab, spawnPoint.transform.position,new Quaternion(0,0,0,0));
            GameController.Instance.isPlayerOnGame = true;
            playerInstance.BoostSpeed(spawnAnimForce,spawnPoint.transform.forward);
            Invoke("MakeSpawnRigid",0.5f);
            
        }
    }

    private void MakeSpawnRigid(bool rigid=true)
    {
        spawnPoint.GetComponentInChildren<MeshCollider>().isTrigger = !rigid;
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
        OnLevelComplete.Invoke();
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
        if (nextSceneName != "")
        {
            Destroy(playerInstance.gameObject);
            SceneLoader.Instance.LoadAdditiveLevel(nextSceneName); 
        } 
    }

    public void ReLoadScene()
    {
        SceneLoader.Instance.LoadAdditiveLevel(SceneLoader.Instance.currentLevel);
        canSpawn = false;
    }
    
    

    

    
}
