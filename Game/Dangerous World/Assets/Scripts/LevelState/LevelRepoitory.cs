public static class LevelRepository
{
    const string KEY_PREFIX = "_L";

    public static LevelState GetState(string id)
    {
        if(Repository.TryReadValue<LevelState>(GetKey(id), out var LevelState))
            return LevelState;
        return new();
    }

    public static void WriteState(string id, LevelState levelState)
    {
        Repository.SaveValue(GetKey(id), levelState);
    }

    public static void Remove(string id) => Repository.RemoveData(GetKey(id));

    private static string GetKey(string id) => $"{KEY_PREFIX}{id}";
}