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
    public bool isObjetive;
    
}

[Serializable]
public class TopoHole
{
    public GameObject spawnPlaceObject;
    private Topo topoPrefab;
    private BounzableObject topoInstance;

    public void CleanData()
    {
        topoInstance = null;
    }
    public Topo GetOBJPrefab()
    {
        return topoPrefab;
    }
    public BounzableObject GetOBJInstance()
    {
        return topoInstance;
    }
    
    public void setOBJs(Topo topoPrefabin, BounzableObject topoInstancein)
    {
        topoPrefab = topoPrefabin;
        topoInstance = topoInstancein;
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
                Topo topoPrefab = topoList[rngOBJ];
                if (topoPrefab != null)
                {
                    //instanciarlo
                    BounzableObject topoInstance = Instantiate(topoPrefab.obj, spawnPlaces[rng].spawnPlaceObject.transform);
                    //pasarselo al topoHole
                    topoHole.setOBJs(topoPrefab, topoInstance);
                    //decirle que esta ocupado
                    spawnPlaces.Remove(topoHole);
                    spawnPlacesInUse.Add(topoHole);
                    //quitarlo de la lista
                    topoList.Remove(topoPrefab);
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
        LeanTween.move(topoInstance.gameObject, topoInstance.transform.position + Vector3.up * altura,
                animationTime / 2)
            .setEase(animationCurve).setOnComplete(
                () =>
                {
                    LeanTween.move(topoInstance.gameObject, Vector2.zero, animationTime / 2).setDelay(waitTime).setEase(animationCurve).setOnComplete(
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
        Destroy(topoInstance.gameObject);
        Topo topoPrefab = topoHole.GetOBJPrefab();
        topoList.Add(topoPrefab);
    }
    public override void Activate()
    {
        foreach (Topo topoObj in topoList)
        {
            if (topoObj.isObjetive)
            {
                countObjetives += 1;
            }
        }
        if (countObjetives > 0)
        {
            SetAsObjetive(true);
        }
        Debug.Log("topo activado");
        isPuzzlePlaying = true;
        SpawnTopo();
    }

    public override void Pause()
    {
        isPuzzlePlaying = false;
    }

    public override void OnCompletePuzzle()
    {
        Pause();
        if (trigger != null)
        {
            trigger.Activate();
        }
    }

    public override void ObjetivesChecker(BounzableObject objCheck)
    {
        foreach (TopoHole topoHole in spawnPlaces)
        {
            BounzableObject topoInstance = topoHole.GetOBJInstance();
            if (topoInstance == objCheck)
            {
                spawnPlaces.Add(topoHole);
                spawnPlacesInUse.Remove(topoHole);
                if (topoInstance.isObjetive)
                {
                    countObjetives -= 1;
                    if (countObjetives == 0)
                    {
                        LevelController.Instance.CompleteObjetive(this.transform.gameObject);
                        OnCompletePuzzle();
                    }
                }
            }
        }
    }

}
