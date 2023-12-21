using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PauseUI : MonoBehaviour, IInGameMenu
{
    [SerializeField] private AnimationClip _openAnimation;
    [SerializeField] private AnimationClip _closeAnimation;
    [SerializeField] private Animation _animation;
    
    public Action<IInGameMenu> OnClose { get; set; }
    
    private InputActions _inputActions;

    public void Initialize(InputActions inputActions) => _inputActions = inputActions;

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
    private void Close(InputAction.CallbackContext ctx) => Close();

    private void OnValidate()
    {
        if (_animation == null) _animation = GetComponent<Animation>();
    }
}
