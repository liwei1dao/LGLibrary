using UnityEngine.UI;
namespace LG
{

    public class SceneLoadingViewComp : Module_BaseViewComp
    {
        private Slider LoadProgress;

        public override void LGLoad(ModuleBase _ModelContorl, params object[] _Agr)
        {
            ShowLevel = UILevel.HightUI;
            base.LGLoad(_ModelContorl, "LoadingView");
            LoadProgress = UIGameobject.OnSubmit<Slider>("LoadProgress");
        }

        public void UpdataProgress(float _Progress)
        {
            LoadProgress.value = _Progress;
        }
    }
}
