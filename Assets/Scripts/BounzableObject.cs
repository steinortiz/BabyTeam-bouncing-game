using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum BounceObjectType
{
    Default,
    Coin,
    Enemy,

}
public class BounzableObject : MonoBehaviour
{
    public BounzableScriptableObject data;
    public bool isObjetive;
    public BounceObjectType objectType;
    public bool doesNeedSuperStrike;
    public int life;
    public ParticleSystem particles;
    [SerializeField] private AbstractPuzzle trigger =null;

    public delegate void BallDestroyed();
    public event BallDestroyed ballDestroyedEvent;

    public bool Interact(bool onSuperStrike)
    {
        
        CheckPuzzle();
        CheckObjetive();
        CheckDestroy(onSuperStrike);
        
        if (objectType == BounceObjectType.Enemy)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void Start()
    {
        if (isObjetive)
        {
            SetObjetive();
        }
    }

    void CheckPuzzle()
    {
        if (trigger!= null)
        {
            trigger.Activate();
        }
    }
    void SetObjetive()
    {
        LevelController.Instance.SetObjetive(this.gameObject);
    }
    void CheckObjetive()
    {
        if (isObjetive)
        {
            LevelController.Instance.CompleteObjetive(this.gameObject);
        }
    }
    void CheckDestroy(bool onSuperStrike)
    {
        if (!doesNeedSuperStrike || onSuperStrike)
        {
            if (life>0)
            {
                life -= 1;
                if (life == 0)
                {
                    Invoke("DestroyBO",Time.deltaTime); 
                }
            }  
        }
        
    }
    void DestroyBO()
    {
        
        if (particles != null)
        {
            ParticleSystem particlesSys = Instantiate<ParticleSystem>(particles, transform.localPosition,transform.rotation);
            particlesSys.Play();
        }
        Destroy(this.gameObject);
    }

    void SetParentToReport()
    {
        
    }

    
    
}
