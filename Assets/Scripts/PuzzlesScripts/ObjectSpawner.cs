using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawner : AbstractPuzzle
{
    [SerializeField] private GameObject objectToSpawn;
    public override void Activate()
    {
        if (objectToSpawn != null)
        {
            GameObject obj = Instantiate(objectToSpawn, this.transform.position, this.transform.rotation);
            if (obj.transform.TryGetComponent(out AbstractPuzzle puzzle))
            {
                puzzle.Activate();
            }
        }
    }

    public override void Pause()
    {
        throw new System.NotImplementedException();
    }

    public override void OnCompletePuzzle()
    {
        throw new System.NotImplementedException();
    }

    public override void ObjetivesChecker(BounzableObject obj)
    {
        throw new System.NotImplementedException();
    }
}
