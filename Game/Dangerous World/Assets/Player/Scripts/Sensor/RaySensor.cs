using UnityEngine;

public class RaySensor : Sensor
{
    [SerializeField] private Vector2 _rayOffset;
    [SerializeField] private float _rayDistance;
    [SerializeField] private Vector2 _rayDirection;
    [SerializeField] private LayerMask _layerMask;
    [SerializeField] private bool _followRotation;


    public override bool IsIntersect => _intersectHit.collider;
    public override RaycastHit2D IntersectHit => _intersectHit;

    private RaycastHit2D _intersectHit;
    private Vector2 RayStart => (Vector2) transform.position+_rayOffset;
    private Vector2 RayEnd => RayStart + _rayDirection*_rayDistance;

    private void FixedUpdate()
    {
        var (origin, direction) = _followRotation
            ? ((Vector2)transform.position + Rotate(_rayOffset, transform.rotation.z * 2), Rotate(_rayDirection, transform.rotation.z * 2))
            : ((Vector2)transform.position + _rayOffset, _rayDirection);
        _intersectHit = Physics2D.Raycast(origin, direction, _rayDistance, _layerMask);
    }

    private void OnValidate(){
        _rayDirection = _rayDirection.normalized;
    }

    private void OnDrawGizmos(){
        if (UnityEditor.Selection.activeGameObject != gameObject || !enabled) return;
        Gizmos.color = IsIntersect ? Color.yellow : Color.grey;
        Gizmos.DrawLine(RayStart, RayEnd);
    }
}
