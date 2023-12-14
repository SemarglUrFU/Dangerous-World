using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(ICheckPointObserver))]
public class PlayerRevive : MonoBehaviour, IDamageble
{
    [SerializeField] private UnityEvent _onDeath;
    [SerializeField] private UnityEvent _onRevive;
    [SerializeField] private bool _invulnerable = false;
    [SerializeField] private GameObject _transitionObject;

    public bool Invulnerable { get => _invulnerable; set => _invulnerable = value; }
    public UnityEvent OnNewCheckPoint => _checkPointObserver.OnNewCheckPoint;
    public UnityEvent OnDeath => _onDeath;
    public  UnityEvent OnRevive => _onRevive;

    private ISceneTransition _transition;
    private ICheckPointObserver _checkPointObserver;

    private void Awake() => _checkPointObserver = GetComponent<ICheckPointObserver>();
    private void Start() => _transition = _transitionObject.GetComponent<ISceneTransition>();

    public bool ApplyDamage()
    {
        if (Invulnerable) {return false;}
        _onDeath.Invoke();
        // TODO DELETE
        Revive();

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
        }
        
    }
}
