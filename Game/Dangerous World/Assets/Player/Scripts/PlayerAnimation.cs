using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private Transform _anchor;

    public void Dash(bool isStarted) => _animator.SetBool("Dash", isStarted);
    public void VerticalState(int direction) => _animator.SetInteger("VerticalDirection", direction);
    public void Move(float velocity) => _animator.SetFloat("Move", Mathf.Abs(velocity));
    public void Dead() => _animator.SetBool("Dead", true);
    public void Rewive() => _animator.SetBool("Dead", false);
    public void Rotate(float angle) => _anchor.eulerAngles = new(0, 0, angle);

    private void OnValidate()
    {
        _animator = _animator != null ? _animator : GetComponentInChildren<Animator>();
        _anchor = _anchor != null ? _anchor : transform;
    }
}
