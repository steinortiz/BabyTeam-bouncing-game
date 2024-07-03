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
    OnEnable,
    OnCollision
}

public abstract class AbstractPuzzle : MonoBehaviour
{
    [SerializeField] private ActivateInstruction activateInstruction;
    public bool doesNeedSuperStrike;
    public bool destroyOnComplete;
    public PuzzleType objectType;
    public UnityEvent onPlayerCollitionEvent;
    public UnityEvent onPuzzleActivateEvent;
    public UnityEvent onPuzzleDisactiveEvent;
    public UnityEvent onPuzzleCompletedEvent;
    [HideInInspector] protected bool isPuzzleActive;
    private AbstractPuzzle _puzzleFather;
    private bool isObjetiveInFather;

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
    
    public void SetAsObjetive()
    {
        LevelController.Instance.SetObjetive(this.transform.gameObject);
    }
    public virtual void Activate()
    {
        isPuzzleActive = true;
        onPuzzleActivateEvent?.Invoke();
    }
    public virtual void Activate(SuperStrike player)
    {
        Activate();
    }
    public bool CheckEnemy()
    {
        if (objectType == PuzzleType.Enemy) return true;
        return false;
        
    }
    public void Disactive()
    {
        isPuzzleActive = false;
        onPuzzleDisactiveEvent?.Invoke();
    }
    public void CompletePuzzle()
    {
        Debug.Log("Puzzle Completed");
        Disactive();
        LevelController.Instance?.CompleteObjetive(this.transform.gameObject);
        onPuzzleCompletedEvent?.Invoke();
        if(destroyOnComplete) Destroy(this.gameObject,Time.deltaTime);
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.transform.TryGetComponent(out SuperStrike player))
        {
            
            OnPlayerCollisionHandler(player);
            if(CheckEnemy())player.KillBall();
        }
    }

    public virtual void OnPlayerCollisionHandler(SuperStrike player)
    {
        if (activateInstruction == ActivateInstruction.OnCollision)
        {
            if (!doesNeedSuperStrike || player.isSuperStrikeActive)
            {
                Activate(player);
            }
        }
        onPlayerCollitionEvent?.Invoke();
    }
    public void SetPuzzleFather(AbstractPuzzle puzzle, bool isObjetiveIN)
    {
        _puzzleFather = puzzle;
        isObjetiveInFather = isObjetiveIN;
    }
}
