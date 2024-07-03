using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonObjectController : AbstractPuzzle
{
    [SerializeField] private Vector3 scaleDisactive;
    [SerializeField] private Vector3 scaleActive;
    [SerializeField] private float animTime;
    [SerializeField] private LeanTweenType animType;
    [SerializeField] private bool isEnabled;
    

    public override void Activate()
    {
        if (isEnabled)
        {
            this.transform.LeanScale(scaleActive, animTime).setEase(animType);
            base.Activate(); 
        }
    }

    public override void Disactivate()
    {
        if (isEnabled)
        {
            this.transform.LeanScale(scaleDisactive, animTime).setEase(animType);
            base.Disactivate();
        }
    }

    public void SetButtonEnabled(bool enabled)
    {
        isEnabled = enabled;
    }
}
