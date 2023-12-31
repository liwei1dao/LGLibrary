using UnityEngine;

namespace LG;

public interface ViewComp
{
    int GetIndex();
    void SetIndex(int _Index);
    UILevel GetLevel();
    void SetCanvas(Canvas _Canvas);
    void Show();
    void Hide();
}

/// <summary>
/// 模块界面基础组件
/// </summary>
public abstract class Module_BaseViewComp : ModelCompBase, ViewComp
{
    protected UILevel ShowLevel = UILevel.LowUI;
    private int Index;
    public GameObject UIGameobject;
    protected Canvas Canvas;
    #region 框架构造
    /// <summary>
    /// 基础界面组件
    /// </summary>
    /// <param name="_ModelContorl"></param>
    /// <param name="_Agr">第一个参数 资源根路径</param>
    public override void LGLoad(ModuleBase module, params object[] agrs)
    {
        base.LGLoad(module, agrs);
        string PrefabName = (string)agrs[0];
        UIGameobject = ViewManagerModule.Instance.CreateView(this, Module.LoadAsset<GameObject>("Prefab", PrefabName));
        ViewManagerModule.Instance.SetViewToTop(this);
    }

    public override void Close()
    {
        ViewManagerModule.Instance.DeletView(this);
        GameObject.Destroy(UIGameobject);
        base.Close();
    }
    #endregion

    public int GetIndex()
    {
        return Index;
    }

    public void SetIndex(int _Index)
    {
        Index = _Index;
        Canvas.sortingOrder = _Index;
    }

    public UILevel GetLevel()
    {
        return ShowLevel;
    }

    public void SetCanvas(Canvas _Canvas)
    {
        Canvas = _Canvas;
    }

    public void Show()
    {
        ViewManagerModule.Instance.SetViewToTop(this);
        UIGameobject.SetActive(true);
    }

    public void Hide()
    {
        UIGameobject.SetActive(false);
    }
}


/// <summary>
/// 模块界面基础组件
/// </summary>
public class Model_BaseViewComp<C> : ModelCompBase<C>, ViewComp where C : ModuleBase, new()
{
    protected UILevel ShowLevel = UILevel.LowUI;
    private int Index;
    public GameObject UIGameobject;
    protected Canvas Canvas;
    protected bool ishow;
    #region 框架构造
    /// <summary>
    /// 基础界面组件
    /// </summary>
    /// <param name="_ModelContorl"></param>
    /// <param name="_Agr">第一个参数 资源根路径</param>
    public override void LGLoad(ModuleBase module, params object[] agr)
    {
        base.LGLoad(module);
        string PrefabName = (string)agr[0];
        ShowLevel = agr.Length >= 2 ? (UILevel)agr[1] : UILevel.LowUI;
        UIOption option = agr.Length >= 3 ? (UIOption)agr[2] : UIOption.Create;
        if (option == UIOption.Create)
        {
            GameObject uiasset = Module.LoadAsset<GameObject>("Prefab", PrefabName);
            UIGameobject = ViewManagerModule.Instance.CreateView(this, uiasset);
        }
        else if (option == UIOption.Find)
        {
            UIGameobject = ViewManagerModule.Instance.FindView(this, PrefabName);
        }
        else if (option == UIOption.Empty)
        {
            UIGameobject = ViewManagerModule.Instance.CreateEmptyView(this, PrefabName);
        }
        else
        {
            UIGameobject = ViewManagerModule.Instance.FindView(this, PrefabName);
            if (UIGameobject == null)
            {
                GameObject uiasset = Module.LoadAsset<GameObject>("Prefab", PrefabName);
                UIGameobject = ViewManagerModule.Instance.CreateView(this, uiasset);
            }
        }
        if (UIGameobject == null)
        {
            Debug.LogError("查找界面失败:" + PrefabName);
        }
        else
        {
            UIGameobject.SetLayer(LayerMask.NameToLayer("UI"));
            ViewManagerModule.Instance.SetViewToTop(this);
        }
    }

    public override void Close()
    {
        ViewManagerModule.Instance.DeletView(this);
        GameObject.Destroy(UIGameobject);
        base.Close();
    }
    #endregion

    public int GetIndex()
    {
        return Index;
    }

    public void SetIndex(int _Index)
    {
        Index = _Index;
        Canvas.sortingOrder = _Index;
    }

    public UILevel GetLevel()
    {
        return ShowLevel;
    }

    public void SetCanvas(Canvas _Canvas)
    {
        Canvas = _Canvas;
    }

    public virtual void Show()
    {
        ViewManagerModule.Instance.SetViewToTop(this);
        UIGameobject.SetActive(true);
        ishow = true;
    }

    public virtual void Hide()
    {
        UIGameobject.SetActive(false);
        ishow = false;
    }
}


