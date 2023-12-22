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
    public void SetMoving(float speed) => _moving = speed;

    private void OnValidate()
    {
        _animator ??= GetComponent<Animator>();
    }
}
