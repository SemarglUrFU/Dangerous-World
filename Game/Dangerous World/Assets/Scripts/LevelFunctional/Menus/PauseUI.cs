using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class PauseUI : MonoBehaviour, IInGameMenu
{
#if UNITY_EDITOR
    [SerializeField] private SceneAsset _levelMenuAsset;
#endif
    [SerializeField] private AnimationClip _openAnimation;
    [SerializeField] private AnimationClip _closeAnimation;
    [SerializeField] private Animation _animation;
    [SerializeField] private string _levelMenu;

    public Action<IInGameMenu> OnClose { get; set; }
    
    private InputActions _inputActions;

    public void Initialize(InputActions inputActions)
    {
        _inputActions = inputActions;
    }

    [ContextMenu("Open")]
    public void Open()
    {
        _animation.clip = _openAnimation;
        _animation.Play();
        _inputActions.UI.Close.started += Close;
    }

    [ContextMenu("Close")]
    public void Close()
    {
        _inputActions.UI.Close.started -= Close;
        OnClose?.Invoke(this);
        _animation.clip = _closeAnimation;
        _animation.Play();
    }

    public void ReloadLevel()
    {
        LevelLoader.LoadLevel(LevelLoader.Description);
    }

    public void OpenLevelMenu()
    {
        SceneLoader.Load(_levelMenu, SceneLoader.UseTransition.Both, true);
    }

    private void Close(InputAction.CallbackContext ctx) => Close();

    private void OnDestroy()
    {
        if (_inputActions != null) { _inputActions.UI.Close.started -= Close; }
    }

    private void OnValidate()
    {
        if (_animation == null) _animation = GetComponent<Animation>();
#if UNITY_EDITOR
        _levelMenu = _levelMenuAsset.name;
#endif
    }
}
