using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class GravityController : AbstractPuzzle
{
    public enum Direccion
    {
        Default,
        Up,
        Left,
        Right,
        Front,
        Back
    }

    public Direccion directionGravity;

    void SetGravity(Direccion direc)
    {
        float magnitude= Physics.gravity.magnitude;
        Vector3 newGrav = Vector3.down;
        Vector3 newHor = Vector3.right;
        Vector3 newVer = Vector3.forward;
        switch (direc)
        {
            case Direccion.Up:
                newGrav =  Vector3.up;
                break;
            case Direccion.Left:
                newGrav =  Vector3.left;
                newHor = Vector3.down;
                newVer= Vector3.forward;
                break;
            case Direccion.Right:
                newGrav =  Vector3.right;
                newHor = Vector3.up;
                newVer= Vector3.forward;
                break;
            case Direccion.Back:
                newGrav =  Vector3.back;
                newVer= Vector3.up;
                break;
            case  Direccion.Front:
                newGrav =  Vector3.forward;
                newVer= Vector3.down;
                break;
            default:
                newGrav =  Vector3.down;
                break;
        }
        LevelController.Instance.gravityDir = newGrav;
        LevelController.Instance.horzDir = newHor;
        LevelController.Instance.vertDir = newVer;
        Physics.gravity =newGrav*magnitude;
    }

    public override void Activate()
    {
        base.Activate();
        SetGravity(directionGravity);
    }

    public override void Disactivate()
    {
        base.Disactivate();
        SetGravity(Direccion.Default);
    }
}
