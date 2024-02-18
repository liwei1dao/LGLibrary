using UnityEngine;
using System.Collections.Generic;
using System;

namespace LG
{

    /// <summary>
    /// 队列对象池
    /// </summary>
    public class GameObjectPoolByQueue
    {
        public GameObject Root;
        public ModuleBase Module;
        public Func<UnityEngine.Object> CreateFun;
        public Queue<UnityEngine.Object> Pool;
        public GameObjectPoolByQueue(string pname, Func<GameObject> cf)
        {
            Root = ObjectPoolModule.Instance.ObjectPools.CreateChild(pname);
            CreateFun = cf;
            Pool = new Queue<UnityEngine.Object>();
        }

        public UnityEngine.Object Get()
        {
            if (Pool.Count > 0)
            {
                return Pool.Dequeue();
            }
            else
            {
                return CreateFun();
            }
        }

        public void Push(UnityEngine.Object obj)
        {
            Pool.Enqueue(obj);
        }

        /// <summary>
        /// 释放
        /// </summary>
        public void Release()
        {
            Root.ClearChilds();
            Pool.Clear();
        }

    }

    /// <summary>
    /// 字典对象池
    /// </summary>
    public class GameObjectPoolByDictionary
    {
        public GameObject Root;
        public Func<string, UnityEngine.Object> CreateFun;
        public Dictionary<string, Queue<UnityEngine.Object>> Pool;
        public GameObjectPoolByDictionary(string pname, Func<string, UnityEngine.Object> cf)
        {
            Root = ObjectPoolModule.Instance.ObjectPools.CreateChild(pname);
            CreateFun = cf;
            Pool = new Dictionary<string, Queue<UnityEngine.Object>>();
        }

        public UnityEngine.Object Get(string key)
        {
            if (Pool.ContainsKey(key) && Pool[key].Count > 0)
            {
                return Pool[key].Dequeue();
            }
            else
            {
                UnityEngine.Object obj = CreateFun(key);
                return obj;
            }
        }

        public void Push(string key, UnityEngine.Object obj)
        {
            if (!Pool.ContainsKey(key))
            {
                Pool[key] = new Queue<UnityEngine.Object>();
            }
            Pool[key].Enqueue(obj);
        }

        /// <summary>
        /// 释放
        /// </summary>
        public void Release()
        {
            Root.ClearChilds();
            Pool.Clear();
        }
    }
}