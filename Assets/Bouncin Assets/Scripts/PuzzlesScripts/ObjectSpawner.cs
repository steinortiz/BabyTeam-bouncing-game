using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
public class SpawnPointObj
{
    public Transform spawnPoint;
    //[HideInInspector]
    public AbstractPuzzle objectSpawned;
}
public class ObjectSpawner : AbstractPuzzle
{
    [SerializeField] private List<AbstractPuzzle> objectsToSpawn;
    [SerializeField] private SpawnPointObj[] spawnPoints;
    private int destroyCount;

    public bool randomizeSpawn = false;
    private int completedCount;
    public  void Activate()
    {
        base.Activate();
        if (randomizeSpawn)
        {
            objectsToSpawn = objectsToSpawn.OrderBy( x => Random.value ).ToList( );
        }
        destroyCount = objectsToSpawn.Count;
        foreach (AbstractPuzzle objData in objectsToSpawn)
        {
            foreach (SpawnPointObj data in spawnPoints)
            {
                if (data.objectSpawned == null)
                {
                    AbstractPuzzle objBall = Instantiate(objData, data.spawnPoint.position, data.spawnPoint.rotation);
                    data.objectSpawned = objBall;
                    objBall.onPuzzleCompletedEvent.AddListener(()=>
                    {
                        GetCompletedObjects(objBall);
                    });
                    break;
                }
            }
        }
    }

    public override void Disactivate()
    {
        foreach (SpawnPointObj data in spawnPoints)
        {
            if (data.objectSpawned != null)
            {
                Destroy(data.objectSpawned.gameObject);
            }
        }
        base.Disactivate();
    }

    private void GetCompletedObjects(AbstractPuzzle obj)
    {
        destroyCount -= 1;
        if (destroyCount == 0)
        {
            CompletePuzzle();
        }
    }
    
    
    

}
