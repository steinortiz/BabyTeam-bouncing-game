using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ParticleSystemJobs;

public class SuperStrike : MonoBehaviour
{
    private Rigidbody rb;
    public bool canDo;
    [Range(0f, 1f)] public float influence;
    public float fallSpeed;
    public float bounceSpeed;
    public float minimalBounceSpeed;
    public float moveSpeed;
    [Range(0f,1f)] public float roce;
    [Range(0f,1f)] public float roceVertical;
    [Range(0f, 1f)] public float roceHorizontal;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
    
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(x, 0, z);
        //transform.Translate(movement * moveSpeed * Time.deltaTime); NO DEBE SER PORQUE GENERA RESISTENCIA CUANDO LA VELOCIDAD VA A HACIA UN LADO Y lo mueves al otro
        rb.velocity += movement * (moveSpeed * Time.deltaTime);
        
        if (Input.GetButtonDown("Jump") && (canDo || rb.velocity.normalized.y <0))
        {
            BoostSpeed(fallSpeed, Vector3.down + influence * movement);
        }
        
        Vector3 dragMagnitude = roce *rb.velocity.sqrMagnitude * rb.velocity.normalized;
        rb.velocity -= dragMagnitude * Time.deltaTime;
    }

    void Rose()
    {
        var c = 0.1f;
        var dragMagnitude = c *rb.velocity.sqrMagnitude * rb.velocity.normalized;
    }

    
    void BoostSpeed(float boost,Vector3 dir)
    {
        rb.velocity += dir*boost;
    }
    void BoostSpeed(float boost)
    {
        BoostSpeed(boost,rb.velocity.normalized);
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.collider.tag == "ground")
        {
            //Colision Velocidad en Y
            if (collision.relativeVelocity.magnitude > minimalBounceSpeed)
            {
                if (collision.relativeVelocity.magnitude > bounceSpeed)
                {
                    // Si es mayor que salga a la bounceSpeed (efecto cartoon)
                    rb.velocity = rb.velocity.normalized*bounceSpeed;
                }
                else
                {
                    // si es igual o menor a la bounceSpeed  que decaiga con taza roce Vertical)
                    rb.velocity = rb.velocity.normalized*((rb.velocity.magnitude - minimalBounceSpeed*roceVertical)); 
                }
            
            }
            else
            {
                rb.velocity = influence*Vector3.up + rb.velocity.normalized*((rb.velocity.magnitude + minimalBounceSpeed*roceVertical)); 
            }
            
            // Velocidad en Z
            if (rb.velocity.z != 0f)
            {
                rb.velocity -= Vector3.forward * (roceHorizontal * rb.velocity.z);

            }
            // Velocidad en X
            if (rb.velocity.x != 0f)
            {
                rb.velocity -= Vector3.right * (roceHorizontal * rb.velocity.x);
            }
            // Velocidad Angular
            if (rb.angularVelocity.magnitude > 0f)
            {
                rb.angularVelocity =Vector3.zero;
            }
        }
        
    }
}
