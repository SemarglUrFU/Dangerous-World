using System;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    private int _verticalState = 0;
    private float _moving = 0;


    // public void Jump(bool isStarted) => ;
    // public void Dash(bool isStarted) => ;
    // public void Dead() => 
    // public void Rewive() => 
    public void SetVerticalState(int direction) => _verticalState = direction;
    public void SetMoving(float speed) => _moving = speed;

    private void OnValidate()
    {
        _animator ??= GetComponent<Animator>();
    }
}
