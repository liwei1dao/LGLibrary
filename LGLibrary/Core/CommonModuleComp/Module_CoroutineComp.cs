using System.Collections;

namespace LG;

public class Module_CoroutineComp : ModelCompBase
{
    /// <summary>
    /// 模块内协程任务列表
    /// </summary>
    protected List<CoroutineTask> CoroutineTasks;
    public override void LGLoad(ModuleBase _ModelContorl, params object[] _Agr)
    {
        if (CoroutineModule.Instance == null)
        {
            Log.Error("CoroutineModule User but No Load");
            return;
        }
        CoroutineTasks = new List<CoroutineTask>();
        base.LGLoad(_ModelContorl);
        base.LoadEnd();
    }

    public CoroutineTask StartCoroutine(IEnumerator coroutine)
    {
        CoroutineTask task = CoroutineModule.Instance.StartCoroutineTask(coroutine);
        task.Finished += TaskFinshed;
        CoroutineTasks.Add(task);
        return task;
    }

    public void StopCoroutine(CoroutineTask task)
    {
        task.Stop();
    }

    public void StopAllCoroutine()
    {
        for (int i = 1; i < CoroutineTasks.Count; i++)
        {
            StopCoroutine(CoroutineTasks[i]);
        }
    }

    private void TaskFinshed(CoroutineTask task, bool IsFinsh)
    {
        if (CoroutineTasks.Contains(task))
        {
            CoroutineTasks.Remove(task);
        }
    }

}