using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class ExtraJumpEffector : MonoBehaviour
{
    [SerializeField] private int _addJumps = 1;
    [SerializeField] private float _cooldown = 1f;
    [SerializeField] private UnityEvent _OnActivated;
    [SerializeField] private UnityEvent _OnDisabled;
    [SerializeField] private UnityEvent _OnEnabled;

    private bool _disabled;

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (_disabled || !collider.TryGetComponent<IExtraJumping>(out var entity))
            return;
        entity.ExtraJumpsLeft += _addJumps;
        _disabled = true;
        _OnActivated.Invoke();
        _OnDisabled.Invoke();
        StartCoroutine(EnableRoutine());
    }

    private IEnumerator EnableRoutine()
    {
        yield return new WaitForSeconds(_cooldown);
        _disabled = false;
        _OnEnabled.Invoke();
    }
}
