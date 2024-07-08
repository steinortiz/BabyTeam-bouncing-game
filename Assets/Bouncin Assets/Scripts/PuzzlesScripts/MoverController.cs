using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoverController : AbstractPuzzle
{
    [SerializeField] private float timeToMove;
    [SerializeField] private int distanceToMove;

    public override void Activate()
    {
        Vector3 forward = this.transform.forward;
        Vector3 finalpos = forward +( forward.normalized *distanceToMove);
        this.transform
            .LeanMoveLocal(finalpos,timeToMove)
            .setOnComplete(() =>
            {
                CompletePuzzle();
            });
    base.Activate();
    }
}
