using System.Collections;
using Sirenix.OdinInspector;
using UnityEngine;

namespace LG
{

    public class ModuleManager : MonoBehaviour
    {
        [SerializeField, LabelText("模块列表")]
        protected Dictionary<string, ModuleBase> Modules = new();

        #region 单例接口
        private static ModuleManager instance = null;
        public static ModuleManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = FindObjectOfType(typeof(ModuleManager)) as ModuleManager;
                    if (instance == null)
                    {
                        GameObject obj = new GameObject(typeof(ModuleManager).Name, typeof(ModuleManager));
                        instance = obj.GetComponent<ModuleManager>();

                    }
                }
                return instance;
            }
        }
        #endregion

        private void AddModules(ModuleBase module)
        {
            Modules.Add(module.ModuleName, module);
        }

        private IEnumerator ModuleStart<T>(T module, ModelLoadBackCall<T> backCall) where T : ModuleBase<T>, new()
        {
            yield return new WaitForEndOfFrame();
            module.LGStart();
            backCall?.Invoke(module);
        }

        public void StartModule<T>(ModelLoadBackCall<T> backCall = null, params object[] agrs) where T : ModuleBase<T>, new()
        {
            string ModelName = typeof(T).Name;
            if (!Modules.ContainsKey(ModelName))
            {
                T module = new T
                {
                    ModuleName = ModelName
                };
                module.LGLoad((model) =>
                {
                    StartCoroutine(ModuleStart<T>(model, backCall));
                }, agrs);
            }
            else
            {
                Debug.LogError("This Model Already Load:" + typeof(T).Name);
            }
        }


        public void StartModuleByTag<T>(string tag, ModelLoadBackCall<T> backCall = null, params object[] agrs) where T : ModuleBase<T>, new()
        {
            string ModelName = typeof(T).Name;
            if (!Modules.ContainsKey(ModelName))
            {
                T module = new T
                {
                    ModuleName = ModelName,
                    ModuleTag = tag,
                };
                module.LGLoad((model) =>
                {
                    StartCoroutine(ModuleStart<T>(model, backCall));
                }, agrs);
            }
            else
            {
                Debug.LogError("This Model Already Load:" + typeof(T).Name);
            }
        }

        public void StartModuleObj<T>(string moduleName, T module, ModelLoadBackCall<T> backCall = null, params object[] agrs) where T : ModuleBase<T>, new()
        {
            if (!Modules.ContainsKey(moduleName))
            {
                module.ModuleName = moduleName;
                module.LGLoad((model) =>
                {
                    StartCoroutine(ModuleStart<T>(model, backCall));
                }, agrs);
            }
            else
            {
                Debug.LogWarning("This Model Already Load:" + moduleName);
            }
        }


        public T GetModuleByTag<T>(string mtag) where T : class, IModule
        {
            foreach (var module in Modules)
            {
                if (module.Value.ModuleTag == mtag)
                {
                    return module.Value as T;
                }
            }
            return null;
        }

        public void CloseModule<T>() where T : ModuleBase<T>, new()
        {
            string ModelName = typeof(T).Name;
            if (Modules.ContainsKey(ModelName))
            {
                Modules[ModelName].LGClose();
                Modules.Remove(ModelName);
            }
        }
    }
}