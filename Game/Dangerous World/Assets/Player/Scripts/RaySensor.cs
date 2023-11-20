using UnityEngine;

public class RaySensor : Sensor
{
    [SerializeField] private Vector2 _rayOffset;
    [SerializeField] private float _rayDistance;
    [SerializeField] private Vector2 _rayDirection;
    [SerializeField] LayerMask _layerMask;

    public override bool IsIntersect => _intersectHit.collider;
    public override RaycastHit2D IntersectHit => _intersectHit;

    private RaycastHit2D _intersectHit;
    private Vector2 RayStart => (Vector2) transform.position+_rayOffset;
    private Vector2 RayEnd => RayStart + _rayDirection*_rayDistance;

    private void FixedUpdate()
    {
        _intersectHit = Physics2D.Raycast(RayStart, _rayDirection, _rayDistance, _layerMask);
    }

    private void OnValidate(){
        _rayDirection = _rayDirection.normalized;
    }

    private void OnDrawGizmos(){
        Gizmos.color = IsIntersect ? Color.yellow : Color.grey;
        Gizmos.DrawLine(RayStart, RayEnd);
    }
}
