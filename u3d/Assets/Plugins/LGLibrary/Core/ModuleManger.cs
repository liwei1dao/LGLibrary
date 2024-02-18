using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace LG
{

    public class ModuleManager : MonoBehaviour
    {
        [ShowInInspector, LabelText("模块列表")]
        [DictionaryDrawerSettings(KeyLabel = "模块名称", ValueLabel = "模块对象", DisplayMode = DictionaryDisplayOptions.OneLine, IsReadOnly = true)]
        private Dictionary<string, ModuleBase> modules = new Dictionary<string, ModuleBase>();
        private List<IUpdataMode> updataModes = new List<IUpdataMode>();

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
                        DontDestroyOnLoad(obj);
                        instance = obj.GetComponent<ModuleManager>();
                    }
                }
                return instance;
            }
        }
        #endregion

        void Update()
        {
            foreach (var module in updataModes)
            {
                if (module.GetState() == ModelState.Runing)
                {
                    module.Update(Time.deltaTime);
                }
            }
        }


        // private void AddModules(ModuleBase module)
        // {
        //     modules.Add(module.ModuleName, module);
        // }

        private IEnumerator ModuleStart<T>(T module, ModelLoadBackCall<T> backCall) where T : ModuleBase<T>, new()
        {
            yield return new WaitForEndOfFrame();
            module.LGStart();
            backCall?.Invoke(module);
            if (module is IUpdataMode)
            {
                updataModes.Add(module as IUpdataMode);
            }
        }

        public void StartModule<T>(ModelLoadBackCall<T> backCall = null, params object[] agrs) where T : ModuleBase<T>, new()
        {
            string ModelName = typeof(T).Name;
            if (!modules.ContainsKey(ModelName))
            {
                T module = new T
                {
                    ModuleName = ModelName
                };
                module.LGLoad((model) =>
                {
                    StartCoroutine(ModuleStart<T>(model, backCall));
                }, agrs);
                modules[ModelName] = module;
            }
            else
            {
                Debug.LogError("This Model Already Load:" + typeof(T).Name);
            }
        }


        public void StartModuleByTag<T>(string tag, ModelLoadBackCall<T> backCall = null, params object[] agrs) where T : ModuleBase<T>, new()
        {
            string ModelName = typeof(T).Name;
            if (!modules.ContainsKey(ModelName))
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
                modules[ModelName] = module;
            }
            else
            {
                Debug.LogError("This Model Already Load:" + typeof(T).Name);
            }
        }

        public void StartModuleObj<T>(string moduleName, T module, ModelLoadBackCall<T> backCall = null, params object[] agrs) where T : ModuleBase<T>, new()
        {
            if (!modules.ContainsKey(moduleName))
            {
                module.ModuleName = moduleName;
                module.LGLoad((model) =>
                {
                    StartCoroutine(ModuleStart<T>(model, backCall));
                }, agrs);
                modules[module.ModuleName] = module;
            }
            else
            {
                Debug.LogWarning("This Model Already Load:" + moduleName);
            }
        }


        public T GetModuleByTag<T>(string mtag) where T : class, IModule
        {
            foreach (var module in modules)
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
            if (modules.ContainsKey(ModelName))
            {
                if (modules[ModelName] is IUpdataMode)
                {
                    updataModes.Remove(modules[ModelName] as IUpdataMode);
                }
                modules[ModelName].LGClose();
                modules.Remove(ModelName);

            }
        }
    }
}