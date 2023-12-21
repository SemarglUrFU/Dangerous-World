using UnityEngine;
using UnityEngine.InputSystem;

public class InGameMenu : MonoBehaviour
{
    [SerializeField] private ISceneTransition _sceneTransition;
    [SerializeField] private PauseUI _pauseUI;
    [SerializeField] private EndLevelUI _endLevelUI;

    public IInGameMenu CurrentMenu => _currentMenu;

    private IInGameMenu _currentMenu;
    private InputActions _inputActions;

    public void Initialize(InputActions inputActions)
    {
        _inputActions = inputActions;
        _inputActions.UI.Close.started += OnEnterPauseMenu;
    }

    public void OpenMenu(IInGameMenu menu)
    {
        if (_currentMenu != null) 
        { 
            _currentMenu.OnClose -= OnMenuClose;
            _currentMenu.Close();
        } 
        else
        {
            _inputActions.UI.Close.started -= OnEnterPauseMenu;
            _sceneTransition.PlayIn();
        }
        _currentMenu = menu;
        menu.Open();
        menu.OnClose += OnMenuClose;
    }

    public void CloseCurrentMenu() => _currentMenu.Close();


    private void OnMenuClose(IInGameMenu menu)
    {
        if (_currentMenu != menu) { return; }
        _currentMenu.OnClose -= OnMenuClose;
        _currentMenu = null;
        _sceneTransition.PlayOut();
        _inputActions.UI.Close.started += OnEnterPauseMenu;
    }

    private void OnEnterPauseMenu(InputAction.CallbackContext ctx) => OpenMenu(_pauseUI);


    private void OnValidate()
    {
        _sceneTransition ??= GetComponentInChildren<ISceneTransition>();
        _pauseUI = _pauseUI != null ? _pauseUI : GetComponentInChildren<PauseUI>();
        _endLevelUI = _endLevelUI != null ? _endLevelUI : GetComponentInChildren<EndLevelUI>();
    }
}
