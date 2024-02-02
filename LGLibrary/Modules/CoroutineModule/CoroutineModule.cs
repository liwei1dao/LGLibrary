using System.Collections;

namespace LG
{

    public class CoroutineModule : ModuleBase<CoroutineModule>
    {
        private CoroutineModuleDataComp DataComp;
        public override void LGLoad(params object[] agrs)
        {
            DataComp = AddComp<CoroutineModuleDataComp>();
            base.LGLoad(agrs);
        }

        public CoroutineTask StartCoroutineTask(IEnumerator coroutine)
        {
            return DataComp.StartCoroutine(coroutine);
        }
        public void StartTask(CoroutineTask Task)
        {
            ModuleManager.Instance.StartCoroutine(Task.CallWrapper());
        }
    }
}