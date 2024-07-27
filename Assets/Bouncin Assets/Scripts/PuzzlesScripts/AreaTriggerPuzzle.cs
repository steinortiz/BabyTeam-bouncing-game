using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaTriggerPuzzle : AbstractPuzzle
{
    [SerializeField] private bool onPlayerEntry;
    
    public void OnTriggerEnter(Collider other)
    {
        if (!onPlayerEntry || other.transform.tag == "Player")
        {
            Activate();
        }
    }
    public void OnTriggerExit(Collider other)
    {
        if (!onPlayerEntry || other.transform.tag == "Player")
        {
            Disactivate();
        }
    }
}
