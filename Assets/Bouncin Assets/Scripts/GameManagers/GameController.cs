using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    
    
    [SerializeField] private List<AudioSource> _audioSources=new List<AudioSource>();
    [Range(0f,1f)]public float generalVolumen;
    public Languages generalLanguage;
    public int totalCoins;
    public CoinsController coinPrefab;
    
    [SerializeField] public SuperStrike playerPrefab;


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
        //SaveLoadManager
    }
    
    
    // AUDIO MANAGEMENT
    public bool PlayerAudio(AudioClip clip, bool inLoop =false)
    {
        foreach (AudioSource source in _audioSources)
        {
            if (!source.isPlaying)
            {
                source.clip = clip;
                source.loop = inLoop;
                source.volume = generalVolumen;
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
