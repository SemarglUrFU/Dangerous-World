using UnityEngine;

[RequireComponent(typeof(PlayerMovement))]
public class PlayerInput : MonoBehaviour
{
    [SerializeField] PlayerMovement _playerMovement;
    private InputActions _input;

    private void Awake(){
        _input = new();
    }

    private void Start(){
        _input.Main.Move.started  += (ctx) => _playerMovement.InputMove(ctx.ReadValue<float>());
        _input.Main.Move.canceled += (ctx) => _playerMovement.InputMove(0f);
        _input.Main.Jump.started  += (ctx) => _playerMovement.SetInputJump(true);
        _input.Main.Jump.canceled += (ctx) => _playerMovement.SetInputJump(false);
        _input.Main.Dash.started  += (ctx) => _playerMovement.SetInputDash(true);
        _input.Main.Dash.canceled += (ctx) => _playerMovement.SetInputDash(false);
    }

    private void OnEnable()
    {
        _input.Enable();
    }

    private void OnDisable()
    {
        _input.Main.Disable();
    }

    private void OnValidate(){
        _playerMovement = _playerMovement != null ? _playerMovement : GetComponent<PlayerMovement>();
    }
}
