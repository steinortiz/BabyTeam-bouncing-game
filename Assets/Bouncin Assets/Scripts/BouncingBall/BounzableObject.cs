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

    public void OnCollisionExit(Collision collision)
    {
        if (collision.collider.transform.TryGetComponent(out SuperStrike player))
        {
            if (GameController.Instance != null && data != null) GameController.Instance.PlaySFX(data.sound);
            //player?.SetBounce(data);
        }
        
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.transform.TryGetComponent(out SuperStrike player))
        {
            //onPlayerCollitionEvent?.Invoke();
            OnPlayerCollisionHandler(player);
            if(data !=null)player.SetBounce(data);
        }
    }

    public virtual void OnPlayerCollisionHandler(SuperStrike player)
    {
        
    }
}
