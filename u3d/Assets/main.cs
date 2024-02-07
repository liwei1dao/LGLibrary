using LG;
using Sirenix.OdinInspector;
using UnityEngine;

public class main : LGMain
{
    [LabelText("是否使用远程服务器")]
    public bool IsRemoteServer;
    protected override void StartApp()
    {
        //加载UI管理模块
        ModuleManager.Instance.StartModule<GUIManagerModule>((module) =>
        { 
        
        }, new Vector2(1440, 2560), 0f);
    }
}
