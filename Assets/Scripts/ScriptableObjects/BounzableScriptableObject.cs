using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BounceObjectType
{
    Ground,
    Enemy,
    BoostBall
    
}
[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/BounzableObject", order = 1)]
public class BounzableScriptableObject : ScriptableObject
{
    public BounceObjectType type;
    public float bounceSpeed;
    [Range(0f,1f)] public float rapidezDeCambio;
    public AudioClip sound;
}
