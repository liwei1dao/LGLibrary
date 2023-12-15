
using System;
using System.Linq;
using System.Collections.Generic;

namespace LG.Combat;

/// <summary>
/// 实体状态
/// </summary>
public abstract class EntityState : IReference
{
    public string StateName { get; set; }
    public int Priority { get; set; }
    public void SetData(string stateName, int priority)
    {
        StateName = stateName;
        Priority = priority;
    }
    public virtual bool TryEnter(StateComponent comp)
    {
        return true;
    }
    /// <summary>
    /// 状态进入
    /// </summary>
    public virtual void OnEnter(StateComponent comp)
    {
    }
    /// <summary>
    /// 状态退出
    /// </summary>
    public virtual void OnExit(StateComponent comp)
    {

    }
    /// <summary>
    /// 状态移除
    /// </summary>
    public virtual void OnRemoved(StateComponent comp)
    {

    }

    public virtual void Clear()
    {

    }
}
