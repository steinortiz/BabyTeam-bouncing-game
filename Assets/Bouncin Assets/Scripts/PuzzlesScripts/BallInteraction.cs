using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BallType
{
    Default,
    Enemy,

}
public class BallInteraction : AbstractPuzzle
{
    public bool destroyOnComplete;
    public BallType ballType;
    public override void ActivateSuper(SuperStrike player)
    {
        base.ActivateSuper(player);
        if(CheckEnemy())player.KillBall();
    }

    public bool CheckEnemy()
    {
        if (ballType == BallType.Enemy) return true;
        return false;
        
    }

    public override void CompletePuzzle()
    {
        base.CompletePuzzle();
        if (destroyOnComplete)
        {
            Destroy(this.gameObject,Time.deltaTime);
        }
    }
}

