using LG;
using Sirenix.OdinInspector;
using UnityEngine;

public class main : LGMain
{
    [LabelText("�Ƿ�ʹ��Զ�̷�����")]
    public bool IsRemoteServer;
    protected override void StartApp()
    {
        //����UI����ģ��
        ModuleManager.Instance.StartModule<GUIManagerModule>((module) =>
        { 
        
        }, new Vector2(1440, 2560), 0f);
    }
}
