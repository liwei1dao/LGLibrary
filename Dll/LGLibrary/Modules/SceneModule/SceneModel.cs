namespace LG
{

    /// <summary>
    /// 场景模块控制器
    /// </summary>
    public class SceneModule : ModuleBase<SceneModule>
    {
        private SceneChedulerComp ChangeSceneComp;
        private SceneLoadingViewComp LoadingViewComp;
        public int LoadingSteps = 100;                                            //加载进度平率
        public override void LGLoad(params object[] agrs)
        {
            IScenesChedulerBase Cheduler;
            if (agrs.Length == 1 && agrs[0] is IScenesChedulerBase)
            {
                Cheduler = agrs[0] as IScenesChedulerBase;
            }
            else
            {
                Cheduler = new ScenesDefaultCheduler();
                agrs = new object[] { Cheduler };
            }
            CoroutineComp = AddComp<Module_CoroutineComp>();
            ResourceComp = AddComp<Module_ResourceComp>();

            ChangeSceneComp = AddComp<SceneChedulerComp>(Cheduler);
            base.LGLoad(agrs);
        }
        public override void LGStart()
        {
            base.LGStart();
        }

        /// <summary>
        /// 设置场景调度器监控
        /// </summary>
        /// <param name="Cheduler"></param>
        public void SetSceneCheduler(IScenesChedulerBase Cheduler)
        {
            if (ChangeSceneComp != null)
            {
                ChangeSceneComp.SetCheduler(Cheduler);
            }
        }

        /// <summary>
        /// 获取通用加载组件
        /// </summary>
        /// <returns></returns>
        public SceneLoadingViewComp GetLoadingViewComp()
        {
            if (LoadingViewComp == null)
            {
                LoadingViewComp = AddComp<SceneLoadingViewComp>();
            }
            return LoadingViewComp;
        }

        /// <summary>
        /// 跳转场景
        /// </summary>
        /// <param name="SceneId"></param>
        /// <param name="CallBack"></param>
        public void ChangeScene(ISceneLoadCompBase SceneLoadComp)
        {
            ChangeSceneComp.ChangeScene(SceneLoadComp);
        }
    }
}