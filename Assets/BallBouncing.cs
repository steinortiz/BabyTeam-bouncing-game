using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class BallBouncing : MonoBehaviour
{
    Rigidbody rb;
    [SerializeField] private float baseSpeed;
    private float bounceForce;
    [SerializeField] private float speedBoost;
   
    public bool canBounce; 

    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.transform.GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if(Input.GetButtonDown("Jump") || Input.GetButtonDown("Fire1"))
        {
            OnBounceApply(-gameObject.transform.up);
        }
    }

    void OnCollisionEnter()
    {
        if (canBounce)
        {
            rb.velocity = gameObject.transform.up * speedBoost;
            canBounce = false;
            
        }
        else
        {
            rb.velocity = gameObject.transform.up * baseSpeed;
        }
    }
    void OnBounceApply(Vector3 direction)
    {
        //rb.velocity = speedBoost;
        canBounce = true;
    }
}
