using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MachineUIController : MonoBehaviour
{
    [SerializeField] private Canvas machineUI;
    
    [SerializeField] private List<Image> ballsRewardsImages;
    [SerializeField] private List<Image> levelsImages;
    [SerializeField] private List<Image> secretLevelsImages;
    
    public static MachineUIController Instance { get; private set; }
    private void Awake() 
    {

        if (Instance != null && Instance != this) 
        { 
            Destroy(this.gameObject); 
        } 
        else 
        {
            Instance = this; 
        } 
    }
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    
}
