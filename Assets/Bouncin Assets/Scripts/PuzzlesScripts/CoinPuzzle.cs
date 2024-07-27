using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinPuzzle : AbstractPuzzle
{
    public override void CompletePuzzle()
    {
        base.CompletePuzzle();
        SaveLoadManager.Data.GetCurrentPlayer().currentCoins += 1;
        MachineInteractionsReciever.Instance.SpawnCoin();
    }
    
}
