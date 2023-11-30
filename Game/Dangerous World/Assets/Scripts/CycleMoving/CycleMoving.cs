using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class CycleMoving : MonoBehaviour, ICycleMoving
{
    [SerializeField] protected bool _isMoving = true;
    [SerializeField] protected float _velocity = 10f;
    [SerializeField] protected List<Vector2> _points = new(){Vector2.zero};
    [SerializeField] protected CycleType _cycleType;

    public bool IsMoving => _isMoving;

    protected Rigidbody2D _rigidbody2D;
    protected Vector2 _startPosition;
    protected Vector2 _currentTarget;
    protected Vector2 _direction;
    protected enum CycleType : byte { forwardAndBackward, cycle }
    protected CycleIndex cycleIndex;

    void Awake() {_startPosition = _rigidbody2D.position; Reset();}

    public void Reset() {
        cycleIndex = new(_cycleType, _points.Count, 1); 
        SetTarget(_points[cycleIndex.Next]);
    }
    public void ContinueMoving() {_isMoving = true; _rigidbody2D.velocity = _direction * _velocity;}
    public void PauseMoving() {_isMoving = false; _rigidbody2D.velocity = Vector2.zero;}

    void FixedUpdate(){
        if (!_isMoving) return;
        if (!TargetReached()) return;
        SetTarget(_points[cycleIndex.Next]);
    }

    protected bool TargetReached(){
        var position = _rigidbody2D.position;
        if (Vector2.Angle(_direction, _currentTarget - position) < 180)
            return false;
        _rigidbody2D.position = _currentTarget;
        return true;
    }

    protected void SetTarget(Vector2 target){
        _currentTarget = _startPosition + target;
        _direction = (_currentTarget - _rigidbody2D.position).normalized;
        _rigidbody2D.velocity = _direction * _velocity;
    }

    protected class CycleIndex
    {
        public int Next => NextFunction();
        
        public CycleIndex(CycleType cycleType, int count, int current = 0)
        {
            (_current, _count) = (current-1, count);
            NextFunction = cycleType == CycleType.cycle ? Cycle : ForwardAndBackward;
        }

        private readonly Func<int> NextFunction;
        private int _current;
        private readonly int _count;

        private int ForwardAndBackward()
        {
            ++_current;
            if (_current >= _count * 2 - 2) _current = 0;
            else if (_current >= _count) return _count - (_current - _count) - 2;
            return _current;
        }

        private int Cycle()
        {
            if (++_current >= _count)
                _current = 0;
            return _current;
        }
    }


    protected void OnValidate()
    {
        if (_points.Count == 0 || _points[0] != Vector2.zero){
            _points.Insert(0, Vector2.zero);
        }
        _rigidbody2D ??= GetComponent<Rigidbody2D>();
    }

    protected void OnDrawGizmos(){
        if (UnityEditor.Selection.activeGameObject != gameObject || _points.Count == 0) return;
        
        const float radius = 1f;
        Gizmos.color = Color.yellow;

        Gizmos.DrawWireSphere(GetPosition(_points[0]), radius);
        Gizmos.color = Color.green;
        for (var i = 1; i < _points.Count; i++)
        {
            Gizmos.DrawLine(GetPosition(_points[i-1]), GetPosition(_points[i]));
            Gizmos.DrawWireSphere(GetPosition(_points[i]), radius);
        }
        if (_cycleType == CycleType.cycle)
            Gizmos.DrawLine(GetPosition(_points[^1]), GetPosition(_points[0]));

        Vector3 GetPosition(Vector2 point){
            return (Vector3)(point + (!Application.isPlaying ? transform.position : _startPosition));
        }
    }
}
