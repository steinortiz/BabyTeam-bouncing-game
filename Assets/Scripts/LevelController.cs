using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    [SerializeField] private SuperStrike playerPrefab;
    private SuperStrike playerInstance=null;
    private bool isPlayerOnGame;
    [SerializeField] private GameObject spawnPoint;
    public List<BounzableObject> objetivos = new List<BounzableObject>();

   

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
    public void OnDestroyPlayer()
    {
        isPlayerOnGame = false;
    }

    public void CheckDestroy(BounzableObject obj)
    {
        if (objetivos.Contains(obj))
        {
            objetivos.Remove(obj);
        }

        if (objetivos.Count == 0)
        {
           WintheGame(); 
        }
    }

    private void WintheGame()
    {
        Debug.Log("Ganaste el level");
    }

}
