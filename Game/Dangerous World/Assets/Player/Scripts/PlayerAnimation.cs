using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private Transform _anchor;
    private int _verticalState = 0;
    private float _moving = 0;


    // public void Jump(bool isStarted) => ;
    // public void Dash(bool isStarted) => ;
    // public void Dead() => 
    // public void Rewive() => 
    public void Rotate(float angle) => _anchor.eulerAngles = new(0, 0, angle);
    public void SetVerticalState(int direction) => _verticalState = direction;
    public void SetMoving(float speed) 
    {
        if (speed != 0)
            _animator.SetBool("stop", false);
        else
            _animator.SetBool("stop", true);
        _moving = speed; 
    }

    private void OnValidate()
    {
        _animator = _animator != null ? _animator : GetComponentInChildren<Animator>();
        _anchor = _anchor != null ? _anchor : transform;
    }
}
