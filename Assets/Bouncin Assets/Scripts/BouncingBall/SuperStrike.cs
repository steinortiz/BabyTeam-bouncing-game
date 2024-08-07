using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.ParticleSystemJobs;
using UnityEngine.Timeline;

public class SuperStrike : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    private bool canSuperStrike = true;
    public bool canStruperStrikeOnUp;
    [SerializeField] private bool dontSpin;
    public bool isSuperStrikeActive { get; private set;}
    [SerializeField] private ParticleSystem speedFx;
    [SerializeField] private GameObject speedFxPivot;
    [SerializeField] private AudioClip speedSFX;
    [SerializeField] private ParticleSystem strikeFx;
    [SerializeField] private AudioClip strikeSFX;
    [SerializeField] private AudioSource audioSource;
    public  RewardScriptableObject ballData;
    


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
        rb.velocity += movement * (ballData.moveSpeed * Time.deltaTime);
        rb.angularVelocity += spin * (ballData.spinSpeed);
        
        if (Input.GetButtonDown("Jump") && (canStruperStrikeOnUp || rb.velocity.normalized.y <0) && canSuperStrike)
        {
            OnSuperStrike(ballData.fallSpeed, LevelController.Instance.gravityDir + ballData.influence * movement);
        }
        Vector3 dragMagnitude = ballData.airRoce *rb.velocity.sqrMagnitude * rb.velocity.normalized;
        rb.velocity -= dragMagnitude * Time.deltaTime;
        
    }

    public void BoostSpeed(float boost,Vector3 dir)
    {
        rb.velocity += dir*boost;
    }

    void OnSuperStrike(float boost,Vector3 dir)
    {
        BoostSpeed( boost, dir);
        speedFxPivot.transform.rotation= Quaternion.LookRotation(rb.velocity.normalized);
        speedFx.Play();
        if(AudioManager.Instance!=null)AudioManager.Instance.PlaySFX(speedSFX, true);
        isSuperStrikeActive = true;
    }

    void SuperStrikeDone()
    {
        if (isSuperStrikeActive)
        {
            speedFx.Stop();
            if(AudioManager.Instance!=null)AudioManager.Instance.StopSFX(speedSFX);
            isSuperStrikeActive = false;
            ParticleSystem strike = Instantiate<ParticleSystem>(strikeFx, this.transform.localPosition,this.transform.rotation);
            strike.Play();
            if(AudioManager.Instance!=null)AudioManager.Instance.PlaySFX(strikeSFX);
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

    public void SetUP()
    {
        ballData = GameController.Instance.GetBallData();
        GetComponent<MeshRenderer>().material = ballData.ballMaterial;
    }
}
