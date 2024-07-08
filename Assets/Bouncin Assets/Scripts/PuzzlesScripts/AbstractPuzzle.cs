using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;


public enum PuzzleType
{
    Default,
    Enemy,
    Objetive,

}

public enum ActivateInstruction
{
    None,
    OnEnable,
    OnNormalCollision,
    OnSuperStrike,
}

public abstract class AbstractPuzzle : BounzableObject
{
    
    [SerializeField] private ActivateInstruction activateInstruction;
    protected bool isPuzzleActive;
    protected bool isPuzzleBlocked;
    public bool destroyOnComplete;
    public PuzzleType objectType;
    //public UnityEvent onPlayerCollitionEvent;
    public UnityEvent onPuzzleActivateEvent;
    public UnityEvent onPuzzleDisactiveEvent;
    public UnityEvent onPuzzleCompletedEvent;

    public void Start()
    {
        if (objectType == PuzzleType.Objetive) SetAsObjetive();
    }

    public void OnEnable()
    {
        if (activateInstruction == ActivateInstruction.OnEnable)
        {
            Activate();
        }
    }

    private void OnDestroy()
    {
        //onPlayerCollitionEvent.RemoveAllListeners();
        onPuzzleActivateEvent.RemoveAllListeners();
        onPuzzleDisactiveEvent.RemoveAllListeners();
        onPuzzleCompletedEvent.RemoveAllListeners();
    }

    public virtual void Activate()
    {
        if (!isPuzzleBlocked)
        {
            onPuzzleActivateEvent?.Invoke();
            isPuzzleActive = true;
            bool isDead = HurtOnBounce();
            if (isDead)
            {
                CompletePuzzle();
            } 
        }
    }
    public virtual void Activate(SuperStrike player)
    {
        player?.SetBounce(data);
        GameController.Instance.PlayerAudio(data.sound);
        Activate();
    }

    public virtual void Block(bool value)
    {
        isPuzzleBlocked = value;
    }
    
    public virtual void Disactivate()
    {
        if (!isPuzzleBlocked)
        {
            isPuzzleActive = false;
            onPuzzleDisactiveEvent?.Invoke();
        }
    }
    
    public virtual void CompletePuzzle()
    {
        Disactivate();
        LevelController.Instance?.CompleteObjetive(this.transform.gameObject);
        onPuzzleCompletedEvent?.Invoke();
        if (destroyOnComplete)
        {
            Destroy(this.gameObject,Time.deltaTime);
        }
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.transform.TryGetComponent(out SuperStrike player))
        {
            //onPlayerCollitionEvent?.Invoke();
            OnPlayerCollisionHandler(player);
            if(CheckEnemy())player.KillBall();
        }
    }
    
    public virtual void OnPlayerCollisionHandler(SuperStrike player)
    {
        
        if (activateInstruction == ActivateInstruction.OnNormalCollision)
        {
            Activate(player);
        }
        if(activateInstruction == ActivateInstruction.OnSuperStrike && player.isSuperStrikeActive)
        {
            Activate(player);
        }
        
    }
    
    public void SetAsObjetive()
    {
        LevelController.Instance.SetObjetive(this.transform.gameObject);
    }
    
    public bool CheckEnemy()
    {
        if (objectType == PuzzleType.Enemy) return true;
        return false;
        
    }
}