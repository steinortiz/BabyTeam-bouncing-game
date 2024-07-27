using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


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
public class AudioManager : MonoBehaviour
{
    [SerializeField] private List<MusicSource> musicPlaylist;
    [SerializeField] private AudioSource musicPlayer;
    [SerializeField] private AudioSource sfxPlayer;
    [SerializeField] private List<AudioSource> _audioSources=new List<AudioSource>();
    [Range(0f,1f)]public float generalVolumen;
    
    public static AudioManager Instance { get; private set; }
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
    
    // AUDIO MANAGEMENT
    public void PlaySFX(AudioClip clip, bool looping =false)
    {
        /*if (looping)
        {
            sfxPlayer.clip = clip;
            sfxPlayer.loop = looping;
            sfxPlayer.Play();
            return;
        }
        sfxPlayer.PlayOneShot(clip);
        */
        
        foreach (AudioSource source in _audioSources)
        {
            if (!source.isPlaying)
            {
                source.clip = clip;
                source.loop = looping;
                source.volume = generalVolumen;
                source.Play();
                break;
            }
        }
    }

    public void StopSFX(AudioClip clip)
    {
        /*if (clip == sfxPlayer)
        {
            sfxPlayer.Stop();
            return ;
        }*/
        foreach (AudioSource source in _audioSources)
        {
            if (source.isPlaying && source.clip == clip)
            {
                source.Stop();
                break;
            }
        }
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

    public void UpdateVolume()
    {
        
    }
}
