using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinsController : MonoBehaviour
{
    public Vector3 originalScreenPosition;
    public float dif;
    private Vector3 stepDif;
    private Rigidbody rb;
    public float xMin = -5f;
    public float xMax = 5f;
    public float yMin = -5f;
    public float yMax = 5f;
    public float zMin = -5f;
    public float zMax = 5f;

    public void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private Vector3 mOffset;
    private float mZCoord;
    

    private Vector3 GetMouseWorldPos()
    {
        Vector3 mousePoint = Input.mousePosition;
        mousePoint.z = mZCoord;
        return Camera.main.ScreenToWorldPoint(mousePoint);
    }
    
    public void OnMouseDown()
    {
        
        Vector3 aux = Camera.main.transform.position;
        Vector3 step =  Vector3.back*dif + transform.position;//Vector3.MoveTowards( transform.position,new Vector3(transform.position.x,aux.y,aux.z), dif);
        stepDif = step - transform.position;
        transform.position = step;
        
        mZCoord = Camera.main.WorldToScreenPoint(gameObject.transform.position).z;
        mOffset = gameObject.transform.position - GetMouseWorldPos();
        
        rb.useGravity = false;
        
    }

    public void OnMouseUp()
    {
        rb.useGravity = true;
        //transform.position -= stepDif;
        rb.AddForce(-stepDif * dif*100f, ForceMode.Force);
    }

    public void OnMouseDrag()
    {
        transform.position = GetMouseWorldPos()+ mOffset;
        // Clamp the position to be within the defined boundaries
        //transform.position.x = Mathf.Clamp(transform.position.x, xMin, xMax);
        //transform.position.y = Mathf.Clamp(transform.position.y, yMin, yMax);
        //pos.z = Mathf.Clamp(transform.localPosition.z, zMin, zMax);
        //transform.position = Camera.main.ScreenToWorldPoint(pos);

    }
    
    public void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "CoinReceptor")
        {
            if (!MachineInteractionsReciever.Instance.isLoaded || MachineInteractionsReciever.Instance.palanca.interactable)
            {
                MachineInteractionsReciever.Instance.LoadMachine();
                Destroy(this.gameObject);
            }
        }

        if (other.transform.tag == "CoinDespawner")
        {
            MachineInteractionsReciever.Instance.SpawnCoin();
            Destroy(this.gameObject);
        }
    }
}
