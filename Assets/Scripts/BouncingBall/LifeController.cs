using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeController : MonoBehaviour
{
    public int life;
    public ParticleSystem particles;
    [SerializeField]private bool changeMaterial;
    [SerializeField] private List<Material> materials =new List<Material>();
    [SerializeField]private MeshRenderer _meshRenderer;
    
    
    public delegate void ObjectDestroyed();
    public event ObjectDestroyed OnObjectDestroyedEvent;

    public void Start()
    {
        _meshRenderer = GetComponent<MeshRenderer>();
        if (life <= 0)
        {
            UpdateLife(1);
        }
    }
    public bool Interact()
    {
        life -= 1;
        UpdateMaterial();
        if (life == 0)
        {
            if (particles != null)
            {
                ParticleSystem particlesSys = Instantiate<ParticleSystem>(particles, transform.localPosition, transform.rotation);
                particlesSys.Play();
                Destroy(particlesSys, particlesSys.totalTime);
            }
            return true;
        }

        return false;
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
