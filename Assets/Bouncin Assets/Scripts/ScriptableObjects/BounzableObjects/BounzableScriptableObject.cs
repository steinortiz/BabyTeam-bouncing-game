using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/BounzableObject", order = 1)]
public class BounzableScriptableObject : ScriptableObject
{
    
    public float bounceSpeed;
    [Range(0f,1f)] public float rapidezDeCambio;
    public AudioClip sound;
}
