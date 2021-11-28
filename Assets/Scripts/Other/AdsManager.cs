using System.Collections;
using System.Collections.Generic;
using UnityEngine.Advertisements;
using UnityEngine;

public class AdsManager : MonoBehaviour
{

    private string iosVideo = "Interstitial_iOS";

#if UNITY_IOS
    string gameId = "4477704";
#else
    string gameId = "4477705";
#endif

    private void Start()
    {
        Advertisement.Initialize(gameId);
    }

    public void PlayAd()
    {
        if (Advertisement.IsReady(iosVideo))
        {
            Advertisement.Show(iosVideo);
        }
    }
}
