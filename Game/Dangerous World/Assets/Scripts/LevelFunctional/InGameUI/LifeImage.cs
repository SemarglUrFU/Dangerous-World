using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class LifeImage : MonoBehaviour
{
    [SerializeField] private Sprite _enableSprite;
    [SerializeField] private Sprite _disableSprite;
    [SerializeField] private bool _enabled = true;
    [SerializeField] private Image _image;

    public void Initialize(bool enabled)
    {
        _enabled = enabled;
        _image.sprite = enabled ? _enableSprite : _disableSprite;
    }

    public void Enable()
    {
        if (_enabled) { return; }
        _enabled = true;
        _image.sprite = _enableSprite;
    }

    public void Disable()
    {
        if (!_enabled) { return; }
        _enabled = false;
        _image.sprite = _disableSprite;
    }

    private void OnValidate()
    {
        _image ??= GetComponent<Image>();
        Initialize(_enabled);
    }
}
