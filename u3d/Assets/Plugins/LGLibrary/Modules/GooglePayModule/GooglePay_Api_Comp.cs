using UnityEngine;
using Newtonsoft.Json;
using System;


namespace LG
{

  public class GooglePay_Api_Comp : ModelCompBase<GooglePayModule>
  {
    class BuyProductData
    {
      public string productId;
      public bool isConsumable;
    }
    private AndroidJavaObject javaObject;
    public override void LGLoad(ModuleBase module, params object[] agrs)
    {
      base.LGLoad(module, agrs);
      javaObject = new AndroidJavaObject("com.liwei1dao.googlepay.GooglePay");
      LoadEnd();
    }

    /// <summary>
    /// 初始化sdk
    /// </summary>
    /// <param name="goName"></param>
    /// <param name="publicKey"></param>
    public void Init(string goName, string publicKey)
    {
      Debug.Log("[GooglePlay_IAPBridge]Init：" + goName + "=====" + publicKey);
      if (Application.platform != RuntimePlatform.Android)
        return;
      javaObject.Call("Init", goName, publicKey);
    }

    /// <summary>
    /// 判断是否支持
    /// </summary>
    /// <returns></returns>
    public bool IsIAPSupported()
    {
      if (Application.platform != RuntimePlatform.Android)
        return false;
      return javaObject.Call<bool>("IsIAPSupported");
    }

    public void RequestProducts(string jsonData)
    {
      Debug.Log("[GooglePlay_IAPBridge]RequstProduct：" + jsonData);
      if (Application.platform != RuntimePlatform.Android)
        return;
      javaObject.Call("RequstProduct", jsonData);
    }

    public void BuyProduct(string productId, bool isConsumable)
    {
      BuyProductData buyProductData = new BuyProductData();
      buyProductData.productId = productId;
      buyProductData.isConsumable = isConsumable;
      string jsonData = JsonConvert.SerializeObject(buyProductData);
      Debug.Log("[GooglePlay_IAPBridge]BuyProduct：" + jsonData);
      if (Application.platform != RuntimePlatform.Android)
        return;
      javaObject.Call("BuyProduct", jsonData);
    }

  }
}
