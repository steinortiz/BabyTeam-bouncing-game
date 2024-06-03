using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum BounceObjectType
{
    Default,
    Enemy,
    Puzzle,
    Objetive,

}

public class BounzableObject : MonoBehaviour
{
    public BounzableScriptableObject data;
    public BounceObjectType objectType;
    [SerializeField] private AbstractPuzzle trigger = null;
    public bool doesNeedSuperStrike;
    private AbstractPuzzle _puzzleFather;
    private bool isObjetiveInFather;
    
    private void Start()
    {
        SetObjetive();
    }
    
    public bool Interact(bool isSuperStrike)
    {
        CheckPuzzle(isSuperStrike);
        GameController.Instance.PlayerAudio(data.sound);
        return CheckEnemy();

    }
    void CheckPuzzle(bool isSuperStrike)
    {
        if (trigger != null)
        {
            if (!doesNeedSuperStrike || isSuperStrike)
            {
                trigger.Activate();
            }
        }
    }

    bool CheckEnemy()
    {
        if (objectType == BounceObjectType.Enemy)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void SetObjetive()
    {
        if (objectType == BounceObjectType.Objetive)
        {
            LevelController.Instance.SetObjetive(this.gameObject); 
        }
    }
    public void CheckCompleteObjetive()
    {
        if (objectType == BounceObjectType.Objetive)
        {
            LevelController.Instance.CompleteObjetive(this.gameObject);
        }
        CheckObjetiveInFather();
    }

    public bool GetIsObjetive()
    {
        if (objectType == BounceObjectType.Objetive)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public void SetPuzzleFather(AbstractPuzzle puzzle, bool isObjetiveIN)
    {
        _puzzleFather = puzzle;
        isObjetiveInFather = isObjetiveIN;
    }

    public void CheckObjetiveInFather()
    {
        if (_puzzleFather != null)
        {
            _puzzleFather.ObjetivesChecker(isObjetiveInFather);
        }
        
    }
}
