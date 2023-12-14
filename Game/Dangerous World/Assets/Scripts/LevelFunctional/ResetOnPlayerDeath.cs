using UnityEngine;
using UnityEngine.Events;

public class ResetOnPlayerDeath : MonoBehaviour
{
    [SerializeField] private UnityEvent _onReset;

    public UnityEvent OnReset => _onReset;

    private PlayerRevive _playerRevive;
    private ListenerStatus _listenerStatus = ListenerStatus.Disabled;
    private void Start() => _playerRevive = Player.Instance.GetComponent<PlayerRevive>();

    public void AddToResetOnCurrentCheckpoint()
    {
        if (_listenerStatus != ListenerStatus.Disabled) { throw new System.Exception("Listener has already added"); }
        _playerRevive.OnRevive.AddListener(Reset);
        _playerRevive.OnNewCheckPoint.AddListener(RemoveListeners);
        _listenerStatus = ListenerStatus.OnCurrent;
    }

    public void AddToResetOnAnyCheckpoint()
    {
        if (_listenerStatus != ListenerStatus.Disabled) { throw new System.Exception("Listener has already added"); }
        _playerRevive.OnRevive.AddListener(Reset);
        _listenerStatus = ListenerStatus.OnAny;
    }

    public void RemoveListeners()
    {
        _playerRevive.OnRevive.RemoveListener(Reset);
        if (_listenerStatus == ListenerStatus.OnCurrent)
        { _playerRevive.OnNewCheckPoint.RemoveListener(RemoveListeners); }
        _listenerStatus = ListenerStatus.Disabled;
    }

    private void Reset()
    {
        RemoveListeners();
        OnReset.Invoke();
    }

    enum ListenerStatus : byte { Disabled, OnCurrent, OnAny }
}