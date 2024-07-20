using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] private string mainMenuPath;
    [SerializeField] private string mainLevelPath;
    [SerializeField] private string defaulLevelPath;
    [SerializeField] public string currentLevel = "";
    private UnityAction callbackOnLoad;
    
    public static SceneLoader Instance { get; private set; }
    private void Awake() 
    { 
        // If there is an instance, and it's not me, delete myself.
    
        if (Instance != null && Instance != this) 
        { 
            Destroy(this.gameObject); 
        } 
        else 
        { 
            //DontDestroyOnLoad(this.gameObject);
            Instance = this; 
        } 
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    public void LoadMainMenu()
    {
        UiController.Instance.CloseFullCortina( () =>
        {
            LoadScene(mainMenuPath,callback:() =>
            {
                UiController.Instance.UnPauseUI();
                GameController.Instance.isPlaying = false;
                UiController.Instance.OpenCortinas();
            });
        });
    }
    
    public void LoadMainLevel()
    {
        UiController.Instance.CloseFullCortina( () =>
        {
            LoadScene(mainLevelPath,callback: () =>
            {
                GameController.Instance.isPlaying = true;
                LoadAdditiveLevel(defaulLevelPath,false);
            });
        });
    }

    public void LoadAdditiveLevel(string levelName, bool needCortina = true)
    {
        
        UnityAction callback = () =>
        {
            UiController.Instance.OpenCortinas();
        };
        
        if (needCortina)
        {
            UiController.Instance.CloseSemiCortina( 1080*Vector3.left, () =>
            {
                if(currentLevel !=levelName && SceneManager.loadedSceneCount>1) SceneManager.UnloadSceneAsync(currentLevel);
                currentLevel = levelName;
                LoadScene(levelName,true, callback);
            });
        }
        else
        {
            if(currentLevel !=levelName && SceneManager.loadedSceneCount>1) SceneManager.UnloadSceneAsync(currentLevel);
            currentLevel = levelName;
            LoadScene(levelName,true, callback);
        }
    }

    public void LoadScene(string sceneName, bool isAdditive=false, UnityAction callback =null)
    {
        callbackOnLoad = callback;
        LoadSceneMode mode = LoadSceneMode.Single;
        if(isAdditive) mode = LoadSceneMode.Additive;
        SceneManager.LoadScene(sceneName, mode);
    }
    
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        callbackOnLoad?.Invoke();
    }
}
