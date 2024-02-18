using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace LG
{

    //模块初始化回调委托
    public delegate void ModelLoadBackCall<T>(T Module) where T : IModule;
    //模块基础状态
    public enum ModelState
    {
        Close = -1,          //关闭状态
        Loading = 1,        //加载中状态
        LoadEnd = 2,        //加载完成状态
        Start = 3,          //启动状态
        Runing = 4,         //运行中
    }

    public interface IModule
    {
        /// <summary>
        /// 模块初始化
        /// </summary>
        /// <param name="agrs"></param>
        void LGLoad(params object[] agrs);
        /// <summary>
        /// 加载完毕
        /// </summary>
        /// <returns></returns>
        bool LoadEnd();
        /// <summary>
        /// 模块启动
        /// </summary>
        /// <param name="agrs"></param>
        void LGStart();

        /// <summary>
        /// 激活
        /// </summary>
        void LGActivation();

        /// <summary>
        /// 模块关闭
        /// </summary>
        /// <param name="agrs"></param>
        void LGClose();
    }
    //更新模块
    public interface IUpdataMode : IModule
    {
        ModelState GetState();
        void Update(float time);
    }
    [Serializable]
    public abstract class ModuleBase : IModule
    {
        /// <summary>
        /// 模块名称
        /// </summary>
        [LabelText("模块名称"), ReadOnly]
        public string ModuleName;
        /// <summary>
        /// 模块标签
        /// </summary>
        [LabelText("模块标签")]
        public string ModuleTag = string.Empty;
        /// <summary>
        /// 模块状态
        /// </summary>
        [LabelText("模块状态"), ReadOnly]
        public ModelState State = ModelState.Close;
        /// <summary>
        /// 组件列表
        /// </summary>
        [ShowInInspector, LabelText("组件列表")]
        protected List<ModelCompBase> Comps = new List<ModelCompBase>();
        protected Module_CoroutineComp CoroutineComp;                               //协程组件（需要则初始化）
        protected Module_TimerComp TimerComp;                                       //计时器组件 （需要则初始化）
        protected Module_ResourceComp ResourceComp;                                 //资源管理组件（需要则初始化）
        protected Module_SoundComp SoundComp;                                       //声音组件 （需要则初始化）
        public ModuleBase()
        {
            State = ModelState.Close;
        }

        public ModelState GetState()
        {
            return this.State;
        }

        public virtual void LGLoad(params object[] agrs)
        {
            State = ModelState.Loading;
            for (int i = 0; i < Comps.Count; i++)
            {
                Comps[i].LGLoad(this, agrs);
            }
            LoadEnd();
        }

        public virtual bool LoadEnd()
        {
            if (State >= ModelState.LoadEnd) //模块已经加载成功了
                return false;
            for (int i = 0; i < Comps.Count; i++)
            {
                if (Comps[i].State != ModelState.LoadEnd)
                {
                    return false;
                }
            }
            if (State < ModelState.LoadEnd)
            {
                State = ModelState.LoadEnd;
                return true;
            }
            else
            {
                return false;
            }
        }

        public virtual void LGStart()
        {
            State = ModelState.Start;
            for (int i = 0; i < Comps.Count; i++)
            {
                Comps[i].LGStart();
            }
            State = ModelState.Runing;
        }

        public virtual void LGActivation()
        {

        }

        public virtual void LGClose()
        {

        }

        protected virtual C AddComp<C>(params object[] agrs) where C : ModelCompBase, new()
        {
            C Comp = new C();
            Type compType = Comp.GetType();
            ModelCompBaseAttribute compAttribute = (ModelCompBaseAttribute)Attribute.GetCustomAttribute(compType, typeof(ModelCompBaseAttribute));
            if (compAttribute != null)
            {
                Comp.Name = compAttribute.Name;
            }
            else
            {
                Comp.Name = compType.Name;
            }

            Comps.Add(Comp);
            if (State > ModelState.Close)
                Comp.LGLoad(this, agrs);
            if (State == ModelState.Start)
                Comp.LGStart();
            return Comp;
        }


        #region 资源管理组件扩展
        /// <summary>
        /// 创建对象
        /// </summary>
        /// <param name="ObjectPath"></param>
        /// <param name="Parnt"></param>
        /// <returns></returns>
        public GameObject CreateObj(string BundleOrPath, string ObjectPath, GameObject Parnt = null)
        {
            GameObject obj = LoadAsset<GameObject>(BundleOrPath, ObjectPath);
            if (obj == null)
            {
                return null;
            }
            if (Parnt == null)
                return obj.CreateToParnt(null);
            else
                return obj.CreateToParnt(Parnt);
        }

        public UnityEngine.Object LoadAsset(string BundleOrPath, string AssetName)
        {
            if (ResourceComp == null)
            {
                Log.Exception("模块未加载 ResourceComp");
            }
            return ResourceComp.LoadAsset<UnityEngine.Object>(BundleOrPath, AssetName);
        }

        public void LoadBundle(string BundleOrPath)
        {
            AssetBundle bundle = ResourceComp.LoadAssetBundle(BundleOrPath);
        }

        public T LoadAsset<T>(string BundleOrPath, string AssetName) where T : UnityEngine.Object
        {
            return ResourceComp.LoadAsset<T>(BundleOrPath, AssetName);
        }
        public T[] LoadAllAsset<T>(string BundleOrPath, string AssetName) where T : UnityEngine.Object
        {
            return ResourceComp.LoadAllAsset<T>(BundleOrPath, AssetName);
        }
        #endregion

        #region 协程组件扩展
        /// <summary>
        /// 启动协程
        /// </summary>
        public CoroutineTask StartCoroutine(IEnumerator coroutine)
        {
            if (CoroutineComp == null)
            {
                Log.Exception(ModuleName + " No Load CoroutineComp");
                return null;
            }
            return CoroutineComp.StartCoroutine(coroutine);
        }
        #endregion

        #region 计时器组件扩展
        /// <summary>
        /// 启动计时器
        /// </summary>
        public uint VP(float start, Action handler)
        {
            if (TimerComp == null)
            {
                Debug.LogError(ModuleName + " No Load TimerComp");
                return 0;
            }
            return TimerComp.VP(start, handler);
        }

        /// <summary>
        /// 启动计时器
        /// </summary>
        /// <param name="start">延迟时间</param>
        /// <param name="interval">间隔时间</param>
        /// <param name="handler">处理函数</param>
        /// <returns></returns>
        public uint VP(float start, int interval, Action handler)
        {
            if (TimerComp == null)
            {
                Debug.LogError(ModuleName + " No Load TimerComp");
                return 0;
            }
            return TimerComp.VP(start, interval, handler);
        }

        #endregion

        #region 声音组件
        public AudioSource PlayMusic(string Music, bool IsBackMusic = false)
        {
            if (SoundComp == null)
            {
                Debug.LogError(ModuleName + " No Load SoundComp");
                return null;
            }
            return SoundComp.PlayMusic(Music, IsBackMusic);
        }
        public AudioSource PlayMusic(AudioClip Music, bool IsBackMusic = false)
        {
            if (SoundComp == null)
            {
                Debug.LogError(ModuleName + " No Load SoundComp");
                return null;
            }
            return SoundComp.PlayMusic(Music, IsBackMusic);
        }
        public AudioSource PlayMusic(string Music, float MusicValue, bool IsBackMusic = false)
        {
            if (SoundComp == null)
            {
                Debug.LogError(ModuleName + " No Load SoundComp");
                return null;
            }
            return SoundComp.PlayMusic(Music, MusicValue, IsBackMusic);
        }
        public AudioSource PlayMusic(AudioClip Music, float MusicValue, bool IsBackMusic = false)
        {
            if (SoundComp == null)
            {
                Debug.LogError(ModuleName + " No Load SoundComp");
                return null;
            }
            return SoundComp.PlayMusic(Music, MusicValue, IsBackMusic);
        }
        public void StopBackMusic()
        {
            if (SoundComp == null)
            {
                Debug.LogError(ModuleName + " No Load SoundComp");
            }
            SoundComp.StopBackMusic();
        }

        public void PauseBackMusic()
        {
            if (SoundComp == null)
            {
                Debug.LogError(ModuleName + " No Load SoundComp");
                return;
            }
            SoundComp.PauseBackMusic();
        }
        public void UnPauseBackMusic()
        {
            if (SoundComp == null)
            {
                Debug.LogError(ModuleName + " No Load SoundComp");
                return;
            }
            SoundComp.UnPauseBackMusic();
        }

        #endregion

        #region 对象池
        public void NewPool(string poolname, Func<GameObject> cf)
        {
            ObjectPoolModule.Instance.NewObjectPool(poolname, cf);
        }
        public void NewPool<T>(string poolname, Func<string, T> cf) where T : UnityEngine.Object
        {
            ObjectPoolModule.Instance.NewObjectPool<T>(poolname, cf);
        }
        public T GetPool<T>(string poolname) where T : UnityEngine.Object
        {
            return ObjectPoolModule.Instance.Get<T>(poolname);
        }
        public void PushPool(string poolname, GameObject obj)
        {
            ObjectPoolModule.Instance.Push(poolname, obj);
        }
        public T GetPool<T>(string poolname, string key) where T : UnityEngine.Object
        {
            return ObjectPoolModule.Instance.Get<T>(poolname, key);
        }
        public void PushPool<T>(string poolname, string key, T obj) where T : UnityEngine.Object
        {
            ObjectPoolModule.Instance.Push<T>(poolname, key, obj);
        }
        #endregion
    }

    public abstract class ModuleBase<T> : ModuleBase where T : ModuleBase<T>, new()
    {
        #region 单例接口
        protected static T instance = null;
        public static T Instance
        {
            get
            {
                if (instance == null)
                {
                    Log.Exception($"This Model No LoadEnd:{typeof(T).Name}");
                }
                return instance;
            }
            protected set
            {
                instance = value;
            }
        }
        #endregion

        protected ModelLoadBackCall<T> LoadBackCall;

        public ModuleBase()
              : base()
        {
            instance = this as T;
        }

        public virtual void LGLoad(ModelLoadBackCall<T> loadBackCall, params object[] agrs)
        {
            LoadBackCall = loadBackCall;
            LGLoad(agrs);
        }

        public override bool LoadEnd()
        {
            // Log.Debug("LoadEnd Check!");
            if (base.LoadEnd())
            {
                // Log.Debug("LoadEnd Finsh!");
                LoadBackCall?.Invoke(instance);
                return true;
            }
            else
            {
                // Log.Debug("LoadEnd Check False!");
                return false;
            }
        }
    }
}