public interface ICheckPointObserver
{
    public ICheckPoint ActiveCheckpoint {get;}
    public void UpdateActiveCheckpoint(ICheckPoint checkPoint);
}
