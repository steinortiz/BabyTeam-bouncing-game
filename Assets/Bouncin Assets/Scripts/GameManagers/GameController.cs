using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;


public enum MusicContext
{
    MainMenu,
    Level,
    PauseMenu,
}

[Serializable]
public class MusicSource
{
    public MusicContext context;
    public AudioClip clip;
}

public class GameController : MonoBehaviour
{
    
    public bool isPlayerOnGame;
    [SerializeField] private List<MusicSource> musicPlaylist;
    [SerializeField] private AudioSource musicPlayer;
    [SerializeField] private AudioSource sfxPlayer;
    [SerializeField] private List<AudioSource> _audioSources=new List<AudioSource>();
    [Range(0f,1f)]public float generalVolumen;
    public Languages generalLanguage;
    public int totalCoins;
    public CoinsController coinPrefab;
    [SerializeField] public SuperStrike playerPrefab;
    [SerializeField] private List<RewardScriptableObject> AllBallsData =new List<RewardScriptableObject>();
    public RewardScriptableObject currentBallData;

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
    public void PlaySFX(AudioClip clip, bool looping =false)
    {
        if (looping)
        {
            sfxPlayer.clip = clip;
            sfxPlayer.loop = looping;
            sfxPlayer.Play();
            return;
        }
        sfxPlayer.PlayOneShot(clip);
        /*foreach (AudioSource source in _audioSources) //////// Deprecated
        {
            if (!source.isPlaying)
            {
                source.clip = clip;
                source.loop = inLoop;
                source.volume = generalVolumen;
                source.Play();
                return true;
            }
        }*/
    }

    public void StopSFX(AudioClip clip)
    {
        if (clip == sfxPlayer)
        {
            sfxPlayer.Stop();
            return ;
        }
        /*foreach (AudioSource source in _audioSources) //////// Deprecated
        {
            if (source.isPlaying && source.clip == clip)
            {
                source.Stop();
                return true;
            }
        }*/
    }

    public void PlayMusic(MusicContext context)
    {
        var source = musicPlaylist.First(x => x.context == context);
        musicPlayer.Stop();
        musicPlayer.clip= source.clip;
        musicPlayer.Play();
    }

    public void PauseMusic(bool isPaused)
    {
        if (isPaused)
        {
            musicPlayer.Pause();
        }
        else
        {
            musicPlayer.Play();
        }
        
    }

    public RewardScriptableObject GetBallData()
    {
        int ballRandomIndex = Random.Range(0, AllBallsData.Count);
        if(!isPlayerOnGame) currentBallData = AllBallsData[ballRandomIndex];
        return currentBallData;
    }
}
