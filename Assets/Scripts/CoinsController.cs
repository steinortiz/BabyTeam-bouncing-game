using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinsController : MonoBehaviour
{
    public Vector3 mousePosition;
    public float dif;
    public Vector3 stepDif;
    public Rigidbody rb;
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

    public void Update()
    {
        transform.position = new Vector3(transform.position.x,transform.position.y,Mathf.Clamp(transform.position.z, zMin, zMax));
    }

    public Vector3 GetMousePos()
    {
        return Camera.main.WorldToScreenPoint(transform.position);
    }
    
    
    public void OnMouseDown()
    {
        if (Camera.main.orthographic)
        {

            Vector3 aux = Camera.main.transform.position;
            Vector3 step =  Vector3.MoveTowards( transform.position,new Vector3(transform.position.x,aux.y,aux.z), dif);
            stepDif = step - transform.position;
            transform.position = step;
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, Camera.main.transform.position, dif);
        }
        
        mousePosition = Input.mousePosition - GetMousePos();
        Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition - mousePosition);
        rb.MovePosition(pos);
    }

    public void OnMouseUp()
    {
        if (Camera.main.orthographic)
        {
            //transform.position -= stepDif;
            rb.AddForce(-stepDif * dif*100f, ForceMode.Force);
        }
        else
        {
            transform.position += (Vector3.MoveTowards( Camera.main.transform.position, transform.position,dif)- Camera.main.transform.position) +transform.position;
        }
        
    }

    public void OnMouseDrag()
    {
        Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition - mousePosition);
        // Clamp the position to be within the defined boundaries
        pos.x = Mathf.Clamp(pos.x, xMin, xMax);
        pos.y = Mathf.Clamp(pos.y, yMin, yMax);
        pos.z = Mathf.Clamp(transform.position.z, zMin, zMax);
        rb.MovePosition(pos);
    }
}
