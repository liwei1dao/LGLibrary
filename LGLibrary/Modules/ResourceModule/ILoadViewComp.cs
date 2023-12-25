namespace LG;
public interface ILoadViewComp : ViewComp, IModelCompBase
{
    void UpdateProgress(float progress, string describe);
}