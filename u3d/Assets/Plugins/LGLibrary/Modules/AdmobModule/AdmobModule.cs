using System;
using UnityEngine;
using System.Collections.Generic;
using GoogleMobileAds.Api;
using GoogleMobileAds.Common;

namespace LG
{
  public class AdmobModule : ModuleBase<AdmobModule>, IAdv
  {
    public AdvvWeights Weights { get; set; }
    public AdvInitializationEvent InitializationEvent { get; set; }
    public AdvLoadEvent LoadEvent { get; set; }
    public AdvRewardEvent RewardEvent { get; set; }
    public Dictionary<AdvType, string> advs;
    public Action<string> logEvent;
    private Admob_OpenAd_Comp OpenAd_Comp;
    private Admob_BannerAd_Comp BannerAd_Comp;  //横幅广告
    private Admob_InterstitialAd_Comp InterstitialAd_Comp; //插屏广告
    private Admob_Video_RewardedAd_Comp Video_RewardedAd_Comp;     //激励广
    private Admob_Interstitial_RewardedAd_Comp Interstitial_RewardedAd_Comp; //插屏奖励广告
    public override void LGLoad(params object[] agrs)
    {
      advs = agrs[0] as Dictionary<AdvType, string>;
      OpenAd_Comp = AddComp<Admob_OpenAd_Comp>();
      BannerAd_Comp = AddComp<Admob_BannerAd_Comp>();
      InterstitialAd_Comp = AddComp<Admob_InterstitialAd_Comp>();
      Video_RewardedAd_Comp = AddComp<Admob_Video_RewardedAd_Comp>();
      Interstitial_RewardedAd_Comp = AddComp<Admob_Interstitial_RewardedAd_Comp>();
      base.LGLoad(agrs);
    }
    public void Initialize()
    {
      MobileAds.Initialize(initStatus =>
      {
        InitializationEvent(this, true, "Initialization Succ!");
      });
    }
    #region 接口
    public void OpenAd_Load()
    {
      OpenAd_Comp.Request();
    }
    public bool OpenAd_IsReady()
    {
      return OpenAd_Comp.IsReady();
    }
    public void OpenAd_Show()
    {
      OpenAd_Comp.Show();
    }
    public void OpenAd_Hide()
    {
      OpenAd_Comp.Hide();
    }

    public void BannerAd_Load(AdPosition advpos)
    {
      BannerAd_Comp.Request(advpos);
    }
    public bool BannerAd_IsReady()
    {
      return BannerAd_Comp.IsReady();
    }
    public void BannerAd_Show(AdPosition advpos)
    {
      BannerAd_Comp.Show(advpos);
    }
    public void BannerAd_Hide()
    {
      BannerAd_Comp.Hide();
    }

    public void Intersitial_Load()
    {
      InterstitialAd_Comp.Request();
    }
    public bool Intersitial_IsReady()
    {
      return InterstitialAd_Comp.IsReady();
    }
    public void Intersitial_Show()
    {
      InterstitialAd_Comp.Show();
    }
    public void Intersitial_Hide()
    {
      InterstitialAd_Comp.Hide();
    }

    public void Video_RewardedAd_Load()
    {
      Video_RewardedAd_Comp.Request();
    }
    public bool Video_RewardedAd_IsReady()
    {
      return Video_RewardedAd_Comp.IsReady();
    }
    public void Video_RewardedAd_Show()
    {
      Video_RewardedAd_Comp.Show();
    }
    public void Video_RewardedAd_Hide()
    {
      Video_RewardedAd_Comp.Hide();
    }

    public void Interstitial_RewardedAd_Load()
    {
      Interstitial_RewardedAd_Comp.Request();
    }
    public bool Interstitial_RewardedAd_IsReady()
    {
      return Interstitial_RewardedAd_Comp.IsReady();
    }
    public void Interstitial_RewardedAd_Show()
    {
      Interstitial_RewardedAd_Comp.Show();
    }
    public void Interstitial_RewardedAd_Hide()
    {
      Interstitial_RewardedAd_Comp.Hide();
    }
    #endregion
  }
}