using UnityEngine;
using UnityEngine.Events;

[SelectionBase]
[RequireComponent(typeof(Collider2D))]
public class Coin : MonoBehaviour
{
    [SerializeField] private UnityEvent _onPick;
    [SerializeField] private UnityEvent _onReset;
    [SerializeField] private Collider2D _trigger;

    private CoinsCollector _coinsCollector;

    public void Pick(CoinsCollector coinCollector)
    {
        _coinsCollector = coinCollector;
        _coinsCollector.Pick();
        _trigger.enabled = false;
        _onPick.Invoke();
    }

    public void Reset()
    {
        _onReset.Invoke();
        _trigger.enabled = true;
        _coinsCollector.Remove();
        _coinsCollector = null;
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (!_coinsCollector && collider.TryGetComponent<CoinsCollector>(out var coinCollector)) { Pick(coinCollector); }
    }

    private void OnValidate()
    {
        _trigger = _trigger != null ? _trigger : GetComponent<Collider2D>();
        if (_trigger.isTrigger == false) { Debug.LogError("Coin collader must be trigger"); }
    }
}