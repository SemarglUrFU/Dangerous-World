using UnityEngine;

public class LevelBootstrap : MonoBehaviour
{
    [SerializeField] private InGameUI _inGameUI;
    [SerializeField] private int _lifes = 3;
    void Start()
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
    }
}
