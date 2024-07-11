using System;
using System.Collections;
using System.Collections.Generic;
using Babyteam.SO.UI;
using UnityEngine;
using UnityEngine.UI;

public class MachineInteractionsReciever : MonoBehaviour
{
    [Header("Machine lights")]
    [SerializeField] private MeshRenderer machineLights;
    [SerializeField] private Material turnOnMaterial;
    [SerializeField] private Material turnOffMaterial;

    [Header("Machine Buttons")] 
    [SerializeField] private ExtendedButton machineButton;

    [Header("UI")] 
    [SerializeField] private List<Image> ballsRewardsImages;
    [SerializeField] private List<Image> levelsImages;
    [SerializeField] private List<Image> secretLevelsImages;


    private void OnEnable()
    {
        machineButton.onSubmit.AddListener(PlayerButtonInteraction);
    }

    private void OnDisable()
    {
        machineButton.onSubmit.RemoveListener(PlayerButtonInteraction);
    }

    public void TurnOnMachineLights()
    {
        machineLights.material = turnOnMaterial;
    }

    public void TurnOffMachineLights()
    {
        machineLights.material = turnOffMaterial;
    }

    public void TurnOnButtonLight()
    {
        
    }

    private void PlayerButtonInteraction()
    {
        LevelController.Instance.SpawnPlayer();
    }
    
}
