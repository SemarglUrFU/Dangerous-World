using System.Collections;
using UnityEngine;
using UnityEngine.Events;

[SelectionBase]
public class ExtraJumpEffector : MonoBehaviour
{
    [SerializeField] private int _addJumps = 1;
    [SerializeField] private float _cooldown = 1f;
    [SerializeField] private UnityEvent _OnInteracted;
    [SerializeField] private UnityEvent _OnActive;

    private bool _enabled = true;
    private Coroutine _coroutine;

    private void Disable()
    {
        _enabled = false;
        _OnInteracted.Invoke();
        _coroutine = StartCoroutine(EnableRoutine());
    }

    private IEnumerator EnableRoutine()
    {
        yield return new WaitForSeconds(_cooldown);
        Enable();
    }

    private void Enable()
    {
        _enabled = true;
        _OnActive.Invoke();
    }

    public void Reset()
    {
        if (_enabled) { return; }
        StopCoroutine(_coroutine);
        Enable();
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (!_enabled || !collider.TryGetComponent<IExtraJumping>(out var entity))
            return;
        entity.ExtraJumpsLeft += _addJumps;
        Disable();
    }


}
