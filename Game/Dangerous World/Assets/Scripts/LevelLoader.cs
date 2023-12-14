using System;

public static class LevelLoader
{
    public static string Id => _levelIngameState?._levelConfig.Id;
    public static int? Number => _levelIngameState?._configListIndex + 1;
    public static LevelIngameState State => _levelIngameState;

    private static LevelIngameState _levelIngameState;

    public static void LoadLevel(LevelIngameState levelIngameState)
    {
        _levelIngameState = levelIngameState;
        SceneLoader.Load(_levelIngameState._levelConfig.Scene, SceneLoader.UseTransition.Both, true);
        SceneLoader.OnSceneExit += LevelExit;
    }

    public static void LevelPassed(int points)
    {
        // TODO: Move to another class
        var state = LevelRepository.GetState(Id);
        state.Passed = true;
        state.Points = points;
        LevelRepository.WriteState(Id, state);
    }

    private static void LevelExit()
    {
        SceneLoader.OnSceneExit -= LevelExit;
        _levelIngameState = null;
    }
}