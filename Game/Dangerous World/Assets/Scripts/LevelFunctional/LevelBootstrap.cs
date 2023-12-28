using UnityEngine;

public class LevelBootstrap : MonoBehaviour
{
    [SerializeField] private InGameUI _inGameUI;
    [SerializeField] private InGameMenu _inGameMenu;
    [SerializeField] private FadeTransition _inGameMenuTransition;
    [SerializeField] private PauseUI _pauseUI;
    [SerializeField] private EndLevelUI _endLevelUI;
    [SerializeField] private AdsUI _adsUI;
    [SerializeField] private LevelFinish[] _levelFinishs;

    private void Start()
    {
        var inputActions = new InputActions();
        Player.Instance.GetComponent<PlayerInput>().Initialize(inputActions);

#if UNITY_EDITOR
        var lifesCount = LevelLoader.Description?.LevelConfig.Lifes ?? 12;
#else
        var lifesCount = LevelLoader.Description.LevelConfig.Lifes;
#endif
        var lifeCounter = new LifeCounter();
        lifeCounter.Initialize(lifesCount);
        var playerRevive = Player.Instance.GetComponent<PlayerRevive>();
        playerRevive.Initialize(lifeCounter);

        var coinsCounter = new CoinsCounter();
        coinsCounter.Initialize(FindObjectsOfType<Coin>().Length);
        var coinsCollector = Player.Instance.GetComponent<CoinsCollector>();
        coinsCollector.OnPick += coinsCounter.Add;
        coinsCollector.OnRemove += coinsCounter.Remove;

        _inGameUI.Initialize(lifeCounter, coinsCounter);

        _pauseUI.Initialize(inputActions);
        _endLevelUI.Initialize(inputActions, _adsUI, lifeCounter, coinsCounter);
        _inGameMenu.Initialize(inputActions, _inGameMenuTransition);
        _adsUI.Initialize(inputActions);
        lifeCounter.OnLifesOver += () =>
        {
#if UNITY_EDITOR
            if (LevelLoader.Number == 0) _endLevelUI.Set(false, coinsCounter, -1);
            else _endLevelUI.Set(false, coinsCounter, LevelLoader.Description.LevelState.Points);
#else
            _endLevelUI.Set(false, coinsCounter, LevelLoader.Description.LevelState.Points);
#endif
            _inGameMenu.OpenMenu(_endLevelUI);
        };
        inputActions.UI.Enable();


        foreach (var levelFinish in _levelFinishs)
        {
            levelFinish.Initialize(coinsCounter, _inGameMenu, _endLevelUI);
        }


        Destroy(this);
    }

    [ContextMenu("Validate")]
    private void OnValidate()
    {
        if (_inGameUI == null) _inGameUI = GetComponentInChildren<InGameUI>();
        if (_inGameMenu == null) _inGameMenu = GetComponentInChildren<InGameMenu>();
        if (_pauseUI == null) _pauseUI = GetComponentInChildren<PauseUI>();
        if (_endLevelUI == null) _endLevelUI = GetComponentInChildren<EndLevelUI>();
        _levelFinishs = FindObjectsOfType<LevelFinish>();
    }
}
