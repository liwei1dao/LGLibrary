namespace LG.Combat;

[System.AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
sealed class EnableUpdateAttribute : Attribute
{
    public EnableUpdateAttribute()
    {
    }
}

public class CombatContext
{
    private static CombatContext instance;
    public static CombatContext Instance
    {
        get
        {
            instance ??= new CombatContext();
            return instance;
        }
    }
    public Dictionary<Type, List<Entity>> Entities { get; private set; } = new Dictionary<Type, List<Entity>>();
    public List<Component> AllComponents { get; private set; } = new List<Component>();
    public void LGUpdate(float time)
    {
        if (AllComponents.Count == 0)
        {
            return;
        }
        for (int i = AllComponents.Count - 1; i >= 0; i--)
        {
            var item = AllComponents[i];
            if (item.IsDisposed)
            {
                AllComponents.RemoveAt(i);
                continue;
            }
            if (item.Disable)
            {
                continue;
            }
            item.LGUpdate(time);
        }
    }
}
