using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
class CycleMovingPlatform : CycleMoving, IMovingPlatform
{
    public Vector2 Velocity => _rigidbody2D.velocity;
}