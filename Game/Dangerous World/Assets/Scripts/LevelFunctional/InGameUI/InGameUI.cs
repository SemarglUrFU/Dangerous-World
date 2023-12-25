using UnityEngine;

public class InGameUI : MonoBehaviour
{
    [SerializeField] private LifesUI _lifesUI;
    [SerializeField] private CoinsUI _coinsUI;

    public void Initialize(LifeCounter lifeCounter, CoinsCounter coinsCounter)
    {
        _lifesUI.Initialize(lifeCounter.Count, lifeCounter.Left);
        lifeCounter.OnLifesAdd += _lifesUI.Add;
        lifeCounter.OnLifesSpend += _lifesUI.Spend;
        lifeCounter.OnSet += _lifesUI.Set;

        _coinsUI.Initialize(coinsCounter.Total, coinsCounter.Сollected);
        coinsCounter.OnAdd += () => _coinsUI.SetCollected(coinsCounter.Сollected);
        coinsCounter.OnRemove += () => _coinsUI.SetCollected(coinsCounter.Сollected);
    }
}
