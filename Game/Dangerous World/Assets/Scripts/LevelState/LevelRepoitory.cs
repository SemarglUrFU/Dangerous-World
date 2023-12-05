public static class LevelRepository
{
    const string KEY_PREFIX = "_L";

    public static LevelState GetState(string id)
    {
        if(Repository.TryReadValue<LevelState>($"{KEY_PREFIX}{id}", out var LevelState))
            return LevelState;
        return new();
    }

    public static void WriteState(string id, LevelState LevelState)
    {
        Repository.SaveValue($"{KEY_PREFIX}{id}", LevelState);
    }
}