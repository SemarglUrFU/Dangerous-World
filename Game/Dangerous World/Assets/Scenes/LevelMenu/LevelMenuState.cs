using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelMenuState : MonoBehaviour
{
    [SerializeField] private TMP_Text _name;
    [SerializeField] private Image _preview;
    [SerializeField] private GameObject _starsContainer;
    [SerializeField] private Sprite _starsActiveSprite;
    [SerializeField] private Sprite _starsDisableSprite;
    [SerializeField] private GameObject _lock;
    [SerializeField] private TMP_Text _cost;
    [SerializeField] private Button _button;
    [SerializeField] private Image[] _stars = new Image[3];

    public Button Button => _button;

    public void UpdateVisual(string name, Sprite preview, bool unlocked, int stars = 0, int cost = 0)
    {
        _name.text = name;
        _preview.sprite = preview;
        if (unlocked)
        {
            _lock.SetActive(false);
            _button.interactable = true;
            _cost.gameObject.SetActive(false);
            if (stars > 0)
            {
                for (var i = 0; i < _stars.Length; i++)
                    _stars[i].sprite = (i < stars) ? _starsActiveSprite : _starsDisableSprite;
                _starsContainer.SetActive(true);
            }
            else _starsContainer.SetActive(false);
        }
        else
        {
            _lock.SetActive(true);
            _button.interactable = false;
            if (cost > 0)
            {
                _cost.text = cost.ToString();
                _cost.gameObject.SetActive(true);
            }
            else
            {
                _cost.gameObject.SetActive(false);
            }
            _starsContainer.SetActive(false);
        }
    }

    private void OnValidate()
    {
        _button = GetComponent<Button>();
        _stars = _starsContainer.GetComponentsInChildren<Image>();
    }
}
