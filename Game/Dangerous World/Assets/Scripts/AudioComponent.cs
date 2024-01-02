using UnityEngine;
using UnityEngine.Audio;

public class AudioComponent : MonoBehaviour
{
    public static AudioComponent Instance { get; private set; }

    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioMixer _audioMixer;

    const string MASTER_KEY = "Master";
    const string MUSIC_KEY = "Music";
    const string SFX_KEY = "SFX";

    public void Initialize()
    {
        DontDestroyOnLoad(this);
        _audioSource.Play();
        MusicEnabled = MusicEnabled;
        SoundEnabled = SoundEnabled;
        Instance = this;
    }

    public bool MusicEnabled
    {
        get { return Prefs.MusicEnabled; }
        set
        {
            if (value){ _audioMixer.SetFloat(MUSIC_KEY, 0); _audioSource.UnPause();}
            else{ _audioMixer.SetFloat(MUSIC_KEY, -80); _audioSource.Pause();}
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

    public bool Mute{set => _audioMixer.SetFloat(MASTER_KEY, value ? -80f : 0f);}

    private void OnValidate()
    {
        _audioSource = GetComponent<AudioSource>();
    }
}
