using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum BounceObjectType
{
    Default,
    Enemy,
    Coin,

}
public class BounzableObject : MonoBehaviour
{
    public BounzableScriptableObject data;
    public BounceObjectType type;
    public int life;
    public bool doesDamage;
    public ParticleSystem particles;
    
    public bool Interact()
    {
        Hurt();
        return doesDamage;
    }

    void Hurt()
    {
        if (life>0)
        {
            life -= 1;
            if (life == 0)
            {
                Invoke("Killself",Time.deltaTime); 
            }
        }
    }
    void Killself()
    {
        if (particles != null)
        {
            ParticleSystem particlesSys = Instantiate<ParticleSystem>(particles, transform.localPosition,transform.rotation);
            particlesSys.Play();
        }
        Destroy(this.gameObject);
    }
    
    
}
