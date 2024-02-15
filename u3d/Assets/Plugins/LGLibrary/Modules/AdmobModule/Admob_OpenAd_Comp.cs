using UnityEngine;
using GoogleMobileAds.Api;
using System;


namespace LG
{
  /// <summary>
  /// Admob广告模块 开屏广告
  /// </summary>
  public class Admob_OpenAd_Comp : ModelCompBase<AdmobModule>
  {
    private string adUnitId;
    private AdvState state;
    private AppOpenAd ad;
    public override void LGLoad(ModuleBase module, params object[] agrs)
    {
      base.LGLoad(module, agrs);
      state = AdvState.NoLoad;
      if (Module.advs.ContainsKey(AdvType.AppOpenAd))
      {
        adUnitId = Module.advs[AdvType.AppOpenAd];
      }
      LoadEnd();
    }

    [Obsolete]
    public void Request()
    {
      if (string.IsNullOrEmpty(adUnitId))
      {
        Debug.LogError("Admob_OpenAd_Comp No adUnitId");
        Module.logEvent?.Invoke("Admob_OpenAd_Comp No adUnitId");
        return;
      }
      if (state == AdvState.Loading || state == AdvState.Loaded)
      {
        return;
      }
      state = AdvState.Loading;
      AdRequest request = new AdRequest.Builder().Build();
      // Load an app open ad for portrait orientation
      AppOpenAd.Load(adUnitId, ScreenOrientation.Portrait, request, ((appOpenAd, error) =>
      {
        if (error != null)
        {
          // Handle the error.
          state = AdvState.NoLoad;
          Module.LoadEvent(AdvType.AppOpenAd, false);
          Debug.LogError("Admob_OpenAd_Comp Request Failed:" + error.GetMessage());
          Module.logEvent?.Invoke("Admob_OpenAd_Comp Request Failed:" + error.GetMessage());
          return;
        }
        // App open ad is loaded.
        ad = appOpenAd;
        state = AdvState.Loaded;
        Module.LoadEvent(AdvType.AppOpenAd, true);
        Debug.Log("Admob_OpenAd_Comp Request Succ!");
        Module.logEvent?.Invoke("Admob_OpenAd_Comp Request Succ!");
      }));
    }

    public bool IsReady()
    {
      return state == AdvState.Loaded;
    }

    public void Show()
    {
      state = AdvState.Playing;
      ad.Show();
    }

    public void Hide()
    {
      if (state == AdvState.Loading || state == AdvState.Loaded)
      {
        return;
      }
      ad.Destroy();
      state = AdvState.NoLoad;
    }
  }
}