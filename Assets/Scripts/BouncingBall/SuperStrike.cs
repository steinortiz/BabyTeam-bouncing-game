using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.ParticleSystemJobs;
using UnityEngine.Timeline;

public class SuperStrike : MonoBehaviour
{
    private Rigidbody rb;
    public bool canDo;
    [Range(0f, 1f)] public float influence;
    public float fallSpeed;
    public float moveSpeed;
    [Range(0f,1f)] public float airRoce;
    private bool onSuperStrike=false;


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
        
        Vector3 dragMagnitude = airRoce *rb.velocity.sqrMagnitude * rb.velocity.normalized;
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
        onSuperStrike = true;
    }
    void BoostSpeed(float boost)
    {
        BoostSpeed(boost,rb.velocity.normalized);
    }

    void Sound()
    {
    }
    
    // aqui toda la wea sobre matar la bola, ojala la feña no se vuelva loca con esto pls.
    void KillBall()
    {
        // avisarle al manager para que pasen cosas
        //Spawn de particulas
        //Morir
        Destroy(this.gameObject);
    }
    private void OnCollisionExit(Collision collision)
    {
        // Velocidad Angular
        if (rb.angularVelocity.magnitude > 0f)
        {
            rb.angularVelocity =Vector3.zero;
        }
        Sound();
        if (collision.collider.transform.TryGetComponent(out BounzableObject bounzable))
        {
            rb.velocity = rb.velocity.normalized*(bounzable.data.bounceSpeed+(1f-bounzable.data.rapidezDeCambio)*(rb.velocity.magnitude - bounzable.data.bounceSpeed));
            bool doKill = bounzable.Interact(onSuperStrike);
            if (doKill)
            {
                Invoke("KillBall", Time.deltaTime);
            }
        }
        onSuperStrike = false;

    }
}

/* Deprecated OnCollisionExit
 if (collision.relativeVelocity.magnitude > other.data.maxBounceSpeed)
{
    // Si es mayor que salga a la bounceSpeed (efecto cartoon)
    rb.velocity = rb.velocity.normalized*other.data.maxBounceSpeed;
}
else
{
   rb.velocity = rb.velocity.normalized*(other.data.minBounceSpeed+(1f-other.data.rapidezDeCambio)*(rb.velocity.magnitude - other.data.minBounceSpeed));  
}*/