using UnityEngine;

[SelectionBase]
public class PlatformEffector : MonoBehaviour
{
#if UNITY_EDITOR
    [SerializeField] private bool _flip;

    private void OnValidate()
    {
        var _surfaceEffector = GetComponentInChildren<SurfaceEffector2D>();
        var _spriteRenderer = GetComponentInChildren<SpriteRenderer>();

        var speed = _surfaceEffector.speed;
        if (_flip)
        { if (speed > 0) { _surfaceEffector.speed = -speed; } }
        else
        { if (speed < 0) { _surfaceEffector.speed = -speed; } }

        _spriteRenderer.flipX = _flip;
    }
#endif
}