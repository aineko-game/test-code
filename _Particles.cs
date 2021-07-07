using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _Particles : MonoBehaviour
{
    [HideInInspector] public ParticleSystem[] _psChildrenList;

    public bool isDestroySelfOnDisable = false;

    private void Start()
    {
        _psChildrenList = GetComponentsInChildren<ParticleSystem>(true);

        ConfigureSpeed(Globals.Instance.gameSpeed);
    }

    private void Awake()
    {
        Globals.particlesList.Add(this);
    }

    private void OnDestroy()
    {
        Globals.particlesList.Remove(this);
    }

    private void OnDisable()
    {
        if (transform.parent != null && transform.parent.name != "Instances")
        {
            if (isDestroySelfOnDisable)
                Destroy(gameObject);
        }
    }

    public void ConfigureSpeed(float speed_)
    {
        foreach (ParticleSystem psChild_ in _psChildrenList)
        {
            ParticleSystem.MainModule psMain_ = psChild_.main;
            psMain_.simulationSpeed = speed_;
        }
    }

    public void ConfigureMaxParticles(int count_)
    {
        foreach (ParticleSystem psChild_ in _psChildrenList)
        {
            ParticleSystem.MainModule psMain_ = psChild_.main;
            psMain_.maxParticles = count_;
        }
    }
}
