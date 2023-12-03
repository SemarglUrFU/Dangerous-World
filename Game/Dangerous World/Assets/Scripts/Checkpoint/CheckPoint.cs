using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider2D))]
public class CheckPoint : MonoBehaviour, ICheckPoint
{
    [SerializeField] private UnityEvent OnActivate;
    [SerializeField] private UnityEvent OnDeactivate;

    public Vector2 Position => (Vector2) transform.position;

    public void UpdateStatus(bool isActive)
    {
        if (isActive)
        {
            OnActivate.Invoke();
        }
        else
        {
            OnDeactivate.Invoke();
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.TryGetComponent<ICheckPointObserver>(out var observer)) return;
        observer.UpdateActiveCheckpoint(this);
    }
}
