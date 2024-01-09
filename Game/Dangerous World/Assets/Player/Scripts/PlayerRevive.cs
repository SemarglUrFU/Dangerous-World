using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(ICheckPointObserver))]
public class PlayerRevive : MonoBehaviour, IDamageble
{
    [SerializeField] private UnityEvent _onDeath;
    [SerializeField] private UnityEvent _onRevive;
    [SerializeField] private FadeTransition _transition;
    [SerializeField] private CheckPointObserver _checkPointObserver;

    public bool Invulnerable { get; set ; } = false;
    public bool Dead { get; set; } = false;
    public UnityEvent OnNewCheckPoint => _checkPointObserver.OnNewCheckPoint;
    public UnityEvent OnDeath => _onDeath;
    public UnityEvent OnRevive => _onRevive;

    private LifeCounter _lifeCounter;

    public void Initialize(LifeCounter lifeCounter) 
    {
        _lifeCounter = lifeCounter;
        _lifeCounter.OnSet += OnSetLifes;
    }

    public bool ApplyDamage()
    {
        if (Dead || Invulnerable) {return false;}
        Dead = true;
        _onDeath.Invoke();
        if (_lifeCounter.Spend()) { Revive(); }
        return true;
    }

    public void Revive()
    {
        _transition.PlayIn();
        _transition.OnCurrentAnimationEnded.AddListener(ContinueRevive);

        void ContinueRevive(){
            _onRevive.Invoke();
            transform.position = _checkPointObserver.Position;
            _transition.PlayOut();
            Dead = false;
        }
    }

    private void OnSetLifes(int count)
    {
        if (count > 0 && Dead)
        {
            Revive();
        }
    }

    [ContextMenu("Validate")]
    private void OnValidate()
    {
        _checkPointObserver = GetComponent<CheckPointObserver>();
        _transition = GetComponentInChildren<FadeTransition>();
    }
}
