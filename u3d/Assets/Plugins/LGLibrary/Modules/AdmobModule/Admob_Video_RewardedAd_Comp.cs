using UnityEngine;
using GoogleMobileAds.Api;
using GoogleMobileAds.Common;
using System;

namespace LG
{
  /// <summary>
  /// Admob广告模块 激励广告
  /// </summary>
  public class Admob_Video_RewardedAd_Comp : ModelCompBase<AdmobModule>
  {
    private string adUnitId;
    private AdvState state;
    private RewardedAd rewardedAd;

    public override void LGLoad(ModuleBase module, params object[] agrs)
    {
      base.LGLoad(module, agrs);
      state = AdvState.NoLoad;
      if (Module.advs.ContainsKey(AdvType.Video_RewardedAd))
      {
        adUnitId = Module.advs[AdvType.Video_RewardedAd];
      }
      LoadEnd();
    }

    /// <summary>
    /// 请求广告
    /// </summary>
    /// <param name="adUnitId"></param>
    /// <param name="func"></param>
    public void Request()
    {
      if (string.IsNullOrEmpty(adUnitId))
      {
        Debug.LogError("Admob_Video_RewardedAd_Comp No adUnitId");
        Module.logEvent?.Invoke("Admob_Video_RewardedAd_Comp No adUnitId");
        return;
      }
      if (state == AdvState.Loading || state == AdvState.Loaded)
      {
        return;
      }
      state = AdvState.Loading;
      var adRequest = new AdRequest();
      RewardedAd.Load(adUnitId, adRequest,
          (RewardedAd ad, LoadAdError error) =>
          {
            if (error != null || ad == null)
            {
              state = AdvState.NoLoad;
              Module.LoadEvent(AdvType.AppOpenAd, false);
              Debug.LogError("Admob_Video_RewardedAd_Comp Request Failed:" + error.GetMessage());
              return;
            }
            rewardedAd = ad;
            state = AdvState.Loaded;
            Module.LoadEvent(AdvType.AppOpenAd, true);
            Debug.Log("Admob_Video_RewardedAd_Comp Request Succ!");
            Module.logEvent?.Invoke("Admob_Video_RewardedAd_Comp Request Succ!");
          });
    }
    public bool IsReady()
    {
      return state == AdvState.Loaded;
    }
    public void Show()
    {
      if (this.rewardedAd.CanShowAd())
      {
        state = AdvState.Playing;
        this.rewardedAd.Show((Reward reward) =>
        {
          Debug.Log("Admob_Video_RewardedAd_Comp Show Succ!");
          Module.logEvent?.Invoke("Admob_Video_RewardedAd_Comp Show Succ!");
          state = AdvState.NoLoad;
          Module.RewardEvent(AdvType.Video_RewardedAd, true);
        });
      }
      else
      {
        state = AdvState.NoLoad;
        Module.RewardEvent(AdvType.Video_RewardedAd, false);
      }
    }
    public void Hide()
    {
      if (state == AdvState.Loading || state == AdvState.Loaded)
      {
        return;
      }
      this.rewardedAd?.Destroy();
      state = AdvState.NoLoad;
    }
  }
}