using System;
using UnityEngine;
using UnityEngine.UI;

public class AudioButtonToggle : MonoBehaviour
{
    [Serializable] enum AudioButtonToggleType { Music, SFX }
    [SerializeField] private AudioButtonToggleType _type;
    [SerializeField] private Sprite _enabledSprite;
    [SerializeField] private Sprite _disabledSprite;
    [SerializeField] private Image _icon;

    private void Start()
    {
        var value = (_type == AudioButtonToggleType.SFX) ? Prefs.SoundEnabled : Prefs.MusicEnabled;
        if (value) { _icon.sprite = _enabledSprite; } else { _icon.sprite = _enabledSprite; }
    }

    public void Toggle()
    {
        var value = !((_type == AudioButtonToggleType.SFX) ? Prefs.SoundEnabled : Prefs.MusicEnabled);
        _icon.sprite = value ? _enabledSprite : _disabledSprite;
        if (_type == AudioButtonToggleType.SFX) { Prefs.SoundEnabled = value; } else { Prefs.MusicEnabled = value; }
    }
}
