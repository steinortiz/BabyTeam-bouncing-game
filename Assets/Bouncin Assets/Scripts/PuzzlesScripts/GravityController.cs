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
        switch (direc)
        {
            case Direccion.Up:
                newGrav = magnitude* Vector3.up;
                break;
            case Direccion.Left:
                newGrav = magnitude* Vector3.left;
                break;
            case Direccion.Right:
                newGrav = magnitude* Vector3.right;
                break;
            case Direccion.Back:
                newGrav = magnitude* Vector3.back;
                break;
            case  Direccion.Front:
                newGrav = magnitude* Vector3.forward;
                break;
            default:
                newGrav = magnitude* Vector3.down;
                break;
        }
        Physics.gravity =newGrav;
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
