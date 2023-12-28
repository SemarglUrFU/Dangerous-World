using System;
using UnityEngine;
using UnityEngine.Audio;

public static class Prefs
{
    const string KEY_POINTS = "P_";
    const string SELECTED_LEVEL_KEY = "SL_";
    const string SFX_KEY = "_S";
    const string MUSIC_KEY = "_M";

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

    public static bool SoundEnabled
    {
        get => Repository.ReadInt(SFX_KEY, 1) > 0;
        set => Repository.SaveValue(SFX_KEY, value ? 1 : 0);
    }
    public static bool MusicEnabled
    {
        get => Repository.ReadInt(MUSIC_KEY, 1) > 0;
        set => Repository.SaveValue(MUSIC_KEY, value ? 1 : 0);
    }

    public static void __ResetPoints()
    {
        Repository.RemoveData(KEY_POINTS);
    }
}