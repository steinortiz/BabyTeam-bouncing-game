using System;
using System.Collections;
using System.Collections.Generic;
using Babyteam.SO.UI;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class MachineInteractionsReciever : MonoBehaviour
{

    public bool isLoaded;
    
    [Header("Palanca")] 
    [SerializeField] public ExtendedButton palanca;
    [SerializeField] private LeanTweenType animTypePalanca;
    [SerializeField] private float animTimePalanca;
    [SerializeField] private AudioClip palancaCrackSound;
    [SerializeField] private AudioClip palancaCrackSound2;
    
    [Header("Coin Receptor")]
    [SerializeField] private GameObject receptor;
    [SerializeField] private GameObject fakeCoin;
    [SerializeField] private CoinsController coinPrefab;
    [SerializeField] private Transform coinSpawnCollection;
    [SerializeField] private AudioClip coinClapSound;

    [Header("Machine lights")]
    [SerializeField] private MeshRenderer m_Lights;
    [SerializeField] private Material turnOnLightsMaterial;
    [SerializeField] private Material turnOffLightsMaterial;
    [SerializeField] private MeshRenderer m_YellowLight;
    [SerializeField] private Material yellowLightMaterial;
    [SerializeField] private MeshRenderer m_GreenLight;
    [SerializeField] private Material greenLightMaterial;

    [Header("Machine Buttons")] 
    [SerializeField] private ExtendedButton m_Button;
    [SerializeField] private Material turnOnButtonMaterial;
    [SerializeField] private Material turnOffButtonMaterial;
    [SerializeField] private AudioClip buttonClickSound;

    [Header("Ball Rewards")]
    [SerializeField] private RewardController rewardPrefab;
    [SerializeField] private Transform rewardCollection;
    

    public static MachineInteractionsReciever Instance { get; private set; }
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

    private void OnEnable()
    {
        palanca.onSubmit.AddListener(PalancaButtonInteraction);
       
    }

    private void OnDisable()
    {
        palanca.onSubmit.RemoveAllListeners();
        m_Button.onSubmit.RemoveAllListeners();
    }

    private void Start()
    {
        //SpawnAllCoins();
        SpawnCoin();
    }

    public void TurnOnMachineLights()
    {
        m_Lights.material = turnOnLightsMaterial;
    }

    public void TurnOffMachineLights()
    {
        m_Lights.material = turnOffLightsMaterial;
    }

    public void TurnLightsCoins(bool green =false)
    {
        if (green)
        {
            m_YellowLight.material = turnOffLightsMaterial;
            m_GreenLight.material = greenLightMaterial;
        }
        else
        {
            m_YellowLight.material = yellowLightMaterial;
            m_GreenLight.material = turnOffLightsMaterial;
        }
    }

    public void SpawnAllCoins()
    {
        int coins = SaveLoadManager.Data.GetCurrentPlayer().currentCoins;
        for (int i = 0; i < coins; i++)
        {
            SpawnCoin();
        }
    }

    public void SpawnCoin()
    {
        Instantiate(coinPrefab, coinSpawnCollection);
    }
    
    public void LoadMachine()
    {
        AudioManager.Instance.PlaySFX(coinClapSound);
        fakeCoin.gameObject.SetActive(true);
        palanca.interactable = true;
        
    }

    private void PalancaButtonInteraction()
    {
        palanca.interactable = false;
        isLoaded = true;
        // Animation
        Vector3 begin = new Vector3(0,179,0);
        Vector3 end = new Vector3(0,360,0);
        AudioManager.Instance.PlaySFX(palancaCrackSound);
        LeanTween.rotateLocal(palanca.gameObject,begin, animTimePalanca/2).setEase(animTypePalanca);
        LeanTween.rotateLocal(receptor, begin, animTimePalanca/2).setEase(animTypePalanca).setOnComplete(() =>
        {
            palanca.transform.localRotation = Quaternion.Euler(0, 181, 0);
            receptor.transform.localRotation = Quaternion.Euler(0, 181, 0);
            fakeCoin.gameObject.SetActive(false);
            int random = Random.Range(0, 5);
            if (random == 0)
            {
                SpawnCoin();
            }
            else
            {
                PrepareForSpawn();
            }
            isLoaded = false;
            AudioManager.Instance.PlaySFX(palancaCrackSound2);
            LeanTween.rotateLocal(palanca.gameObject, end, animTimePalanca/2).setEase(animTypePalanca);
            LeanTween.rotateLocal(receptor, end, animTimePalanca / 2).setEase(animTypePalanca);
        });
    }

    private void PrepareForSpawn()
    {
        TurnLightsCoins(true);
        TurnOnMachineLights();
        m_Button.onSubmit.AddListener(SpawnButtonInteraction);
        m_Button.interactable = true;
        m_Button.GetComponent<MeshRenderer>().material = turnOnButtonMaterial;
        
        /// Cargar escena de nuevo.....
        /// depende si quiero que empice denuevo desde el primer nivel o desde este mismo...
        if (LevelController.Instance.mustReload)
        {
            LevelController.Instance.ReLoadScene();
        }
    }
    private void SpawnButtonInteraction()
    {
        AudioManager.Instance.PlaySFX(buttonClickSound);
        if (LevelController.Instance.spawnPoint.canSpawn)
        {
            //SaveLoadManager.Data.GetCurrentPlayer().currentCoins -= 1;
            m_Button.GetComponent<MeshRenderer>().material = turnOffButtonMaterial;
            m_Button.interactable = false;
            TurnLightsCoins();
            LevelController.Instance.spawnPoint.CompletePuzzle();
        }
    }

    public void SpawnReward(RewardScriptableObject data)
    {
        RewardController reward = Instantiate(rewardPrefab, rewardCollection);
        reward.SetUP(data);
    }

    public void PrepareForEscape()
    {
        TurnOnMachineLights();
        m_Button.onSubmit.AddListener(EscapeButtonInteraction);
        m_Button.interactable = true;
        m_Button.GetComponent<MeshRenderer>().material = turnOnButtonMaterial;
    }

    private void EscapeButtonInteraction()
    {
        AudioManager.Instance.PlaySFX(buttonClickSound);
        SceneLoader.Instance.LoadMainMenu();
    }

}
