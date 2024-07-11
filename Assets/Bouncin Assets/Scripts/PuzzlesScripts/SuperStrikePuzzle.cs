using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuperStrikePuzzle : AbstractPuzzle
{
    public bool canSuperStrike;
    public override void ActivateSuper(SuperStrike player)
    {
        Debug.Log(canSuperStrike);
        player.ActivateSuperStrike(canSuperStrike);
        base.ActivateSuper(player);
    }
}
