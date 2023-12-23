using UnityEngine;

public abstract class Sensor : MonoBehaviour
{
    public abstract bool IsIntersect { get; }
    public abstract RaycastHit2D IntersectHit { get; }
    protected Vector2 Rotate(Vector2 vector, float angle)
    {
        return new Vector2
        {
            x = vector.x * Mathf.Cos(angle) - vector.y * Mathf.Sin(angle),
            y = vector.x * Mathf.Sin(angle) + vector.y * Mathf.Cos(angle)
        };
    }
}
