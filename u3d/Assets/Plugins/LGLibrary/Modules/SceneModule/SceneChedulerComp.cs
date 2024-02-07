using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace LG
{
    /// <summary>
    /// 场景调度组件
    /// </summary>
    public class SceneChedulerComp : ModelCompBase<SceneModule>
    {
        private float LoadSceneProgress;
        private IScenesChedulerBase Cheduler;
        private ISceneLoadCompBase CurrSceneLoadComp;
        #region 框架构造
        public override void LGLoad(ModuleBase module, params object[] agrs)
        {
            Cheduler = (IScenesChedulerBase)agrs[0];
            base.LGLoad(module, agrs);
            base.LoadEnd();
        }
        #endregion
        public void SetCheduler(IScenesChedulerBase _Cheduler)
        {
            Cheduler = _Cheduler;
        }
        public void ChangeScene(ISceneLoadCompBase SceneLoadComp)
        {
            LoadSceneProgress = 0;
            Module.StartCoroutine(ChangeSceneCoroutine(SceneLoadComp));
        }
        #region 切换场景
        private IEnumerator ChangeSceneCoroutine(ISceneLoadCompBase SceneLoadComp)
        {
            Cheduler.StartLoadChanage();
            yield return UnCurrScene(0, 0.3f);
            yield return LoadScene(SceneLoadComp.GetSceneName(), 0.3f, 0.7f);
            yield return LoadSceneControl(SceneLoadComp, 0.7f, 1);
            Cheduler.EndLoadChanage();
        }

        private IEnumerator UnCurrScene(float Start, float Target)
        {
            int currProcess = 0;
            int toProcess = 0;
            if (CurrSceneLoadComp != null)
            {
                Module.StartCoroutine(CurrSceneLoadComp.UnloadScene());
                while (LoadSceneProgress < Target)
                {
                    toProcess = (int)(CurrSceneLoadComp.GetProcess() * 100);
                    while (currProcess < toProcess)
                    {
                        currProcess += Module.LoadingSteps;
                        LoadSceneProgress = Mathf.Lerp(Start, Target, currProcess / 100.0f);
                        Cheduler.UpdataProgress(LoadSceneProgress);
                        yield return new WaitForEndOfFrame();
                    }
                    yield return new WaitForEndOfFrame();
                }
            }
            else
            {
                toProcess = 100;
                while (LoadSceneProgress < Target)
                {
                    while (currProcess < toProcess)
                    {
                        currProcess += Module.LoadingSteps;
                        LoadSceneProgress = Mathf.Lerp(Start, Target, currProcess / 100.0f);
                        Cheduler.UpdataProgress(LoadSceneProgress);
                        yield return new WaitForEndOfFrame();
                    }
                    yield return new WaitForEndOfFrame();
                }
            }
        }
        private IEnumerator LoadScene(string sceneName, float Start, float Target)
        {
            int currProcess = 0;
            int toProcess = 0;
            AsyncOperation op = SceneManager.LoadSceneAsync(sceneName);
            op.allowSceneActivation = false;
            while (op.progress < 0.9f)
            {
                toProcess = (int)op.progress * 100;

                while (currProcess < toProcess)
                {
                    currProcess += Module.LoadingSteps;
                    LoadSceneProgress = Mathf.Lerp(Start, Target, currProcess / 100.0f);
                    Cheduler.UpdataProgress(LoadSceneProgress);
                    yield return new WaitForEndOfFrame();
                }
                yield return new WaitForEndOfFrame();
            }
            toProcess = 100;
            while (currProcess < toProcess)
            {
                currProcess += Module.LoadingSteps;
                LoadSceneProgress = Mathf.Lerp(Start, Target, currProcess / 100.0f);
                Cheduler.UpdataProgress(LoadSceneProgress);
                yield return new WaitForEndOfFrame();
            }
            op.allowSceneActivation = true;
            yield return op;
        }
        private IEnumerator LoadSceneControl(ISceneLoadCompBase SceneLoadComp, float Start, float Target)
        {
            CurrSceneLoadComp = SceneLoadComp;
            int currProcess = 0;
            int toProcess = 0;
            if (CurrSceneLoadComp != null)
            {
                Module.StartCoroutine(CurrSceneLoadComp.LoadScene());
                while (LoadSceneProgress < Target)
                {
                    toProcess = (int)(CurrSceneLoadComp.GetProcess() * 100);
                    while (currProcess < toProcess)
                    {
                        currProcess += Module.LoadingSteps;
                        LoadSceneProgress = Mathf.Lerp(Start, Target, currProcess / 100.0f);
                        Cheduler.UpdataProgress(LoadSceneProgress);
                        yield return new WaitForEndOfFrame();
                    }
                    yield return new WaitForEndOfFrame();
                }
            }
            else
            {
                toProcess = 100;
                while (LoadSceneProgress < Target)
                {
                    while (currProcess < toProcess)
                    {
                        currProcess += Module.LoadingSteps;
                        LoadSceneProgress = Mathf.Lerp(Start, Target, currProcess / 100.0f);
                        Cheduler.UpdataProgress(LoadSceneProgress);
                        yield return new WaitForEndOfFrame();
                    }
                    yield return new WaitForEndOfFrame();
                }
            }
        }
        #endregion
    }
}