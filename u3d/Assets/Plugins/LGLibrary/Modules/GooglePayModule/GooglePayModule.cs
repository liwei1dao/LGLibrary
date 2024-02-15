using UnityEngine;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace LG
{
  /// <summary>
  ///  GooglePay 商品购买结果监听代理
  /// </summary>
  /// <param name="productId"></param>
  /// <param name="isSucc"></param>
  public delegate void GooglePayPurchaseResultlisten(string productId, bool isSucc);
  /// <summary>
  /// Google 支付模块
  /// </summary>
  public class GooglePayModule : ModuleBase<GooglePayModule>
  {
    class RequestSkuData
    {
      public string[] productIds;
    }

    private const string Unity3dObjName = "GooglePay";
    private string googlekey = "";
    private List<string> productIds;
    private GooglePay_Api_Comp Api_Comp;
    private GooglePayPurchaseResultlisten listens;
    private GoogleReceive Receive;
    private bool IsInitSucc;
    public override void LGLoad(params object[] agrs)
    {
      base.LGLoad(agrs);
      googlekey = (string)agrs[0];
      productIds = agrs[1] as List<string>;
      Api_Comp = AddComp<GooglePay_Api_Comp>();
      GameObject obj = new GameObject(Unity3dObjName);
      Receive = obj.AddComponent<GoogleReceive>();
      Receive.BillingSetupBackClick = BillingSetupBackClick;
      Receive.ProductBackClick = BuyProductBackCall;
      GameObject.DontDestroyOnLoad(obj);
      Api_Comp.Init(Unity3dObjName, googlekey);
      LoadEnd();
    }

    public void RegisterPurchaseResultlisten(GooglePayPurchaseResultlisten _listen)
    {
      listens += _listen;
    }
    public void UnRegisterPurchaseResultlisten(GooglePayPurchaseResultlisten _listen)
    {
      listens -= _listen;
    }

    public void RequstProducts(List<string> productIds)
    {
      RequestSkuData data = new RequestSkuData();
      data.productIds = productIds.ToArray();
      string jsonData = JsonConvert.SerializeObject(data);
      Debug.Log("[IAPBridge]RequstProducts:" + jsonData);
      Api_Comp.RequestProducts(jsonData);
    }

    public void SendBuyProduct(string productId, bool isConsumable)
    {
      if (!IsInitSucc)
      {
        Debug.Log("GooglePayModule No Init Succ!");
        return;
      }

      Debug.Log(string.Format("[IAPBridge]SendBuyProduct:{0} isConsumable:{1}", productId, isConsumable));
      Api_Comp.BuyProduct(productId, isConsumable);
    }

    /// <summary>
    /// 初始化回调
    /// </summary>
    /// <param name="responseCode"></param>
    private void BillingSetupBackClick(string responseCode)
    {
      if (responseCode == "OK")
      {
        IsInitSucc = true;
        RequstProducts(productIds);
      }
    }

    /// <summary>
    /// 购买商品回调
    /// </summary>
    /// <param name="productId"></param>
    /// <param name="issucc"></param>
    private void BuyProductBackCall(string productId, bool issucc)
    {
      listens(productId, issucc);
    }
  }
}
