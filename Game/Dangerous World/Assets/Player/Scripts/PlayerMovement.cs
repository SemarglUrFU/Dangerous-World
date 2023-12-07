using System;
using Unity.Mathematics;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour, IExtraJumping
{
    [SerializeField] private Rigidbody2D _rigidbody;
    [SerializeField] private Sensor _groundSensor;
    [SerializeField] private Sensor _leftSensor;
    [SerializeField] private Sensor _rightSensor;
    [SerializeField] private Sensor _topSensor;

    [SerializeField] private Transform _visualAchor;    
    [SerializeField] private PhysicsMaterial2D maxFriction;
    [SerializeField] private PhysicsMaterial2D minFriction;

    private float _defaultGravityScale;

    private void Awake()
    {
        SetRotation(_facingRight);
        JumpReset();
        _defaultGravityScale = _rigidbody.gravityScale;
    }

    private void FixedUpdate()
    {
        UpdateRotation();
        HandleGrounding();
        HandleMove();
        HandleJump();
        HandleDashing();
    }

    #region Rotation
    [SerializeField] private bool _facingRight;
    private bool _previousFacingRight;
    private bool UpdateRotation()
    {
        if (_previousFacingRight == _facingRight)
            return false;
        _previousFacingRight = _facingRight;
        SetRotation(_facingRight);
        return true;
    }
    private bool GetRotationByDirection(float direction) => direction == 0 ? _facingRight : direction > 0;
    private void SetRotation(bool facingRight)
    {
        _facingRight = facingRight;
        var localScale = _visualAchor.localScale;
        localScale.x = facingRight ? 1f : -1f;
        _visualAchor.localScale = localScale;
    }
    #endregion Rotation

    #region Grounding
    private float _lastGroundTime;
    private float _groundAngle;
    private int _nearWall = 0;
    private bool _wasGrounded;
    private Vector2 _movingPlatformVelocity;

    private void HandleGrounding()
    {
        if (_groundSensor.IsIntersect)
        {
            _lastGroundTime = Time.time;
            _groundAngle = Vector2.Angle(_groundSensor.IntersectHit.normal, Vector2.up);

            if (_groundSensor.IntersectHit.collider.TryGetComponent<IMovingPlatform>(out var movingPlatform))
                _movingPlatformVelocity = movingPlatform.Velocity;
            else
                _movingPlatformVelocity = Vector2.zero;

            if (!_wasGrounded)
            {
                _wasGrounded = true;
                // TODO Event
            }
        } 
        else if (_wasGrounded)
        {
            _movingPlatformVelocity = Vector2.zero;
            _wasGrounded = false;
        }

        if (_leftSensor.IsIntersect || _rightSensor.IsIntersect)
        {
            _nearWall = _leftSensor.IsIntersect ? -1 : 1;
        }
        else
        {
            _nearWall = 0;
        }
    }
    #endregion Grounding

    #region Walking
    [Header("Walking")]
    [SerializeField] private float _walkSpeed = 5;
    [SerializeField] private float _slideSpeed = 5;
    [SerializeField] private float _airSpeed = 10;
    [SerializeField] private float _acceleration = 50;
    [SerializeField] private float _decceleration = 100;
    [SerializeField] private float _airAcceleration = 20;
    [SerializeField] private float _airDeceleration = 5;
    [SerializeField][Range(1, 89)] private float _maxSurfaceAngle = 45;
    [SerializeField][Range(1, 89)] private float _slideSurfaceAngle = 45;
    private float _wallSurfaceAngle = 90;
    private float _wallJumpLockUntil = float.MinValue;

    private void HandleMove()
    {
        if (_isDashing || _wallJumpLockUntil > Time.time) return;

        var direction = math.sign(_input.move);
        var velocity = _rigidbody.velocity;

        _facingRight = GetRotationByDirection(direction);

        _rigidbody.sharedMaterial = minFriction;
        if (_groundSensor.IsIntersect)
        {
            var groundNormal = _groundSensor.IntersectHit.normal;
            if (direction != 0)
            {
                // Walk
                var angle = Vector2.Angle(new(direction, 0), groundNormal) - 90;
                if (angle <= _maxSurfaceAngle)
                {
                    var alongGround = new Vector2(groundNormal.y, -groundNormal.x);
                    var targetVelocity = _input.move * _walkSpeed * alongGround + _movingPlatformVelocity;
                    var acceleration = _acceleration * (1 + angle / _maxSurfaceAngle);
                    velocity = Vector2.MoveTowards(velocity, targetVelocity, acceleration * Time.deltaTime);
                }
            }
            else
            {
                // Deccelerate
                if (_groundAngle < _maxSurfaceAngle){
                    velocity = Vector2.MoveTowards(velocity, _movingPlatformVelocity, _decceleration * Time.deltaTime);
                    if ((velocity-_movingPlatformVelocity).magnitude < 0.1)
                        _rigidbody.sharedMaterial = maxFriction;
                }
            }
            // Slide
            if (_groundAngle > _slideSurfaceAngle && _groundAngle < _wallSurfaceAngle)
            {
                var slidingDirection = math.sign(groundNormal.x);
                var alongGround = new Vector2(groundNormal.y, -groundNormal.x);
                velocity = Vector2.MoveTowards(velocity, _slideSpeed * slidingDirection * alongGround + _movingPlatformVelocity, _acceleration * Time.deltaTime);
            }
        }
        else
        {
            _rigidbody.sharedMaterial = minFriction;
            var targetXVelocity = _airSpeed * _input.move;
            var acceleration = _input.move != 0 ? _airAcceleration : _airDeceleration;
            velocity.x = Mathf.MoveTowards(velocity.x, targetXVelocity, acceleration * Time.deltaTime);
        }
        _rigidbody.velocity = velocity;
    }
    #endregion Walking

    #region Jumping
    [Header("Jumping")]
    [SerializeField] private float _jumpTime = 1f;
    [SerializeField] private Vector2 _jumpVelocity = new(10f, 15f);
    [SerializeField] private float _jumpCutoffTime = 0.2f;
    [SerializeField] private float _coyoteTime = 0.1f;
    [SerializeField] private int _extraJumps = 1;

    public int ExtraJumpsLeft {get => _extraJumpsLeft; set => _extraJumpsLeft=value;}
    private int _extraJumpsLeft;

    private enum JumpState : byte
    {
        none, started, ended
    }
    private JumpState _jumpState = JumpState.ended;
    private bool _wasReleased;
    private float _jumpStartTime = float.MinValue;

    private void HandleJump()
    {
        if (_isDashing && _jumpState != JumpState.ended)
            ForceStopJump();
        
        if (_jumpState == JumpState.ended && _groundSensor.IsIntersect)
            JumpReset();

        if (_jumpState == JumpState.started)
        {
            if ((_wasReleased && !(_jumpStartTime + _jumpCutoffTime > Time.time)) || 
                (_topSensor.IsIntersect && !_topSensor.IntersectHit.collider.TryGetComponent<PlatformEffector2D>(out var _)))
                _jumpState = JumpState.ended;
            ContinueJump();
        }
    }
    private void StartJump()
    {
        _wasReleased = false;
        if (_jumpState != JumpState.started)
        {
            if (_jumpState == JumpState.none && _lastGroundTime + _coyoteTime > Time.time)
            {
                if (_groundAngle > _slideSurfaceAngle)
                {      
                    const float pulseVelocity = 15f;   
                    var velocity = _rigidbody.velocity;
                    var pulse = MathF.Sign(_groundSensor.IntersectHit.normal.x) * pulseVelocity;
                    velocity.x = pulse > 0 ? Mathf.Max(velocity.x, pulse) : Mathf.Min(velocity.x, pulse);
                    _rigidbody.velocity = velocity;
                }
                _jumpState = JumpState.started;
                _jumpStartTime = Time.time;
            }
            else if (_nearWall != 0){
                JumpReset();
                _jumpState = JumpState.started;
                _jumpStartTime = Time.time;
                var velocity = _rigidbody.velocity;
                velocity.x = -_nearWall * _jumpVelocity.x;
                _rigidbody.velocity = velocity;
                _wallJumpLockUntil = Time.time + _jumpCutoffTime;
                _facingRight = GetRotationByDirection(-_nearWall);
            }
            else if (_extraJumpsLeft > 0)
            {
                var inputDirection = math.sign(_input.move);
                var rigidbodyVelocity = _rigidbody.velocity;
                if (inputDirection != 0)
                    _rigidbody.velocity = new(_jumpVelocity.x*inputDirection, rigidbodyVelocity.y);

                _extraJumpsLeft--;
                _jumpState = JumpState.started;
                _jumpStartTime = Time.time;
            }
        }
    }
    private void StopJump()
    {
        _wasReleased = true;
    }
    private void ForceStopJump()
    {
        StopJump();
        _jumpState = JumpState.ended;
    }
    private void ContinueJump()
    {
        var velocity = _rigidbody.velocity;
        velocity.y = _jumpVelocity.y;
        _rigidbody.velocity = velocity - _rigidbody.gravityScale * Time.deltaTime * Physics2D.gravity;
        if (_jumpStartTime + _jumpTime < Time.time)
        {
            _jumpState = JumpState.ended;
        }
    }
    private void JumpReset()
    {
        _extraJumpsLeft = _extraJumps;
        _jumpState = JumpState.none;
    }
    #endregion Jumping

    #region Dashing
    [Header("Dash")]
    [SerializeField] private float _dashVelocity = 7f;
    [SerializeField] private float _dashTime = 0.2f;
    [SerializeField] private float _dashCooldown = 1f;
    private bool _isDashing;
    private float _dashStartTime = float.MinValue;
    private float _dashEndTime = float.MinValue;
    private int _dashDirection;
    
    private void HandleDashing()
    {
        if (_isDashing)
            ContinueDash();
    }
    private void StartDash()
    {
        if (!_isDashing && _dashEndTime + _dashCooldown >= Time.time)
            return;
        _dashStartTime = Time.time;
        _dashDirection = _nearWall != 0
            ? -_nearWall : _facingRight ? 1 : -1;
        _rigidbody.gravityScale = 0;
        _rigidbody.sharedMaterial = minFriction;
        _facingRight = GetRotationByDirection(_dashDirection);
        _isDashing = true;
    }
    private void StopDash()
    {
        _rigidbody.velocity = new Vector2(_dashDirection*_airSpeed, 0);
        _rigidbody.gravityScale = _defaultGravityScale;
        _dashEndTime = Time.time;
        _isDashing = false;
    }
    private void ContinueDash()
    {
        if (_dashStartTime+_dashTime < Time.time || _nearWall == _dashDirection)
        {
            StopDash(); return;
        } 
        _rigidbody.velocity = new Vector2(_dashDirection * _dashVelocity, 0);
    }
    #endregion Dashing

    #region Input
    private readonly Input _input = new();
    public void InputMove(float value)
    {
        _input.move = (math.abs(value) > 0.2) ? value : 0;
    }

    public void SetInputJump(bool value)
    {
        _input.jump = value;
        if (value)
        {
            StartJump();
        }
        else
        {
            StopJump();
        }

    }

    public void SetInputDash(bool value)
    {
        _input.dash = value;
        if (value)
        {
            StartDash();
        }
    }

    private class Input
    {
        public float move = 0f;
        public bool jump = false;
        public bool dash = false;

    }
    #endregion Input

    private void OnValidate()
    {
        _rigidbody ??= GetComponent<Rigidbody2D>();
        if (_maxSurfaceAngle > _slideSurfaceAngle) _slideSurfaceAngle = _maxSurfaceAngle;
    }
}