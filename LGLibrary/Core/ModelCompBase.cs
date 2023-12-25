namespace LG;

public interface IModelCompBase
{
    void LGLoad(ModuleBase module, params object[] agrs);
    void LGStart();
    void LGActivation();
    void Close();
}

/// <summary>
/// 模块组件基类
/// </summary>
public abstract class ModelCompBase : IModelCompBase
{
    protected ModuleBase? Module;
    public ModelState State = ModelState.Close;                             //组件状态
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


    public virtual void LGStart()
    {
        State = ModelState.Start;
    }
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
