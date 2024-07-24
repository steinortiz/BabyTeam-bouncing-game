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
    private List<GameObject> objetiveList = new List<GameObject>();
    private bool isObjetiveComplete=false;
    [SerializeField] private bool spawnReward;
    [SerializeField] private GameObject spawnPoint;
    [SerializeField] private LeanTweenType spawnAnimType;
    [SerializeField] private float spawnAnimTime;
    [SerializeField] private float spawnAnimDelay;
    [SerializeField] private float spawnAnimForce;
    [SerializeField] private ExitController ExitPoint;
    [HideInInspector] public Vector3 gravityDir= Vector3.down;
    [HideInInspector] public Vector3 horzDir= Vector3.right;
    [HideInInspector] public Vector3 vertDir = Vector3.forward;
    public UnityEvent OnLevelComplete;
    [HideInInspector]public bool mustReload;
    [HideInInspector]public bool canSpawn;
    
    private void Start()
    {
        bool mustSpawn = false;
        if (GameController.Instance != null)
        {
            mustSpawn= GameController.Instance.isPlayerOnGame;
            Debug.Log(mustSpawn);
        }
        MachineUIController.Instance.UpdateLevelImages();
        SpawnProcess(mustSpawn);
    }

    public void SpawnProcess(bool spawn = false)
    {
        spawnPoint.gameObject.SetActive(true);
        LeanTween.moveLocal(spawnPoint, spawnPoint.transform.position + spawnPoint.transform.forward, spawnAnimTime)
            .setEase(spawnAnimType).setOnComplete(() =>
            {
                canSpawn = true;
                if (spawn)
                {
                    SpawnPlayer();
                    LeanTween.moveLocal(spawnPoint, spawnPoint.transform.position - spawnPoint.transform.forward,spawnAnimTime).setEase(spawnAnimType).setDelay(0.5f).setOnComplete(
                        () =>
                        {
                            spawnPoint.gameObject.SetActive(false);
                        });
                }
            });
    }

    public void SpawnPlayer()
    {
        if (GameController.Instance != null && spawnPoint!=null)
        {
            MakeSpawnRigid(false);
            playerInstance = Instantiate(GameController.Instance.playerPrefab, spawnPoint.transform.position,new Quaternion(0,0,0,0));
            playerInstance.SetUP();
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
        GameController.Instance.isPlayerOnGame = false;
        mustReload = true;
    }

    public void CompleteObjetive(GameObject obj)
    {
        if (objetiveList.Contains(obj))
        {
            objetiveList.Remove(obj);
        }

        if (objetiveList.Count == 0 && !isObjetiveComplete)
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
        isObjetiveComplete = true;
        ExitPoint.gameObject?.SetActive(true);
        SaveLoadManager.Data.playerSavedData.completedLevels.Add(SceneLoader.Instance.currentLevel);
        MachineUIController.Instance.UpdateLevelImages();
        LeanTween.moveLocal(ExitPoint.gameObject, ExitPoint.transform.position + ExitPoint.transform.forward, spawnAnimTime).setEase(spawnAnimType);
    }

    public void LoadNextLevelScene(string nextSceneName)
    {
        RewardScriptableObject rewardData =null;// = playerInstance.GetUP();
        Destroy(playerInstance.gameObject);
        LeanTween.moveLocal(ExitPoint.gameObject, ExitPoint.transform.position - ExitPoint.transform.forward, spawnAnimTime).setEase(spawnAnimType).setOnComplete(
            () =>
            {
                if (spawnReward)
                {
                    MachineInteractionsReciever.Instance.SpawnReward(rewardData);
                }
                else
                {
                    if (nextSceneName != "")
                    {
                        SceneLoader.Instance.LoadAdditiveLevel(nextSceneName); 
                    }
                }
                
            });
        
    }
    public void ReLoadScene()
    {
        SceneLoader.Instance.LoadAdditiveLevel(SceneLoader.Instance.currentLevel);
    }

}
