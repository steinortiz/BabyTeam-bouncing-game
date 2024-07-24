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
    [SerializeField] private ExtendedButton palanca;
    [SerializeField] private LeanTweenType animTypePalanca;
    [SerializeField] private float animTimePalanca;
    
    [Header("Coin Receptor")]
    [SerializeField] private GameObject receptor;
    [SerializeField] private GameObject fakeCoin;
    [SerializeField] private CoinsController coinPrefab;
    [SerializeField] private Transform coinSpawnCollection;

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

    [Header("Ball Rewards")]
    [SerializeField] private GameObject rewardPrefab;
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
        m_Button.onSubmit.AddListener(PlayerButtonInteraction);
    }

    private void OnDisable()
    {
        palanca.onSubmit.RemoveListener(PalancaButtonInteraction);
        m_Button.onSubmit.RemoveListener(PlayerButtonInteraction);
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

    public void SpawnCoin()
    {
        Instantiate(coinPrefab, coinSpawnCollection);
        GameController.Instance.totalCoins += 1;
    }
    
    public void LoadMachine()
    {
        fakeCoin.gameObject.SetActive(true);
        palanca.interactable = true;
        isLoaded = true;
    }

    private void PalancaButtonInteraction()
    {
        palanca.interactable = false;
        // Animation
        Vector3 begin = new Vector3(0,179,0);
        Vector3 end = new Vector3(0,360,0);
        LeanTween.rotateLocal(palanca.gameObject,begin, animTimePalanca/2).setEase(animTypePalanca);
        LeanTween.rotateLocal(receptor, begin, animTimePalanca/2).setEase(animTypePalanca).setOnComplete(() =>
        {
            palanca.transform.localRotation = Quaternion.Euler(0, 181, 0);
            receptor.transform.localRotation = Quaternion.Euler(0, 181, 0);
            fakeCoin.gameObject.SetActive(false);
            int random = Random.Range(0, 2);
            if (random == 0)
            {
                SpawnCoin();
            }
            else
            {
                PrepareForSpawn();
            }
            isLoaded = false;
            LeanTween.rotateLocal(palanca.gameObject, end, animTimePalanca/2).setEase(animTypePalanca);
            LeanTween.rotateLocal(receptor, end, animTimePalanca / 2).setEase(animTypePalanca);
        });
    }

    private void PrepareForSpawn()
    {
        TurnLightsCoins(true);
        TurnOnMachineLights();
        GameController.Instance.totalCoins += 1;
        m_Button.interactable = true;
        m_Button.GetComponent<MeshRenderer>().material = turnOnButtonMaterial;
        
        /// Cargar escena de nuevo.....
        /// depende si quiero que empice denuevo desde el primer nivel o desde este mismo...
        if (LevelController.Instance.mustReload)
        {
            LevelController.Instance.ReLoadScene();
        }
    }
    private void PlayerButtonInteraction()
    {
        if (LevelController.Instance.canSpawn)
        {
            m_Button.GetComponent<MeshRenderer>().material = turnOffButtonMaterial;
            m_Button.interactable = false;
            TurnLightsCoins();
            LevelController.Instance.SpawnPlayer();
        }
    }

    public void SpawnReward(RewardScriptableObject data)
    {
        Instantiate(rewardPrefab, rewardCollection);
    }

}
