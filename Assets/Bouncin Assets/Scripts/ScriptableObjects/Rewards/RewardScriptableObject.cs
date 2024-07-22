using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Ball", menuName = "ScriptableObjects/BallData", order = 1)]
public class RewardScriptableObject : ScriptableObject
{
    public Sprite rewardImage;
    public Material ballMaterial;
    [Range(0f, 1f)] public float influence =0.43f;
    public float fallSpeed=20f;
    public float moveSpeed=14f;
    public float spinSpeed=1000f;
    [Range(0f,1f)] public float airRoce= 0.122f;
}
