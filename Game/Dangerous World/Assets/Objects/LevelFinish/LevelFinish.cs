using System;
using UnityEngine;
using UnityEngine.Events;

[SelectionBase]
[RequireComponent(typeof(Collider2D))]
public class LevelFinish : MonoBehaviour
{
    [SerializeField] private UnityEvent OnTriggered;

    [SerializeField] private Collider2D _collider;
    private CoinsCounter _coinsCounter;
    private InGameMenu _inGameMenu;
    private EndLevelUI _endLevel;

    public static int GetCurrentStars(CoinsCounter coinsCounter) => GetStars(coinsCounter.Сollected, coinsCounter.Total);

    public void Initialize(CoinsCounter coinsCounter, InGameMenu inGameMenu, EndLevelUI endLevel)
    {
        _coinsCounter = coinsCounter;
        _inGameMenu = inGameMenu;
        _endLevel = endLevel;
    }

    public void FinishTriggered()
    {
        var stars = GetCurrentStars(_coinsCounter);
        LevelLoader.LevelPassed(stars);
        _endLevel.Set(true, _coinsCounter, stars);
        _inGameMenu.OpenMenu(_endLevel);
    }

    private static int GetStars(int collectedCoins, int totalCoins)
    {
        if (totalCoins == 0) {return 3;}
        var starCost = totalCoins / 3f;
        return (int)MathF.Ceiling(collectedCoins / starCost);
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.TryGetComponent<PlayerRevive>(out var playerRevive)
            && !playerRevive.Dead && (System.Object)_inGameMenu.CurrentMenu != _endLevel)
        {
            _collider.enabled = false;
            playerRevive.Invulnerable = true;
            OnTriggered.Invoke();
            FinishTriggered();
        }
    }
}
