using UnityEngine;

namespace LG.Combat;

public interface IPosition
{
    Vector3 Position { get; set; }
}
public sealed class CombatEntity : Entity, IPosition
{
    public Vector3 Position { get; set; }


    #region 行动点事件
    public void ListenActionPoint(ActionPointType actionPointType, Action<Entity> action)
    {
        GetComponent<ActionPointComponent>().AddListener(actionPointType, action);
    }

    public void UnListenActionPoint(ActionPointType actionPointType, Action<Entity> action)
    {
        GetComponent<ActionPointComponent>().RemoveListener(actionPointType, action);
    }

    public void TriggerActionPoint(ActionPointType actionPointType, Entity action)
    {
        GetComponent<ActionPointComponent>().TriggerActionPoint(actionPointType, action);
    }
    #endregion
}