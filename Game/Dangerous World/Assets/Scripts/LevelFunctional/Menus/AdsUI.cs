using System;
using InstantGamesBridge;
using InstantGamesBridge.Modules.Advertisement;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class AdsUI : MonoBehaviour, IInGameMenu
{
    public Action<IInGameMenu> OnClose { get; set; }
    public Action OnCloseWithReward { get; set; }
    public Action OnCloseWithNoReward { get; set; }

    [SerializeField] private AnimationClip _openAnimation;
    [SerializeField] private AnimationClip _closeAnimation;
    [SerializeField] private Animation _animation;
    [SerializeField] private Button _btnClose;
    [SerializeField] private Button _btnShowAds;

    private InputActions _inputActions;
    public void Initialize(InputActions inputActions)
    {
        _inputActions = inputActions;
        _btnShowAds.onClick.AddListener(ShowAds);
        _btnClose.onClick.AddListener(Close);
    }

    public void Open()
    {
        _inputActions.UI.Close.started += Close;
        _animation.clip = _openAnimation;
        _animation.Play();
    }

    public void ShowAds()
    {
        Bridge.advertisement.ShowRewarded();
        Bridge.advertisement.rewardedStateChanged += OnRewardedStateChanged;
        _inputActions.UI.Close.started -= Close;
    }

    private void OnAdsLoadError()
    {
        _btnShowAds.interactable = false;
        _btnShowAds.GetComponent<TMP_Text>().text = "Ошибка";
        _inputActions.UI.Close.started += Close;
    }

    private void OnRewardedStateChanged(RewardedState state)
    {
        if (state == RewardedState.Failed) { OnAdsLoadError(); }
        else if (state == RewardedState.Rewarded) { CloseWithReward(); }
    }

    public void Close()
    {
        CloseMenu();
        OnCloseWithNoReward?.Invoke();
        OnClose?.Invoke(this);
    }
    private void Close(InputAction.CallbackContext context) => Close();

    public void CloseWithReward()
    {
        CloseMenu();
        OnCloseWithReward?.Invoke();
    }

    private void CloseMenu()
    {
        _inputActions.UI.Close.started -= Close;
        Bridge.advertisement.rewardedStateChanged -= OnRewardedStateChanged;
        _animation.clip = _closeAnimation;
        _animation.Play();
    }

    private void OnValidate()
    {
        if (_animation == null) _animation = GetComponent<Animation>();
    }
}
