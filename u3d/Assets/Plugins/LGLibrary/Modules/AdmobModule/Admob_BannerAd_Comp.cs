using UnityEngine;
using GoogleMobileAds.Api;
using System;


namespace LG
{
  /// <summary>
  /// Admob广告模块 Banner广告
  /// </summary>
  public class Admob_BannerAd_Comp : ModelCompBase<AdmobModule>
  {
    private string adUnitId;
    private AdvState state;
    private BannerView bannerView;

    public override void LGLoad(ModuleBase module, params object[] agrs)
    {
      base.LGLoad(module, agrs);
      if (Module.advs.ContainsKey(AdvType.BannerAd))
      {
        adUnitId = Module.advs[AdvType.BannerAd];
      }
      LoadEnd();
    }

    public void Request(AdPosition advpos)
    {
      if (string.IsNullOrEmpty(adUnitId))
      {
        Debug.LogError("Admob_BannerAd_Comp No adUnitId");
        Module.logEvent?.Invoke("Admob_BannerAd_Comp No adUnitId");
        return;
      }
      if (state == AdvState.Loading || state == AdvState.Loaded)
      {
        return;
      }
      state = AdvState.Loading;
      /// <summary>
      /// BannerView 的构造函数包含以下参数：
      /// adUnitId - AdMob 广告单元 ID，BannerView 应通过该 ID 加载广告。
      /// AdSize - 要使用的 AdMob 广告尺寸（有关详情，请参阅横幅广告尺寸）。
      /// AdPosition - 应放置横幅广告的位置。AdPosition 枚举列出了有效的广告位置值。
      /// </summary>
      this.bannerView = new BannerView(adUnitId, AdSize.Banner, (GoogleMobileAds.Api.AdPosition)advpos);
      // 广告请求成功加载后调用。
      this.bannerView.OnBannerAdLoaded += (() =>
      {
        state = AdvState.Loaded;
        Module.LoadEvent(AdvType.BannerAd, true);
        this.bannerView.Hide();
        Debug.Log("Admob_BannerAd_Comp Request Succ!");
        Module.logEvent?.Invoke("Admob_BannerAd_Comp Request Succ!");
      });
      // 广告请求加载失败时调用。
      this.bannerView.OnBannerAdLoadFailed += ((LoadAdError error) =>
      {
        state = AdvState.NoLoad;
        Module.LoadEvent(AdvType.BannerAd, false);
        Debug.LogError("Admob_BannerAd_Comp Request Failer:" + error.ToString());
        Module.logEvent?.Invoke("Admob_BannerAd_Comp Request Failer:" + error.ToString());
      });
      // Create an empty ad request.
      AdRequest request = new AdRequest.Builder().Build();
      // Load the banner with the request.
      this.bannerView.LoadAd(request);

    }
    public bool IsReady()
    {
      return state == AdvState.Loaded;
    }

    /// <summary>
    /// 请求Banner广告
    /// </summary>
    public void Show(AdPosition advpos)
    {
      state = AdvState.Playing;
      bannerView.Show();
    }

    public void Hide()
    {
      if (state == AdvState.Loading || state == AdvState.Loaded)
      {
        return;
      }
      bannerView?.Destroy();
      state = AdvState.NoLoad;
    }
  }
}