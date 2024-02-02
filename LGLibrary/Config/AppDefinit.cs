using Sirenix.OdinInspector;

namespace LG
{

    [LabelText("平台")]
    public enum AppPlatform
    {
        IOS,
        Android,
        Windows,
    }

    [LabelText("资源模式")]
    public enum AppResModel
    {
        debug,
        release,
    }

    [LabelText("UI级别")]
    public enum UILevel
    {
        LowUI,
        NormalUI,
        HightUI,
    }

    [LabelText("UI加载方式")]
    public enum UIOption
    {
        Create = 1,                  //创建界面
        Find = 2,                    //寻找界面
        Auto = 3,                    //自动 先找 找不到就创建
        Empty = 4,                   //空界面
    }
}