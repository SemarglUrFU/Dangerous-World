using UnityEngine;

public class LevelBootstrap : MonoBehaviour
{
    [SerializeField] private int _lifes = 3;
    [Space]
    [SerializeField] private InGameUI _inGameUI;
    [SerializeField] private InGameMenu _inGameMenu;
    [SerializeField] private PauseUI _pauseUI;
    [SerializeField] private EndLevelUI _endLevelUI;
    [SerializeField] private LevelFinish[] _levelFinishs;

    private void Start()
    {
        var lifeCounter = new LifeCounter();
        lifeCounter.Initialize(_lifes);
        var playerRevive = Player.Instance.GetComponent<PlayerRevive>();
        playerRevive.Initialize(lifeCounter);

        var coinsCounter = new CoinsCounter();
        coinsCounter.Initialize(FindObjectsOfType<Coin>().Length);
        var coinsCollector = Player.Instance.GetComponent<CoinsCollector>();
        coinsCollector.OnPick += coinsCounter.Add;
        coinsCollector.OnRemove += coinsCounter.Remove;

        _inGameUI.Initialize(lifeCounter, coinsCounter);


        var inputActions = new InputActions();
        _pauseUI.Initialize(inputActions);
        _endLevelUI.Initialize(inputActions);
        _inGameMenu.Initialize(inputActions);
        // TODO _adsMenu Init(livecounter) + bind to revive
        inputActions.UI.Enable();


        foreach (var levelFinish in _levelFinishs)
        {
            levelFinish.Initialize( coinsCounter, _inGameMenu,_endLevelUI);
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
