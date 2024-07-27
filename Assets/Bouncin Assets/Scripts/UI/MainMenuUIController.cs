using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuUIController : MonoBehaviour
{
    public void PlayGameButtonAction()
    {
        SceneLoader.Instance.LoadMainLevel();
    }

    public void OptionButtonAction()
    {
        
    }
    
    public void ExitGameButtonAction()
    {
        
    }

    public void CleanAllMemorie()
    {
        SaveLoadManager.Data.CleanAllData();
    }
}
