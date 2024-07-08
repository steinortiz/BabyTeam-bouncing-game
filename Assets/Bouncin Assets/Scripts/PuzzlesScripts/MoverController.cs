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
    [SerializeField] private int limitMovements =-1;
    private int counter;
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
            if (limitMovements <=0 || counter < limitMovements)
            {
                counter += 1;
                isMoving=true;
                Vector3 forward = this.transform.forward;
                Vector3 finalpos = this.transform.localPosition +( forward.normalized *distanceToMove);
                this.transform
                    .LeanMoveLocal(finalpos,timeToMove).setEase(animType).setOnComplete(() =>
                    {
                        isMoving = false;
                    });
                base.Activate();
            }
            else
            {
                CompletePuzzle();
            }
        }
    }

    public override void CompletePuzzle()
    {
        if (returnToInitialPosOnComplete)
        {
            LeanTween.cancel(this.transform.gameObject);
            this.transform
                .LeanMoveLocal(initialLocalPosition,timeToMove).setEase(animType).setOnComplete(LateComplete);
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
}
