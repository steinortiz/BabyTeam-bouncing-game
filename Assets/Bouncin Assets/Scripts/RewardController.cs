using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewardController : MonoBehaviour
{
    [SerializeField] private MeshRenderer renderer;
    [SerializeField] private RewardScriptableObject data;
    [SerializeField] private AudioClip ballBounceSound;

    public void SetUP(RewardScriptableObject dataPlayer)
    {
        data = dataPlayer;
        renderer.material = dataPlayer.ballMaterial;
    }
    
    private void OnMouseUpAsButton()
    {
        if(!SaveLoadManager.Data.GetCurrentPlayer().rewards.Contains(data))SaveLoadManager.Data.GetCurrentPlayer().rewards.Add(data);
        MachineUIController.Instance.UpdateRewardsImages();
        MachineUIController.Instance.SetView("Rewards");
        MachineInteractionsReciever.Instance.PrepareForEscape();
    }

    public void OnCollisionEnter(Collision collision)
    {
        AudioManager.Instance.PlaySFX(ballBounceSound);
    }
}
