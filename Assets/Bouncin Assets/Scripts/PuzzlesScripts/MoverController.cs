using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoverController : AbstractPuzzle
{
    [SerializeField] private float timeToMove;
    [SerializeField] private int distanceToMove;
    [SerializeField] private LeanTweenType animType;
    [SerializeField] private bool returnToInitialPosOnComplete;
    private Vector3 initialLocalPosition;
    private bool isMoving;

    void Start()
    {
        base.Start();
        initialLocalPosition = this.transform.localPosition;

    }

    public override void Activate()
    {
        if (!isMoving && !isPuzzleBlocked)
        {
            
            isMoving=true;
            Vector3 forward = this.transform.forward;
            Vector3 finalpos = this.transform.localPosition +( forward.normalized *distanceToMove);
            this.transform
                .LeanMoveLocal(finalpos,timeToMove).setEase(animType).setOnComplete(() =>
                {
                    isMoving = false;
                    Disactivate();
                });
            base.Activate();
            
        }
    }

    public override void CompletePuzzle()
    {
        if (returnToInitialPosOnComplete)
        {
            ReturnToOriginalPos(LateComplete);
        }
        else
        {
            LateComplete();
        }
    }

    void LateComplete()
    {
        base.CompletePuzzle();
    }

    public void ReturnToOriginalPosition()
    {
        LeanTween.cancel(this.transform.gameObject);
        Vector3 finalpos = this.transform.localPosition;
        this.transform.LeanMoveLocal(finalpos, timeToMove).setEase(animType);

    }
    private void ReturnToOriginalPos(Action callback = null)
    {
        LeanTween.cancel(this.transform.gameObject);
        Vector3 finalpos = this.transform.localPosition;
        this.transform.LeanMoveLocal(finalpos,timeToMove).setEase(animType).setOnComplete(callback);
    }
}
