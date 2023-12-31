using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider2D))]
public class CheckPoint : MonoBehaviour, ICheckPoint
{
    [SerializeField] private UnityEvent OnActivate;
    [SerializeField] private UnityEvent OnReActivate;
    [SerializeField] private UnityEvent OnDeactivate;

    public Vector2 Position => (Vector2)transform.position;

    private bool _isActive = false;

    public void UpdateStatus(bool isActive)
    {
        if (isActive)
        {
            if (_isActive) { OnReActivate.Invoke(); }
            else { OnActivate.Invoke(); }
            _isActive = true;
        }
        else
        {
            OnDeactivate.Invoke();
            _isActive = false;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.TryGetComponent<ICheckPointObserver>(out var observer)) return;
        observer.UpdateActiveCheckpoint(this);
    }
}
