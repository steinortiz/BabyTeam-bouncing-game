using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitController : MonoBehaviour
{
    public delegate void ExitLevel();
    public string nextSceneName;
    public event ExitLevel BallExitLevelEvent;
    private void OnTriggerEnter(Collider other)
    {
        LevelController.Instance.LoadNextLevelScene(nextSceneName);
        
    }
}
