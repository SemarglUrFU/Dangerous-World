using UnityEngine;

public class CircleSensor : Sensor
{
    [SerializeField] private Vector2 _castOffset;
    [SerializeField] private float _castRadius;
    [SerializeField] private Vector2 _castDirection;
    [SerializeField] private float _castDistance;
    [SerializeField] private LayerMask _layerMask;
    [SerializeField] private bool _followRotation;


    public override bool IsIntersect => _intersectHit.collider;
    public override RaycastHit2D IntersectHit => _intersectHit;

    private RaycastHit2D _intersectHit;
    private void FixedUpdate()
    {
        var (origin, direction) = _followRotation
            ? ((Vector2)transform.position + Rotate(_castOffset, transform.rotation.z*2), Rotate(_castDirection, transform.rotation.z*2))
            : ((Vector2)transform.position + _castOffset, _castDirection);
        _intersectHit = Physics2D.CircleCast(origin, _castRadius, direction, _castDistance, _layerMask);
    }

    private void OnValidate()
    {
        _castDirection = _castDirection.normalized;
    }
    private void OnDrawGizmos(){
        if (UnityEditor.Selection.activeGameObject != gameObject || !enabled) return;
        Gizmos.color = IsIntersect ? Color.yellow : Color.grey;
        Gizmos.DrawWireSphere(transform.position + (Vector3)_castOffset, _castRadius);
        Gizmos.DrawWireSphere(transform.position + (Vector3)_castOffset + (Vector3)(_castDirection*_castDistance), _castRadius);
    }
}
