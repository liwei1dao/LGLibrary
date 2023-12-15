namespace LG.Combat;

public class CombatContext
{
    private static CombatContext instance;
    public static CombatContext Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new CombatContext();
            }
            return instance;
        }
    }
}
