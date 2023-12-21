using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(ICheckPointObserver))]
public class PlayerRevive : MonoBehaviour, IDamageble
{
    [SerializeField] private UnityEvent _onDeath;
    [SerializeField] private UnityEvent _onRevive;

    public bool Invulnerable { get; set ; } = false;
    public bool Dead { get; set; } = false;
    public UnityEvent OnNewCheckPoint => _checkPointObserver.OnNewCheckPoint;
    public UnityEvent OnDeath => _onDeath;
    public UnityEvent OnRevive => _onRevive;

    private ISceneTransition _transition;
    private ICheckPointObserver _checkPointObserver;
    private LifeCounter _lifeCounter;

    public void Initialize(LifeCounter lifeCounter) {_lifeCounter = lifeCounter;}

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

    [ContextMenu("Validate")]
    private void OnValidate()
    {
        _checkPointObserver = GetComponent<ICheckPointObserver>();
        _transition = GetComponentInChildren<ISceneTransition>();
    }
}
