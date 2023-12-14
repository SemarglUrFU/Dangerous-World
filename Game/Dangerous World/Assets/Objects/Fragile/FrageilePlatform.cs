using System.Collections;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(ResetOnPlayerDeath))]
public class FrageilePlatform : MonoBehaviour
{
    [SerializeField] private UnityEvent _OnReset;
    [SerializeField] private UnityEvent _OnStartBreaking;
    [SerializeField] private UnityEvent _OnBreak;
    [SerializeField] private float _breakTime;

    private Coroutine _coroutine;
    private bool isBreak = false;

    public void StartBreaking()
    {
        _OnStartBreaking.Invoke();
        _coroutine = StartCoroutine(BreakCoroutine());
        isBreak = true;
    }

    private IEnumerator BreakCoroutine()
    {
        yield return new WaitForSeconds(_breakTime);
        _OnBreak.Invoke();
    }

    public void Reset()
    {
        StopCoroutine(_coroutine);
        _OnReset.Invoke();
        isBreak = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!isBreak && collision.collider.TryGetComponent<Player>(out var _))
        { StartBreaking(); }
    }
}
