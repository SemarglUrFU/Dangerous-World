using System;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

[SelectionBase]
[RequireComponent(typeof(Rigidbody2D))]
public class CycleMoving : MonoBehaviour, ICycleMoving
{
    [SerializeField] protected bool _isMoving = true;
    [SerializeField][Range(0.1f, 1000f)] protected float _velocity = 5f;
    [SerializeField][Range(0f, 10000f)] protected float _maxAcceleration = 0f;
    [SerializeField][Range(0.1f, 1000f)] protected float _minimalVelocity = 1f;
    [SerializeField] protected List<Vector2> _points = new() { Vector2.zero };
    [SerializeField] protected CycleType _cycleType;
    [SerializeField] protected Rigidbody2D _rigidbody;

    public bool IsMoving => _isMoving;

    protected Vector2 _startPosition;
    protected Vector2 _currentTarget;
    protected float _segmentLength;
    protected Vector2 _delta;
    protected enum CycleType : byte { PingPong, Cycle }
    protected CycleIndex _cycleIndex;

    void Awake() { _startPosition = _rigidbody.position; Reset(); }

    public void Reset()
    {
        _cycleIndex = new(_cycleType, _points.Count, 1);
        SetTarget(_points[_cycleIndex.Next]);
    }
    public void ContinueMoving() { _isMoving = true; }
    public void PauseMoving() { _isMoving = false; }

    protected void FixedUpdate()
    {
        if (!_isMoving) return;
        if (!TargetReached()) return;
        SetTarget(_points[_cycleIndex.Next]);
    }

    protected bool TargetReached()
    {
        var position = _rigidbody.position;
        var velocity = _velocity;
        if (_maxAcceleration != 0)
        {
            var deltaVelocity = _velocity - _minimalVelocity;
            var _accelerationDistance = deltaVelocity * deltaVelocity / _maxAcceleration;
            var toCurrentTarget = (_currentTarget - position).magnitude;
            var toPreviousTarget = _segmentLength - toCurrentTarget;

            if (toPreviousTarget < _accelerationDistance)
            {
                velocity = Mathf.Lerp(_minimalVelocity, velocity, Mathf.InverseLerp(0, _accelerationDistance, toPreviousTarget));
            }
            else if (toCurrentTarget < _accelerationDistance)
            {
                velocity = Mathf.Lerp(_minimalVelocity, velocity, Mathf.InverseLerp(0, _accelerationDistance, toCurrentTarget));
            }
        }
        var nextPosition = Vector2.MoveTowards(position, _currentTarget, velocity * Time.deltaTime);
        _delta = nextPosition - position;
        if (_delta == Vector2.zero)
            return true;
        _rigidbody.MovePosition(nextPosition);
        return false;
    }

    protected void SetTarget(Vector2 target)
    {
        var newTarget = _startPosition + target;
        _segmentLength = (newTarget - _currentTarget).magnitude;
        _currentTarget = newTarget;
    }

    protected class CycleIndex
    {
        public int Next => NextFunction();

        public CycleIndex(CycleType cycleType, int count, int current = 0)
        {
            (_current, _count) = (current - 1, count);
            NextFunction = cycleType == CycleType.Cycle ? Cycle : ForwardAndBackward;
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
        if (_points.Count == 0 || _points[0] != Vector2.zero) { _points.Insert(0, Vector2.zero); }
        _rigidbody = _rigidbody != null ? _rigidbody : GetComponent<Rigidbody2D>();
        _minimalVelocity = math.min(_minimalVelocity, _velocity);
    }

#if UNITY_EDITOR
    protected void OnDrawGizmos()
    {
        if (UnityEditor.Selection.activeGameObject != gameObject || _points.Count == 0) return;

        const float radius = 1f;
        Gizmos.color = Color.yellow;

        Gizmos.DrawWireSphere(GetPosition(_points[0]), radius);
        Gizmos.color = Color.green;
        for (var i = 1; i < _points.Count; i++)
        {
            Gizmos.DrawLine(GetPosition(_points[i - 1]), GetPosition(_points[i]));
            Gizmos.DrawWireSphere(GetPosition(_points[i]), radius);
        }
        if (_cycleType == CycleType.Cycle)
            Gizmos.DrawLine(GetPosition(_points[^1]), GetPosition(_points[0]));

        Vector3 GetPosition(Vector2 point)
        {
            return (Vector3)(point + (!Application.isPlaying ? transform.position : _startPosition));
        }
    }
#endif
}
