using System;
using UnityEngine;


namespace LG
{
    /// <summary>
    /// 对象池模块
    /// </summary>
    public class ObjectPoolModule : ModuleBase<ObjectPoolModule>
    {
        public GameObject ObjectPools { get; private set; }
        public ObjectPoolsComp objectPoolsComp;
        public override void LGLoad(params object[] agrs)
        {
            ObjectPools = new GameObject("ObjectPools");
            UnityEngine.Object.DontDestroyOnLoad(ObjectPools);
            objectPoolsComp = AddComp<ObjectPoolsComp>();
            base.LGLoad(agrs);
        }

        public void NewObjectPool(string poolname, Func<GameObject> cf)
        {
            objectPoolsComp.RegisterQueuePool(poolname, cf);
        }

        public T Get<T>(string poolname) where T : UnityEngine.Object
        {
            return objectPoolsComp.GetByQueuePool<T>(poolname);
        }

        public void Push<T>(string poolname, T obj) where T : UnityEngine.Object
        {
            objectPoolsComp.PushByQueuePool<T>(poolname, obj);
        }
        public void NewObjectPool<T>(string poolname, Func<string, T> cf) where T : UnityEngine.Object
        {
            objectPoolsComp.RegisterDictionaryPool(poolname, cf);
        }
        public T Get<T>(string poolname, string key) where T : UnityEngine.Object
        {
            return objectPoolsComp.GetByDictionaryPool<T>(poolname, key);
        }
        public void Push<T>(string poolname, string key, T obj) where T : UnityEngine.Object
        {
            objectPoolsComp.PushByDictionaryPool<T>(poolname, key, obj);
        }
    }
}