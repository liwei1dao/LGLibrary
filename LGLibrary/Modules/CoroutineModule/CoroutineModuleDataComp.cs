using System.Collections;

namespace LG;

public class CoroutineModuleDataComp : ModelCompBase<CoroutineModule>
{
    #region 框架构造
    public override void LGLoad(ModuleBase module, params object[] _Agr)
    {
        AllTask = new List<CoroutineTask>();
        base.LGLoad(module);
        base.LoadEnd();
    }
    #endregion
    private List<CoroutineTask> AllTask;

    public CoroutineTask StartCoroutine(IEnumerator coroutine)
    {
        CoroutineTask task = new CoroutineTask(coroutine);
        task.Finished += TaskFinished;
        AllTask.Add(task);
        Module.StartTask(task);
        return task;
    }

    /// <summary>
    ///  任务完成通知
    /// </summary>
    public void TaskFinished(CoroutineTask Task, bool IsFinish)
    {
        AllTask.Remove(Task);
    }

}