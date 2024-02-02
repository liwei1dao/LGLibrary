namespace LG
{
    using Sirenix.OdinInspector;
    using UnityEngine;

    public abstract class LGMain : MonoBehaviour
    {
        [LabelText("App资源加载方式")]
        protected AppResModel AppResModel;
        [LabelText("资源服务器地址")]
        protected string ResServiceAddr = string.Empty;

        protected virtual void Awake()
        {

        }

        protected virtual void SetConfig()
        {
            AppConfig.IsOpenVersionCheck = AppResModel == AppResModel.release;
            AppConfig.AppResModel = AppResModel;
            AppConfig.ResServiceAddr = ResServiceAddr;
#if UNITY_IOS
            AppConfig.TargetPlatform = AppPlatform.IOS;
#elif UNITY_ANDROID
            AppConfig.TargetPlatform = AppPlatform.Android;
#else
            AppConfig.TargetPlatform = AppPlatform.Windows;
#endif
        }


        protected abstract void StartApp();
    }
}