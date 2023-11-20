using TMPro;
using UnityEngine;

[RequireComponent(typeof(PlayerMovement))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] PlayerMovement _playerMovement;
    private InputActions _input;

    void Awake(){
        _input = new();
    }

    void Start(){
        _input.Main.Move.started  += (ctx) => _playerMovement.InputMove(ctx.ReadValue<float>());
        _input.Main.Move.canceled += (ctx) => _playerMovement.InputMove(0f);
        _input.Main.Jump.started  += (ctx) => _playerMovement.SetInputJump(true);
        _input.Main.Jump.canceled += (ctx) => _playerMovement.SetInputJump(false);
        _input.Main.Dash.started  += (ctx) => _playerMovement.SetInputDash(true);
        _input.Main.Dash.canceled += (ctx) => _playerMovement.SetInputDash(false);
        _input.Enable();
    }

    void OnValidate(){
        _playerMovement ??= GetComponent<PlayerMovement>();
    }
}
