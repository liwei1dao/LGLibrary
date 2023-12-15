namespace LG.Combat;

/// <summary>
/// 基础实体对象
/// </summary>
public abstract class Component
{
    public Entity Entity { get; set; }
    public bool IsDisposed { get; set; }
    public virtual bool DefaultEnable { get; set; } = true;
    private bool enable = false;
    public bool Enable
    {
        set
        {
            if (enable == value) return;
            enable = value;
            if (enable) OnEnable();
            else OnDisable();
        }
        get
        {
            return enable;
        }
    }
    public bool Disable => enable == false;
    public virtual void LGInit(Entity entity, params object[] agrs)
    {
        Entity = entity;
    }
    public virtual void LGStart()
    {

    }
    public virtual void OnEnable()
    {

    }
    public virtual void OnDisable()
    {

    }
    public virtual void LGUpdate(float time)
    {

    }
    public virtual void OnDestroy()
    {

    }
    private void Dispose()
    {
        if (Entity.EnableLog) Log.Debug($"{GetType().Name}->Dispose");
        Enable = false;
        IsDisposed = true;
    }


    public T Publish<T>(T TEvent) where T : class
    {
        Entity.Publish(TEvent);
        return TEvent;
    }

    public void Subscribe<T>(Action<T> action) where T : class
    {
        Entity.Subscribe(action);
    }

    public void UnSubscribe<T>(Action<T> action) where T : class
    {
        Entity.UnSubscribe(action);
    }


    public static void Destroy(Component comp)
    {
        try
        {
            comp.OnDestroy();
        }
        catch (Exception e)
        {
            Log.Error(e);
        }
        comp.Dispose();
    }
}