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
    public BounzableObject obj;
    public bool isTopoObjetive;
    public int lifeSaved=0;

}

[Serializable]
public class TopoHole
{
    public GameObject spawnPlaceObject;
    private Topo topoData;
    private BounzableObject topoInstance;

    public void CleanData()
    {
        topoInstance = null;
    }
    public Topo GetOBJData()
    {
        return topoData;
    }
    public BounzableObject GetOBJInstance()
    {
        return topoInstance;
    }
    
    public void setOBJs(Topo topoPrefabIn, BounzableObject topoInstanceIn)
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
    public AbstractPuzzle trigger =null;
    [SerializeField] private bool isPuzzlePlaying;
    private float timer;

    private void Start()
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
        if (countObjetives > 0)
        {
            SetAsObjetive(true);
        }
    }

    private void Update()
    {
        if (isPuzzlePlaying)
        {
            timer += Time.deltaTime;
            if (timer > delayDeAparicion)
            {
                timer = 0f;
                SpawnTopo();
            }
        }
    }

    bool SpawnTopo()
    {
        Debug.Log("Intentado Spawing");
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
                    BounzableObject topoInstance = Instantiate(thisTopoData.obj, spawnPlaces[rng].spawnPlaceObject.transform);
                    
                    topoInstance.SetPuzzleFather(this,thisTopoData.isTopoObjetive);
                    if (topoInstance.TryGetComponent(out LifeController _lifeController))
                    {
                        _lifeController.Start();
                        _lifeController.UpdateLife(thisTopoData.lifeSaved);
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
        else
        {
            Debug.Log("Spawing Failed");
            
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
        BounzableObject topoInstance = topoHole.GetOBJInstance();
        Topo topoData = topoHole.GetOBJData();
        if( topoInstance.TryGetComponent(out LifeController _lifeController))
        {
            topoData.lifeSaved = _lifeController.life;
        }
        Destroy(topoInstance.gameObject);
        topoList.Add(topoData);
    }
    public override void Activate()
    {
        
        Debug.Log("topo activado");
        isPuzzlePlaying = true;
        SpawnTopo();
    }

    public override void Pause()
    {
        isPuzzlePlaying = false;
    }
    
    public override void ObjetivesChecker(bool isObjectObjetive)
    {
        if (isObjectObjetive)
        {
            countObjetives -= 1;
            if (countObjetives <= 0)
            {
                OnCompletePuzzle();
            }
        }
    }
    public override void OnCompletePuzzle()
    {
        Pause();
        LevelController.Instance.CompleteObjetive(this.transform.gameObject);
        if (trigger != null)
        {
            trigger.Activate();
        }
    }

}
