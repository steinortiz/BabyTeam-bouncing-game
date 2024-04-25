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
    public bool canStruperStrikeOnUp;
    [Range(0f, 1f)] public float influence;
    public float fallSpeed;
    public float moveSpeed;
    [Range(0f,1f)] public float airRoce;
    [SerializeField] private bool dontSpin;
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
        
        if (Input.GetButtonDown("Jump") && (canStruperStrikeOnUp || rb.velocity.normalized.y <0))
        {
            BoostSpeed(fallSpeed, Vector3.down + influence * movement);
            
        }
        
        Vector3 dragMagnitude = airRoce *rb.velocity.sqrMagnitude * rb.velocity.normalized;
        rb.velocity -= dragMagnitude * Time.deltaTime;
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

    void KillBall()
    {
        // avisarle al manager para que pasen cosas
        //Spawn de particulas
        //Morir
        LevelController.Instance.OnDestroyPlayer();
        Destroy(this.gameObject);
    }
    private void OnCollisionExit(Collision collision)
    {
        // Velocidad Angular
        if (rb.angularVelocity.magnitude > 0f && dontSpin)
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
        if (collision.collider.transform.TryGetComponent(out LifeController _lifeController))
        {
            _lifeController.Interact(onSuperStrike);
        }
        onSuperStrike = false;

    }
}
