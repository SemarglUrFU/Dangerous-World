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
    [SerializeField] private AudioSource _audioSource;

    private void Start()
    {
#if UNITY_EDITOR
        if (AudioComponent.Instance == null) { return; }
#endif
        var value = (_type == AudioButtonToggleType.SFX) ? AudioComponent.Instance.SoundEnabled : AudioComponent.Instance.MusicEnabled;
        if (value) { _icon.sprite = _enabledSprite; } else { _icon.sprite = _enabledSprite; }
    }

    public void Toggle()
    {
#if UNITY_EDITOR
        if (AudioComponent.Instance == null) { return; }
#endif
        var value = !((_type == AudioButtonToggleType.SFX) ? AudioComponent.Instance.SoundEnabled : AudioComponent.Instance.MusicEnabled);
        _icon.sprite = value ? _enabledSprite : _disabledSprite;
        if (_type == AudioButtonToggleType.SFX)
        {
            AudioComponent.Instance.SoundEnabled = value;
            if (value) {_audioSource.Play();}
        }
        else
        {
            AudioComponent.Instance.MusicEnabled = value;
            _audioSource.Play();
        }

    }
}
