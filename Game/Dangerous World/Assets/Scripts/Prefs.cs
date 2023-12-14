using System;

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


    public static void __ResetPoints()
    {
        Repository.RemoveData(KEY_POINTS);
    }
}