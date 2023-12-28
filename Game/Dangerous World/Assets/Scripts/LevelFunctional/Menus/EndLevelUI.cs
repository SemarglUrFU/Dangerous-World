using System;
using InstantGamesBridge;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class EndLevelUI : MonoBehaviour, IInGameMenu
{
#if UNITY_EDITOR
    [SerializeField] private SceneAsset _levelMenuAsset;
#endif
    [SerializeField] private TMP_Text _levelNumber;
    [SerializeField] private TMP_Text _header;
    [SerializeField] private TMP_Text _coins;
    [SerializeField] private TMP_Text _stars;
    [SerializeField] private Button _menuBtn;
    [SerializeField] private Button _nextOrRestartBtn;
    [SerializeField] private string _levelMenu;
    [SerializeField] private AdsUI _adsUI;
    [Space]
    [SerializeField] private AnimationClip _openAnimation;
    [SerializeField] private AnimationClip _closeAnimation;
    [SerializeField] private Animation _animation;

    public Action<IInGameMenu> OnClose { get; set; }

    private InputActions _inputActions;
    private LevelIngameState _nextLevelState;
    private LifeCounter _lifeCounter;
    private Action MenuOpenAction;
    private bool _rewardWasUsed = false;

    public void Initialize(InputActions inputActions, AdsUI adsUI, LifeCounter lifeCounter, CoinsCounter coinsCounter)
    {
        _inputActions = inputActions;
        _levelNumber.text = LevelLoader.Number.ToString();
        _menuBtn.onClick.AddListener(OpenLevelMenu);
        _adsUI = adsUI;
        _adsUI.OnCloseWithNoReward += OpenEndLevelMenu;
        _adsUI.OnCloseWithReward += OnLiveReward;
        _lifeCounter = lifeCounter;
    }

    public void Set(bool win, CoinsCounter coinsCounter, int stars)
    {
        _coins.text = $"{coinsCounter.Сollected} / {coinsCounter.Total}";
        MenuOpenAction = OpenEndLevelMenu;
        _stars.text = $"{stars} / 3";
        if (win)
        {
            _header.text = "Пройден";
            _nextOrRestartBtn.GetComponentInChildren<TMP_Text>().text = "Далее";
            _nextOrRestartBtn.onClick.AddListener(LoadNextLevel);
            _nextOrRestartBtn.onClick.RemoveListener(ReloadLevel);
            if (LevelLoader.TryGetNextLevel(out _nextLevelState))
            { _nextOrRestartBtn.interactable = true; }
            else { _nextOrRestartBtn.interactable = false; }
        }
        else
        {
            if (Bridge.advertisement.isBannerSupported && !_rewardWasUsed) { MenuOpenAction = OpenAdsMenu; }
            _header.text = "Провален";
            _nextOrRestartBtn.GetComponentInChildren<TMP_Text>().text = "Заново";
            _nextOrRestartBtn.onClick.RemoveListener(LoadNextLevel);
            _nextOrRestartBtn.onClick.AddListener(ReloadLevel);
        }
    }

    public void Open() => MenuOpenAction();

    private void OpenEndLevelMenu()
    {
        _animation.clip = _openAnimation;
        _animation.Play();
        _inputActions.UI.Close.started += OpenLevelMenu;
    }

    private void OpenAdsMenu()
    {
        _rewardWasUsed = true;
        _adsUI.Open();
    }

    private void OnLiveReward()
    {
        var lifes = Mathf.RoundToInt(_lifeCounter.Count * 0.5f + 0.01f);
        _lifeCounter.Set((int)lifes);
        OnClose?.Invoke(this);
    }

    public void Close()
    {
        _inputActions.UI.Close.started -= OpenLevelMenu;
        OnClose?.Invoke(this);
        _animation.clip = _closeAnimation;
        _animation.Play();
    }

    private void ReloadLevel()
    {
        LevelLoader.LoadLevel(LevelLoader.Description);
    }

    private void OpenLevelMenu()
    {
        SceneLoader.Load(_levelMenu, SceneLoader.UseTransition.Both, true);
    }
    private void OpenLevelMenu(InputAction.CallbackContext ctx) => OpenLevelMenu();

    private void LoadNextLevel()
    {
        LevelLoader.LoadLevel(_nextLevelState);
    }

    private void OnDestroy()
    {
        if (_inputActions != null) { _inputActions.UI.Close.started -= OpenLevelMenu; }
    }

    private void OnValidate()
    {
        if (_animation == null) _animation = GetComponent<Animation>();
#if UNITY_EDITOR
        _levelMenu = _levelMenuAsset.name;
#endif
    }
}
