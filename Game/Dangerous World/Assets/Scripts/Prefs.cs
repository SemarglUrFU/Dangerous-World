using System;
using UnityEngine.Audio;

public static class Prefs
{
    const string KEY_POINTS = "_P";
    const string SELECTED_LEVEL_KEY = "_SL";

    public static int Points => Repository.ReadInt(KEY_POINTS);

    public static void AddPoints(int count)
    {
        if (count < 0) throw new Exception("Points cannot be reduced");
        var points = Points + count;
        Repository.SaveValue(KEY_POINTS, points);
    }

    public static int SelectedLevel
    {
        get => Repository.ReadInt(SELECTED_LEVEL_KEY);
        set => Repository.SaveValue(SELECTED_LEVEL_KEY, value);
    }

    public static void AudioMixerInit(AudioMixer audioMixer, string keyMusic, string keySFX)
    {
        _audioMixer = audioMixer;
        _keyMusic = keyMusic;
        _keySFX = keySFX;
    }
    public static bool SoundEnabled
    {
        get => _soundEnabled;
        set { _soundEnabled = value; _audioMixer.SetFloat(_keySFX, value ? 0f : -80f);}
    }
    public static bool MusicEnabled
    {
        get => _musicEnabled;
        set { _musicEnabled = value; _audioMixer.SetFloat(_keyMusic, value ? 0f : -80f);}
    }
    private static bool _soundEnabled = true;
    private static bool _musicEnabled = true;
    private static AudioMixer _audioMixer;
    private static string _keyMusic;
    private static string _keySFX;


    public static void __ResetPoints()
    {
        Repository.RemoveData(KEY_POINTS);
    }
}