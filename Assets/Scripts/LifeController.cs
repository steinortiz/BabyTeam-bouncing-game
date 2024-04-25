using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeController : MonoBehaviour
{
    public int life;
    public bool doesNeedSuperStrike;
    public ParticleSystem particles;
    [SerializeField]private bool changeMaterial;
    [SerializeField] private List<Material> materials =new List<Material>();
    [SerializeField]private MeshRenderer _meshRenderer;
    private BounzableObject _bounzableObject;
    
    
    public delegate void ObjectDestroyed();
    public event ObjectDestroyed OnObjectDestroyedEvent;

    public void Start()
    {
        _meshRenderer = this.GetComponent<MeshRenderer>();
        _bounzableObject = this.GetComponent<BounzableObject>();
        if (life <= 0)
        {
            UpdateLife(1);
        }
    }
    public void Interact(bool isSuperStrike)
    {
        if (!doesNeedSuperStrike || isSuperStrike)
        {
            life -= 1;
            UpdateMaterial();
            if (life == 0)
            {
                _bounzableObject.CheckCompleteObjetive();
                if (particles != null)
                { 
                    
                    ParticleSystem particlesSys = Instantiate<ParticleSystem>(particles, transform.localPosition, transform.rotation);
                    particlesSys.Play();
                    Destroy(particlesSys, particlesSys.totalTime);
                }
                Destroy(this.gameObject);
            }
        }
    }

    public void UpdateLife(int lifevalue)
    {
        life = lifevalue;
        UpdateMaterial();
        
    }

    private void UpdateMaterial()
    {
        if (changeMaterial && life>0 && materials!=null)
        {
            _meshRenderer.material = materials[life-1];
        }
    }
    
    
}
