using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class TimerQuiz : AbstractPuzzle
{
    [SerializeField] private float time;
    [SerializeField] private TMP_Text timerText;
    private float currentTime;
    
    void Update()
    {
        if (isPuzzleActive)
        {
            currentTime -= Time.deltaTime;
            if(timerText !=null) timerText.text = currentTime.ToString();
            if (currentTime <= 0)
            {
                Disactivate();
            }
        }
    }

    public override void Activate()
    {
        currentTime = time;
       
        timerText.gameObject?.SetActive(true);
        base.Activate();
    }

    public override void Disactivate()
    {
        timerText.gameObject?.SetActive(false);
        base.Disactivate();
    }
    
    
}
