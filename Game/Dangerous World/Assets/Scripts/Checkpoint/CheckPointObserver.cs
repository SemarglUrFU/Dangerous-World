using UnityEngine;
using UnityEngine.Events;

public class CheckPointObserver : MonoBehaviour, ICheckPointObserver
{
    [SerializeField] private UnityEvent _onNewCheckPoint;
    public UnityEvent OnNewCheckPoint => _onNewCheckPoint;
    public ICheckPoint ActiveCheckpoint => _activeCheckpoint;
    public bool Enabled { get; set; } = true;
    public Vector2 Position => _activeCheckpoint?.Position ?? _defaultPostion;

    private ICheckPoint _activeCheckpoint;
    private Vector2 _defaultPostion;

    public void UpdateActiveCheckpoint(ICheckPoint checkPoint)
    {
        if (!Enabled) { return; }
        if (checkPoint != _activeCheckpoint)
        {
            _activeCheckpoint?.UpdateStatus(false);
            _activeCheckpoint = checkPoint;
        };
        checkPoint.UpdateStatus(true);
        _onNewCheckPoint.Invoke();
    }

    private void Start() => _defaultPostion = transform.position;
}
