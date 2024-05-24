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
            Destroy(this); 
        } 
        else 
        { 
            Instance = this; 
        } 
    }
    
    [SerializeField] private bool adminMode;
    [SerializeField] private List<AudioSource> _audioSources=new List<AudioSource>();
    [SerializeField] private SuperStrike playerPrefab;
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
        playerInstance = Instantiate(playerPrefab, spawnPoint.transform.position,new Quaternion(0,0,0,0));
        isPlayerOnGame = true;
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
        ExitPoint.gameObject.SetActive(true);

    }

    public void LoadNextLevelScene()
    {
        Debug.Log("Exit");
        if (nexSceneName != "")
        {
            SceneManager.LoadScene(nexSceneName);
        } 
    }

    public bool PlayerAudio(AudioClip clip, bool inLoop =false)
    {
        foreach (AudioSource source in _audioSources)
        {
            if (!source.isPlaying)
            {
                source.clip = clip;
                source.loop = inLoop;
                source.Play();
                return true;
            }
        }
        return false;
    }

    public bool StopAudio(AudioClip clip)
    {
        foreach (AudioSource source in _audioSources)
        {
            if (source.isPlaying && source.clip == clip)
            {
                source.Stop();
                return true;
            }
        }
        return false;
    }
    

    
}
