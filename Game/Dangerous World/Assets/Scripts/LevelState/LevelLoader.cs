using System;
using UnityEngine.SceneManagement;

public static class LevelLoader
{
    public static string Id => _levelIngameState?._levelConfig.Id;
    public static int? Number => _levelIngameState?._configListIndex + 1;
    public static LevelIngameState State => _levelIngameState;

    private static LevelIngameState _levelIngameState;

    public static void LoadLevel(LevelIngameState levelIngameState)
    {
        _levelIngameState = levelIngameState;
        SceneManager.LoadScene(levelIngameState._levelConfig.Scene);
    }

    public static void OnLevelExit()
    {
        _levelIngameState = null;
    }

    public static void OnLevelPassed(int points)
    {
        if (points < 1) throw new Exception("Passed level must has at least 1 point");
        var state = LevelRepository.GetState(Id);
        state.Points = points;
        LevelRepository.WriteState(Id, state);
    }
}