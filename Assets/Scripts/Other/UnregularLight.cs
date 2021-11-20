using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnregularLight : MonoBehaviour
{
    [SerializeField] private float changePeriodMin;
    [SerializeField] private float changePeriodMax;
    [SerializeField] private float maxLightedTime;

    private float changePeriod;
    private float startIntensity;
    private float lastChangeTime;
    private new UnityEngine.Experimental.Rendering.Universal.Light2D light;

    private void Start()
    {
        light = GetComponent<UnityEngine.Experimental.Rendering.Universal.Light2D>();
        changePeriod = Random.Range(changePeriodMin, changePeriodMax);
        startIntensity = light.falloffIntensity;
    }

    private void Update()
    {
        if (Time.time - lastChangeTime < changePeriod)
            return;

        lastChangeTime = Time.time;

        light.intensity = 1;
        StartCoroutine(ChangeIntensityBack());
        changePeriod = Random.Range(changePeriodMin, changePeriodMax);
    }
    IEnumerator ChangeIntensityBack()
    {
        yield return new WaitForSeconds(maxLightedTime);
        light.intensity = 0;
    }
}
