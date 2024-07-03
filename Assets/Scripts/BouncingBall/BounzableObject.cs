using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BounzableObject : AbstractPuzzle
{
    public BounzableScriptableObject data;

    public override void Activate(SuperStrike player)
    {
        player?.SetBounce(this);
        GameController.Instance.PlayerAudio(data.sound);
        if(transform.TryGetComponent(out LifeController _lifeController))
            {
                bool isCompleted = _lifeController.Interact();
                if (isCompleted)CompletePuzzle();
            }
        base.Activate(player);
    }
}
