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

    public void Start()
    {
        _meshRenderer = GetComponent<MeshRenderer>();
        if (life <= 0)
        {
            SetLife(1);
        }
    }
    public bool AddTolife(int value)
    {
        life += value;
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

    public void SetLife(int lifevalue)
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
