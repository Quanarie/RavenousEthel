using System.Collections;
using System.Collections.Generic;
using UnityEngine.Advertisements;
using UnityEngine;

public class AdsManager : MonoBehaviour, IUnityAdsListener
{

    private string iosVideo = "Interstitial_iOS";
    private string iosRewardedVideo = "Rewarded_iOS";

#if UNITY_IOS
    string gameId = "4477704";
#else
    string gameId = "4477705";
#endif

    private void Start()
    {
        Advertisement.Initialize(gameId);
        Advertisement.AddListener(this);
    }

    public void PlayAd()
    {
        if (Advertisement.IsReady(iosVideo))
        {
            Advertisement.Show(iosVideo);
        }
    }

    public void PlayRewardedAd()
    {
        if (Advertisement.IsReady(iosRewardedVideo))
        {
            Advertisement.Show(iosRewardedVideo);
        }
    }

    public void OnUnityAdsReady(string placementId)
    {
        
    }

    public void OnUnityAdsDidError(string message)
    {
        
    }

    public void OnUnityAdsDidStart(string placementId)
    {
        
    }

    public void OnUnityAdsDidFinish(string placementId, ShowResult showResult)
    {
        if (placementId == iosRewardedVideo && showResult == ShowResult.Finished)
        {

        }
    }
}
