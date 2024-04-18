using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public abstract class AbstractPuzzle : MonoBehaviour
{
    public void SetAsObjetive(bool set)
    {
        if (set)
        {
            LevelController.Instance.SetObjetive(this.transform.gameObject);
        }
    }
    public abstract void Activate();
    public abstract void Pause();
    public abstract void OnCompletePuzzle();

    public abstract void ObjetivesChecker(BounzableObject obj);
}
