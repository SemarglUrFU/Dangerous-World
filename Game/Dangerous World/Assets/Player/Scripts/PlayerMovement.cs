using System;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour, IExtraJumping
{
    [SerializeField] private UnityEvent _onGrounded;
    [SerializeField] private UnityEvent<float> _onRotate;
    [SerializeField] private UnityEvent<int> _onVerticalDirectionChange;
    [SerializeField] private UnityEvent<float> _onMoving;
    [SerializeField] private UnityEvent<int> _onJump; // 0 - end, 1 - jump, 2 - extra jump 
    [SerializeField] private UnityEvent<bool> _onDash;

    [SerializeField] private Rigidbody2D _rigidbody;
    [SerializeField] private Sensor _topSensor;
    [SerializeField] private Sensor _leftSensor;
    [SerializeField] private Sensor _rightSensor;
    [SerializeField] private Sensor _groundSensor;
    [SerializeField] private Sensor _centerGroundSensor;

    [SerializeField] private Transform _visualAchor;
    [SerializeField] private PhysicsMaterial2D _maxFriction;
    [SerializeField] private PhysicsMaterial2D _minFriction;

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
    [Header("Grounding")]
    [SerializeField][Range(1f, 90f)] private float _maxRotationSpeed;
    private float _lastGroundTime;
    private float _groundAngle;
    private int _nearWall = 0;
    private bool _wasGrounded;
    private Vector2 _movingPlatformVelocity;
    private int _verticalDirection;
    private float _previousAngle;

    private void HandleGrounding()
    {
        if (_groundSensor.IsIntersect)
        {
            _lastGroundTime = Time.time;
            _groundAngle = Vector2.Angle(_groundSensor.IntersectHit.normal, Vector2.up);

            if (_groundSensor.IntersectHit.collider.TryGetComponent<IMovingPlatform>(out var movingPlatform))
            { _movingPlatformVelocity = movingPlatform.Velocity; }
            else { _movingPlatformVelocity = Vector2.zero; }

            if (!_wasGrounded)
            {
                _wasGrounded = true;
                _verticalDirection = 0;
                _onGrounded.Invoke();
                _onVerticalDirectionChange.Invoke(0);
            }
            var normal = _centerGroundSensor.IsIntersect ? _centerGroundSensor.IntersectHit.normal : _groundSensor.IntersectHit.normal;
            SmoothRotate(Vector2.SignedAngle(Vector2.up, normal));
        }
        else
        {
            var verticalDirection = Math.Sign(_rigidbody.velocity.y - _movingPlatformVelocity.y);
            if (verticalDirection != _verticalDirection)
            {
                _verticalDirection = verticalDirection;
                _onVerticalDirectionChange.Invoke(_verticalDirection);
            }

            if (_wasGrounded)
            {
                Rotate(0);
                _movingPlatformVelocity = Vector2.zero;
                _wasGrounded = false;
            }
            else if (_previousAngle != 0)
            {
                SmoothRotate(0);
            }
        }

        if (_leftSensor.IsIntersect || _rightSensor.IsIntersect)
        {
            _nearWall = _leftSensor.IsIntersect ? -1 : 1;
        }
        else
        {
            _nearWall = 0;
        }

        void SmoothRotate(float angle)
        {
            angle = Math.Clamp(angle, -_maxSurfaceAngle, _maxSurfaceAngle);
            angle = Mathf.MoveTowards(_previousAngle, angle, _maxRotationSpeed);
            Rotate(angle);
        }

        void Rotate(float angle)
        {
            _onRotate.Invoke(angle);
            _previousAngle = angle;
        }
    }
    #endregion Grounding

    #region Walking
    [Header("Walking")]
    [SerializeField] private float _walkSpeed = 20f;
    [SerializeField] private float _slideSpeed = 30f;
    [SerializeField] private float _airSpeed = 15f;
    [SerializeField] private float _acceleration = 50f;
    [SerializeField] private float _decceleration = 30f;
    [SerializeField] private float _afterFallDecceleration = 500f;
    [SerializeField][Range(0f, max: 1f)] private float _stopVelocityCutOut = 0.05f;
    [SerializeField] private float _airAcceleration = 50f;
    [SerializeField] private float _airDeceleration = 30f;
    [SerializeField][Range(1, 89)] private float _maxSurfaceAngle = 60f;
    [SerializeField][Range(1, 89)] private float _slideSurfaceAngle = 60f;
    [SerializeField][Range(80, 90)] private float _wallSurfaceAngle = 90f;
    private float _wallJumpLockUntil = float.MinValue;

    private void HandleMove()
    {
        if (_isDashing || _wallJumpLockUntil > Time.time) return;

        var direction = Math.Sign(_input.move);
        var velocity = _rigidbody.velocity;

        _facingRight = GetRotationByDirection(direction);

        _rigidbody.sharedMaterial = _minFriction;
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
                    var currentDirection = Math.Sign(velocity.x - _movingPlatformVelocity.x);
                    var acceleration = currentDirection != 0 && direction != currentDirection
                        ? (_wasGrounded ? _decceleration : _afterFallDecceleration) : _acceleration;
                    velocity = Vector2.MoveTowards(velocity, targetVelocity, acceleration * (1 + angle / _maxSurfaceAngle) * Time.deltaTime);
                }
            }
            else
            {
                // Deccelerate
                if (_groundAngle < _maxSurfaceAngle && !_groundSensor.IntersectHit.collider.TryGetComponent<SurfaceEffector2D>(out var _))
                {
                    var decceleration = _wasGrounded ? _decceleration : _afterFallDecceleration;
                    velocity = Vector2.MoveTowards(velocity, _movingPlatformVelocity, decceleration * Time.deltaTime);
                    if ((velocity - _movingPlatformVelocity).magnitude < _stopVelocityCutOut || !_wasGrounded) { _rigidbody.sharedMaterial = _maxFriction; }
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
            _rigidbody.sharedMaterial = _minFriction;
            var targetXVelocity = _airSpeed * _input.move;
            var acceleration = _input.move != 0 ? _airAcceleration : _airDeceleration;
            velocity.x = Mathf.MoveTowards(velocity.x, targetXVelocity, acceleration * Time.deltaTime);
        }
        var _movingRelativeVelocity = velocity.x - _movingPlatformVelocity.x;
        if (MathF.Abs(_movingRelativeVelocity) > 0.1f) { _onMoving.Invoke(_movingRelativeVelocity); }
        else { _onMoving.Invoke(0f); }
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

    public int ExtraJumpsLeft { get => _extraJumpsLeft; set => _extraJumpsLeft = value; }
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
            { _jumpState = JumpState.ended; _onJump.Invoke(0); }
            ContinueJump();
        }
    }
    private void StartJump()
    {
        _wasReleased = false;
        if (_jumpState == JumpState.started || _topSensor.IsIntersect) { return; }
        if (_jumpState == JumpState.none && _lastGroundTime + _coyoteTime > Time.time)
        {
            if (_groundAngle > _slideSurfaceAngle)
            {
                var velocity = _rigidbody.velocity;
                var pulse = MathF.Sign(_groundSensor.IntersectHit.normal.x) * _jumpVelocity.y;
                velocity.x = pulse > 0 ? Mathf.Max(velocity.x, pulse) : Mathf.Min(velocity.x, pulse);
                _rigidbody.velocity = velocity;
            }
            _jumpState = JumpState.started;
            _jumpStartTime = Time.time;
            _onJump.Invoke(1);
        }
        else if (_nearWall != 0)
        {
            JumpReset();
            _jumpState = JumpState.started;
            _jumpStartTime = Time.time;
            var velocity = _rigidbody.velocity;
            velocity.x = -_nearWall * _jumpVelocity.x;
            _rigidbody.velocity = velocity;
            _wallJumpLockUntil = Time.time + _jumpCutoffTime;
            _facingRight = GetRotationByDirection(-_nearWall);
            _onJump.Invoke(2);
        }
        else if (_extraJumpsLeft > 0)
        {
            var inputDirection = MathF.Sign(_input.move);
            if (inputDirection != 0)
                _rigidbody.velocity = new(_jumpVelocity.x * inputDirection, _rigidbody.velocity.y);

            _extraJumpsLeft--;
            _jumpState = JumpState.started;
            _jumpStartTime = Time.time;
            _onJump.Invoke(2);
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
        _onJump.Invoke(0);
    }
    private void ContinueJump()
    {
        var velocity = _rigidbody.velocity;
        velocity.y = _jumpVelocity.y;
        _rigidbody.velocity = velocity - _rigidbody.gravityScale * Time.deltaTime * Physics2D.gravity;
        if (_jumpStartTime + _jumpTime < Time.time)
        {
            _jumpState = JumpState.ended;
            _onJump.Invoke(0);
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
        if (_isDashing) { ContinueDash(); }
    }
    private void StartDash()
    {
        if (_isDashing || _dashEndTime + _dashCooldown >= Time.time) { return; }
        _dashStartTime = Time.time;
        _dashDirection = _nearWall != 0
            ? -_nearWall : _facingRight ? 1 : -1;
        _rigidbody.gravityScale = 0;
        _rigidbody.sharedMaterial = _minFriction;
        _facingRight = GetRotationByDirection(_dashDirection);
        _isDashing = true;
        _onDash.Invoke(true);
    }
    private void StopDash()
    {
        _rigidbody.velocity = new Vector2(_dashDirection * _airSpeed, 0);
        _rigidbody.gravityScale = _defaultGravityScale;
        _dashEndTime = Time.time;
        _isDashing = false;
        _onDash.Invoke(false);
    }
    private void ContinueDash()
    {
        if (_dashStartTime + _dashTime < Time.time || _nearWall == _dashDirection)
        {
            StopDash();
            return;
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
        if (value) { StartJump(); }
        else { StopJump(); }
    }

    public void SetInputDash(bool value)
    {
        _input.dash = value;
        if (value) { StartDash(); }
    }

    private class Input
    {
        public float move = 0f;
        public bool jump = false;
        public bool dash = false;

    }
    #endregion Input

    private void OnDisable()
    {
        _rigidbody.gravityScale = _defaultGravityScale;
        _rigidbody.sharedMaterial = _maxFriction;
        if (_isDashing) { StopDash(); }
        _jumpState = JumpState.none;
    }

    private void OnValidate()
    {
        _rigidbody = _rigidbody != null ? _rigidbody : GetComponent<Rigidbody2D>();
        if (_maxSurfaceAngle > _slideSurfaceAngle) { _slideSurfaceAngle = _maxSurfaceAngle; }
    }
}