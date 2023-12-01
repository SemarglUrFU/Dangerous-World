using UnityEngine;

class CycleMovingPlatform : CycleMoving, IMovingPlatform
{
    public Vector2 Velocity { get => _delta / Time.deltaTime; }
}