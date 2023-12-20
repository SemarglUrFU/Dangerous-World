using UnityEngine;

public class PlatformEffector : MonoBehaviour
{
    [SerializeField] private bool _flip;

    private void OnValidate()
    {
        var _surfaceEffector = GetComponent<SurfaceEffector2D>();
        var _spriteRenderer = GetComponent<SpriteRenderer>();

        var speed = _surfaceEffector.speed;
        if (_flip)
        { if (speed > 0) { _surfaceEffector.speed = -speed; } }
        else
        { if (speed < 0) { _surfaceEffector.speed = -speed; } }

        _spriteRenderer.flipX = _flip;
    }
}