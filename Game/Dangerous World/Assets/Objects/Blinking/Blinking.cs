using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

[SelectionBase]
public class Blinking : MonoBehaviour
{
    [SerializeField] private List<UnityEvent> _phaseEvent = new(0);
    [SerializeField] private List<float> _phaseTime = new(0);
    [SerializeField] private int _initialPhase;

    private int _phase;
    private bool _isRunning;
    private Coroutine _coroutine;

    public void Run()
    {
        if (_isRunning) { return; }
        _phase = _initialPhase;
        _isRunning = true;
        _coroutine = StartCoroutine(Coroutine());
    }

    public void ForceStop()
    {
        _isRunning = false;
        StopCoroutine(_coroutine);
    }

    public void Stop()
    {
        _isRunning = false;
    }

    private IEnumerator Coroutine()
    {
        while (_isRunning)
        {
            for (; _phase < _phaseEvent.Count; _phase++)
            {
                _phaseEvent[_phase].Invoke();
                yield return new WaitForSeconds(_phaseTime[_phase]);
            }
            _phase = 0;
        }
    }

    private void Start() => Run();

    private void OnValidate()
    {
        if (_phaseTime.Count > _phaseEvent.Count)
        { _phaseTime = new(_phaseTime.Take(_phaseEvent.Count)); }
        else if (_phaseTime.Count < _phaseEvent.Count)
        { _phaseTime.AddRange(Enumerable.Repeat(0f, _phaseEvent.Count - _phaseTime.Count)); }
        for (var i = 0; i < _phaseTime.Count; i++) { if (_phaseTime[i] < 0f) { _phaseTime[i] = 0f; } }
        _initialPhase = Math.Max(0, Math.Min(_initialPhase, _phaseTime.Count - 1));
        _phaseEvent.TrimExcess();
        _phaseTime.TrimExcess();
    }
}
