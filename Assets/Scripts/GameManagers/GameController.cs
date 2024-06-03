using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController Instance { get; private set; }
    
    [SerializeField] private List<AudioSource> _audioSources=new List<AudioSource>();
    [Range(0f,1f)]public float generalVolumen;
    public Languages generalLanguage;
    public int totalCoins;
    public CoinsController coinPrefab;

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

    public void SetLanguage(Languages lang)
    {
        generalLanguage = lang;
        if (LanguageController.Instance != null)
        {
            LanguageController.Instance.UpdateLanguage(lang);
        }
        
    }
}
