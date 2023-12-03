using UnityEngine;

public class CheckPointObserver : MonoBehaviour, ICheckPointObserver
{
    public ICheckPoint ActiveCheckpoint {get => _activeCheckpoint; }
    private ICheckPoint _activeCheckpoint;
    public void UpdateActiveCheckpoint(ICheckPoint checkPoint)
    {
        if (checkPoint == _activeCheckpoint) return;
        _activeCheckpoint?.UpdateStatus(false);
        _activeCheckpoint = checkPoint;
        checkPoint.UpdateStatus(true);
    }
}
