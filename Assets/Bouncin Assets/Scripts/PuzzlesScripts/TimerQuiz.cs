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
    [SerializeField] private List<AbstractPuzzle> objectsToActivate = new List<AbstractPuzzle>();


    void Start()
    {
        foreach (AbstractPuzzle obj in objectsToActivate)
        {
            obj.onPuzzleCompletedEvent.AddListener(() =>
            {
                DeleteFromPuzzle(obj);
            });
        }
    }
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
        foreach (AbstractPuzzle obj in objectsToActivate)
        {
            obj.gameObject.SetActive(true);
        }
        timerText.gameObject?.SetActive(true);
        base.Activate();
    }

    public override void Disactivate()
    {
        foreach (AbstractPuzzle obj in objectsToActivate)
        {
            obj.gameObject.SetActive(false);
        }
        timerText.gameObject?.SetActive(false);
        base.Disactivate();
    }

    public void DeleteFromPuzzle(AbstractPuzzle obj)
    {
        objectsToActivate.Remove(obj);
        if (objectsToActivate.Count == 0)
        {
            CompletePuzzle();
        }
    }
    
}
