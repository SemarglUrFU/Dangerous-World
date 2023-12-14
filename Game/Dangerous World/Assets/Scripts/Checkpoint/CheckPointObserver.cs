using UnityEngine;
using UnityEngine.Events;

public class CheckPointObserver : MonoBehaviour, ICheckPointObserver
{
    public UnityEvent OnNewCheckPoint => _onNewCheckPoint;
    public ICheckPoint ActiveCheckpoint => _activeCheckpoint;
    public Vector2 Position => _activeCheckpoint?.Position ?? _defaultPostion;

    private ICheckPoint _activeCheckpoint;
    private Vector2 _defaultPostion;
    private UnityEvent _onNewCheckPoint = new();

    public void UpdateActiveCheckpoint(ICheckPoint checkPoint)
    {
        if (checkPoint == _activeCheckpoint) return;
        _activeCheckpoint?.UpdateStatus(false);
        _activeCheckpoint = checkPoint;
        checkPoint.UpdateStatus(true);
        _onNewCheckPoint.Invoke();
    }

    private void Start() => _defaultPostion = transform.position;
}
