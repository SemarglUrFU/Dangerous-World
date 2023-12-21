public class LevelIngameState
{
    public LevelConfigList ConfigList => _configList;
    public int ConfigListIndex => _configListIndex;
    public LevelConfig LevelConfig => _levelConfig;
    public LevelState LevelState => _levelState;

    private LevelConfigList _configList;
    private int _configListIndex;
    private LevelConfig _levelConfig;
    private LevelState _levelState;


    public LevelIngameState(LevelConfigList configList, int configListIndex, LevelConfig levelConfig = null, LevelState levelState = null)
    {
        _levelConfig = levelConfig != null ? levelConfig : configList.Levels[configListIndex];
        _configList = configList;
        _configListIndex = configListIndex;
        _levelState = levelState ?? LevelRepository.GetState(_levelConfig.Id);
    }

    public void UpdateState(LevelState state)
    {
        _levelState = state;
        LevelRepository.WriteState(_levelConfig.Id, state);
    }
}