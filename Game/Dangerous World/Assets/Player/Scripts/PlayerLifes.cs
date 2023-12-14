using UnityEngine;
using UnityEngine.Events;

public class PlayerLifes : MonoBehaviour
{
    [SerializeField] private UnityEvent _onLiveSpent;
    [SerializeField] private UnityEvent _onLivesOver;
    [SerializeField] private int _lifeCount = 3;

    public int LifesCount => _lifeCount;
    public int Lifes => _lifes;

    private int _lifes;

    public void SpentLive()
    {
        _onLiveSpent.Invoke();
        if (--_lifes == 0) { _onLivesOver.Invoke(); }
    }

    private void Awake() => _lifes = _lifeCount;
}
