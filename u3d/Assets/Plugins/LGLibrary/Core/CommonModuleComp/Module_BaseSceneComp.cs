﻿using System.Collections;

namespace LG
{
  /// <summary>
  /// 模块场景组件
  /// </summary>
  public abstract class Module_BaseSceneComp<C> : ModelCompBase<C>, ISceneLoadCompBase where C : ModuleBase, new()
  {
    protected string SceneName;
    protected float Process;

    public float GetProcess()
    {
      return Process;
    }
    public virtual ISceneLoadCompBase SetSceneName(string name)
    {
      SceneName = name;
      return this;
    }
    public virtual string GetSceneName()
    {
      return SceneName;
    }

    public abstract IEnumerator LoadScene();

    public abstract IEnumerator UnloadScene();
  }
}