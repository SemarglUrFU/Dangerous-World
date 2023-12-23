using UnityEngine;

[SelectionBase]
class CycleMovingPlatform : CycleMoving, IMovingPlatform
{
    public Vector2 Velocity => _delta / Time.deltaTime; 
}