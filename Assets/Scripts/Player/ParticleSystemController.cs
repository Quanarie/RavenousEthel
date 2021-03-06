using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleSystemController : MonoBehaviour
{
    [SerializeField] private new ParticleSystem particleSystem;
    [SerializeField] private float regularRadius;
    [SerializeField] private float mutantRadius;
    [SerializeField] private float regularRateOverDistance;
    [SerializeField] private float mutantRateOverDistance;

    private void Start()
    {
        particleSystem.Play();

        var particleShape = particleSystem.shape;
        particleShape.radius = regularRadius;

        var particleEmisson = particleSystem.emission;
        particleEmisson.rateOverDistance = regularRateOverDistance;

        /*PlayerAttack.OnMutation += () => { particleShape.radius = mutantRadius;
                                                                particleEmisson.rateOverDistance = mutantRateOverDistance;
        };

        PlayerAttack.OnDemutation += () => { particleShape.radius = regularRadius;
                                                                  particleEmisson.rateOverDistance = regularRateOverDistance;
        };*/
    }
}
