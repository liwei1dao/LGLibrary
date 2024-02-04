using Sirenix.OdinInspector;
using UnityEngine;

namespace LG
{
    public abstract class LGMain : MonoBehaviour
    {
        [SerializeField, LabelText("App资源加载方式")]
        protected AppResModel AppResModel;
        [SerializeField, LabelText("资源服务器地址"), ShowIf("AppResModel", AppResModel.release)]
        protected string ResServiceAddr = string.Empty;

        private void Awake()
        {
            SetConfig();
            StartApp();
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
