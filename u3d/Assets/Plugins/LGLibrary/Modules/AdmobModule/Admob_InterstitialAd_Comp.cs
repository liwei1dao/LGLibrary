using UnityEngine;
using GoogleMobileAds.Api;
using System;
namespace LG
{
  /// <summary>
  /// Admob广告模块 Interstitial广告
  /// </summary>
  public class Admob_InterstitialAd_Comp : ModelCompBase<AdmobModule>
  {
    private string adUnitId;
    private AdvState state;
    private InterstitialAd interstitial;

    public override void LGLoad(ModuleBase module, params object[] agrs)
    {
      base.LGLoad(module, agrs);
      if (Module.advs.ContainsKey(AdvType.IntersitialAd))
      {
        adUnitId = Module.advs[AdvType.IntersitialAd];
      }
      LoadEnd();
    }

    /// <summary>
    /// 请求Interstitial广告
    /// </summary>
    public void Request()
    {
      if (string.IsNullOrEmpty(adUnitId))
      {
        Debug.LogError("Admob_InterstitialAd_Comp No adUnitId");
        Module.logEvent?.Invoke("Admob_InterstitialAd_Comp No adUnitId");
        return;
      }
      if (state == AdvState.Loading || state == AdvState.Loaded)
      {
        return;
      }
      state = AdvState.Loading;

      // create our request used to load the ad.
      var adRequest = new AdRequest();

      // send the request to load the ad.
      InterstitialAd.Load(adUnitId, adRequest,
          (InterstitialAd ad, LoadAdError error) =>
          {
            // if error is not null, the load request failed.
            if (error != null || ad == null)
            {
              state = AdvState.NoLoad;
              Module.LoadEvent(AdvType.AppOpenAd, false);
              Debug.LogError("Admob_InterstitialAd_Comp Request Failed:" + error.GetMessage());
              Module.logEvent?.Invoke("Admob_InterstitialAd_Comp Request Failed:" + error.GetMessage());
              return;
            }
            interstitial = ad;
            state = AdvState.Loaded;
            Module.LoadEvent(AdvType.AppOpenAd, true);
            Debug.Log("Admob_InterstitialAd_Comp Request Succ!");
            Module.logEvent?.Invoke("Admob_InterstitialAd_Comp Request Succ!");
          });
    }
    public bool IsReady()
    {
      return state == AdvState.Loaded;
    }
    public void Show()
    {
      if (this.interstitial.CanShowAd())
      {
        state = AdvState.Playing;
        this.interstitial.Show();
      }
      else
      {
        state = AdvState.NoLoad;
      }

    }
    public void Hide()
    {
      if (state == AdvState.Loading || state == AdvState.Loaded)
      {
        return;
      }
      this.interstitial?.Destroy();
      state = AdvState.NoLoad;
    }
  }
}