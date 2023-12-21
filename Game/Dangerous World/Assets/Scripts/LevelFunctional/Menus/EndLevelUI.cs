using System;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class EndLevelUI : MonoBehaviour, IInGameMenu
{
    [SerializeField] private TMP_Text _levelNumber;
    [SerializeField] private TMP_Text _header;
    [SerializeField] private TMP_Text _coins;
    [SerializeField] private TMP_Text _stars;
    [SerializeField] private Button _menuBtn;
    [SerializeField] private Button _nextOrRestartBtn;
    [SerializeField] private SceneAsset _levelMenu;
    [Space]
    [SerializeField] private AnimationClip _openAnimation;
    [SerializeField] private AnimationClip _closeAnimation;
    [SerializeField] private Animation _animation;

    public Action<IInGameMenu> OnClose { get; set; }

    private InputActions _inputActions;
    private LevelIngameState _nextLevelState;

    public void Initialize(InputActions inputActions)
    {
        _inputActions = inputActions;
        _levelNumber.text = LevelLoader.Number.ToString();
        _menuBtn.onClick.AddListener(OpenLevelMenu);
    }

    public void Set(bool win, CoinsCounter coinsCounter, int stars)
    {
        _coins.text = $"{coinsCounter.Сollected} / {coinsCounter.Total}";
        if (win)
        {
            _header.text = "Пройден";
            _stars.text = $"{stars} / 3";
            _nextOrRestartBtn.GetComponentInChildren<TMP_Text>().text = "Далее";
            _nextOrRestartBtn.onClick.AddListener(LoadNextLevel);
            _nextOrRestartBtn.onClick.RemoveListener(ReloadLevel);
            if (LevelLoader.TryGetNextLevel(out _nextLevelState))
            { _nextOrRestartBtn.interactable = true; }
            else { _nextOrRestartBtn.interactable = false; }
        }
        else
        {
            _header.text = "Провален";
            _stars.text = "0 / 3";
            _nextOrRestartBtn.GetComponentInChildren<TMP_Text>().text = "Заново";
            _nextOrRestartBtn.onClick.RemoveListener(LoadNextLevel);
            _nextOrRestartBtn.onClick.AddListener(ReloadLevel);
        }
    }

    [ContextMenu("Open")]
    public void Open()
    {
        _animation.clip = _openAnimation;
        _animation.Play();
        _inputActions.UI.Close.started += OpenLevelMenu;
    }

    [ContextMenu("Close")]
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
        SceneLoader.Load(_levelMenu.name, SceneLoader.UseTransition.Both, true);
    }
    private void OpenLevelMenu(InputAction.CallbackContext ctx) => OpenLevelMenu();

    private void LoadNextLevel()
    {  
        LevelLoader.LoadLevel(_nextLevelState);
    }

    private void OnValidate()
    {
        if (_animation == null) _animation = GetComponent<Animation>();
    }
}
