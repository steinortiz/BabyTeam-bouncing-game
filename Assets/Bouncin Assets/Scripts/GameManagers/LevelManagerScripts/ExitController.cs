using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class ExitController : AbstractPuzzle
{
    private bool isVisible;
    public bool isFinal;
    public string nextSceneName;
   
    
    [Header("Animation Settings")]
    [SerializeField] private LeanTweenType activateAnimType;
    [SerializeField] private LeanTweenType disactivateAnimType;
    [SerializeField] private float animTime;
    private RewardScriptableObject playerMaterial;
    
    public override void OnOtherTriggerHandler(Collider other)
    {
        if (other.transform.TryGetComponent(out SuperStrike player))
        {
            if (activateInstruction == ActivateInstruction.OnTriggerEnter)
            {
                playerMaterial = player.ballData;
                Destroy(player.gameObject);
                CompletePuzzle();
            }
        }
    }
    
    public override void Activate()
    {
        gameObject.SetActive(true);
        LeanTween.moveLocal(gameObject, transform.position + transform.forward * 1.3f, animTime).setEase(activateAnimType);
        base.Activate();
    }
    
    public override void Disactivate()
    {
        LeanTween.moveLocal(gameObject, transform.position - transform.forward * 1.3f, animTime).setEase(disactivateAnimType).setOnComplete(
            () =>
            {
                gameObject.SetActive(false);
                base.Disactivate();
            });
    }

    public override void CompletePuzzle()
    {
        base.CompletePuzzle();
        LeanTween.moveLocal(gameObject, transform.position - transform.forward*1.3f, animTime).setEase(disactivateAnimType).setOnComplete(() =>
        {
            if (isFinal)
            {
                MachineInteractionsReciever.Instance.SpawnReward(playerMaterial);
                SaveLoadManager.Data.GetCurrentPlayer().currentLevel = nextSceneName;
                SaveLoadManager.Data.GetCurrentPlayer().CleanLevels();
                SaveLoadManager.Data.GetCurrentPlayer().currentBall = null;
                SaveLoadManager.Data.SaveAllPlayerData();
            }
            else
            {
                if (nextSceneName != "")
                {
                    SceneLoader.Instance.LoadAdditiveLevel(nextSceneName); 
                }
            }
        });
    }

}
