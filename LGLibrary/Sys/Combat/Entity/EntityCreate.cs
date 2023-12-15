namespace LG.Combat;
public abstract partial class Entity
{
    public static CombatContext Master => CombatContext.Instance;
    public static bool EnableLog { get; set; } = false;
    public static long BaseRevertTicks { get; set; }
    public static long NewInstanceId()
    {
        if (BaseRevertTicks == 0)
        {
            var now = DateTime.UtcNow.Ticks;
            var str = now.ToString().Reverse();
            BaseRevertTicks = long.Parse(string.Concat(str));
        }
        BaseRevertTicks++;
        return BaseRevertTicks;
    }
    public static Entity NewEntity(Type entityType, long id = 0)
    {
        var entity = Activator.CreateInstance(entityType) as Entity;
        entity.InstanceId = NewInstanceId();
        if (id == 0) entity.Id = entity.InstanceId;
        else entity.Id = id;
        if (!Master.Entities.ContainsKey(entityType))
        {
            Master.Entities.Add(entityType, new List<Entity>());
        }
        Master.Entities[entityType].Add(entity);
        return entity;
    }

    private static void SetupEntity(Entity entity, Entity parent, params object[] agrs)
    {
        parent.SetChild(entity);
        entity.LGInit(agrs);
        entity.LGStart();
    }

    public static void Destroy(Entity entity)
    {
        try
        {
            entity.OnDestroy();
        }
        catch (Exception e)
        {
            Log.Error(e);
        }
        entity.Dispose();
    }
}