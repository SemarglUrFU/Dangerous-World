using UnityEngine;

[SelectionBase]
public class PipeHelper : MonoBehaviour
{
    [Header("To apply the values click 'Apply' in context menu")]
    [SerializeField] private Vector2 _size = new(5f, 5f);
    [SerializeField] private SpriteRenderer _pipeSprite;
    [SerializeField] private SpriteRenderer _effectSprite;
    [SerializeField] private BoxCollider2D _collider;

    [ContextMenu("Apply")]
    private void UpdateSize()
    {
        var xPadding = 0.25f;
        _pipeSprite.size = new(_size.x, 1f);
        _effectSprite.size = new(_size.x - xPadding, _size.y);
        _collider.size = new(_size.x - xPadding, _size.y);
        _collider.offset = new(0, _size.y / 2);
    }
}
