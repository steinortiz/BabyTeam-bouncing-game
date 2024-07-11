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
    private bool canSuperStrike = true;
    public bool canStruperStrikeOnUp;
    [Range(0f, 1f)] public float influence;
    public float fallSpeed;
    public float moveSpeed;
    public float spinSpeed;
    [Range(0f,1f)] public float airRoce;
    [SerializeField] private bool dontSpin;
    public bool isSuperStrikeActive { get; private set;}
    [SerializeField] private ParticleSystem speedFx;
    [SerializeField] private GameObject speedFxPivot;
    [SerializeField] private AudioClip speedSFX;
    [SerializeField] private ParticleSystem strikeFx;
    [SerializeField] private AudioClip strikeSFX;
    [SerializeField] private AudioSource audioSource;
    


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

        Vector3 movement = LevelController.Instance.horzDir * x + LevelController.Instance.vertDir * z;
        Vector3 spin =  LevelController.Instance.horzDir * z + LevelController.Instance.vertDir * -x;
        //transform.Translate(movement * moveSpeed * Time.deltaTime); NO DEBE SER PORQUE GENERA RESISTENCIA CUANDO LA VELOCIDAD VA A HACIA UN LADO Y lo mueves al otro
        rb.velocity += movement * (moveSpeed * Time.deltaTime);
        rb.angularVelocity += spin * (spinSpeed);
        
        if (Input.GetButtonDown("Jump") && (canStruperStrikeOnUp || rb.velocity.normalized.y <0) && canSuperStrike)
        {
            BoostSpeed(fallSpeed, LevelController.Instance.gravityDir + influence * movement);
        }
        Vector3 dragMagnitude = airRoce *rb.velocity.sqrMagnitude * rb.velocity.normalized;
        rb.velocity -= dragMagnitude * Time.deltaTime;
        
    }

    void BoostSpeed(float boost,Vector3 dir)
    {
        rb.velocity += dir*boost;
        speedFxPivot.transform.rotation= Quaternion.LookRotation(rb.velocity.normalized);
        speedFx.Play();
        if(GameController.Instance!=null)GameController.Instance.PlaySFX(speedSFX, true);
        isSuperStrikeActive = true;
    }

    void SuperStrikeDone()
    {
        if (isSuperStrikeActive)
        {
            speedFx.Stop();
            if(GameController.Instance!=null)GameController.Instance.StopSFX(speedSFX);
            isSuperStrikeActive = false;
            ParticleSystem strike = Instantiate<ParticleSystem>(strikeFx, this.transform.localPosition,this.transform.rotation);
            strike.Play();
            if(GameController.Instance!=null)GameController.Instance.PlaySFX(strikeSFX);
            Destroy(strike.gameObject,strike.totalTime+1f);
        }
    }
    
    public void KillBall()
    {
        // avisarle al manager para que pasen cosas
        //Spawn de particulas
        //Morir
        LevelController.Instance.OnDestroyPlayer();
        Destroy(this.gameObject);
    }

    public void SetBounce(BounzableScriptableObject data)
    {
        // Velocidad Angular
        if (rb.angularVelocity.magnitude > 0f && dontSpin)
        {
            rb.angularVelocity =Vector3.zero;
        }
        rb.velocity = rb.velocity.normalized*(data.bounceSpeed+(1f-data.rapidezDeCambio)*(rb.velocity.magnitude - data.bounceSpeed));
        SuperStrikeDone();
    }

    public void ActivateSuperStrike(bool activate)
    {
        canSuperStrike = activate;
    }
}
