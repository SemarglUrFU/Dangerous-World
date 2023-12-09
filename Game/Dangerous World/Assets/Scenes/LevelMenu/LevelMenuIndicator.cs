using UnityEngine;
using UnityEngine.UI;

class LevelMenuIndicator : MonoBehaviour
{
    [SerializeField] private GameObject _barPrefab;
    [SerializeField] private Color _activeColor;
    [SerializeField] private Color _defaultColor;

    private int _lastSelectedIndex = -1;
    private Image[] _bars;

    public void Initialize(int count)
    {
        _bars = new Image[count];
        for (var i = 0; i < count; i++)
        {
            var instanse = Instantiate(_barPrefab, transform);
            _bars[i] = instanse.GetComponent<Image>();
        }
    }

    public void UpdateSelectedLevel(int index)
    {
        if(_lastSelectedIndex != -1)
            _bars[_lastSelectedIndex].color = _defaultColor;
        _bars[index].color = _activeColor;
        _lastSelectedIndex = index;

    }

    private void OnValidate()
    {
        var color = _barPrefab?.GetComponent<Image>()?.color;
        if (color != null) _defaultColor = color.Value;
    }
}