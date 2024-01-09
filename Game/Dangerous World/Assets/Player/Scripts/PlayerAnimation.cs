using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private Rigidbody2D _rigidbody;
    [SerializeField] private Transform _anchor;

    public void Dash(bool isStarted) => _animator.SetBool("Dash", isStarted);
    public void VerticalState(int direction) => _animator.SetInteger("VerticalDirection", direction);
    public void Move(float velocity) => _animator.SetFloat("Move", Mathf.Abs(velocity));
    public void Dead()
    {
        _animator.SetBool("Dead", true);
        _rigidbody.constraints = RigidbodyConstraints2D.None;
    }
    public void Rewive()
    {
        _animator.SetBool("Dead", false);
        _rigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    public void Rotate(float angle) => _anchor.eulerAngles = new(0, 0, angle);

    private void OnValidate()
    {
        _rigidbody = _rigidbody != null ? _rigidbody : GetComponent<Rigidbody2D>();
        _animator = _animator != null ? _animator : GetComponentInChildren<Animator>();
        _anchor = _anchor != null ? _anchor : transform;
    }
}
