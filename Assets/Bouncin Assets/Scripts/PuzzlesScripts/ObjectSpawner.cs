using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawner : AbstractPuzzle
{
    [SerializeField] private GameObject objectToSpawn;
    public  void Activate()
    {
        base.Activate();
        if (objectToSpawn != null)
        {
            GameObject obj = Instantiate(objectToSpawn, this.transform.position, this.transform.rotation);
        }
    }
    

}
