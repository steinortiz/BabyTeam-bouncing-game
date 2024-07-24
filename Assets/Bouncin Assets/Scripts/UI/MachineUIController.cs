using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MachineUIController : MonoBehaviour
{
    [SerializeField] private Canvas machineUI;
    
    [SerializeField] private List<Image> ballsRewardsImages;
    [SerializeField] private List<Image> levelsImages;
    [SerializeField] private List<Image> secretLevelsImages;
    [SerializeField] private Sprite completedLevel;
    [SerializeField] private Sprite completedLink;
    
    public static MachineUIController Instance { get; private set; }
    private void Awake() 
    {

        if (Instance != null && Instance != this) 
        { 
            Destroy(this.gameObject); 
        } 
        else 
        {
            Instance = this; 
        } 
    }

    public void Start()
    {
        UpdateRewardsImages();
        UpdateLevelImages();
    }

    public void UpdateRewardsImages()
    {
        int count = 0;
        foreach (RewardScriptableObject reward in SaveLoadManager.Data.playerSavedData.rewards)
        {
            if(count< ballsRewardsImages.Count) ballsRewardsImages[count].sprite = reward.rewardImage;
            count += 1;
        }
    }
    
    public void UpdateLevelImages()
    {
        int count = 0;
        foreach (string level in SaveLoadManager.Data.playerSavedData.completedLevels)
        {
            if(count< levelsImages.Count) levelsImages[count].sprite = completedLevel;
            count += 1;
        } 
        //levelsImages[LevelController.Instance.levelIndex].sprite = activeLevel
    }
    
}
