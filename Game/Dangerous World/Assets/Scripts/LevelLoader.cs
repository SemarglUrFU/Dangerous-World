public static class LevelLoader
{
    public static string Id => _levelIngameState?.LevelConfig.Id;
    public static int Number => _levelIngameState?.ConfigListIndex + 1 ?? 0;
    public static LevelIngameState Description => _levelIngameState;

    private static LevelIngameState _levelIngameState;

    public static void LoadLevel(LevelIngameState levelIngameState)
    {
        SceneLoader.Load(levelIngameState.LevelConfig.Id, SceneLoader.UseTransition.Both, true);
        SceneLoader.OnSceneLoad += OnLevelLoad;
        void OnLevelLoad()
        {
            SceneLoader.OnSceneLoad -= OnLevelLoad;
            SceneLoader.OnSceneExit += LevelExit;
            _levelIngameState = levelIngameState;
        }
    }

    public static bool TryGetNextLevel(out LevelIngameState levelIngameState)
    {
        levelIngameState = null;
        if (_levelIngameState == null) { return false; }
        var nextIndex = _levelIngameState.ConfigListIndex + 1;
        if (nextIndex >= _levelIngameState.ConfigList.Count) { return false; }
        var playerScore = Prefs.Points;
        var nextLevelConfig = _levelIngameState.ConfigList.Levels[nextIndex];
        if (playerScore < nextLevelConfig.Cost) { return false; }
        levelIngameState = new (_levelIngameState.ConfigList, nextIndex, nextLevelConfig);
        return true;
    }

    public static void LevelPassed(int points)
    {
        var state = _levelIngameState.LevelState;
        state.Passed = true;
        var deltaPoints = points - state.Points;
        if (deltaPoints > 0)
        {
            state.Points = points;
            Prefs.AddPoints(deltaPoints);
        }
        _levelIngameState.UpdateState(state);
    }

    private static void LevelExit()
    {
        SceneLoader.OnSceneExit -= LevelExit;
        _levelIngameState = null;
    }
}