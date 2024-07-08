using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

[Serializable]
public class Topo
{
    public AbstractPuzzle obj;
    public bool isTopoObjetive;
    [HideInInspector]public int lifeSaved=0;
}

[Serializable]
public class TopoHole
{
    public GameObject spawnPlaceObject;
    private Topo topoData;
    private AbstractPuzzle topoInstance;

    public void CleanData()
    {
        topoInstance = null;
    }
    public Topo GetOBJData()
    {
        return topoData;
    }
    public AbstractPuzzle GetOBJInstance()
    {
        return topoInstance;
    }
    
    public void setOBJs(Topo topoPrefabIn, AbstractPuzzle topoInstanceIn)
    {
        topoData = topoPrefabIn;
        topoInstance = topoInstanceIn;
    }
}

public class TopoController : AbstractPuzzle
{
    [SerializeField] private List<TopoHole> spawnPlaces = new List<TopoHole>();
    private List<TopoHole> spawnPlacesInUse = new List<TopoHole>();
    [SerializeField] private List<Topo> topoList = new List<Topo>();
    private int countObjetives = 0;

    // Animacion de Topo
    [SerializeField] private bool randomSpawn;
    [SerializeField] private float delayDeAparicion;
    [SerializeField] private float altura;
    [SerializeField] private float animationTime;
    [SerializeField] private LeanTweenType animationCurve;
    [SerializeField] private float waitTime;
    private float timer;

    
    private void Update()
    {
        if (isPuzzleActive)
        {
            timer -= Time.deltaTime;
            if (timer <=0f )
            {
                timer = delayDeAparicion;
                SpawnTopo();
            }
        }
    }

    private bool SpawnTopo()
    {
        if (spawnPlaces.Count > 0)
        {
            int rng = 0;
            if (randomSpawn)
            {
                rng = Random.Range(0, spawnPlaces.Count);
            }
            TopoHole topoHole = spawnPlaces[rng];
            if (topoList.Count > 0)
            {
                int rngOBJ = Random.Range(0, topoList.Count);
                Topo thisTopoData = topoList[rngOBJ];
                if (thisTopoData != null)
                {
                    //instanciarlo
                    AbstractPuzzle topoInstance = Instantiate(thisTopoData.obj, spawnPlaces[rng].spawnPlaceObject.transform);
                    
                    topoInstance.onPuzzleCompletedEvent.AddListener(ObjetivesChecker);
                    if (topoInstance.TryGetComponent(out LifeController _lifeController))
                    {
                        _lifeController.Start();
                        _lifeController.SetLife(thisTopoData.lifeSaved);
                    }
                    
                    //pasarselo al topoHole
                    topoHole.setOBJs(thisTopoData, topoInstance);
                    
                    //decirle que esta ocupado
                    spawnPlaces.Remove(topoHole);
                    spawnPlacesInUse.Add(topoHole);
                    
                    //quitarlo de la lista
                    topoList.Remove(thisTopoData);
                    
                    //Move objeto
                    MoveSon(topoHole);
                    return true;
                }
            }
        }
        return false; 
    }

    void MoveSon(TopoHole topoHole)
    {
        BounzableObject topoInstance = topoHole.GetOBJInstance();
        Vector3 pos = topoInstance.transform.position;
        LeanTween.move(topoInstance.gameObject, pos + Vector3.up * altura,
                animationTime / 2)
            .setEase(animationCurve).setOnComplete(
                () =>
                {
                    LeanTween.move(topoInstance.gameObject, pos, animationTime / 2).setDelay(waitTime).setEase(animationCurve).setOnComplete(
                            () =>
                            {
                                DispawnTopo(topoHole);
                            });
                });
    }

    void DispawnTopo(TopoHole topoHole)
    {
        spawnPlaces.Add(topoHole);
        spawnPlacesInUse.Remove(topoHole);
        AbstractPuzzle topoInstance = topoHole.GetOBJInstance();
        Topo topoData = topoHole.GetOBJData();
        topoInstance.onPuzzleCompletedEvent.RemoveAllListeners();
        if( topoInstance.TryGetComponent(out LifeController _lifeController))
        {
            topoData.lifeSaved = _lifeController.life;
        }
        
        Destroy(topoInstance.gameObject);
        topoList.Add(topoData);
    }
    public override void Activate()
    {
        foreach (Topo topoData in topoList)
        {
            if( topoData.obj.TryGetComponent(out LifeController _lifeController))
            {
                topoData.lifeSaved = _lifeController.life;
            }
            
            if (topoData.isTopoObjetive)
            {
                countObjetives += 1;
            }
        }
        SpawnTopo();
        base.Activate();
    }

    public void ObjetivesChecker()
    {
        countObjetives -= 1;
        if (countObjetives <= 0)
        {
            CompletePuzzle();
        }
    }
    

}
