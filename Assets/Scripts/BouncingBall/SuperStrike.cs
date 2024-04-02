using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ParticleSystemJobs;

public class SuperStrike : MonoBehaviour
{
    private Rigidbody rb;
    public bool canDo;
    public float fallSpeed;
    public float bounceSpeed;
    public float minimalBounceSpeed;
    public float moveSpeed;
    public float roce;
    
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Jump") && (canDo || rb.velocity.normalized.y <0))
        {
            BoostSpeed(Vector3.down, fallSpeed);
        }

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(x, 0, z);
        transform.Translate(movement * moveSpeed * Time.deltaTime);
        
        if (rb.velocity.z > 0f)
        {
            rb.velocity -=Vector3.forward*roce;
        }
        if (rb.velocity.x > 0f)
        {
            rb.velocity -=Vector3.right*roce;
        }
        
        if (rb.angularVelocity.magnitude > 0f)
        {
            rb.angularVelocity =Vector3.zero;
        }
    }

    public void CollisionBoost(float boost)
    {
        BoostSpeed(rb.velocity.normalized, boost);
    }

    void BoostSpeed(Vector3 dir,float boost)
    {
        rb.velocity += dir*boost;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.relativeVelocity.magnitude > minimalBounceSpeed)
        {
            if (collision.relativeVelocity.magnitude > bounceSpeed)
            {
                rb.velocity = rb.velocity.normalized*bounceSpeed;
            }
            else
            {
                rb.velocity = rb.velocity.normalized*((rb.velocity.magnitude + minimalBounceSpeed) / 2); 
            }
            
        }
        else
        {
            rb.velocity = rb.velocity.normalized * minimalBounceSpeed;
        }
    }
}
