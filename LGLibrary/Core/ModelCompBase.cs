using System;
using Sirenix.OdinInspector;

namespace LG
{

    public interface IModelCompBase
    {
        void LGLoad(ModuleBase module, params object[] agrs);
        void LGStart();
        void LGActivation();
        void Close();
    }


    [System.AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    sealed class ModelCompBaseAttribute : Attribute
    {
        readonly string name;
        readonly int order;

        public ModelCompBaseAttribute(string name, int order)
        {
            this.name = name;
            this.order = order;
        }

        public string Name
        {
            get { return name; }
        }

        public int Order
        {
            get { return order; }
        }
    }
  
    /// <summary>
    /// 模块组件基类
    /// </summary>
    [Serializable]
    public abstract class ModelCompBase : IModelCompBase
    {
        [LabelText("组件名称"),ReadOnly]
        public string Name;
        [LabelText("组件状态"),ReadOnly]
        public ModelState State = ModelState.Close;                             //组件状态
        protected ModuleBase Module;

        public virtual void LGLoad(ModuleBase module, params object[] agrs)
        {
            Module = module;
            State = ModelState.Loading;
        }
        protected virtual void LoadEnd()
        {
            State = ModelState.LoadEnd;
            Module.LoadEnd();
        }

        /// <summary>
        /// 启动
        /// </summary>
        public virtual void LGStart()
        {
            State = ModelState.Start;
        }

        /// <summary>
        /// 激活
        /// </summary>
        public virtual void LGActivation()
        {

        }

        public virtual void Close()
        {
            Module = null;
            State = ModelState.Close;
        }
    }

    public abstract class ModelCompBase<C> : ModelCompBase where C : ModuleBase, new()
    {
        protected new C Module;

        public override void LGLoad(ModuleBase module, params object[] agrs)
        {
            Module = module as C;
            base.LGLoad(module, agrs);
        }
    }
}