using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnController : AbstractPuzzle
{
    private bool isVisible;
    public bool canSpawn;
    
    [Header("Animation Settings")]
    [SerializeField] private LeanTweenType activateAnimType;
    [SerializeField] private LeanTweenType disactivateAnimType;
    [SerializeField] private float animTime;
    [SerializeField] private float ballForce;
    private RewardScriptableObject playerMaterial;

    
    
    public override void CompletePuzzle()
    {
        base.CompletePuzzle();
        SpawnPlayer();
        
    }
    void SpawnPlayer()
    {
        SuperStrike playerInstance = Instantiate(GameController.Instance.playerPrefab, transform.position,new Quaternion(0,0,0,0));
        playerInstance.SetUP();
        GameController.Instance.isPlayerOnGame = true;
        playerInstance.BoostSpeed(ballForce,transform.forward);
        Disactivate();
    }

    public void ActivateAndSpawn()
    {
        gameObject.SetActive(true);
        LeanTween.moveLocal(gameObject, transform.position + transform.forward * 1.3f, animTime).setEase(activateAnimType).setOnComplete(
            ()=>
            {
                base.Activate();
                canSpawn = true;
                SpawnPlayer();
            });
    }
    public override void Activate()
    {
        gameObject.SetActive(true);
        LeanTween.moveLocal(gameObject, transform.position + transform.forward * 1.3f, animTime).setEase(activateAnimType).setOnComplete(()=>
        {
            canSpawn = true;
            base.Activate();
        });
        
    }

    public override void Disactivate()
    {
        canSpawn = false;
        LeanTween.moveLocal(gameObject, transform.position - transform.forward * 1.3f, animTime).setEase(disactivateAnimType).setOnComplete(
            () =>
            {
                //gameObject.SetActive(false);
            });
        base.Disactivate();
    }
    
}
