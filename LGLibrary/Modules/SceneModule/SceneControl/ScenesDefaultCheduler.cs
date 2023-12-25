namespace LG;


public class ScenesDefaultCheduler : IScenesChedulerBase
{
    //private SceneLoadingViewComp LoadingView;

    public void StartLoadChanage()
    {
        //if (LoadingView == null)
        //    LoadingView = SceneModule.Instance.GetLoadingViewComp();
        //LoadingView.Show();
    }

    public void UpdataProgress(float Progress)
    {
        //LoadingView.UpdataProgress(Progress);
    }

    public void EndLoadChanage()
    {
        //LoadingView.Hide();
    }
}