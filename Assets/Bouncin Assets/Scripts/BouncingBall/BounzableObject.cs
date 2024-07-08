using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BounzableObject : MonoBehaviour
{
    public BounzableScriptableObject data;

    public bool HurtOnBounce()
    {
        if(transform.TryGetComponent(out LifeController _lifeController))
        {
            return _lifeController.AddTolife(-1);
            
        }
        return false;
    }
}
