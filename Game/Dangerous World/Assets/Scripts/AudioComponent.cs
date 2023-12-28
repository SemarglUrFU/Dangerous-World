using UnityEngine;
using UnityEngine.Audio;

public class AudioComponent : MonoBehaviour
{
    public static AudioComponent Instance { get; private set; }

    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioMixer _audioMixer;

    const string MUSIC_KEY = "Music";
    const string SFX_KEY = "SFX";

    public void Initialize()
    {
        DontDestroyOnLoad(this);
        MusicEnabled = MusicEnabled;
        SoundEnabled = SoundEnabled;
        Instance = this;
    }

    public bool MusicEnabled
    {
        get { return Prefs.MusicEnabled; }
        set
        {
            if (value){ _audioMixer.SetFloat(MUSIC_KEY, 0); _audioSource.Play();}
            else{ _audioMixer.SetFloat(MUSIC_KEY, -80); _audioSource.Stop();}
            Prefs.MusicEnabled = value;
        }
    }

    public bool SoundEnabled
    {
        get { return Prefs.SoundEnabled; }
        set
        {
            _audioMixer.SetFloat(SFX_KEY, value ? 0 : -80);
            Prefs.SoundEnabled = value;
        }
    }

    private void OnValidate()
    {
        _audioSource = GetComponent<AudioSource>();
    }
}
