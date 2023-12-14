public class LevelIngameState
{
    public readonly LevelConfigList _configList;
    public readonly int _configListIndex;
    public readonly LevelState _levelState;
    public readonly LevelConfig _levelConfig;


    public LevelIngameState(LevelConfigList configList, int configListIndex, LevelConfig levelConfig = null, LevelState levelState = null)
    {
        levelConfig ??= configList.Levels[configListIndex];
        levelState ??= LevelRepository.GetState(levelConfig.Id);
        _configList = configList;
        _configListIndex = configListIndex;
        _levelState = levelState;
        _levelConfig = levelConfig;
    }
}