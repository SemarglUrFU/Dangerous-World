using UnityEngine;
using UnityEngine.Events;

class FadeTransition : MonoBehaviour, ISceneTransition
{
    [SerializeField] private UnityEvent _onCurrentAnimationEnded;
    [SerializeField] private Canvas _canvas;
    [SerializeField] private Animator _animator;

    public UnityEvent OnCurrentAnimationEnded => _onCurrentAnimationEnded;
    public GameObject GameObject => gameObject;

    private const string _nameIn = "inTrigger";
    private const string _nameOut = "outTrigger";

    public void Hide() { _canvas.enabled = false; }
    public void PlayIn() { _onCurrentAnimationEnded.RemoveListener(Hide); Play(_nameIn); }
    public void PlayOut() { Play(_nameOut); _onCurrentAnimationEnded.AddListener(Hide); }

    public void OnAnimationEnded()
    {
        _onCurrentAnimationEnded.Invoke();
        _onCurrentAnimationEnded.RemoveAllListeners();
    }

    private void Play(string name)
    {
        _canvas.enabled = true;
        _animator.SetTrigger(name);
    }

    private void OnValidate()
    {
        if (_canvas == null) _canvas = GetComponent<Canvas>();
        if (_animator == null) _animator = GetComponent<Animator>();
    }
}