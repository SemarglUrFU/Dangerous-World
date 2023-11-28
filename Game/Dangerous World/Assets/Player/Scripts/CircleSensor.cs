using UnityEngine;

public class CircleSensor : Sensor
{
    [SerializeField] private Vector2 _castOffset;
    [SerializeField] private float _castRadius;
    [SerializeField] private Vector2 _castDirection;
    [SerializeField] private float _castDistance;
    [SerializeField] LayerMask _layerMask;

    public override bool IsIntersect => _intersectHit.collider;
    public override RaycastHit2D IntersectHit => _intersectHit;

    private RaycastHit2D _intersectHit;
    private void FixedUpdate(){
        _intersectHit = Physics2D.CircleCast((Vector2)transform.position+_castOffset,_castRadius,_castDirection, _castDistance, _layerMask);
    }

    private void OnValidate()
    {
        _castDirection = _castDirection.normalized;
    }
    private void OnDrawGizmos(){
        if (UnityEditor.Selection.activeGameObject != gameObject) return;
        Gizmos.color = IsIntersect ? Color.yellow : Color.grey;
        Gizmos.DrawWireSphere(transform.position + (Vector3)_castOffset, _castRadius);
        Gizmos.DrawWireSphere(transform.position + (Vector3)_castOffset + (Vector3)(_castDirection*_castDistance), _castRadius);
    }
}
