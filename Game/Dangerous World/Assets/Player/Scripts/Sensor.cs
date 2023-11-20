using UnityEngine;

public abstract class Sensor : MonoBehaviour
{
    public abstract bool IsIntersect { get; }
    public abstract RaycastHit2D IntersectHit { get; }
}
