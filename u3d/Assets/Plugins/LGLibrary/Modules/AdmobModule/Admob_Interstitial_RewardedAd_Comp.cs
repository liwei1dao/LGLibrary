using UnityEngine;
using GoogleMobileAds.Api;
using System;
using GoogleMobileAds.Common;

namespace LG
{
  /// <summary>
  /// Admob广告模块 插页式激励广告
  /// </summary>
  public class Admob_Interstitial_RewardedAd_Comp : ModelCompBase<AdmobModule>
  {
    private string adUnitId;
    private AdvState state;
    private RewardedInterstitialAd rewardedInterstitialAd;

    public override void LGLoad(ModuleBase module, params object[] agrs)
    {
      base.LGLoad(module, agrs);
      state = AdvState.NoLoad;
      if (Module.advs.ContainsKey(AdvType.IntersitialAd_RewardedAd))
      {
        adUnitId = Module.advs[AdvType.IntersitialAd_RewardedAd];
      }
      LoadEnd();
    }

    [Obsolete]
    public void Request()
    {
      if (string.IsNullOrEmpty(adUnitId))
      {
        Debug.LogError("Admob_Interstitial_RewardedAd_Comp No adUnitId");
        Module.logEvent?.Invoke("Admob_Interstitial_RewardedAd_Comp No adUnitId");
        return;
      }
      if (state == AdvState.Loading || state == AdvState.Loaded)
      {
        return;
      }
      state = AdvState.Loading;
      AdRequest request = new AdRequest.Builder().Build();
      // Create an interstitial.
      RewardedInterstitialAd.Load(adUnitId, request, (rewardedInterstitialAd, error) =>
        {
          if (error != null)
          {
            state = AdvState.NoLoad;
            Module.LoadEvent(AdvType.IntersitialAd_RewardedAd, false);
            Debug.LogError("Admob_Interstitial_RewardedAd_Comp Request Failer:" + error.ToString());
            Module.logEvent?.Invoke("Admob_Interstitial_RewardedAd_Comp Request Failer:" + error.ToString());
            return;
          }
          this.rewardedInterstitialAd = rewardedInterstitialAd;
          state = AdvState.Loaded;
          Module.LoadEvent(AdvType.IntersitialAd_RewardedAd, true);
          Debug.Log("Admob_Interstitial_RewardedAd_Comp Request Succ!");
          Module.logEvent?.Invoke("Admob_Interstitial_RewardedAd_Comp Request Succ!");
        });
    }
    public bool IsReady()
    {
      return state == AdvState.Loaded;
    }
    public void Show()
    {
      if (rewardedInterstitialAd != null && rewardedInterstitialAd.CanShowAd())
      {
        rewardedInterstitialAd.Show((Reward reward) =>
        {
          Module.RewardEvent(AdvType.IntersitialAd_RewardedAd, true);
          state = AdvState.NoLoad;
          Debug.Log("Admob_Interstitial_RewardedAd_Comp Show Succ!");
          Module.logEvent?.Invoke("Admob_Interstitial_RewardedAd_Comp Show Succ!");
        });
      }
      else
      {
        state = AdvState.NoLoad;
        Module.RewardEvent(AdvType.IntersitialAd_RewardedAd, false);
      }
    }

    public void Hide()
    {
      if (state == AdvState.Loading || state == AdvState.Loaded)
      {
        return;
      }
      this.rewardedInterstitialAd?.Destroy();
      state = AdvState.NoLoad;
    }
  }
}