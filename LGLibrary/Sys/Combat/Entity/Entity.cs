namespace LG.Combat;

/// <summary>
/// 基础实体对象
/// </summary>
public abstract partial class Entity
{
    public long Id { get; set; }
    private string name;
    public string Name
    {
        get => name;
        set
        {
            name = value;
        }
    }
    public long InstanceId { get; set; }
    public Entity Parent { get; protected set; }
    protected List<Entity> Children;
    public Dictionary<long, Entity> Id2Children { get; private set; } = new Dictionary<long, Entity>();
    public Dictionary<Type, List<Entity>> Type2Children { get; private set; } = new Dictionary<Type, List<Entity>>();
    public Dictionary<Type, Component> Components { get; set; } = new Dictionary<Type, Component>();

    public virtual void OnSetParent(Entity preParent, Entity nowParent)
    {

    }
    public virtual void LGInit(params object[] agrs)
    {

    }
    public virtual void LGStart()
    {

    }
    public virtual void OnDestroy()
    {

    }
    private void Dispose()
    {
        if (EnableLog) Log.Debug($"{GetType().Name}->Dispose");
        if (Children.Count > 0)
        {
            for (int i = Children.Count - 1; i >= 0; i--)
            {
                Destroy(Children[i]);
            }
            Children.Clear();
            Type2Children.Clear();
        }
        Parent?.RemoveChild(this);
        foreach (var component in Components.Values)
        {
            component.Enable = false;
            Component.Destroy(component);
        }
        Components.Clear();
        InstanceId = 0;
    }
    #region 组件
    public T? GetParent<T>() where T : Entity
    {
        return Parent as T;
    }

    public T Add<T>(params object[] agrs) where T : Component
    {
        return AddComponent<T>(agrs);
    }

    public virtual T AddComponent<T>(params object[] agrs) where T : Component
    {
        var component = Activator.CreateInstance<T>();
        component.Entity = this;
        component.LGInit(this, agrs);
        Components.Add(typeof(T), component);
        component.LGStart();
        return component;
    }
    public void RemoveComponent<T>() where T : Component
    {
        var component = Components[typeof(T)];
        if (component.Enable) component.Enable = false;
        Component.Destroy(component);
        Components.Remove(typeof(T));
    }

    public bool HasComponent<T>() where T : Component
    {
        return Components.TryGetValue(typeof(T), out var component);
    }
    public Component GetComponent(Type componentType)
    {
        if (this.Components.TryGetValue(componentType, out var component))
        {
            return component;
        }
        return null;
    }
    public T GetComponent<T>() where T : Component
    {
        if (Components.TryGetValue(typeof(T), out var component))
        {
            return component as T;
        }
        return null;
    }

    public bool TryGet<T>(out T component) where T : Component
    {
        if (Components.TryGetValue(typeof(T), out var c))
        {
            component = c as T;
            return true;
        }
        component = null;
        return false;
    }


    #endregion

    #region 子实体
    private void SetParent(Entity parent)
    {
        Entity preParent = Parent;
        preParent?.RemoveChild(this);
        this.Parent = parent;
        OnSetParent(preParent, parent);
    }


    public virtual E AddEntity<E>() where E : Entity
    {
        E entity = default(E);
        Children.Add(entity);
        entity.LGInit(this);
        return entity;
    }

    public Entity AddChild(Type entityType, params object[] agrs)
    {
        var entity = NewEntity(entityType);
        if (EnableLog) Log.Debug($"AddChild {this.GetType().Name}, {entityType.Name}={entity.Id}");
        SetupEntity(entity, this, agrs);
        return entity;
    }
    public void SetChild(Entity child)
    {
        Children.Add(child);
        Id2Children.Add(child.Id, child);
        if (!Type2Children.ContainsKey(child.GetType())) Type2Children.Add(child.GetType(), new List<Entity>());
        Type2Children[child.GetType()].Add(child);
        child.SetParent(this);
    }

    public T AddChild<T>() where T : Entity
    {
        return AddChild(typeof(T)) as T;
    }

    public T AddChild<T>(object initData) where T : Entity
    {
        return AddChild(typeof(T), initData) as T;
    }
    public virtual void RemoveChild(Entity child)
    {
        Children.Remove(child);
        if (Type2Children.ContainsKey(child.GetType())) Type2Children[child.GetType()].Remove(child);
    }
    public Entity Find(string name)
    {
        foreach (var item in Children)
        {
            if (item.name == name) return item;
        }
        return null;
    }

    public T Find<T>(string name) where T : Entity
    {
        if (Type2Children.TryGetValue(typeof(T), out var chidren))
        {
            foreach (var item in chidren)
            {
                if (item.name == name) return item as T;
            }
        }
        return null;
    }
    #endregion


    #region 事件
    public T Publish<T>(T TEvent) where T : class
    {
        var eventComponent = GetComponent<EventComponent>();
        if (eventComponent == null)
        {
            return TEvent;
        }
        eventComponent.Publish(TEvent);
        return TEvent;
    }
    public SubscribeSubject Subscribe<T>(Action<T> action) where T : class
    {
        var eventComponent = GetComponent<EventComponent>();
        if (eventComponent == null)
        {
            eventComponent = AddComponent<EventComponent>();
        }
        return eventComponent.Subscribe(action);
    }
    public SubscribeSubject Subscribe<T>(Action<T> action, Entity disposeWith) where T : class
    {
        var eventComponent = GetComponent<EventComponent>();
        if (eventComponent == null)
        {
            eventComponent = AddComponent<EventComponent>();
        }
        return eventComponent.Subscribe(action).DisposeWith(disposeWith);
    }

    public void UnSubscribe<T>(Action<T> action) where T : class
    {
        var eventComponent = GetComponent<EventComponent>();
        if (eventComponent != null)
        {
            eventComponent.UnSubscribe(action);
        }
    }
    #endregion
}