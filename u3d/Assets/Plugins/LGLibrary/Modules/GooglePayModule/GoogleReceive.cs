using System;
using System.Collections.Generic;
using UnityEngine;

namespace LG
{
  public class GoogleReceive : MonoBehaviour
  {
    public Action<string> BillingSetupBackClick;
    public Action<string, bool> ProductBackClick;
    void BillingSetupFinished(string responseCode)
    {
      Debug.Log("BillingSetupFinished:" + responseCode);
      this?.BillingSetupBackClick(responseCode);
    }

    void RecieveProductInfos(string jsonData)
    {
      Debug.Log("RecieveProductInfos:" + jsonData);
    }

    //产品列表请求失败
    void ProductRequestFail(string message)
    {
      Debug.Log("ProductRequestFail:" + message);
    }

    //购买成功回调
    void ProductBuyComplete(string productId)
    {
      Debug.Log("ProductBuyComplete:" + productId);
      this?.ProductBackClick(productId, true);
    }

    /// <summary>
    /// 购买取消回调
    /// </summary>
    /// <param name="productId"></param>
    void ProductBuyCancled(string productId)
    {
      Debug.Log("ProductBuyCancled:" + productId);
      this?.ProductBackClick(productId, false);
    }

    //购买失败回调
    void ProductBuyFailed(string productId)
    {
      Debug.Log("ProductBuyFailed:" + productId);
      this?.ProductBackClick(productId, false);
    }

    //获取商品回执回调
    void ProvideContent(string msg)
    {
      Debug.Log("ProvideContent:" + msg);
    }

    /// <summary>
    /// 恢复购买成功
    /// </summary>
    /// <param name="productId"></param>
    void RestoreComplete(string productId)
    {
      Debug.Log("RestoreComplete:" + productId);
    }
  }

}
