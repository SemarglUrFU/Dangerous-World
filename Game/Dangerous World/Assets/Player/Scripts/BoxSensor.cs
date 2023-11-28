using UnityEngine;

public class BoxSensor : Sensor
{
    [SerializeField] private Vector2 _castOffset;
    [SerializeField] private Vector2 _castSize;
    [SerializeField] private Vector2 _castDirection;
    [SerializeField] private float _castDistance;
    [SerializeField] LayerMask _layerMask;

    public override bool IsIntersect => _intersectHit.collider;
    public override RaycastHit2D IntersectHit => _intersectHit;

    private RaycastHit2D _intersectHit;
    private void FixedUpdate()
    {
        _intersectHit = Physics2D.BoxCast((Vector2)transform.position+_castOffset, _castSize, 0, _castDirection, _castDistance, _layerMask);
    }

    private void OnValidate()
    {
        _castDirection = _castDirection.normalized;
    }
    private void OnDrawGizmos()
    {
        if (UnityEditor.Selection.activeGameObject != gameObject) return;
        Gizmos.color = IsIntersect ? Color.yellow : Color.grey;
        Gizmos.DrawWireCube(transform.position + (Vector3)_castOffset, _castSize);
        Gizmos.DrawWireCube(transform.position + (Vector3)_castOffset + (Vector3)(_castDirection * _castDistance), _castSize);
    }
}
