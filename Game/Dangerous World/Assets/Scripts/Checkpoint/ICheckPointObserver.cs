using UnityEngine;
using UnityEngine.Events;

public interface ICheckPointObserver
{
    public UnityEvent OnNewCheckPoint { get; }
    public ICheckPoint ActiveCheckpoint {get; }
    public Vector2 Position { get; }
    public void UpdateActiveCheckpoint(ICheckPoint checkPoint);
}
